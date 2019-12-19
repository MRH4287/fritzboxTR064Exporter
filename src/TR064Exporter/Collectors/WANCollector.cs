﻿using Prometheus.Client;
using PS.FritzBox.API.WANDevice;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TR064Exporter.Collectors
{
    class WANCollector : ICollector
    {
        #region Metrics

        private readonly Gauge _totalBytesSentGauge = Metrics.CreateGauge(Consts.MetricsPrefix + "_WAN_totalBytesSent", "The total number of bytes sent");
        private readonly Gauge _totalBytesReceivedGauge = Metrics.CreateGauge(Consts.MetricsPrefix + "_WAN_totalBytesReceived", "The total number of bytes received");

        private readonly Gauge _totalPackagesSentGauge = Metrics.CreateGauge(Consts.MetricsPrefix + "_WAN_totalPackagesSent", "The total number of packages sent");
        private readonly Gauge _totalPackagesReceivedGauge = Metrics.CreateGauge(Consts.MetricsPrefix + "_WAN_totalPackagesReceived", "The total number of packages received");

        #endregion

        private readonly TRClient<WANCommonInterfaceConfigClient> _client;

        public WANCollector(TRClient<WANCommonInterfaceConfigClient> client)
        {
            _client = client;
        }

        public async Task CollectAsync()
        {
            var service = await _client.Get().ConfigureAwait(false);

            _totalBytesReceivedGauge.Set(await service.GetTotalBytesReceivedAsync().ConfigureAwait(false));
            _totalBytesSentGauge.Set(await service.GetTotalBytesSentAsync().ConfigureAwait(false));

            _totalPackagesReceivedGauge.Set(await service.GetTotalPacketsReceivedAsync().ConfigureAwait(false));
            _totalPackagesSentGauge.Set(await service.GetTotalPacketsSentAsync().ConfigureAwait(false));
        }
    }
}
