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
            var vvv = BllTest.GetAllOptions();
            ViewBag.CCC = string.Join("<br/>", vvv.Select(item => item.OptionValue));
            return View();
        }

    }
}
