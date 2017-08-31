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
        public static List<Options> GetAllOptions() {
            return DalTest.GetAllOptions();

        }
    }
}
