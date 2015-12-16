namespace Bsuir.Misoi.Core.Images
{
    public interface IConvolutionFilter
    {
        void Filter(IImage image, double[,] filter);
    }
}
