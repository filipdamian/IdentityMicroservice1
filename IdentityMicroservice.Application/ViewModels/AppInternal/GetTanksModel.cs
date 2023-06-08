using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityMicroservice.Application.ViewModels.AppInternal
{
    public class GetTanksModel
    {
        public Dictionary<int,List<string>> TankModels { get; set; }

    }
}
