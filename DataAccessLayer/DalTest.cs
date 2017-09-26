using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityLayer.DbEntity;
using Framework;
using MySql.Data.MySqlClient;

namespace DataAccessLayer
{
    public class DalTest : DalBase
    {
        public DalTest(string connKey, int dbType = 0) : base(connKey, dbType) { }


        public List<Options> GetAllOptions()
        {
            string sql = "SELECT * FROM wp_options";
            return GetList<Options>(sql, null);
        }

        public List<Options> GetOptions()
        {
            string sql = "SELECT * FROM wp_options where option_value like @L AND length(option_value)<@Len limit @Lim";
            List<IDataParameter> ps = new List<IDataParameter>();
            ps.Add(new MySqlParameter("@L", "%e%"));
            ps.Add(new MySqlParameter("@Len", 30));
            ps.Add(new MySqlParameter("@Lim", 6));
            return GetList<Options>(sql, ps);
        }

        public int BatchInsert<T>(List<T> list)
        {
            return InsertList(list);
        }


        public List<Product> GetProductsByCategory(int cateId, int limit)
        {
            string sql = "SELECT * FROM product where CategoryId=@CategoryId LIMIT @Limit";
            List<IDataParameter> ps = new List<IDataParameter>();
            ps.Add(new MySqlParameter("@CategoryId", cateId));
            ps.Add(new MySqlParameter("@Limit", limit));
            return GetList<Product>(sql, ps);
        }
    }
}
