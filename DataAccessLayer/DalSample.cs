using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class DalSample : DalBase
    {
        public DalSample(string connKey, int dbType = 0) : base(connKey, dbType)
        {
        }
    }
}
