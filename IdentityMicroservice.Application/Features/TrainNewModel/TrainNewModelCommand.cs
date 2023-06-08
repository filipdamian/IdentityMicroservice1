using IdentityMicroservice.Application.Common.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityMicroservice.Application.Features.TrainNewModel
{
    public class TrainNewModelCommand : IRequest<string>
    {
    }

    public class ImageLabelPredictionCommandHandler : IRequestHandler<TrainNewModelCommand, string>
    {
        private readonly IDeepNeuralNetworkModel _deepNeuralNetworkModel;
        public ImageLabelPredictionCommandHandler(IDeepNeuralNetworkModel deepNeuralNetworkModel)
        {
            _deepNeuralNetworkModel = deepNeuralNetworkModel;
        }

        public async Task<string> Handle(TrainNewModelCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _deepNeuralNetworkModel.LoadImagesInMemory();
                return "Success";
            }
            catch (System.Exception)
            {
                return "Failed";
            }
        }
    }
}
