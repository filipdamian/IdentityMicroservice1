using IdentityMicroservice.Application.ViewModels.AppInternal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityMicroservice.Application.Common.Interfaces
{
    public interface IWebScraperManager
    {
        public Task<LinkedInFullProfileDTO> ScrapeLinkedInUser(string url);
    }
}
