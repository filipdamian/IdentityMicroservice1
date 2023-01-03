using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityMicroservice.Application.ViewModels.AppInternal
{
    public class ImagePrediction : ImageData
    {
        public float[] Score;
        public string PredictedLabel;
    }
}
