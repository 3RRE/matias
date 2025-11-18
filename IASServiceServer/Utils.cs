using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IASServiceServer
{
    public static class Utils
    {
        public static IEnumerable<T> FromDynamicList<T>(IList<dynamic> dynamicList) where T : class
        {
            var results = new List<T>();
            var properties = typeof(T).GetProperties();

            foreach (var dynamicItem in dynamicList)
            {
                var instance = Activator.CreateInstance<T>();
                foreach (var expProp in dynamicItem as IDictionary<string, object>
                                        ?? new Dictionary<string, object>())
                {
                    var property = properties.SingleOrDefault(x => x.Name == expProp.Key);
                    if (property != null)
                    {
                        property.SetValue(instance, expProp.Value);
                    }
                }

                results.Add(instance);
            }

            return results;
        }

        public static IEnumerable<T> FromExpandoList<T>(IList<dynamic> expandoList)
        {
            var results = new List<T>();
            foreach (var expando in expandoList)
            {
                var instance = Activator.CreateInstance<T>();
                new FromExpando().Map(expando, instance);
                results.Add(instance);
            }

            return results;
        }
    }
}
