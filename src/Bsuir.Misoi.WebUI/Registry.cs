using Bsuir.Misoi.Core;
using Bsuir.Misoi.Core.Images;
using Bsuir.Misoi.Core.Images.Implementation;
using Bsuir.Misoi.Core.Images.Implementation.Filters;
using Bsuir.Misoi.Core.Images.Implementation.Hough;
using Bsuir.Misoi.Core.Images.Implementation.Image;
using Bsuir.Misoi.Core.Images.Implementation.Segmentation;
using Bsuir.Misoi.WebUI.Storage;
using Bsuir.Misoi.WebUI.Storage.Implementation;
using Microsoft.Framework.DependencyInjection;

namespace Bsuir.Misoi.WebUI
{
    public class Registry
    {
        public void Register(IServiceCollection services)
        {
            services.AddTransient<IImageFactory, ImageFactory>();
            services.AddTransient<IConvolutionFilter, ConvolutionFilter>();
            services.AddTransient<IImageRepository, ImageRepository>();
            services.AddTransient<IImageDataProvider, ImageDataProvider>();
            services.AddTransient<IBinarizationFilter, AdaptiveBinarizationFilter>();
            services.AddTransient<IImageProcessorsService, ImageProcessorsService>();
            services.AddTransient<IImageUrlProvider, ImageUrlProvider>();
            services.AddTransient<ISegmentationAlgorithm, SegmentationAlgorithm>();
            services.AddTransient<ICarNumerIdentifyService, CarNumnerIdentifyService>();
            services.AddTransient<IFindResultsHandler, FindResultsDrawer>();
            services.AddTransient<ISegmentFindAnalyzer, CarNumberSegmentFindAnalyzer>();
        }
    }
}
