using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class DalFactory
    {
        public static DalTest OldDB = new DalTest("ConnOld");

        public static DalTest NewDB = new DalTest("ConnNew");

        public static DalSample ImportDal = new DalSample("ConnNew");

        public static BizAccess BizAccessDal = new BizAccess("ConnNew");
        
    }
}
