using Prometheus.Client;
using PS.FritzBox.API.LANDevice;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TR064Exporter.Collectors
{
    class WLANCollector : ICollector
    {
        #region Metrics

        private readonly IGauge _totalPackagesSentGauge = Metrics.DefaultFactory.CreateGauge(Consts.MetricsPrefix + "_WLAN_totalPackagesSent", "The total numbet of packages sent");
        private readonly IGauge _totalPackagesReceivedGauge = Metrics.DefaultFactory.CreateGauge(Consts.MetricsPrefix + "_WLAN_totalPackagesReceived", "The total numbet of packages received");

        private readonly IGauge _totalConnectionsGauge = Metrics.DefaultFactory.CreateGauge(Consts.MetricsPrefix + "_WLAN_total_connections", "The total number of connected devies");

        #endregion

        private readonly TRClient<WLANConfigurationClient> _client;

        public WLANCollector(TRClient<WLANConfigurationClient> client)
        {
            _client = client;
        }

        public async Task CollectAsync()
        {
            var service = await _client.Get().ConfigureAwait(false);

            var stat = await service.GetStatisticsAsync().ConfigureAwait(false);
            _totalPackagesReceivedGauge.Set(stat.TotalPacketsReceived);
            _totalPackagesSentGauge.Set(stat.TotalPacketsSent);


            _totalConnectionsGauge.Set(await service.GetTotalAssociationsAsync().ConfigureAwait(false));
        }
    }
}
