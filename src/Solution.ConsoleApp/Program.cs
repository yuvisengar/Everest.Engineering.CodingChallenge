using Microsoft.Extensions.Hosting;
using Everest.Engineering.ConsoleApp.Middleware;

namespace Everest.Engineering.ConsoleApp
{
    /// <summary>Class Program.</summary>
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                services.ConfigureAppSettingsBasedOnEnvironment(hostContext.HostingEnvironment);
                services.Configure(hostContext);
            }).UseWindowsService();
    }
}
