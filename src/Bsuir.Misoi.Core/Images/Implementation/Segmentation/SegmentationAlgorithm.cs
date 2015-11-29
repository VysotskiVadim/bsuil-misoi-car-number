using System.Collections.Generic;
using System;
using System.Linq;

namespace Bsuir.Misoi.Core.Images.Implementation.Segmentation
{
    public class SegmentationAlgorithm : ISegmentationAlgorithm
    {
        private readonly IBinarizationFilter _binarizationFilter;

        public string Name => "Segmentation";

        public SegmentationAlgorithm(IBinarizationFilter binarizationFilter)
        {
            _binarizationFilter = binarizationFilter;
        }

        public ISegmentationResult ProcessImage(IImage inputImage)
        {
            _binarizationFilter.ProcessImage(inputImage);

            var binaryImage = ConvertImageToBinaryMatrix(inputImage);

            ISegmentManager segmentManager = new SegmentsManager(inputImage.Width, inputImage.Height);

            for (int y = 0; y < inputImage.Height; y++)  
            {
                for (int x = 0; x < inputImage.Width; x++)
                {
                    bool leftPixel = false, upperPixel = false, currentPixel = false;
                    int leftPixelXPosition = x - 1;
                    int upperPixelYPosition = y - 1;

                    if (leftPixelXPosition >= 0)
                    {
                        leftPixel = binaryImage[leftPixelXPosition, y];
                    }

                    if (upperPixelYPosition >= 0)
                    {
                        upperPixel = binaryImage[x, upperPixelYPosition];
                    }

                    currentPixel = binaryImage[x, y];
                    if (currentPixel == false)
                    {
                    }
                    else if (leftPixel == false && upperPixel == false)
                    {
                        segmentManager.MarkNewSegment(x, y);
                    }
                    else if (leftPixel && upperPixel == false)
                    {
                        var segmentOfLeftPixel = segmentManager.GetSegmentIdFor(leftPixelXPosition, y);
                        segmentManager.MarkSegment(x, y, segmentOfLeftPixel);
                    }
                    else if (leftPixel == false && upperPixel)
                    {
                        var segmentOfUpperPixel = segmentManager.GetSegmentIdFor(x, upperPixelYPosition);
                        segmentManager.MarkSegment(x, y, segmentOfUpperPixel);
                    }
                    else if (leftPixel & upperPixel)
                    {
                        var segmentOfLeftPixel = segmentManager.GetSegmentIdFor(leftPixelXPosition, y);
                        var segmentOfUpperPixel = segmentManager.GetSegmentIdFor(x, upperPixelYPosition);
                        if (segmentOfLeftPixel == segmentOfUpperPixel)
                        {           
                            segmentManager.MarkSegment(x, y, segmentOfLeftPixel);
                        }
                        else
                        {
                            segmentManager.MergeSegments(segmentOfLeftPixel, segmentOfUpperPixel);
                            segmentManager.MarkSegment(x, y, segmentManager.GetSegmentIdFor(leftPixelXPosition, y)); 
                        }
                    }
                }
            }

            return segmentManager.BuildSegmentationResult();
        }


        private IEnumerable<int> ForX(int middle, int end)
        {
            var final = end - middle;
            for (int i = -middle; i < final; i++)
            {
                yield return i;
            }
        }

        private bool PixelToBinary(Pixel pixel)  // перевод RGB пикселя в строго бинарный вид типа int 
        {
            return pixel.B != 0;   // или R!=0 или G != 0  ?)) 
        }

        private bool[,] ConvertImageToBinaryMatrix(IImage inputImage)  // перевод пиксельного изображения в матричный вид типа int
        {
            var image = new bool[inputImage.Width, inputImage.Height];
            for (int j = 0; j < inputImage.Height; j++)
            {
                for (int i = 0; i < inputImage.Width; i++)
                {
                    image[i, j] = PixelToBinary(inputImage.GetPixel(i, j));
                }
            }
            return image;
        }

        
    }
}
