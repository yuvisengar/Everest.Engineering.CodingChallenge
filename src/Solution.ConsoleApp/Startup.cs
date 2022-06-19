using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Everest.Engineering.ConsoleApp.Executor.Abstractions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Everest.Engineering.ConsoleApp
{
    /// <summary>Class Executor.
    /// Implements the <see cref="Microsoft.Extensions.Hosting.BackgroundService" /></summary>
    public class Startup : BackgroundService
    {
        private readonly IHostEnvironment _env;
        private readonly ILogger<Startup> _logger;
        private readonly IExecutor _executor;

        /// <summary>Initializes a new instance of the <see cref="T:Everest.Engineering.ConsoleApp.Startup" /> class.</summary>
        /// <param name="hostEnvironment">The host environment.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="uiService">The UI service.</param>
        /// <param name="executor">The reservation executor.</param>
        public Startup(IHostEnvironment hostEnvironment,
            ILogger<Startup> logger,
            IExecutor executor)
        {
            _env = hostEnvironment;
            _logger = logger;
            _executor = executor;
        }

        /// <summary>Start as an asynchronous operation.</summary>
        /// <param name="stoppingToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        public override async Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"Executor started at {Environment.MachineName}.");
            _logger.LogInformation($"Executor: The Environment at {Environment.MachineName} is set to {_env.EnvironmentName}");
            await base.StartAsync(stoppingToken);
        }

        /// <summary>Execute as an asynchronous operation.</summary>
        /// <param name="stoppingToken">
        /// Triggered when <see cref="M:Microsoft.Extensions.Hosting.IHostedService.StopAsync(System.Threading.CancellationToken)">StopAsync(System.Threading.CancellationToken)</see> is called.
        /// </param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _executor.RunOperationsAsync(stoppingToken);
        }

        /// <summary>Stop as an asynchronous operation.</summary>
        /// <param name="stoppingToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"Executor: Service Shutting down at {Environment.MachineName}");
            await _executor.CancelOperationAsync(stoppingToken);
            await base.StopAsync(stoppingToken);
        }
    }
}
