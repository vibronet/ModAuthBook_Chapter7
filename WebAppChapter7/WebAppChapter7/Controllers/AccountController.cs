using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebAppChapter7.Controllers
{
    public class AccountController : Controller
    {
        public void SignIn()
        {
            // Send an OpenID Connect sign-in request.
            if (!Request.IsAuthenticated)
            {
                HttpContext.GetOwinContext().Authentication.Challenge(new AuthenticationProperties { RedirectUri = "/" },
                OpenIdConnectAuthenticationDefaults.AuthenticationType);
            }
        }
        public void SignOut()
        {
            HttpContext.GetOwinContext().Authentication.SignOut(
            OpenIdConnectAuthenticationDefaults.AuthenticationType,
            CookieAuthenticationDefaults.AuthenticationType);
        }

        // if you want to targed ADFS (from ADFS 2.x onward), assuming you already updated Startup.Auth.cs 
        // 1- comment out the above declarations for SignIn and SignOut
        // 2- uncomment the ones below
        // please refer to Chapter 5 of http://amzn.to/1QS5kQK for more details.

        //public void SignIn()
        //{
        //    // Send an OpenID Connect sign-in request.
        //    if (!Request.IsAuthenticated)
        //    {
        //        HttpContext.GetOwinContext().Authentication.Challenge(new AuthenticationProperties { RedirectUri = "/" },
        //        WsFederationAuthenticationDefaults.AuthenticationType);
        //    }
        //}
        //public void SignOut()
        //{
        //    HttpContext.GetOwinContext().Authentication.SignOut(
        //    WsFederationAuthenticationDefaults.AuthenticationType,
        //    CookieAuthenticationDefaults.AuthenticationType);
        //}
    }
}