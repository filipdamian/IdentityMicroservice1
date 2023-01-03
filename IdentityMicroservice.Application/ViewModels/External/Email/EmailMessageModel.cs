using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityMicroservice.Application.ViewModels.External.Email
{
    public class EmailMessageModel
    {
        public string Email { get; set; }
        public string Subject { get; set; }

        public string Content { get; set; }
    }

}
