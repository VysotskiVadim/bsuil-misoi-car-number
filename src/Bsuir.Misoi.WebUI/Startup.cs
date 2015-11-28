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
        public void ConfigureServices(IServiceCollection services)
        {
            new Registry().Register(services);
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseMvc();
            app.UseErrorPage();
        }
    }
}
