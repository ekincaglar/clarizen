using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Xml.Linq;
using Ekin.Log;

namespace Ekin.Clarizen
{
    public class OutboundProperties<T> where T : class, new()
    {
        private XNamespace ns = "http://clarizen.com/api";

        public LogFactory Logs { get; private set; }
        public string RawRequest { get; private set; }
        public string OrganizationId { get; private set; }
        public string RuleName { get; private set; }
        public bool HasErrors { get { return Logs != null && Logs.HasErrors(); } }

        public OutboundProperties()
        {
        }

        // Used for testing only
        public List<T> Parse(string Input)
        {
            List<T> result = null;
            byte[] byteArray = Encoding.UTF8.GetBytes(Input);
            using (MemoryStream stream = new MemoryStream(byteArray))
            {
                result = Parse(stream);
            }
            return result;
        }

        public List<T> Parse(Stream InputStream)
        {
            Logs = new LogFactory();
            List<T> result = new List<T>();

            // Parse the request to string, so that it can be sent with errors
            InputStream.Position = 0;
            StreamReader stream = new StreamReader(InputStream);
            RawRequest = HttpUtility.UrlDecode(stream.ReadToEnd());

            XDocument xdoc = null;
            try
            {
                // Load the request to an XML Document for processing
                InputStream.Position = 0;
                xdoc = XDocument.Load(InputStream);

                // Get the properties of the target object that are decorated with the ClarizenOutboundCallProperty attribute
                var properties = (typeof(T)).GetProperties().Where(x => x.GetCustomAttributes(typeof(ClarizenOutboundCallPropertyAttribute), true).Any()).ToList();
                if (properties?.Count == 0)
                {
                    Logs.AddError("Ekin.Clarizen.OutboundProperties", "Parse", $"No properties in the {typeof(T).Name} object are decorated with ClarizenOutboundCallProperty attribute. Nothing to parse.");
                    return null;
                }

                // Parse the XML
                var EntitiesElement = xdoc.Descendants(ns + "Entities").FirstOrDefault();
                if (EntitiesElement != null)
                {
                    foreach (XElement BaseEntityElement in EntitiesElement.Descendants(ns + "BaseEntity"))
                    {
                        T entityObj = new T();

                        if (BaseEntityElement != null)
                        {
                            #region Parse the BaseEntity Id

                            EntityId BaseEntity = GetEntityIdFromNode(BaseEntityElement);
                            if (BaseEntity != null)
                            {
                                PropertyInfo BaseEntityProperty = GetPropertyByAttributeValue(properties, "BaseEntityId");
                                if (BaseEntityProperty != null)
                                {
                                    BaseEntityProperty.SetValue(entityObj, BaseEntity);
                                }
                            }

                            #endregion Parse the BaseEntity Id

                            #region Parse the Field Values for the BaseEntity

                            XElement BaseEntityValuesElement = BaseEntityElement.Element(ns + "Values");
                            if (BaseEntityValuesElement != null)
                            {
                                IEnumerable<XElement> fieldNodes = BaseEntityValuesElement.Descendants(ns + "FieldValue");
                                foreach (XElement element in fieldNodes)
                                {
                                    XElement fieldName = element.Element(ns + "FieldName");
                                    XElement fieldValue = element.Element(ns + "Value");
                                    if (fieldName == null || fieldName.Value == null || fieldValue == null || fieldValue.Value == null)
                                    {
                                        Logs.AddError("Ekin.Clarizen.OutboundProperties", "Parse", $"Request XML could not be parsed. Request body: {RawRequest}");
                                        return null;
                                    }
                                    else
                                    {
                                        PropertyInfo prop = GetPropertyByAttributeValue(properties, fieldName.Value);
                                        if (prop == null)
                                        {
                                            Logs.AddError("Ekin.Clarizen.OutboundProperties", "Parse", $"Request XML could not be parsed. Request body: {RawRequest}");
                                        }
                                        else if (prop.PropertyType.Equals(typeof(EntityId)))
                                        {
                                            EntityId PropertyEntityId = GetEntityIdFromNode(fieldValue);
                                            if (PropertyEntityId != null)
                                            {
                                                prop.SetValueByType(entityObj, PropertyEntityId);
                                            }
                                        }
                                        else
                                        {
                                            prop.SetValueByType(entityObj, fieldValue.Value);
                                        }
                                    }
                                }
                            }

                            #endregion Parse the Field Values for the BaseEntity
                        }

                        result.Add(entityObj);
                    }
                }

                XElement organizationIdElement = xdoc.Descendants(ns + "OrganizationId")?.FirstOrDefault();
                if (organizationIdElement != null)
                {
                    OrganizationId = organizationIdElement.Value;
                }

                XElement ruleNameElement = xdoc.Descendants(ns + "RuleName")?.FirstOrDefault();
                if (ruleNameElement != null)
                {
                    RuleName = ruleNameElement.Value;
                }
            }
            catch (Exception ex)
            {
                Logs.AddError("Ekin.Clarizen.OutboundProperties", "Parse", $"Request could not be parsed. Error: {ex.Message} Request body: {RawRequest}");
                return null;
            }

            return result;
        }

        private EntityId GetEntityIdFromNode(XElement BaseEntityElement)
        {
            if (BaseEntityElement == null) return null;
            XElement IdElement = BaseEntityElement.Element(ns + "Id");
            if (IdElement != null)
            {
                XElement typeName = IdElement.Element(ns + "TypeName");
                XElement typeValue = IdElement.Element(ns + "Value");
                if (typeName?.Value != null || typeValue?.Value != null)
                {
                    return new EntityId($"/{typeName.Value}/{typeValue.Value}");
                }
            }
            Logs.AddError("Ekin.Clarizen.OutboundProperties", "GetEntityIdFromNode", $"{BaseEntityElement.Name} node could not be parsed. Make sure it has Id, Id\\TypeName, Id\\Value nodes.");
            return null;
        }

        private List<string> GetAttributeValues(Type type, string propertyName)
        {
            var property = type.GetProperty(propertyName).GetCustomAttributes(false).Where(x => x.GetType().Name.Equals("ClarizenOutboundCallPropertyAttribute", StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
            if (property != null)
            {
                return ((ClarizenOutboundCallPropertyAttribute)property).ValueNames;
            }
            return new List<string>();
        }

        private PropertyInfo GetPropertyByAttributeValue(List<PropertyInfo> properties, string attributeValue)
        {
            foreach (var prop in properties)
            {
                if (GetAttributeValues(typeof(T), prop.Name).Contains(attributeValue, StringComparer.InvariantCultureIgnoreCase))
                {
                    return prop;
                }
            }
            return null;
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class ClarizenOutboundCallPropertyAttribute : Attribute
    {
        protected List<string> _valueNames { get; set; }
        public List<string> ValueNames { get { return _valueNames; } set { _valueNames = value; } }

        public ClarizenOutboundCallPropertyAttribute()
        {
            _valueNames = new List<string>();
        }

        public ClarizenOutboundCallPropertyAttribute(params string[] valueNames)
        {
            _valueNames = valueNames.ToList();
        }
    }
}