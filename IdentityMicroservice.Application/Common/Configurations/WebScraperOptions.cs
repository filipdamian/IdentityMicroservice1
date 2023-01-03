using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityMicroservice.Application.Common.Configurations
{
    public class WebScraperOptions
    {
        public const string NAME = "WebScraperOptions";
        public ChromeDriverOptions ChromeDriverOptions { get; set; }
        public LinkedInCredentialsOptions LinkedInCredentialsOptions { get; set; }
    }

    public class ChromeDriverOptions
    {
        public string LocalPath { get; set; }
    }

    public class LinkedInCredentialsOptions
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

}
