namespace Bsuir.Misoi.Core.Images.Filtering
{
    public interface IConvolutionFilter
    {
	    void Filter(IImage image, double[,] filter);
    }
}
