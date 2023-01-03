using IdentityMicroservice.Application.Common.Interfaces;
using IdentityMicroservice.Application.ViewModels.AppInternal;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityMicroservice.Application.Features.LinkedinCrawler
{
    public class LinkedInUserProfileCommand : IRequest<LinkedInFullProfileDTO> 
    {
        public string ProfileUrl { get; set; }
    }

    internal class GetLinkedInUserProfileCommandHandler : IRequestHandler<LinkedInUserProfileCommand, LinkedInFullProfileDTO>
    {
        private readonly IWebScraperManager _webScraperManager;

        public GetLinkedInUserProfileCommandHandler(IWebScraperManager webScraperManager)
        {
            _webScraperManager = webScraperManager;
        }

        public Task<LinkedInFullProfileDTO> Handle(LinkedInUserProfileCommand request, CancellationToken cancellationToken)
        {
            var response = _webScraperManager.ScrapeLinkedInUser(request.ProfileUrl);
            return response;
            
        }
    }


}
