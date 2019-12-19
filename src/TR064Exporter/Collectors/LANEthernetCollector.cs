using Prometheus.Client;
using PS.FritzBox.API.LANDevice;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TR064Exporter.Collectors
{
    class LANEthernetCollector : ICollector
    {
        #region Metrics

        private readonly Gauge _bytesSentGauge = Metrics.CreateGauge(Consts.MetricsPrefix + "_LAN_bytesSent", "The number of bytes sent");
        private readonly Gauge _bytesReceivedGauge = Metrics.CreateGauge(Consts.MetricsPrefix + "_LAN_bytesReceived", "The number of bytes received");

        private readonly Gauge _packagesSentGauge = Metrics.CreateGauge(Consts.MetricsPrefix + "_LAN_packagesSent", "The number of packages sent");
        private readonly Gauge _packagesReceivedGauge = Metrics.CreateGauge(Consts.MetricsPrefix + "_LAN_packagesReceived", "The number of packages received");

        private readonly Gauge _maxBitrageGauge = Metrics.CreateGauge(Consts.MetricsPrefix + "_LAN_maxBitrate", "The maximum bitrate");
        private readonly Gauge _lanEnabledGauge = Metrics.CreateGauge(Consts.MetricsPrefix + "_LAN_enabled", "Enabled");

        #endregion

        private readonly TRClient<LANEthernetInterfaceClient> _client;

        public LANEthernetCollector(TRClient<LANEthernetInterfaceClient> client)
        {
            _client = client;
        }
        
        public async Task CollectAsync()
        {
            var service = await _client.Get().ConfigureAwait(false);

            var info = await service.GetInfoAsync().ConfigureAwait(false);
            _maxBitrageGauge.Set(info.MaxBitRate);
            _lanEnabledGauge.Set(info.Enable ? 1 : 0);

            var statistics = await service.GetStatisticsAsync().ConfigureAwait(false);
            _bytesSentGauge.Set(statistics.BytesSent);
            _bytesReceivedGauge.Set(statistics.BytesReceived);
            _packagesSentGauge.Set(statistics.PacketsSent);
            _packagesReceivedGauge.Set(statistics.PacketsReceived);

        }
    }
}
