using IdentityMicroservice.Application.Common.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityMicroservice.Application.Features.ImageLabelPrediction
{
    public class ImageLabelPredictionCommand : IRequest<string>
    {
        public IFormFile File { get; set; }
    }
    internal class ImageLabelPredictionCommandHandler : IRequestHandler<ImageLabelPredictionCommand, string>
    {
        private readonly IDeepNeuralNetworkModel _deepNeuralNetworkModel;

        public ImageLabelPredictionCommandHandler(IDeepNeuralNetworkModel deepNeuralNetworkModel)
        {
            _deepNeuralNetworkModel = deepNeuralNetworkModel;
        }

        public Task<string> Handle(ImageLabelPredictionCommand request, CancellationToken cancellationToken)
        {
            if (request.File.Length > 0)
            {
                string path = @"C:\Users\filip\source\repos\IdentityMicroservice1\IdentityMicroservice1\uploadedPhotos";

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string filePath = path + "\\" + request.File.FileName;

                using (FileStream fileStream = System.IO.File.Create(filePath))
                {
                    request.File.CopyTo(fileStream);
                    fileStream.Flush();
                    //upload finished
                }

                var memStream = new MemoryStream();
                request.File.CopyTo(memStream);
                var byteFile = memStream.ToArray();
                _deepNeuralNetworkModel.MakePredictionsViaTrainedModel(filePath,byteFile);
            }
            else
            {
                //Not uploaded
            }



            return null;
        }
    }

}
