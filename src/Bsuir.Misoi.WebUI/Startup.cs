using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using Microsoft.Framework.DependencyInjection;
using Bsuir.Misoi.Core;
using Bsuir.Misoi.Core.Images.Finding;
using Bsuir.Misoi.Core.Images.Finding.Implementation;
using Microsoft.AspNet.Diagnostics;
using Bsuir.Misoi.WebUI.HostingFileSystem;

namespace Bsuir.Misoi.WebUI
{
    public class Startup
    {
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddTransient<Core.Images.IImageFactory, Core.Images.Implementation.ImageFactory>();
            services.AddTransient<Core.Images.Filtering.IFilterService, Core.Images.Filtering.Implementation.FilterService>();
            services.AddTransient<Bsuir.Misoi.Core.Images.Filtering.IConvolutionFilter, Core.Images.Filtering.Implementation.ConvolutionFilter>();
            services.AddTransient<Bsuir.Misoi.Core.Storage.IImageRepository, Bsuir.Misoi.Core.Storage.Implementation.ImageRepository>();
            services.AddTransient<Bsuir.Misoi.Core.Storage.IImageDataProvider, ImageDataProvider>();
            services.AddTransient<IFindingService, FindingService>();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseMvc();
            app.UseErrorPage();
        }
    }
}
