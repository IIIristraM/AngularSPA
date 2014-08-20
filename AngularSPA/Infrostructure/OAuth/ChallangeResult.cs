using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace App.AudioSearcher
{
    public class ChallengeResult : IHttpActionResult
    {
        private readonly string _loginProvider;
        private readonly HttpRequestMessage _request;

        public ChallengeResult(string loginProvider, ApiController controller)
        {
            _loginProvider = loginProvider;
            _request = controller.Request;
        }    

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            _request.GetOwinContext().Authentication.Challenge(_loginProvider);

            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
            response.RequestMessage = _request;
            return Task.FromResult(response);
        }
    }
}