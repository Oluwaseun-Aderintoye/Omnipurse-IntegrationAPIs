using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IntegrationAPIs.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        public string GetRootUrl()
        {
            // Get the current request URL
            Uri currentUri = Request.Url;

            // Combine the scheme, authority, and application path
            string rootUrl = $"{currentUri.Scheme}://{currentUri.Authority}{Request.ApplicationPath}";

            // Ensure the URL ends with a trailing slash
            return rootUrl.EndsWith("/") ? rootUrl : rootUrl + "/";
        }
    }
}
