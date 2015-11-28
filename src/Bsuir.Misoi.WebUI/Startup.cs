using Microsoft.AspNet.Builder;
using Microsoft.Framework.DependencyInjection;

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
            app.UseStaticFiles();
        }
    }
}
