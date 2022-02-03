using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using Microsoft.Extensions.Options;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Processing;

namespace TopSwagCode.Blazor.Server.Services
{
    public class ComputerVisionOptions
    {
        public const string Position = "ComputerVision";
        public string SubscriptionKey { get; set; } = null!;
        public string Endpoint { get; set; } = null!;
    }

    public class ComputerVisionService : IComputerVisionService
    {
        private readonly ComputerVisionOptions _computerVisionOptions;

        public ComputerVisionService(IOptions<ComputerVisionOptions> computerVisionOptions )
        {
            _computerVisionOptions = computerVisionOptions.Value;
        }
        
        public async Task<string> FaceRec(byte[] imageData)
        {
            var resultFileGuid = Guid.NewGuid().ToString();
            string outputImagePath = $"{resultFileGuid}.jpg";
            string hatImagePath = "hat.png";

            var faces = await GetFaceDescriptions(imageData);

            using var inputImage = Image.Load(imageData);
            using var hat = Image.Load(hatImagePath);

            using var resultImage = inputImage.Clone(ctx =>
            {
                foreach (var face in faces)
                {
                    ctx.DrawHatOnFace(hat, face);
                    ctx.DrawBoxAroundFace(face);
                }
            });

            resultImage.Save(outputImagePath);

            return resultFileGuid;
        }

        private async Task<IList<FaceDescription>> GetFaceDescriptions(byte[] imageData)
        {
            Stream imageFileStream = new MemoryStream(imageData);

            ComputerVisionClient client = new ComputerVisionClient(new ApiKeyServiceClientCredentials(_computerVisionOptions.SubscriptionKey))
            { Endpoint = _computerVisionOptions.Endpoint };

            List<VisualFeatureTypes?> features = new List<VisualFeatureTypes?>()
            {
                VisualFeatureTypes.Faces
            };

            var result = await client.AnalyzeImageInStreamAsync(imageFileStream, features);
            return result.Faces;
        }

        /* 
         * Some of the visual features you can get through the ComputerVisionClient:
         * VisualFeatureTypes.Categories, 
         * VisualFeatureTypes.Description,
         * VisualFeatureTypes.Faces, 
         * VisualFeatureTypes.ImageType,
         * VisualFeatureTypes.Tags, 
         * VisualFeatureTypes.Adult,
         * VisualFeatureTypes.Color, 
         * VisualFeatureTypes.Brands,
         * VisualFeatureTypes.Objects
         */
    }


    public static class HatExstension
    {
        public static IImageProcessingContext DrawHatOnFace(this IImageProcessingContext processingContext, Image hat, FaceDescription face)
        {
            /*
             * The Xmas hat image used has minor offset to the side, where the ball is.
             * Also we would like to look like the hat is on the head.
             * So we need offset for empty space in image.
             * We could create a Hat class containing these offsets.
             * This would let us support other hat's in future and perhaps a "random hat" feature.
             */

            var point = new Point(
                face.FaceRectangle.Left - (92 / 2),
                face.FaceRectangle.Top - hat.Height + (60 / 2)
                );

            return processingContext.DrawImage(hat, point, 1);
        }

    }

    public static class FaceDetectionDebugExstension
    {
        public static IImageProcessingContext DrawBoxAroundFace(this IImageProcessingContext processingContext, FaceDescription face) // TODO. Add position.
        {
            var rect = new Rectangle(
                face.FaceRectangle.Left,
                face.FaceRectangle.Top,
                face.FaceRectangle.Width,
                face.FaceRectangle.Height);

            return processingContext.Draw(
                shape: rect,
                color: Color.Purple,
                thickness: 4);
        }
    }
}
