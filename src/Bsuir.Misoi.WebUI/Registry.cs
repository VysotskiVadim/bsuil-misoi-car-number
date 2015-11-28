using Bsuir.Misoi.Core.Images;
using Bsuir.Misoi.Core.Images.Implementation;
using Bsuir.Misoi.Core.Storage;
using Bsuir.Misoi.Core.Storage.Implementation;
using Bsuir.Misoi.WebUI.HostingFileSystem;
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
            services.AddTransient<IBinarizationFilter, BinarizationFilter>();
            services.AddTransient<IImageProcessorsService, ImageProcessorsService>();
            
        }
    }
}
