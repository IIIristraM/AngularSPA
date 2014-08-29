using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Threading.Tasks;

namespace App.AudioSearcher
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Enable the application to use a cookie to store information for the signed in user
            //app.UseCookieAuthentication(new CookieAuthenticationOptions());

            // Use a cookie to temporarily store information about a user logging in with a third party login provider
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Enable the application to use bearer tokens to authenticate users
            app.UseOAuthBearerTokens(new OAuthAuthorizationServerOptions
            {
                AuthorizeEndpointPath = new PathString("/ExternalLogin"),
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(1),
                AllowInsecureHttp = true,
                Provider = new OAuthAuthorizationServerProvider
                {
                    OnValidateClientRedirectUri = (context) =>
                    {
                        if (context.ClientId == "self")
                        {
                            Uri expectedRootUri = new Uri(context.Request.Uri, "/");

                            if (expectedRootUri.AbsoluteUri == context.RedirectUri)
                            {
                                context.Validated();
                            }
                        }

                        return Task.FromResult<object>(null);
                    }
                }
            });

            //// Uncomment the following lines to enable logging in with third party login providers
            app.UseMicrosoftAccountAuthentication(
                clientId: "00000000481270CE",
                clientSecret: "PAHnw5LH1kpLDWaItJ7FjnIWXWf-B4jN");

            ////app.UseTwitterAuthentication(
            ////   consumerKey: "",
            ////   consumerSecret: "");

            ////app.UseFacebookAuthentication(
            ////   appId: "",
            ////   appSecret: "");

            app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions { 
                ClientId = "219854735390-99mu0vd6kloc2e93e6lqjqvti0e2rmp6.apps.googleusercontent.com",
                ClientSecret = "nBKY3EcNSNVkiWoJZTOyG2Oh"
            });
        }
    }
}
