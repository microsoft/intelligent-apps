using AdatumTaxCorpKnowledgeService.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace AdatumTaxCorpKnowledgeService.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            FaqContext db = new FaqContext();
            var allFaqs = db.Faqs.ToList();
            ViewBag.FaqsList = allFaqs;
            return View();
        }
        
        [HttpPost]
        public JsonResult FindTaxAnswer(QnaMakerQuestion inquiry)
        {
            //TODO : write post method to bot service and initiliaze a webclient to get a result

            return null;
        }
    }
}