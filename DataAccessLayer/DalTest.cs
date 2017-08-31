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
    public static class DalTest
    {
        public static List<Options> GetAllOptions()
        {

            MySqlConnection mysqlcon = new MySqlConnection(ConfigurationManager.AppSettings.Get("ConnMySql"));
            mysqlcon.Open();
            MySqlCommand mysqlcom = new MySqlCommand("SELECT * FROM wp_options", mysqlcon);
            MySqlDataReader sdr = mysqlcom.ExecuteReader();
            List<Options> l = QuickDAL.QuickListReader<Options>(sdr);
            mysqlcom.Dispose();
            mysqlcon.Close();
            mysqlcon.Dispose();
            MySqlParameter sp = new MySqlParameter();
            sp.DbType = DbType.Int64;

            return l;

        }

    }
}
