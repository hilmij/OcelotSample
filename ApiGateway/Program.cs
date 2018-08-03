using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

namespace ApiGateway
{
    public class Program
    {
        public static void Main(string[] args) => new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureAppConfiguration(ConfigureApp)
                .ConfigureServices(ConfigureService)
                .ConfigureLogging(ConfigureLogging)
                .UseIISIntegration()
                .Configure(app => app.UseOcelot().Wait())
                .Build()
                .Run();

        private static void ConfigureService(IServiceCollection serviceCollection)
        {
            serviceCollection.AddOcelot().AddSingletonDefinedAggregator<AllAggregator>().AddStoreOcelotConfigurationInConsul();
        }

        private static void ConfigureApp(WebHostBuilderContext hostingContext, IConfigurationBuilder config)
        {
            config
                .SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", true, true)
                .AddJsonFile("ocelot.json")
                .AddEnvironmentVariables();
        }

        private static void ConfigureLogging(WebHostBuilderContext hostingContext, ILoggingBuilder logging)
        {
            logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
            logging.AddConsole();
            logging.AddDebug();
        }
    }
}
