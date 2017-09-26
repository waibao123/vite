using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BizLayer;

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
            return View();
        }

        public ActionResult MainHealthSite()
        {
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

    }
}
