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

        public ColAttribute(string str, ColType t)
        {
            ColName = str;
            ColType = t;
        }

        public ColAttribute(string str, DbType dt, ColType ct)
        {
            ColName = str;
            ColDbType = dt;
            ColType = ct;
        }

        public string ColName;
        public DbType ColDbType;
        public ColType ColType;
    }

    public enum ColModeEnum
    {
        PointedFirst = 0,
        PointedOnly = 1
    }

    public enum ColType
    {
        Normal = 0,
        PK = 1,
        PK_AI = 2
    }

}
