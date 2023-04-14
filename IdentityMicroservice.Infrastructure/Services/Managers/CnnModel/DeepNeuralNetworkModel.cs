using IdentityMicroservice.Application.Common.Interfaces;
using IdentityMicroservice.Application.ViewModels.AppInternal;
using Microsoft.ML;
using Microsoft.ML.Transforms;
using Microsoft.ML.Vision;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.ML.Vision.ImageClassificationTrainer;

namespace IdentityMicroservice.Infrastructure.Services.Managers.CnnModel
{
    public class DeepNeuralNetworkModel : IDeepNeuralNetworkModel
    {
        private readonly MLContext _mlContext = new MLContext(seed: 1);
        private readonly string Path = @"C:\Users\filip\OneDrive\Desktop\freshwater_fish_dataset";
        private readonly string AllFishInOnePlacePath = @"C:\Users\filip\OneDrive\Desktop\all_freshwater_fish";


        private List<ImageData> GetImgs(string Path)
        {
            List<ImageData> imageDataList = new List<ImageData>();
            var directories = Directory.GetDirectories(Path);
            int nr = 1000;
            foreach (var d in directories)
            {
                var files = Directory.GetFiles(d).Select(x => new ImageData
                {
                    Label = d.Split("\\").Last(), //image name 
                    ImagePath = x.Split("\\").Last() // fish name ( directory name )
                });

                foreach (var f in files)
                {
                    // System.IO.File.Move(Path+"\\"+f.Label+"\\"+f.ImagePath, Path + "\\" + f.Label + "\\"+ nr.ToString()+".jpg");
                    imageDataList.Add(f);
                    nr++;
                }

            }
            return imageDataList;

        }
        //whole model logic here ---> TO DO: break specific logic parts into separate functions 
        public void LoadImagesInMemory()
        {
            var imgs = GetImgs(Path);
            var dataView = _mlContext.Data.LoadFromEnumerable(imgs);
            dataView = _mlContext.Data.ShuffleRows(dataView);

            IDataView shuffledFullImagesDataset = _mlContext.Transforms.Conversion.MapValueToKey(outputColumnName: "LabelAsKey",
                                         inputColumnName: "Label",
                                         keyOrdinality: ValueToKeyMappingEstimator.KeyOrdinality.ByValue)
                                         .Append(_mlContext.Transforms.LoadRawImageBytes(outputColumnName: "Image",
                                                                     imageFolder: AllFishInOnePlacePath,
                                                                     inputColumnName: "ImagePath"))
                                         .Fit(dataView)
                                         .Transform(dataView);

            var trainTestSplit = _mlContext.Data.TrainTestSplit(shuffledFullImagesDataset, testFraction: 0.2, seed: 2);

            var testSet = trainTestSplit.TestSet;
            var trainSet = trainTestSplit.TrainSet;

            var options = new ImageClassificationTrainer.Options()
            {
                FeatureColumnName = "Image",
                LabelColumnName = "LabelAsKey",
                Arch = ImageClassificationTrainer.Architecture.ResnetV250, //check other architectures
                Epoch = 30,
                BatchSize = 10,
                LearningRate = 0.01f,
                MetricsCallback = (metrics) => Console.WriteLine(metrics),
                ValidationSet = testSet,
                TestOnTrainSet = false
                // by default early stopping is turned on and set to monitor accuracy
            };

            var trainingPipeline = _mlContext.MulticlassClassification.Trainers.ImageClassification(options)
                    .Append(_mlContext.Transforms.Conversion.MapKeyToValue(
                        outputColumnName: "PredictedLabel",
                        inputColumnName: "PredictedLabel"));

            ITransformer model = trainingPipeline.Fit(trainSet);

            var predicitions = model.Transform(testSet);
            var metrics = _mlContext.MulticlassClassification.Evaluate(predicitions, labelColumnName: "LabelAsKey", predictedLabelColumnName: "PredictedLabel");

            Console.WriteLine($"Accuracy: {metrics.MacroAccuracy:P2}");

            _mlContext.Model.Save(model, trainSet.Schema, "imageClassifier.zip");
            var imageData = new ImageData()
            {
                ImagePath = @"C:\Users\filip\source\repos\IdentityMicroservice1\IdentityMicroservice1\uploadedPhotos\20221221_140759.jpg"
            };

            var predictor = _mlContext.Model.CreatePredictionEngine<ImageData, ImagePrediction>(model);
            var prediction = predictor.Predict(imageData);

        }

        public void MakePredictionsViaTrainedModel(string photoPath, byte[] byteFile)
        {
            //path of new photo 
            var imageData = new ImageData()
            {
                ImagePath = photoPath,
                    Image = byteFile
            };

            // Load Trained Model
            DataViewSchema predictionPipelineSchema;
            ITransformer predictionPipeline = _mlContext.Model.Load("imageClassifier.zip", out predictionPipelineSchema);

            // Create PredictionEngines
            // Make prediction function (input = ImageData, output = ImagePrediction)
            var predictor = _mlContext.Model.CreatePredictionEngine<ImageData, ImagePrediction>(predictionPipeline);
            var prediction = predictor.Predict(imageData);
            Console.WriteLine($"Image: {(imageData.ImagePath)} predicted as: {prediction.PredictedLabel} with score: {prediction.Score.Max()} ");
        }

    }
}
