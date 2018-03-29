using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web;
using Newtonsoft.Json;
using System.IO;

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

        public static List<Variance> Diff<T>(this T val1, T val2, bool IncludeIdField = false, bool IncludeJsonIgnoreAttributes = false)
        {
            List<Variance> variances = new List<Variance>();
            PropertyInfo[] propInfos = val1.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            if (propInfos.Length > 0)
            {
                foreach (PropertyInfo propInfo in propInfos)
                {
                    Variance v = new Variance();
                    v.Prop = propInfo.Name;
                    v.valA = propInfo.GetValue(val1);
                    v.valB = propInfo.GetValue(val2);
                    if ((object.ReferenceEquals(v.valA, null) ^ object.ReferenceEquals(v.valB, null)) ||
                        ((IncludeJsonIgnoreAttributes || propInfo.GetCustomAttribute(typeof(Newtonsoft.Json.JsonIgnoreAttribute)) != null) &&
                        (IncludeIdField || propInfo.Name.ToLower() != "id") &&
                        v.valA != null && !v.valA.Equals(v.valB)))
                    {
                        variances.Add(v);
                    }
                }
            }
            return variances;
        }

        /// <summary>
        /// Generates tree of items from item list
        /// </summary>
        /// 
        /// <typeparam name="T">Type of item in collection</typeparam>
        /// <typeparam name="K">Type of parent_id</typeparam>
        /// 
        /// <param name="collection">Collection of items</param>
        /// <param name="id_selector">Function extracting item's id</param>
        /// <param name="parent_id_selector">Function extracting item's parent_id</param>
        /// <param name="root_id">Root element id</param>
        /// 
        /// <returns>Tree of items</returns>
        public static IEnumerable<TreeItem<T>> GenerateTree<T, K>(this IEnumerable<T> collection, Func<T, K> id_selector, Func<T, K> parent_id_selector, K root_id = default(K))
        {
            foreach (var c in collection.Where(c => parent_id_selector(c) != null && parent_id_selector(c).Equals(root_id)))
            {
                yield return new TreeItem<T>
                {
                    Item = c,
                    Children = collection.GenerateTree(id_selector, parent_id_selector, id_selector(c))
                };
            }
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
        /// <summary>
        /// Serializes an object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serializableObject"></param>
        /// <param name="fileName"></param>
        public static bool SerializeObject<T>(T serializableObject, string fileName)
        {
            if (serializableObject == null) { return false; }

            try
            {
                string output = JsonConvert.SerializeObject(serializableObject, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
                File.WriteAllText(fileName, output);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Deserializes an xml file into an object list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static T DeSerializeObject<T>(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) { return default(T); }

            try
            {
                string input = File.ReadAllText(fileName);
                return JsonConvert.DeserializeObject<T>(input);
            }
            catch (Exception ex)
            {
                return default(T);
            }
        }

        /// <summary>
        /// Perform a deep Copy of the object, using Json as a serialisation method. NOTE: Private members are not cloned using this method.
        /// </summary>
        /// <typeparam name="T">The type of object being copied.</typeparam>
        /// <param name="source">The object instance to copy.</param>
        /// <returns>The copied object.</returns>
        public static T Clone<T>(this T source)
        {
            // Don't serialize a null object, simply return the default for that object
            if (Object.ReferenceEquals(source, null))
            {
                return default(T);
            }

            // initialize inner objects individually
            // for example in default constructor some list property initialized with some values,
            // but in 'source' these items are cleaned -
            // without ObjectCreationHandling.Replace default constructor values will be added to result
            var deserializeSettings = new JsonSerializerSettings { ObjectCreationHandling = ObjectCreationHandling.Replace };

            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(source), deserializeSettings);
        }


        public static IEnumerable<TSource> Duplicates<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector)
        {
            var grouped = source.GroupBy(selector);
            var moreThen1 = grouped.Where(i => i.IsMultiple());
            return moreThen1.SelectMany(i => i);
        }

        public static IEnumerable<TSource> Duplicates<TSource, TKey>(this IEnumerable<TSource> source)
        {
            return source.Duplicates(i => i);
        }

        public static bool IsMultiple<T>(this IEnumerable<T> source)
        {
            var enumerator = source.GetEnumerator();
            return enumerator.MoveNext() && enumerator.MoveNext();
        }
    }

    public class Variance
    {
        public string Prop { get; set; }
        public object valA { get; set; }
        public object valB { get; set; }
    }

    public class TreeItem<T>
    {
        public T Item { get; set; }
        public IEnumerable<TreeItem<T>> Children { get; set; }
    }
}