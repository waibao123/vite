using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework;
using MySql.Data.MySqlClient;

namespace DataAccessLayer
{
    public abstract class DalBase
    {
        private string ConnStr;
        private int DBType;


        protected DalBase(string connKey, int dbType = 0)
        {
            ConnStr = ConfigurationManager.AppSettings.Get(connKey);
            DBType = dbType;
        }

        public int ExecuteNonQuery(string sql)
        {
            return QuickMySqlDAL.ExecuteNonQuery(sql, ConnStr);
        }

        public List<T> GetList<T>(string sql, List<IDataParameter> ps) where T : class, new()
        {
            return QuickMySqlDAL.GetList<T>(sql, ps, ConnStr);
        }

        public T GetItem<T>(string sql, List<IDataParameter> ps) where T : class, new()
        {
            return QuickMySqlDAL.GetItem<T>(sql, ps, ConnStr);
        }

        public int InsertList<T>(List<T> list)
        {
            return QuickMySqlDAL.QuickInsert<T>(list, ConnStr);
        }

        public int InsertItem<T>(T item)
        {
            return QuickMySqlDAL.QuickInsert<T>(item, ConnStr);
        }

    }
}
