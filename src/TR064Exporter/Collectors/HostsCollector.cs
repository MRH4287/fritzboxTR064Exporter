using Prometheus.Client;
using PS.FritzBox.API.LANDevice;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TR064Exporter.Collectors
{
    class HostsCollector : ICollector
    {
        #region Metrics

        private readonly Gauge _numberOfHostsGauge = Metrics.CreateGauge(Consts.MetricsPrefix + "_hosts_count", "The number of hosts");

        #endregion

        private readonly TRClient<HostsClient> _client;

        public HostsCollector(TRClient<HostsClient> client)
        {
            _client = client;
        }

        public async Task CollectAsync()
        {
            var service = await _client.Get().ConfigureAwait(false);
            var count = await service.GetHostNumberOfEntriesAsync().ConfigureAwait(false);
            _numberOfHostsGauge.Set(count);
        }
    }
}
