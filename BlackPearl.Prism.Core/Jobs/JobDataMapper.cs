using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Quartz;

namespace BlackPearl.Prism.Core.Jobs
{
    internal interface IJobDataMapper
    {
        T Map<T>(JobDataMap mapping)
            where T : class, new();
        JobDataMap Map<T>(T data)
            where T : class, new();

        void UpdateMap<T>(T data, JobDataMap map)
            where T : class, new();
    }
    internal sealed class JobDataMapper : IJobDataMapper
    {
        public T Map<T>(JobDataMap mapping) where T : class, new()
        {
            var result = new T();
            IEnumerable<PropertyInfo> properties = GetProperies<T>();

            foreach (PropertyInfo p in properties)
            {
                if (!mapping.ContainsKey(p.Name))
                {
                    continue;
                }

                p.SetValue(result, mapping[p.Name]);
            }

            return result;
        }

        public JobDataMap Map<T>(T data) where T : class, new()
        {
            var result = new JobDataMap();
            UpdateMap(data, result);
            return result;
        }
        public void UpdateMap<T>(T data, JobDataMap map) where T : class, new()
        {
            IEnumerable<PropertyInfo> properties = GetProperies<T>();

            foreach (PropertyInfo p in properties)
            {
                map.Add(p.Name, p.GetValue(data));
            }
        }
        private static IEnumerable<PropertyInfo> GetProperies<T>() where T : class, new()
        {
            return typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                            .Where(p => p.CanWrite && p.CanWrite);
        }
    }
}
