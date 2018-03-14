using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web;

namespace Ekin.Clarizen
{
    public static class Extensions
    {
        /// <summary>
        /// Iterate through the given type to get a list of its public properties, excluding Id
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string[] GetPropertyList(this Type t, bool IncludeIdField = false)
        {
            if (t == null)
                return null;
            List<string> ret = new List<string> { };
            PropertyInfo[] propInfos = t.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            if (propInfos.Length > 0)
            {
                foreach (PropertyInfo propInfo in propInfos)
                {
                    if ((IncludeIdField || propInfo.Name.ToLower() != "id") && 
                        propInfo.GetCustomAttribute(typeof(Newtonsoft.Json.JsonIgnoreAttribute)) == null)
                        ret.Add(propInfo.Name);
                }
            }
            return ret.ToArray();
        }

        /// <summary>
        /// Compares the values of two Clarizen entities with an option to ignore the ID field. Properties decorated with [JsonIgnore] are ommitted from comparison.
        /// </summary>
        /// <param name="obj">Base object</param>
        /// <param name="target">Target object to compare the base object to</param>
        /// <param name="IncludeIdField">When ommitted the value of the ID property is not compared</param>
        /// <returns></returns>
        public static bool IsEntitySameAs(this object obj, object target, bool IncludeIdField = false)
        {
            if (target == null || target.GetType() != obj.GetType()) return false;

            PropertyInfo[] propInfos = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            if (propInfos.Length > 0)
            {
                foreach (PropertyInfo propInfo in propInfos)
                {
                    object value1 = propInfo.GetValue(obj, null);
                    object value2 = propInfo.GetValue(target, null);

                    if ((object.ReferenceEquals(value1, null) ^ object.ReferenceEquals(value2, null)))
                    {
                        return false;
                    }

                    if (propInfo.GetCustomAttribute(typeof(Newtonsoft.Json.JsonIgnoreAttribute)) == null &&
                        (IncludeIdField || propInfo.Name.ToLower() != "id") &&
                        value1 != null && !value1.Equals(value2))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public static string GetFormattedErrorMessage(this object Error) {
            if (Error is WebException)
            {
                WebException ex = Error as WebException;
                return String.Format("[{0}] {1}", ex.StatusCode(), ex.Message);
            }
            else if (Error is error)
            {
                error err = Error as error;
                return err.formatted;
            }
            return "";
        }

        public static int StatusCode(this WebException ex)
        {
            if (ex.Status == WebExceptionStatus.ProtocolError)
            {
                var response = ex.Response as HttpWebResponse;
                if (response != null)
                {
                    return (int)response.StatusCode;
                }
            }
            return 0;
        }
    }
}