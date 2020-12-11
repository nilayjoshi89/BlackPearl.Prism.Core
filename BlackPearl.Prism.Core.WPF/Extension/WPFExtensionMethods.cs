using System.Linq;
using System.Reflection;

using Prism.Regions;

namespace BlackPearl.Prism.Core.WPF
{
    public static class WPFExtensionMethods
    {
        public static void UpdateNavigationParameter<T>(this NavigationParameters navigationParameter, T data)
            where T : class
        {
            if (data == null)
            {
                return;
            }

            System.Collections.Generic.IEnumerable<PropertyInfo> properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                        .Where(p => p.CanRead && p.CanWrite);

            foreach (PropertyInfo p in properties)
            {
                if (!navigationParameter.ContainsKey(p.Name))
                {
                    navigationParameter.Add(p.Name, p.GetValue(data));
                }
            }
        }

        public static void MapNavigationValueToObject<T>(this NavigationParameters navigationParameter, T data)
            where T : class
        {
            if (data == null)
            {
                return;
            }

            System.Collections.Generic.IEnumerable<PropertyInfo> properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                        .Where(p => p.CanRead && p.CanWrite);

            foreach (System.Collections.Generic.KeyValuePair<string, object> np in navigationParameter)
            {
                properties.FirstOrDefault(p => p.Name == np.Key)?.SetValue(data, np.Value);
            }
        }
    }
}
