using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Framework
{
    public static class QuickMySqlDAL
    {
        public static int ExecuteNonQuery(string sql, string connStr)
        {
            MySqlConnection conn = null;
            MySqlCommand cmd = null;
            try
            {
                conn = new MySqlConnection(connStr);
                conn.Open();
                cmd = new MySqlCommand(sql, conn);
                return cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
                if (conn != null)
                    conn.Dispose();
            }
        }

        public static List<T> GetList<T>(string sql, List<IDataParameter> ps, string connStr) where T : class, new()
        {
            MySqlConnection conn = null;
            MySqlCommand cmd = null;
            List<T> result = null;
            try
            {
                conn = new MySqlConnection(connStr);
                conn.Open();
                cmd = new MySqlCommand(sql, conn);
                if (ps != null)
                    foreach (var p in ps)
                        cmd.Parameters.Add(p);
                MySqlDataReader sdr = cmd.ExecuteReader();
                result = QuickListReader<T>(sdr);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
                if (conn != null)
                    conn.Dispose();
            }
            return result;
        }

        public static T GetItem<T>(string sql, List<IDataParameter> ps, string connStr) where T : class, new()
        {
            MySqlConnection conn = null;
            MySqlCommand cmd = null;
            try
            {
                conn = new MySqlConnection(connStr);
                conn.Open();
                cmd = new MySqlCommand(sql, conn);
                if (ps != null)
                    foreach (var p in ps)
                        cmd.Parameters.Add(p);
                MySqlDataReader sdr = cmd.ExecuteReader();
                if (sdr.Read())
                    return QuickReader<T>(sdr);
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
                if (conn != null)
                    conn.Dispose();
            }
        }

        static List<T> QuickListReader<T>(IDataReader sdr) where T : class, new()
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
                        if (value == null || value is DBNull)
                            continue;
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

        public static int QuickInsert<T>(List<T> list, string connStr)
        {
            int result = 0;
            MySqlConnection conn = null;
            MySqlCommand cmd = null;
            try
            {
                string sql = CreateModifySql(typeof(T), 0);
                conn = new MySqlConnection(connStr);
                conn.Open();
                cmd = new MySqlCommand(sql, conn);
                foreach (T item in list)
                {
                    FillParameters(cmd, item, 0);
                    result += cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
                if (conn != null)
                    conn.Dispose();
            }
            return result;
        }

        public static int QuickInsert<T>(T item, string connStr)
        {
            MySqlConnection conn = null;
            MySqlCommand cmd = null;
            try
            {
                string sql = CreateModifySql(typeof(T), 0);
                conn = new MySqlConnection(connStr);
                conn.Open();
                cmd = new MySqlCommand(sql, conn);
                FillParameters(cmd, item, 0);
                return cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
                if (conn != null)
                    conn.Dispose();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="actionType">0:Insert 1:Update</param>
        /// <returns></returns>
        public static string CreateModifySql(Type type, int actionType)
        {
            List<string> cols = new List<string>();
            string pkCol = null;
            TableAttribute ta = type.GetCustomAttribute(typeof(TableAttribute)) as TableAttribute;
            PropertyInfo[] Props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo p in Props)
            {
                string name = AttrHelper.PickRealColName(p, ta?.ColMode);
                if (name == null)
                    continue;
                ColAttribute ca = p.GetCustomAttribute(typeof(ColAttribute)) as ColAttribute;
                if (ca != null)
                {
                    if (ca.ColType == ColType.PK || ca.ColType == ColType.PK_AI)
                    {
                        pkCol = name;
                        if (actionType == 1)
                            continue;
                    }
                    if (ca.ColType == ColType.PK_AI)
                        continue;
                }
                cols.Add(name);
            }
            if (actionType == 0)
                return string.Format("INSERT INTO {0} ({1}) VALUES ({2})", ta.TableName, string.Join(",", cols), string.Join(",", cols.Select(item => "@" + item)));
            if (actionType == 1)
                return string.Format("UPDATE {0} SET {1} WHERE {2}", ta.TableName, string.Join(",", cols.Select(item => string.Format("{0}=@{0}", item))), string.Format("{0}=@{0}", pkCol));
            return null;
        }

        static void FillParameters<T>(MySqlCommand cmd, T item, int actionType)
        {
            cmd.Parameters.Clear();
            Type type = typeof(T);
            TableAttribute ta = type.GetCustomAttribute(typeof(TableAttribute)) as TableAttribute;
            PropertyInfo[] Props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo p in Props)
            {
                string name = AttrHelper.PickRealColName(p, ta?.ColMode);
                if (name == null)
                    continue;
                ColAttribute ca = p.GetCustomAttribute(typeof(ColAttribute)) as ColAttribute;
                if (actionType == 1 && ca.ColType == ColType.PK_AI)
                    continue;
                cmd.Parameters.AddWithValue("@" + name, p.GetValue(item));
            }
        }


        static T QuickReader<T>(IDataReader sdr) where T : class, new()
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
                    if (value == null || value is DBNull)
                        continue;
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
