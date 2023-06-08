using IdentityMicroservice.Application.Common.Interfaces;
using IdentityMicroservice.Application.ViewModels.AppInternal;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Transforms;
using Microsoft.ML.Vision;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static Microsoft.ML.Vision.ImageClassificationTrainer;

namespace IdentityMicroservice.Infrastructure.Services.Managers.CnnModel
{
    public class DeepNeuralNetworkModel : IDeepNeuralNetworkModel
    {
        private readonly MLContext _mlContext = new MLContext(seed: 1);
        private readonly string FreshwaterFishPathWithLabels = @"C:\Users\filip\OneDrive\Desktop\freshwater_fish_dataset";
        private readonly string AllFishInOnePlacePath = @"C:\Users\filip\OneDrive\Desktop\all_freshwater_fish";


        private List<ImageData> GetImgs(string Path1)
        {
            List<ImageData> imageDataList = new List<ImageData>();
            var directories = Directory.GetDirectories(FreshwaterFishPathWithLabels).ToList();

            directories = OrderDirectories(directories);

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
                    System.IO.File.Move(FreshwaterFishPathWithLabels + "\\" + f.Label + "\\" + f.ImagePath, FreshwaterFishPathWithLabels + "\\" + f.Label + "\\" + nr.ToString() + ".jpg");
                    imageDataList.Add(f);
                    nr++;
                }

            }

            CopyImagesToGeneralFolder(imageDataList);

