using IdentityMicroservice.Application.Common.Interfaces.HttpRequests;
using IdentityMicroservice.Application.ViewModels.External.Auth;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityMicroservice.Infrastructure.Services.HttpClients
{
    public class FacebookApiLoginRequestService : IFacebookApiLoginRequestService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public FacebookApiLoginRequestService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<HttpResponseMessage> GetAsync(string accessToken, CancellationToken cancellationToken)
        {
            var url = "https://graph.facebook.com/me";

            var httpClient = _httpClientFactory.CreateClient();

            var queryParams = new Dictionary<string, string>
            {
                { "fields", "id,name,email" },
                { "access_token", accessToken }
            };

            var urlBuilder = new StringBuilder(url);
            if (queryParams.Count > 0)
            {
                urlBuilder.Append("?");
                foreach (var param in queryParams)
                {
                    urlBuilder.Append($"{param.Key}={Uri.EscapeDataString(param.Value)}&");
                }
                urlBuilder.Length--; // Remove the trailing '&'
            }

            var requestUrl = urlBuilder.ToString();

            var response = await httpClient.GetAsync(requestUrl, cancellationToken);

            return response;
        }
    }
}
