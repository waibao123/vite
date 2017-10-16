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
            string curCategory;
            if (ids.Contains(categoryId))
            {
                list = BizAccess.GetProductsByCategoryId(categoryId);
                curCategory = pcs.First(item => item.Id == categoryId).CategoryName;
            }
            else
            {
                list = BizAccess.GetProductsByCategoryId(ids);
                curCategory = "全部商品";
            }
            ViewBag.ProductCategory = pcs;
            ViewBag.Products = list;
            ViewBag.Website = webId;
            ViewBag.Category = curCategory;
            ViewBag.RecommandList = BizAccess.GetRecommandProduct(webId);
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
            ViewBag.RecommandList = BizAccess.GetRecommandProduct(webId);
            return View();
        }

        public ActionResult Search(string ws, string kw)
        {
            int webId = FormatTools.ParseInt(ws, DefaultWeb);

            ViewBag.ProductList = BizAccess.GetProductsByName(webId, kw);
            if (ViewBag.ProductList == null || ViewBag.ProductList.Count == 0)
                ViewBag.Msg = "无相关结果，请更换查询条件重试";
            ViewBag.Website = webId;
            ViewBag.RecommandList = BizAccess.GetRecommandProduct(webId);
            return View();
        }

    }
}
