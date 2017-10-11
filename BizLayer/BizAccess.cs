using DataAccessLayer;
using EntityLayer.BizEntity;
using EntityLayer.DbEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLayer
{
    public static class BizAccess
    {
        public static List<ProductCategory> GetAllProductCategory(int webId)
        {
            return DalFactory.BizAccessDal.GetAllProductCategory(webId);
        }

        public static List<Product> GetProductsByCategoryId(int cateId)
        {
            List<int> cateIds = new List<int>();
            cateIds.Add(cateId);
            return DalFactory.BizAccessDal.GetProductsByCategoryId(cateIds);
        }

        public static List<Product> GetProductsByCategoryId(List<int> cateIds)
        {
            return DalFactory.BizAccessDal.GetProductsByCategoryId(cateIds);
        }

        public static Product GetProductById(int id)
        {
            return DalFactory.BizAccessDal.GetProductById(id);
        }

        public static List<ProductAttrKVP> GetProductAttrs(int productId)
        {
            return DalFactory.BizAccessDal.GetProductAttrs(productId);
        }

    }
}
