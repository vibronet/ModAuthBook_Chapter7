using Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using System.Diagnostics;

namespace WebAppChapter7
{
    public partial class Startup
    {
        public void ConfigureAuth(IAppBuilder app)
        {
            app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);
            app.Use(async (Context, next) =>
            {
                Debug.WriteLine("1 ==>request, before cookie auth");
                await next.Invoke();
                Debug.WriteLine("6 <==response, after cookie auth");
            });
            app.UseCookieAuthentication(new CookieAuthenticationOptions());
            app.Use(async (Context, next) =>
            {
                Debug.WriteLine("2 ==>after cookie, before OIDC");
                await next.Invoke();
                Debug.WriteLine("5 <==after OIDC");
            });
            app.UseOpenIdConnectAuthentication(
            new OpenIdConnectAuthenticationOptions
            {
                ClientId = "95dcbcfd-5a64-4efe-a5e3-f4ed2043c46c",
                Authority = "https://login.microsoftonline.com/DeveloperTenant.onmicrosoft.com",
                PostLogoutRedirectUri = "https://localhost:44300/"
            }
            );
            app.Use(async (Context, next) =>
            {
                Debug.WriteLine("3 ==>after OIDC, before leaving the pipeline");
                await next.Invoke();
                Debug.WriteLine("4 <==after entering the pipeline, before OIDC");
            });

        }
    }
}