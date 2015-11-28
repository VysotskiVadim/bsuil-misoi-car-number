namespace Bsuir.Misoi.Core.Images.Implementation
{
    public class LaplacianFiveFilter : IImageProcessor
    {
        private readonly IConvolutionFilter _convolutionFilter;

        public LaplacianFiveFilter(IConvolutionFilter convolutionFilter)
        {
            _convolutionFilter = convolutionFilter;
        }

        public string Name => "Edge filter";

        public void ProcessImage(IImage image)
        {
            var filterMatrix = new double[,]
                { { -1, -1, -1, -1, -1, },
                  { -1, -1, -1, -1, -1, },
                  { -1, -1, 24, -1, -1, },
                  { -1, -1, -1, -1, -1, },
                  { -1, -1, -1, -1, -1  }, };
            _convolutionFilter.Filter(image, filterMatrix);
        }
    }
}
