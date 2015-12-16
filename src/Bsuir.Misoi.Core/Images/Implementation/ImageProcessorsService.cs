using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Bsuir.Misoi.Core.Images.Implementation.Filters;
using Bsuir.Misoi.Core.Images.Implementation.Image;
using Bsuir.Misoi.Core.Images.Implementation.Segmentation;

namespace Bsuir.Misoi.Core.Images.Implementation
{
    public class ImageProcessorsService : IImageProcessorsService
    {
        private readonly List<IImageProcessor> _processors;

        public ImageProcessorsService()
        {
            _processors = new List<IImageProcessor>(RegiterProcessors());
        }

        public IEnumerable<string> GetProcessorsNames()
        {
            return _processors.Select(p => p.Name);
        }

        public Task<IProcessServiceResult> ProcessImageAsync(string processorName, IImage image)
        {
            var result = new ImageProcessServiceResult();
            var processor = _processors.FirstOrDefault(p => p.Name == processorName);
            if (processor != null)
            {
                try
                {
                    var imageForProcessing = image.Clone();
                    processor.ProcessImage(imageForProcessing);
                    result.ProcessedImage = imageForProcessing;
                    result.Successful = true;
                }
                catch (Exception e)
                {
                    result.Successful = false;
                    result.ErrorMessage = e.Message;
                }
            }
            else
            {
                result.Successful = false;
                result.ErrorMessage = $"Can't find processor with name {processorName}";
            }

            return Task.FromResult((IProcessServiceResult)result);
        }

        private IEnumerable<IImageProcessor> RegiterProcessors()
        {
            yield return new AdaptiveBinarizationFilter();
            yield return new GammaFilter();
            yield return new LaplacianFiveFilter(new ConvolutionFilter());
            yield return new MedianFilter();
            yield return new SegmentationFilter(new SegmentationAlgorithm(new AdaptiveBinarizationFilter()));
            yield return new FindInImageAndHandleResultProcessor(new TextFindProcessor(), new FindResultsDrawer());
            yield return new FindInImageAndHandleResultProcessor(new AnalyzeSegmentsFindAloritm(new SegmentationAlgorithm(new AdaptiveBinarizationFilter()), new CarNumberSegmentFindAnalyzer(), "Find car number"), new FindResultsDrawer());
            yield return new FindInImageAndHandleResultProcessor(new AnalyzeSegmentsFindAloritm(new SegmentationAlgorithm(new AdaptiveBinarizationFilter()), new CarNumberSegmentFindAnalyzer(), "Clip car nuber"), new FindResultCliper());
        }
    }
}
