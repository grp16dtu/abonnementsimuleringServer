using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AbonnementsimuleringServer.ApiControllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            // API forside
            return View();
        }
    }
}
