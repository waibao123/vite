using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
  public static  class QuickDAL
    {
        public static List<T> QuickListReader<T>(IDataReader sdr) where T : class, new()
        {
            List<T> result = new List<T>();
            try
            {
                Type type = typeof(T);
                TableAttribute ta = type.GetCustomAttribute(typeof(TableAttribute)) as TableAttribute;
                PropertyInfo[] Props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                while (sdr.Read())
                {
                    T item = new T();
                    foreach (PropertyInfo p in Props)
                    {
                        object value;
                        string name = AttrHelper.PickRealColName(p, ta?.ColMode);
                        if (name == null)
                            continue;
                        try
                        {
                            value = sdr[name];
                        }
                        catch
                        {
                            continue;
                        }
                        p.SetValue(item, value, null);
                    }
                    result.Add(item);
                }
            }
            catch (Exception ex)
            {
                result = null;
            }
            return result;
        }


        public static T QuickReader<T>(IDataReader sdr) where T : class, new()
        {
            T result;
            try
            {
                result = new T();
                Type type = typeof(T);
                PropertyInfo[] Props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                foreach (PropertyInfo p in Props)
                {
                    object value;
                    try
                    {
                        value = sdr[p.Name];
                    }
                    catch
                    {
                        continue;
                    }
                    p.SetValue(result, value, null);
                }
            }
            catch
            {
                result = null;
            }
            return result;
        }

    }
}
