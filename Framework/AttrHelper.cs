using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
    public static class AttrHelper
    {
        public static string PickRealColName(PropertyInfo p, ColModeEnum? colMode)
        {
            ColAttribute ca = p.GetCustomAttribute(typeof(ColAttribute)) as ColAttribute;
            if (colMode == ColModeEnum.PointedOnly && ca == null)
                return null;
            if (ca != null)
                return ca.ColName;
            return p.Name;
        }
    }
}
