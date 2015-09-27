namespace Bsuir.Misoi.Core.Images.Filtering
{
    public interface IFilter
    {
		string Name { get; }

		void Filter(IImage image);
    }
}
