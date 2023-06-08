using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Transforms;

namespace IdentityMicroservice.Application.Common.Interfaces
{
    public interface IDeepNeuralNetworkModel
    {
        public (EstimatorChain<KeyToValueMappingTransformer>, IDataView) LoadImagesInMemory();
        public string MakePredictionsViaTrainedModel(string photoPath, byte[] byteFile);
        //public void RetrainModel(EstimatorChain<KeyToValueMappingTransformer> trainingPipeline, IDataView trainSet, string savedModelPath, string newImagesFolderPath);
        public void RetrainModel(ITransformer preTrainedModel, IDataView trainSet, string savedModelPath, string newImagesFolderPath);
        public IDataView LoadTrainSetFromDisk();
    }
}
