using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityMicroservice.Application.ViewModels.AppInternal
{
    public class LinkedInFullProfileDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProfilePicture { get; set; }
        public string Summary { get; set; }
        public string CurrentEmployer { get; set; }

        public List<LinkedInExperienceDTO> Experiences { get; set; }
    }
}
