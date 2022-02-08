using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Prometheus.Client.AspNetCore;
using TR064Exporter.Collectors;
using TR064Exporter.Loki;
using TR064Exporter.Models;

namespace TR064Exporter
{
    class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient();

            services.AddSingleton(provider =>
            {
                var config = provider.GetService<IConfiguration>();

                return config.Get<Config>();
            });

            services.AddSingleton<Connection>();
            services.AddSingleton(typeof(TRClient<>));
            services.AddSingleton<LokiClient>();

            // services.AddHostedService<Collector>();
            services.AddHostedService<LokiLogCollector>();

            // Collectors

            services.AddSingleton<ICollector, WLANCollector>();
            services.AddSingleton<ICollector, DeviceInfoCollector>();
            services.AddSingleton<ICollector, HostsCollector>();
            services.AddSingleton<ICollector, LANEthernetCollector>();
            services.AddSingleton<ICollector, DSLCollector>();
            services.AddSingleton<ICollector, WANCollector>();
            services.AddSingleton<ICollector, HomeAutomationCollector>();

            
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory logger, IConfiguration configuration)
        {
            app.UsePrometheusServer();
        }
    }
}
