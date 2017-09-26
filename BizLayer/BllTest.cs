using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;
using EntityLayer.DbEntity;

namespace BizLayer
{
    public static class BllTest
    {
        public static List<Options> GetAllOptions()
        {
            return DalFactory.OldDB.GetOptions();
        }


        public static int BatchInsert<T>(List<T> list)
        {
            return DalFactory.NewDB.BatchInsert(list);
        }

        public static List<Product> GetProductsByCategory(int cateId, int limit)
        {
            return DalFactory.NewDB.GetProductsByCategory(cateId, limit);
        }

    }
}