            return imageDataList;
        }

        private void CopyImagesToGeneralFolder(List<ImageData> imageDataList)
        {
            if (!Directory.Exists(AllFishInOnePlacePath))
            {
                Directory.CreateDirectory(AllFishInOnePlacePath);
            }

            foreach (var imageData in imageDataList)
            {
                string fileName = imageData.Label + "\\" + imageData.ImagePath;
                string sourcePath = Path.Combine(FreshwaterFishPathWithLabels, fileName);
                string destinationPath = Path.Combine(AllFishInOnePlacePath, imageData.ImagePath);

                File.Copy(sourcePath, destinationPath, overwrite: true);
            }
        }

        private List<string> OrderDirectories(List<string> directories)
        {
            List<string> sortedDirectories;
            return sortedDirectories = directories.OrderBy(dir =>
            {
                string directoryName = Path.GetFileName(dir);
                Match match = Regex.Match(directoryName, @"\d+");
                if (match.Success)
                {
                    return int.Parse(match.Value);
                }
                else
                {
                    return int.MaxValue; // This will place directories without numbers at the end of the sorted list
                }
            }).ToList();
        }

        //whole model logic here ---> TO DO: break specific logic parts into separate functions 
        //public (EstimatorChain<KeyToValueMappingTransformer>, IDataView) LoadImagesInMemory()
        //{
        //    var imgs = GetImgs(FreshwaterFishPathWithLabels);
        //    var dataView = _mlContext.Data.LoadFromEnumerable(imgs);
        //    dataView = _mlContext.Data.ShuffleRows(dataView);

        //    IDataView shuffledFullImagesDataset = _mlContext.Transforms.Conversion.MapValueToKey(outputColumnName: "LabelAsKey",
        //                                 inputColumnName: "Label",
        //                                 keyOrdinality: ValueToKeyMappingEstimator.KeyOrdinality.ByValue)
        //                                 .Append(_mlContext.Transforms.LoadRawImageBytes(outputColumnName: "Image",
        //                                                             imageFolder: AllFishInOnePlacePath,
        //                                                             inputColumnName: "ImagePath"))
        //                                 .Fit(dataView)
        //                                 .Transform(dataView);

        //    var trainTestSplit = _mlContext.Data.TrainTestSplit(shuffledFullImagesDataset, testFraction: 0.2, seed: 2);

        //    var testSet = trainTestSplit.TestSet;
        //    var trainSet = trainTestSplit.TrainSet;

        //    // Save the trainSet to disk
        //    using (var stream = new FileStream("trainSet.tsv", FileMode.Create))
        //    {
        //        _mlContext.Data.SaveAsText(trainSet, stream, separatorChar: '\t', headerRow: true, schema: true);
        //    }

        //    var options = new ImageClassificationTrainer.Options()
        //    {
        //        FeatureColumnName = "Image",
        //        LabelColumnName = "LabelAsKey",
        //        Arch = ImageClassificationTrainer.Architecture.ResnetV250, //check other architectures
        //        Epoch = 30,
        //        BatchSize = 10,
        //        LearningRate = 0.01f,
        //        MetricsCallback = (metrics) => Console.WriteLine(metrics),
        //        ValidationSet = testSet,
        //        TestOnTrainSet = false
        //        // by default early stopping is turned on and set to monitor accuracy
        //    };

        //    var trainingPipeline = _mlContext.MulticlassClassification.Trainers.ImageClassification(options)
        //            .Append(_mlContext.Transforms.Conversion.MapKeyToValue(
        //                outputColumnName: "PredictedLabel",
        //                inputColumnName: "PredictedLabel"));

        //    ITransformer model = trainingPipeline.Fit(trainSet);

        //    var predicitions = model.Transform(testSet);
        //    var metrics = _mlContext.MulticlassClassification.Evaluate(predicitions, labelColumnName: "LabelAsKey", predictedLabelColumnName: "PredictedLabel");

        //    Console.WriteLine($"Accuracy: {metrics.MacroAccuracy:P2}");

        //    _mlContext.Model.Save(model, trainSet.Schema, "imageClassifierTest.zip");
        //    var imageData = new ImageData()
        //    {
        //        ImagePath = @"C:\Users\filip\source\repos\IdentityMicroservice1\IdentityMicroservice1\uploadedPhotos\20221221_140759.jpg"
        //    };

        //    var predictor = _mlContext.Model.CreatePredictionEngine<ImageData, ImagePrediction>(model);
        //    var prediction = predictor.Predict(imageData);

        //    return (trainingPipeline, trainSet);

        //}


        public (EstimatorChain<KeyToValueMappingTransformer>, IDataView) LoadImagesInMemory()
        {
            var imgs = GetImgs(FreshwaterFishPathWithLabels);
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
            var trainValidationSplit = _mlContext.Data.TrainTestSplit(trainTestSplit.TrainSet, testFraction: 0.2, seed: 2);

            var testSet = trainTestSplit.TestSet;
            var trainSet = trainValidationSplit.TrainSet;
            var validationSet = trainValidationSplit.TestSet; // This will be the new validation set

            // Save the trainSet to disk
            using (var stream = new FileStream("trainSet.tsv", FileMode.Create))
            {
                _mlContext.Data.SaveAsText(trainSet, stream, separatorChar: '\t', headerRow: true, schema: true);
            }

            var accMetric = new List<double>();
            var lossMetric = new List<double>();
            int nrEpochs = 0;

            var options = new Options()
            {
                FeatureColumnName = "Image",
                LabelColumnName = "LabelAsKey",
                Arch = ImageClassificationTrainer.Architecture.ResnetV250, //check other architectures
                Epoch = 40,
                BatchSize = 10,
                LearningRate = 0.01f,
                MetricsCallback = (metrics) => { Console.WriteLine(metrics); if (metrics.Train != null) { accMetric.Add(metrics.Train.Accuracy); lossMetric.Add(metrics.Train.CrossEntropy); nrEpochs++; }; },
                ValidationSet = validationSet,
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

            _mlContext.Model.Save(model, trainSet.Schema, "imageClassifierTest.zip");
            var imageData = new ImageData()
            {
                ImagePath = @"C:\Users\filip\source\repos\IdentityMicroservice1\IdentityMicroservice1\uploadedPhotos\20221221_140759.jpg"
            };

            var predictor = _mlContext.Model.CreatePredictionEngine<ImageData, ImagePrediction>(model);
            var prediction = predictor.Predict(imageData);


            double[] epochs = Enumerable.Range(1, nrEpochs).Select(x => (double)x).ToArray();

            var plt = new ScottPlot.Plot(600, 400);

            plt.PlotScatter(epochs, accMetric.ToArray(), label: "Accuracy", lineWidth: 2);
            plt.PlotScatter(epochs, lossMetric.ToArray(), label: "Loss", lineWidth: 2);
            plt.Title("Model Training");
            plt.XLabel("Epochs");
            plt.YLabel("Value");
            plt.Legend();

            plt.SaveFig("output5.png");


            return (trainingPipeline, trainSet);
        }



        public IDataView LoadTrainSetFromDisk()
        {
            // Load the trainSet from disk
            var trainSet = _mlContext.Data.LoadFromTextFile<ImageDataWithLabelAsKey>("trainSet.tsv", separatorChar: '\t', hasHeader: true, allowQuoting: true);
            return trainSet;
        }


        public string MakePredictionsViaTrainedModel(string photoPath, byte[] byteFile)
        {
            //path of new photo 
            var imageData = new ImageData()
            {
                ImagePath = photoPath,
                Image = byteFile
            };

            // Load Trained Model
            DataViewSchema predictionPipelineSchema;
            ITransformer predictionPipeline = _mlContext.Model.Load("imageClassifierTest.zip", out predictionPipelineSchema);

            // Create PredictionEngines
            // Make prediction function (input = ImageData, output = ImagePrediction)
            var predictor = _mlContext.Model.CreatePredictionEngine<ImageData, ImagePrediction>(predictionPipeline);
            var prediction = predictor.Predict(imageData);
            Console.WriteLine($"Image: {(imageData.ImagePath)} predicted as: {prediction.PredictedLabel} with score: {prediction.Score.Max()} ");

            return prediction.PredictedLabel;

        }

        //public void RetrainModel(EstimatorChain<KeyToValueMappingTransformer> trainingPipeline, IDataView trainSet, string savedModelPath, string newImagesFolderPath)
        //{
        //    // Load saved model
        //    DataViewSchema modelSchema;
        //    ITransformer model = _mlContext.Model.Load(savedModelPath, out modelSchema);

        //    // Load new images from the folder
        //    var newImages = GetImgs(newImagesFolderPath);
        //    var newDataView = _mlContext.Data.LoadFromEnumerable(newImages);
        //    newDataView = _mlContext.Data.ShuffleRows(newDataView);

        //    // Combine train data with new images
        //    var concatenatedData = _mlContext.Transforms.Concatenate("Features", trainSet.Schema.Select(s => s.Name).Where(c => c != "Label").ToArray()).Fit(trainSet).Transform(trainSet);

        //    // Train the model with the combined dataset
        //    ITransformer retrainedModel = trainingPipeline.Fit(concatenatedData);

        //    // Save the retrained model
        //    _mlContext.Model.Save(retrainedModel, trainSet.Schema, savedModelPath);
        //}

        public void RetrainModel(ITransformer preTrainedModel, IDataView trainSet, string savedModelPath, string newImagesFolderPath)
        {
            // Load new images from the folder
            var newImages = GetImgs(FreshwaterFishPathWithLabels);
            var newDataView = _mlContext.Data.LoadFromEnumerable(newImages);
            newDataView = _mlContext.Data.ShuffleRows(newDataView);

            // Combine train data with new images
            //1

            IDataView shuffledFullImagesDataset = _mlContext.Transforms.Conversion.MapValueToKey(outputColumnName: "LabelAsKey",
                                       inputColumnName: "Label",
                                       keyOrdinality: ValueToKeyMappingEstimator.KeyOrdinality.ByValue)
                                       .Append(_mlContext.Transforms.LoadRawImageBytes(outputColumnName: "Image",
                                                                   imageFolder: AllFishInOnePlacePath,
                                                                   inputColumnName: "ImagePath"))
                                       .Fit(newDataView)
                                       .Transform(newDataView);


            //1 ... way to split data
            var trainTestSplit = _mlContext.Data.TrainTestSplit(shuffledFullImagesDataset, testFraction: 0.2, seed: 2);

            var testSet = trainTestSplit.TestSet;
            // var trainSet = trainTestSplit.TrainSet;

            // Initialize options with the pre-trained model
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
                TestOnTrainSet = false,
                // ModelLocation = savedModelPath // Path to the pre-trained model
            };

            // Create the training pipeline
            var trainingPipeline = _mlContext.MulticlassClassification.Trainers.ImageClassification(options)
                .Append(_mlContext.Transforms.Conversion.MapKeyToValue(
                    outputColumnName: "PredictedLabel",
                    inputColumnName: "PredictedLabel"));

            // Train the model with the combined dataset
            ITransformer retrainedModel = trainingPipeline.Fit(trainTestSplit.TrainSet);

            var predicitions = retrainedModel.Transform(testSet);
            // Evaluate prediction Accuracy
            var metrics = _mlContext.MulticlassClassification.Evaluate(predicitions, labelColumnName: "LabelAsKey", predictedLabelColumnName: "PredictedLabel");

            Console.WriteLine($"Accuracy: {metrics.MacroAccuracy:P2}");

            // Save the retrained model
            _mlContext.Model.Save(retrainedModel, trainSet.Schema, savedModelPath);
        }


        public class ImageDataWithLabelAsKey
        {
            [LoadColumn(0)] public string ImagePath;
            [LoadColumn(1)] public byte[] Image;
            [LoadColumn(2)] public string Label;
            [LoadColumn(3)] public uint LabelAsKey;
        }

    }
}
