using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityMicroservice.Application.Common.Interfaces
{
    public interface IDeepNeuralNetworkModel
    {
        public void MakePredictionsViaTrainedModel(string photoPath, byte[] byteFile);
    }
}
