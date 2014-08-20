using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Domain.Entities;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using Domain.Contracts;
using Domain.Implementation;
using Microsoft.Owin.Security;
using Services.Contracts;
using System.Security.Claims;
using Microsoft.Owin.Security.OAuth;
using Microsoft.Owin.Security.Cookies;

namespace App.AudioSearcher.Api
{
    [Authorize]
    [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
    public class AccountController : ApiController
    {
        private readonly ITestService _testService;

        public AccountController(/*IUnitOfWork unitOfWork, IUserManagerFactory userManagerFactory,*/ ITestService testService)
            //: base(unitOfWork, userManagerFactory)
        {
            _testService = testService;
        }

        [HttpGet]
        public ExternalLoginData UserInfo()
        {
            var externalLoginData = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

            if (externalLoginData == null)
            {
                throw new InvalidOperationException("Error in OAuth configuration");
            }

            return externalLoginData;
        }

        [AllowAnonymous]
        [HttpGet]
        public IEnumerable<ExternalLoginUrl> ExternalLoginUrls()
        {
            IEnumerable<AuthenticationDescription> descriptions = Authentication.GetExternalAuthenticationTypes();
            List<ExternalLoginUrl> loginUrls = new List<ExternalLoginUrl>();

            string state = RandomOAuthStateGenerator.Generate(256);
            
            foreach (AuthenticationDescription description in descriptions)
            {
                var loginUrl = new ExternalLoginUrl
                {
                    ProviderName = description.Caption,
                    Url = Url.Route("ExternalLogin", new
                    {
                        provider = description.AuthenticationType,
                        response_type = "token",
                        client_id = "self",
                        redirect_uri = new Uri(Request.RequestUri, "/").AbsoluteUri,
                        state = state
                    }),
                    State = state
                };
                loginUrls.Add(loginUrl);
            }

            return loginUrls;
        }

        [OverrideAuthentication]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalCookie)]
        [AllowAnonymous]
        [HttpGet]
        [Route("ExternalLogin", Name = "ExternalLogin")]
        public IHttpActionResult ExternalLogin(string provider)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return new ChallengeResult(provider, this);
            }

            var externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

            if (externalLogin == null)
            {
                return InternalServerError();
            }

            if (externalLogin.LoginProvider != provider)
            {
                Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                return new ChallengeResult(provider, this);
            }

            IEnumerable<Claim> claims = externalLogin.GetClaims();
            ClaimsIdentity identity = new ClaimsIdentity(claims, OAuthDefaults.AuthenticationType);
            Authentication.SignIn(identity);

            return Ok();
        }

        [HttpGet]
        public IHttpActionResult Logout()
        {
            // TODO implemet some logic
            return Ok();
        }

        private IAuthenticationManager Authentication
        {
            get { return Request.GetOwinContext().Authentication; }
        }
    }
}