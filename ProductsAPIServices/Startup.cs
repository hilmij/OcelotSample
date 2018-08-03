using System;
using System.Linq;
using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ProductsAPIServices
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();

            RegisterInCosul(app);
        }

        private static async void RegisterInCosul(IApplicationBuilder app)
        {
            var features = app.Properties["server.Features"] as FeatureCollection;
            var address = new Uri(app.Properties["server.Features"] as FeatureCollection?.Get<IServerAddressesFeature>()?.Addresses.First());
            await new ConsulClient().Agent.ServiceRegister(new AgentServiceRegistration
            {
                Address = address.Host,
                ID = "products1",
                Name = "products",
                Port = address.Port
            });
        }
    }
}
