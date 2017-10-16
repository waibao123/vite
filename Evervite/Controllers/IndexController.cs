using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BizLayer;
using EntityLayer.Enums;

namespace Evervite.Controllers
{
    public class IndexController : Controller
    {
        //
        // GET: /Index/

        public ActionResult Index()
        {
            //var vvv = BllTest.GetAllOptions();
            //ViewBag.CCC = string.Join("<br/>", vvv.Select(item => item.OptionValue));
            return View();
        }

        public ActionResult MainSite()
        {
            ViewBag.CategoryList = BizAccess.GetAllProductCategory((int)WebsiteEnum.EnerVite);
            ViewBag.ProductList = BizAccess.GetRecommandProduct((int)WebsiteEnum.EnerVite);
            ViewBag.WebsiteId = (int)WebsiteEnum.EnerVite;
            return View();
        }

        public ActionResult ProductList()
        {
            return View();
        }

        public ActionResult ProductMilkList()
        {
            return View();
        }

        public ActionResult ProductHealthList()
        {
            return View();
        }

        public ActionResult ProductDetail()
        {
            return View();
        }

        public ActionResult ProductMilkDetail()
        {
            return View();
        }

        public ActionResult ProductHealthDetail()
        {
            return View();
        }

        public ActionResult MainMilkSite()
        {
            ViewBag.CategoryList = BizAccess.GetAllProductCategory((int)WebsiteEnum.OZ);
            ViewBag.ProductList = BizAccess.GetRecommandProduct((int)WebsiteEnum.OZ);
            ViewBag.WebsiteId = (int)WebsiteEnum.OZ;
            return View();
        }

        public ActionResult MainHealthSite()
        {
            ViewBag.CategoryList = BizAccess.GetAllProductCategory((int)WebsiteEnum.NTSA);
            ViewBag.ProductList = BizAccess.GetRecommandProduct((int)WebsiteEnum.NTSA);
            ViewBag.WebsiteId = (int)WebsiteEnum.NTSA;
            return View();
        }

        public ActionResult ContractUs()
        {
            return View();
        }

        public ActionResult BrandStory()
        {
            return View();
        }

        public ActionResult HelpCenter()
        {
            return View();
        }

        public ActionResult Search()
        {
            return View();
        }

        public ActionResult Footer()
        {
            return PartialView();
        }


        public ActionResult MainHeader()
        {
            return PartialView();
        }

        public ActionResult MainMilkHeader()
        {
            return PartialView();
        }

        public ActionResult MainHealthHeader()
        {
            return PartialView();
        }

        public ActionResult MainBrandHeader()
        {
            return PartialView();
        }

        public ActionResult HealthBrandStory()
        {
            return View();
        }

        public ActionResult MilkBrandStory()
        {
            return View();
        }


        public ActionResult MainBrandStory()
        {
            return View();
        }


        public ActionResult ProductionMonitor()
        {
            return View();
        }

    }
}
