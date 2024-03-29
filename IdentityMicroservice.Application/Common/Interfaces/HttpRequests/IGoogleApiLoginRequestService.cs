﻿using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityMicroservice.Application.Common.Interfaces.HttpRequests
{
    public interface IGoogleApiLoginRequestService
    {
        Task<HttpResponseMessage> GetAsync(string accessToken, CancellationToken cancellationToken);
    }
}
