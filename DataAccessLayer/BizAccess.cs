using EntityLayer.BizEntity;
using EntityLayer.DbEntity;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class BizAccess : DalBase
    {
        public BizAccess(string connKey, int dbType = 0) : base(connKey, dbType)
        {
        }


        public List<ProductCategory> GetAllProductCategory(int webId)
        {
            string sql = "SELECT * FROM energroup.productcategory WHERE WebId=@WebId";
            List<IDataParameter> ps = new List<IDataParameter>();
            ps.Add(new MySqlParameter("@WebId", webId));
            return GetList<ProductCategory>(sql, ps);
        }

        public List<Product> GetProductsByCategoryId(List<int> cateIds)
        {
            string sql = string.Format("SELECT * FROM product WHERE Id IN (SELECT ProductId FROM productcatebelong WHERE CategoryId IN({0}))", string.Join(",", cateIds));
            return GetList<Product>(sql, null);
        }

        public Product GetProductById(int id)
        {
            string sql = "SELECT * FROM product WHERE Id=@Id LIMIT 1";
            List<IDataParameter> ps = new List<IDataParameter>();
            ps.Add(new MySqlParameter("@Id", id));
            return GetItem<Product>(sql, ps);
        }

        public List<ProductAttrKVP> GetProductAttrs(int productId)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("SELECT pa.AttrName,pam.AttrValue");
            sbSql.Append(" FROM ProductAttrMapping pam");
            sbSql.Append(" INNER JOIN productattr pa");
            sbSql.Append(" ON pam.AttrId = pa.Id");
            sbSql.Append(" AND pam.ProductId = @ProductId");
            sbSql.Append(" ORDER BY pam.Idx asc");
            List<IDataParameter> ps = new List<IDataParameter>();
            ps.Add(new MySqlParameter("@ProductId", productId));
            return GetList<ProductAttrKVP>(sbSql.ToString(), ps);
        }
    }
}
