using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Owin;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Microsoft.Owin.Security.Notifications;
using System.Threading.Tasks;
//using Microsoft.Owin.Security.Notifications;
[assembly: OwinStartup(typeof(DemoAppService.Startup))]
namespace DemoAppService
{
    public class Startup
    {
        string clientId = "8a055c3d-c973-4d2b-b148-aa85c52d78bb";
        string redirectUri = "https://localhost:44321/";
        static string tenant = "https://EPAM.onmicrosoft.com/ec5762fa-d50c-4125-abe4-02884eb433b2";
        static string suthoritytemp = string.Empty;
        string authority = $"https://EPAM.onmicrosoft.com/ec5762fa-d50c-4125-abe4-02884eb433b2";

        

        /// <summary>
        /// Configure OWIN to use OpenIdConnect 
        /// </summary>
        /// <param name="app"></param>
        public void Configuration(IAppBuilder app)
        {
            app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);
            app.UseCookieAuthentication(new CookieAuthenticationOptions());
            app.UseOpenIdConnectAuthentication(new OpenIdConnectAuthenticationOptions() {
                // Sets the ClientId, authority, RedirectUri as obtained from web.config
                ClientId = clientId,
                Authority = authority,
                RedirectUri = redirectUri,
                // PostLogoutRedirectUri is the page that users will be redirected to after sign-out. In this case, it is using the home page
                PostLogoutRedirectUri = redirectUri,
                Scope = OpenIdConnectScope.OpenIdProfile,
                ResponseType = OpenIdConnectResponseType.IdToken,
                // ValidateIssuer set to false to allow personal and work accounts from any organization to sign in to your application
                // To only allow users from a single organizations, set ValidateIssuer to true and 'tenant' setting in web.config to the tenant name
                // To allow users from only a list of specific organizations, set ValidateIssuer to true and use ValidIssuers parameter 
                TokenValidationParameters = new TokenValidationParameters(){
                    ValidateIssuer=true
                },
                Notifications = new OpenIdConnectAuthenticationNotifications(){
                    AuthenticationFailed = OnAuthenticationFailed
                }
            });
        }

        private Task OnAuthenticationFailed(AuthenticationFailedNotification<OpenIdConnectMessage, OpenIdConnectAuthenticationOptions> arg)
        {
            arg.HandleResponse();
            arg.Response.Redirect("/?errormessage"+arg.Exception.Message);
            return Task.FromResult(0);
        }
    }
}