using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Everest.Engineering.ConsoleApp.Middleware
{
    public static class ConfigureEnvironment
    {
        /// <summary>Configures the application settings based on environment.</summary>
        /// <param name="services">The services.</param>
        /// <param name="hostEnvironment">The host environment.</param>
        public static void ConfigureAppSettingsBasedOnEnvironment(this IServiceCollection services, IHostEnvironment hostEnvironment)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(hostEnvironment.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{hostEnvironment.EnvironmentName}.json", optional: false)
                .AddEnvironmentVariables();
            builder.Build();
        }
    }
}
