namespace Bsuir.Misoi.Core.Images.Implementation
{
    public interface IConvolutionFilter
    {
        void Filter(IImage image, double[,] filter);
    }
}
