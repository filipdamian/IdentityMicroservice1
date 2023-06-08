using IdentityMicroservice.Application.Common.Interfaces;
using Microsoft.ML;
using System.Reflection;
using System.Threading.Tasks;

namespace IdentityMicroservice.Infrastructure.Services.Cronjobs.ResumeTrainingOnModelCronJob
{
    public class ResumeTrainingOnModel : IJob
    {
        private readonly IDeepNeuralNetworkModel _deepNeuralNetworkModel;
        private readonly MLContext _mlContext = new MLContext(seed: 1);
        private readonly ITransformer _preTrainedModel;

        public ResumeTrainingOnModel(IDeepNeuralNetworkModel deepNeuralNetworkModel)
        {
            _deepNeuralNetworkModel = deepNeuralNetworkModel;
            DataViewSchema modelSchema;
            ITransformer preTrainedModel = _mlContext.Model.Load("imageClassifierTest.zip", out modelSchema);
            _preTrainedModel = preTrainedModel;
        }

        public async Task Execute()
        {
            try
            {
                // Get the paths to the saved model and the folder containing new images
                string savedModelPath = "imageClassifierTest.zip";
                string newImagesFolderPath = "C:\\Users\\filip\\source\\repos\\IdentityMicroservice1\\IdentityMicroservice1\\uploadedPhotos\\";

                var trainset = _deepNeuralNetworkModel.LoadTrainSetFromDisk();

                _deepNeuralNetworkModel.RetrainModel(_preTrainedModel, trainset, savedModelPath, newImagesFolderPath);
            }
            catch (System.Exception)
            {
                throw;
            }
          
        }

        public static string GetJobName()
        {
            return MethodBase.GetCurrentMethod().DeclaringType.Name;
        }
    }
}
