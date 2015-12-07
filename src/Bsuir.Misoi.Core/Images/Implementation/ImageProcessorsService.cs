using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bsuir.Misoi.Core.Images.Implementation
{
    public class ImageProcessorsService : IImageProcessorsService
    {
        private readonly IConvolutionFilter _convolutionFilter;
        private readonly ISegmentationAlgorithm _segmentationAlogithm;
        private readonly IFindResultDrawer _findResultDrawer;

        private readonly List<IImageProcessor> _processors;

        public ImageProcessorsService(IConvolutionFilter convolutionFilter, ISegmentationAlgorithm segmentationAlogithm, IFindResultDrawer findResultDrawer)
        {
            _convolutionFilter = convolutionFilter;
            _segmentationAlogithm = segmentationAlogithm;
            _findResultDrawer = findResultDrawer;
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
                    result.SourceImage = image.Clone();
                    processor.ProcessImage(image);
                    result.ProcessedImage = image;
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
            yield return new BinarizationFilter();
            yield return new GammaFilter();
            yield return new LaplacianFiveFilter(_convolutionFilter);
            yield return new MedianFilter();
            yield return new SegmentationFilter(_segmentationAlogithm);
            yield return new SelectFoundedAreaImageProcessor(new TextFindProcessor(), _findResultDrawer);
        }
    }
}
