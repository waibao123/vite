using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class TableAttribute : Attribute
    {
        public TableAttribute(string tableName, ColModeEnum colMode)
        {
            TableName = tableName;
            ColMode = colMode;
        }

        public string TableName;

        public ColModeEnum ColMode;
    }


    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ColAttribute : Attribute
    {
        public ColAttribute(string str)
        {
            ColName = str;
        }

        public ColAttribute(string str, DbType t)
        {
            ColName = str;
            ColDbType = t;
        }

        public string ColName;
        public DbType ColDbType;
    }

    public enum ColModeEnum
    {
        PointedFirst = 0,
        PointedOnly = 1
    }



}
