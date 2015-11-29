using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bsuir.Misoi.Core.Images.Implementation
{
    public class SegmentationFilter : IImageProcessor
    {

        private ISegmentationAlgorithm _segmentationAlgoritm;

        public SegmentationFilter(ISegmentationAlgorithm segmentationAlgoritm)
        {
            _segmentationAlgoritm = segmentationAlgoritm;
        }

        public string Name => "Segmentation";

        public void ProcessImage(IImage inputImage)
        {
            var segments = _segmentationAlgoritm.ProcessImage(inputImage);

            var image = segments.SegmentationMatrix;
            for (int j = 0; j < inputImage.Height; j++)
            {
                for (int i = 0; i < inputImage.Width; i++)
                {
                    var random = new Random(image[i, j]);
                    var pixel = new Pixel { R = (byte)random.Next(255), G = (byte)random.Next(255), B = (byte)random.Next(255) };
                    inputImage.SetPixel(i, j, pixel);
                }
            }
        }
    }
}
