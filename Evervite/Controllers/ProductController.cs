using BizLayer;
using EntityLayer.BizEntity;
using EntityLayer.DbEntity;
using EntityLayer.Enums;
using Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Evervite.Controllers
{
    public class ProductController : Controller
    {
        //
        // GET: /ProductList/

        //public ActionResult Index()
        //{
        //    return View();
        //}


        public ActionResult List(string idStr)
        {
            List<ProductCategory> pcs = BizAccess.GetAllProductCategory((int)WebsiteEnum.EnerVite);
            List<int> ids = pcs.Select(item => item.Id).ToList();
            int id = FormatTools.ParseInt(idStr);
            List<Product> list;
            if (ids.Contains(id))
                list = BizAccess.GetProductsByCategoryId(id);
            else
                list = BizAccess.GetProductsByCategoryId(ids);
            ViewBag.ProductCategory = pcs;
            ViewBag.Product = list;
            return View();
        }

        public ActionResult Detail(string idStr)
        {
            int id =1;
            Product p = BizAccess.GetProductById(id);
            List<ProductAttrKVP> attrs = BizAccess.GetProductAttrs(p.Id);

            ViewBag.Product = p;
            ViewBag.ProductAttr = attrs;
            return View();
        }

    }
}
