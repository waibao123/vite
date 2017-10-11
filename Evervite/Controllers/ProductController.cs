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

        static int DefaultWeb = (int)WebsiteEnum.EnerVite;

        public ActionResult List(string ws, string ct)
        {
            int webId = FormatTools.ParseInt(ws, DefaultWeb);
            int categoryId = FormatTools.ParseInt(ct);
            List<ProductCategory> pcs = BizAccess.GetAllProductCategory(webId);
            List<int> ids = pcs.Select(item => item.Id).ToList();
            List<Product> list;
            if (ids.Contains(categoryId))
                list = BizAccess.GetProductsByCategoryId(categoryId);
            else
                list = BizAccess.GetProductsByCategoryId(ids);
            ViewBag.ProductCategory = pcs;
            ViewBag.Products = list;
            ViewBag.Website = webId;
            return View();
        }

        public ActionResult Detail(string ws, int id)
        {
            int webId = FormatTools.ParseInt(ws, DefaultWeb);
            Product p = BizAccess.GetProductById(id);
            List<ProductAttrKVP> attrs = BizAccess.GetProductAttrs(p.Id);

            ViewBag.Product = p;
            ViewBag.ProductAttr = attrs;
            ViewBag.Website = webId;
            return View();
        }

    }
}
