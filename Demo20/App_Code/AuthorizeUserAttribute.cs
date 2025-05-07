using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
namespace Demo20.App_Code
{
    public class AuthorizeUserAttribute:AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            bool IsValidUser;
            if (httpContext.Session["userid"]==null)
                IsValidUser = false;
            else 
                IsValidUser = true;
            return IsValidUser;
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { action = "Login", Controller = "General" }));
        }
    }
}