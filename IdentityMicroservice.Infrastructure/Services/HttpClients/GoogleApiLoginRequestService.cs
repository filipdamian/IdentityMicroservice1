using IdentityMicroservice.Application.Common.Interfaces.HttpRequests;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityMicroservice.Infrastructure.Services.HttpClients
{
    public class GoogleApiLoginRequestService : IGoogleApiLoginRequestService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        //private readonly ClientPolicy _clientPolicy;

        public GoogleApiLoginRequestService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<HttpResponseMessage> GetAsync(string accessToken, CancellationToken cancellationToken)
        {
            var url = "https://www.googleapis.com/userinfo/v2/me";

            var httpClient = _httpClientFactory.CreateClient();

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            //var response = await _clientPolicy.DipocketRetryPolicy.ExecuteAsync(() => httpClient.GetAsync(url, cancellationToken));
            var response = await httpClient.GetAsync(url, cancellationToken);

            return response;
        }
    }
}
