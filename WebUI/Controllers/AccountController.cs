using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace WebUI.Controllers
{
    public class AccountController : Controller
    {
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult LogOn()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult LogOn(string username, string password, string returnUrl)
        {
            if (FormsAuthentication.Authenticate(username, password))
            {
                returnUrl = returnUrl ?? Url.Action("Index", "Admin");
                FormsAuthentication.SetAuthCookie(username, false);
                return Redirect(returnUrl);
            }
            else
            {
                ViewData["lastLoginFailed"] = true;
                return View();
            }
        }
    }
}
