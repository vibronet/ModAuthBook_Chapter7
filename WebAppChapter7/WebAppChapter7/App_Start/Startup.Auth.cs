using Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Owin.Security.Notifications;
using Microsoft.IdentityModel.Protocols;

namespace WebAppChapter7
{
    public partial class Startup
    {
        public void ConfigureAuth(IAppBuilder app)
        {
            app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);

            // Uncomment the below and the exception throwing line later on to see the diagnostic middleware in action
            //app.UseErrorPage(new ErrorPageOptions()
            //{
            //    ShowCookies = true,
            //    ShowEnvironment = true,
            //    ShowQuery = true,
            //    ShowExceptionDetails = true,
            //    ShowHeaders = true,
            //    ShowSourceCode = true,
            //    SourceCodeLineCount = 10
            //});

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
                PostLogoutRedirectUri = "https://localhost:44300/",
                // uncomment the below if you want to decouple the validity window of the session (e.g. session cookie) from the validity of the incoming token
                // UseTokenLifetime = false,
                Notifications = new OpenIdConnectAuthenticationNotifications()
                {
                    RedirectToIdentityProvider = (context) =>
                    {
                        Debug.WriteLine("*** RedirectToIdentityProvider");
                        // example of functionality you can implement with RedirectToIdentityProvider:
                        // dynamic assignment of RedirectUri and PostLogoutRedirectUri from the request
                        //string appBaseUrl = context.Request.Scheme + "://" + context.Request.Host + context.Request.PathBase;
                        //context.ProtocolMessage.RedirectUri = appBaseUrl + "/";
                        //context.ProtocolMessage.PostLogoutRedirectUri = appBaseUrl;

                        // Another example: manipulation of the authentication experience by changing OpenID Connect parameters
                        // context.ProtocolMessage.Prompt = "login";
                        return Task.FromResult(0);
                    },
                    // an alternative to inlining the notification implementation: 
                    // RedirectToIdentityProvider = Startup.RedirectToIdentityProvider,
                    MessageReceived = (context) =>
                    {
                        Debug.WriteLine("*** MessageReceived");
                        return Task.FromResult(0);
                    },
                    SecurityTokenReceived = (context) =>
                    {
                        Debug.WriteLine("*** SecurityTokenReceived");
                        return Task.FromResult(0);
                    },
                    SecurityTokenValidated = (context) =>
                    {
                        Debug.WriteLine("*** SecurityTokenValidated");
                        // example of functionality you can implement with SecurityTokenValidated:
                        // this is code you might write for checking if the incoming token represents a user present in a local DB
                        //string userID = context.AuthenticationTicket.Identity.FindFirst(ClaimTypes.NameIdentifier).Value;
                        //if (db.Users.FirstOrDefault(b => (b.UserID == userID)) == null)
                        //    throw new System.IdentityModel.Tokens.SecurityTokenValidationException();

                        //another example: you can use SecurityTokenValidated to add custom claims
                        //string userID = context.AuthenticationTicket.Identity.FindFirst(ClaimTypes.NameIdentifier).Value;
                        //Claim userHair = new Claim("http://mycustomclaims/hairlength", RetrieveHairLength(userID), ClaimValueTypes.Double, "LocalAuthority");
                        //context.AuthenticationTicket.Identity.AddClaim(userHair);
                        return Task.FromResult(0);
                    },
                    AuthorizationCodeReceived = (context) =>
                    {
                        Debug.WriteLine("*** AuthorizationCodeReceived");
                        return Task.FromResult(0);
                    },
                    AuthenticationFailed = (context) =>
                    {
                        Debug.WriteLine("*** AuthenticationFailed");
                        //context.OwinContext.Response.Redirect("/Home/Error");
                        //context.HandleResponse();
                        return Task.FromResult(0);
                    },
                },
                // example of explicit assignment of TokenValidationParameters
                // in this case, the code mimics what you'd do for taking control of the issuer validation logic
                //TokenValidationParameters = new System.IdentityModel.Tokens.TokenValidationParameters
                //{
                //    ValidateIssuer = false,
                //    IssuerValidator = (issuer, token, tvp) =>
                //    {
                //        //if(db.Issuers.FirstOrDefault(b => (b.Issuer == issuer)) == null)
                //        return issuer;
                //        //else
                //        //    throw new SecurityTokenInvalidIssuerException("Invalid issuer");
                //    }
                //}
            }
            );
            app.Use(async (Context, next) =>
            {
                Debug.WriteLine("3 ==>after OIDC, before leaving the pipeline");
                // Uncomment together with the diagnostic middleware earlier in the pipeline to see the diagnostic features in action
                // throw new System.Exception("Shenanigans!");
                await next.Invoke();
                Debug.WriteLine("4 <==after entering the pipeline, before OIDC");
            });

        }
        // an example of explicit declaration of a notification
        //public static Task RedirectToIdentityProvider(RedirectToIdentityProviderNotification<OpenIdConnectMessage, OpenIdConnectAuthenticationOptions> notification)
        //{
        //    notification.ProtocolMessage.Prompt = "login";
        //    return Task.FromResult(0);
        //}
    }

}