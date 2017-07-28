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