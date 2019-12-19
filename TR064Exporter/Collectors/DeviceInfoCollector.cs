using Prometheus.Client;
using PS.FritzBox.API;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TR064Exporter.Collectors
{
    class DeviceInfoCollector : ICollector
    {
        #region Metrics

        private readonly Gauge _uptimeGauge = Metrics.CreateGauge(Consts.MetricsPrefix + "_deviceInfo_uptime", "The Uptime of the Fritzbox");

        #endregion

        private readonly TRClient<DeviceInfoClient> _client;

        public DeviceInfoCollector(TRClient<DeviceInfoClient> client)
        {
            _client = client;
        }

        public async Task CollectAsync()
        {
            var service = await _client.Get().ConfigureAwait(false);
            var deviceInfo = await service.GetDeviceInfoAsync().ConfigureAwait(false);

            _uptimeGauge.Set(deviceInfo.UpTime);
        }
    }
}
