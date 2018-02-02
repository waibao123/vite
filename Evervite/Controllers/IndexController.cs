using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BizLayer;
using EntityLayer.Enums;
using EntityLayer.DbEntity;
using Framework;

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
            int webId = (int)WebsiteEnum.EnerVite;
            List<Product> list = BizAccess.GetStarProduct(webId);
            list.ForEach(item => item.GalleryImages = ImageHelper.GetPics(Server.MapPath("~/Content/ProductImage"), webId, item.Name, PickPicMode.FirstGallery));

            ViewBag.CategoryList = BizAccess.GetAllProductCategory(webId);
            ViewBag.ProductList = list;
            ViewBag.WebsiteId = webId;
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

        public ActionResult Advantage(string p)
        {
            ViewBag.PageType = FormatTools.ParseInt(p);
            return View();
        }

        public ActionResult ContactUs(string p)
        {
            ViewBag.PageType = FormatTools.ParseInt(p);
            return View();
        }

        public ActionResult MainMilkSite()
        {
            int webId = (int)WebsiteEnum.OZ;
            List<Product> list = BizAccess.GetStarProduct(webId);
            list.ForEach(item => item.GalleryImages = ImageHelper.GetPics(Server.MapPath("~/Content/ProductImage"), webId, item.Name, PickPicMode.FirstGallery));

            ViewBag.CategoryList = BizAccess.GetAllProductCategory(webId);
            ViewBag.ProductList = list;
            ViewBag.WebsiteId = webId;
            return View();
        }

        public ActionResult MainHealthSite()
        {
            int webId = (int)WebsiteEnum.NTSA;
            List<Product> list = BizAccess.GetStarProduct(webId);
            list.ForEach(item => item.GalleryImages = ImageHelper.GetPics(Server.MapPath("~/Content/ProductImage"), webId, item.Name, PickPicMode.FirstGallery));

            ViewBag.CategoryList = BizAccess.GetAllProductCategory(webId);
            ViewBag.ProductList = list;
            ViewBag.WebsiteId = webId;
            return View();
        }

        public ActionResult ContractUs(string loadCnt, string title, string name, string email, string telNo, string content)
        {
            ViewBag.Cnt = "F";
            if (string.IsNullOrWhiteSpace(loadCnt))
                return View();

            string msg = null;
            bool suc = false;
            if (FormatTools.IsAnyNullOrWhiteSpace(title, name, email, telNo, content))
                msg = "请完整填写信息后再进行提交";
            else
            {
                if (suc = EmailHelper.SendEmail(title, name, email, telNo, content))
                    msg = "提交成功，我们将尽快与您联系";
                else
                    msg = "提交失败，请稍后重试";
            }
            if (!suc)
            {
                ViewBag.Title = title;
                ViewBag.Name = name;
                ViewBag.Email = email;
                ViewBag.TelNo = telNo;
                ViewBag.Content = content;
            }
            ViewBag.Msg = msg;
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

        public ActionResult GroupIntroduce()
        {
            return PartialView();
        }

        public ActionResult ProduceControl()
        {
            return PartialView();
        }

        public ActionResult CompanyCultrue()
        {
            return PartialView();
        }

        public ActionResult QualificationCertification()
        {
            return PartialView();
        }

        public ActionResult ProductionQualityControl()
        {
            return PartialView();
        }

        public ActionResult SocialEvents()
        {
            return PartialView();
        }

        public ActionResult InvestmentPromotionAgency()
        {
            return PartialView();
        }

        public ActionResult CompanyNews()
        {
            return PartialView();
        }

        public ActionResult Recruit()
        {
            return PartialView();
        }

        public ActionResult Job()
        {
            return PartialView();
        }

        public ActionResult FindUs(string loadCnt, string title, string name, string email, string telNo, string content)
        {
            ViewBag.Cnt = "F";
            if (string.IsNullOrWhiteSpace(loadCnt))
                return View();

            string msg = null;
            bool suc = false;
            if (FormatTools.IsAnyNullOrWhiteSpace(title, name, email, telNo, content))
                msg = "请完整填写信息后再进行提交";
            else
            {
                if (suc = EmailHelper.SendEmail(title, name, email, telNo, content))
                    msg = "提交成功，我们将尽快与您联系";
                else
                    msg = "提交失败，请稍后重试";
            }
            if (!suc)
            {
                ViewBag.Title = title;
                ViewBag.Name = name;
                ViewBag.Email = email;
                ViewBag.TelNo = telNo;
                ViewBag.Content = content;
            }
            ViewBag.Msg = msg;
            return PartialView();
        }

        public ActionResult CompanyIntroduce()
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
