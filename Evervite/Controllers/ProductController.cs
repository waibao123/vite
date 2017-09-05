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


        public ActionResult List(int id)
        {
            ViewBag.CCC = id;
            return View();
        }

        public ActionResult Detail(string productType)
        {
            ViewBag.CCC = productType;
            return View();
        }

    }
}
