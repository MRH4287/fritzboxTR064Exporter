using Prometheus.Client;
using PS.FritzBox.API.WANDevice;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TR064Exporter.Collectors
{
    class DSLCollector : ICollector
    {

        #region Metrics

        private readonly Gauge _EnabledGauge = Metrics.CreateGauge(Consts.MetricsPrefix + "_DSL_Enabled", "Is the DSL enabled");
        private readonly Gauge _DownstreamAttenuationGauge = Metrics.CreateGauge(Consts.MetricsPrefix + "_DSL_DownstreamAttenuation", "The Downstream Attentuation");
        private readonly Gauge _UpstreamAttenuationGauge = Metrics.CreateGauge(Consts.MetricsPrefix + "_DSL_UpstreamAttenuation", "The Upstream Attentuation");
        private readonly Gauge _DownstreamCurrentRateGauge = Metrics.CreateGauge(Consts.MetricsPrefix + "_DSL_DownstreamCurrentRate", "The current Bitrate of the Downstream");
        private readonly Gauge _DownstreamMaxRateGauge = Metrics.CreateGauge(Consts.MetricsPrefix + "_DSL_DownstreamMaxRate", "The maximal Downstream");
        private readonly Gauge _DownstreamNoiseMarginGauge = Metrics.CreateGauge(Consts.MetricsPrefix + "_DSL_DownstreamNoiseMargin", "The Downstream Noise Margin");
        private readonly Gauge _DownstreamPowerGauge = Metrics.CreateGauge(Consts.MetricsPrefix + "_DSL_DownstreamPower", "The Downstream Power");
        private readonly Gauge _UpstreamCurrentRateGauge = Metrics.CreateGauge(Consts.MetricsPrefix + "_DSL_UpstreamCurrentRate", "The current Upstream Rate");
        private readonly Gauge _UpstreamMaxRateGauge = Metrics.CreateGauge(Consts.MetricsPrefix + "_DSL_UpstreamMaxRate", "The maximal Upstream Rate");
        private readonly Gauge _UpstreamNoiseMarginGauge = Metrics.CreateGauge(Consts.MetricsPrefix + "_DSL_UpstreamNoiseMargin", "The Upstream Noise Margin");
        private readonly Gauge _UpstreamPowerGauge = Metrics.CreateGauge(Consts.MetricsPrefix + "_DSL_UpstreamPower", "The Upstream Power");

        private readonly Gauge _ATUCCRCErrorsGauge = Metrics.CreateGauge(Consts.MetricsPrefix + "_DSL_ATUCCRCErrors", "The number of ATCCCRC Errors");
        private readonly Gauge _ATUCFECErrorsGauge = Metrics.CreateGauge(Consts.MetricsPrefix + "_DSL_ATUCFECErrors", "The number of ATUCFEC Errors");
        private readonly Gauge _ATUCHECErrorsGauge = Metrics.CreateGauge(Consts.MetricsPrefix + "_DSL_ATUCHECErrors", "The number of ATUCHEC Errors");
        private readonly Gauge _CellDelinGauge = Metrics.CreateGauge(Consts.MetricsPrefix + "_DSL_CellDelin", "The Cell Delin");
        private readonly Gauge _CRCErrorsGauge = Metrics.CreateGauge(Consts.MetricsPrefix + "_DSL_CRCErrors", "The number CRC errors");
        private readonly Gauge _ErroredSecsGauge = Metrics.CreateGauge(Consts.MetricsPrefix + "_DSL_ErroredSecs", "The Errored Secs");
        private readonly Gauge _FECErrorsGauge = Metrics.CreateGauge(Consts.MetricsPrefix + "_DSL_FECErrors", "The number FEC Errors");
        private readonly Gauge _HECErrorsGauge = Metrics.CreateGauge(Consts.MetricsPrefix + "_DSL_HECErrors", "The number ERC Errors");
        private readonly Gauge _InitErrorsGauge = Metrics.CreateGauge(Consts.MetricsPrefix + "_DSL_InitErrors", "The number of Init Errors");
        private readonly Gauge _InitTimeoutsGauge = Metrics.CreateGauge(Consts.MetricsPrefix + "_DSL_InitTimeouts", "The number Init Timouts");
        private readonly Gauge _LinkRetrainGauge = Metrics.CreateGauge(Consts.MetricsPrefix + "_DSL_LinkRetrain", "The Link Retrain");
        private readonly Gauge _LossOfFramingGauge = Metrics.CreateGauge(Consts.MetricsPrefix + "_DSL_LossOfFraming", "The Loss of Framing");
        private readonly Gauge _ReceiveBlocksGauge = Metrics.CreateGauge(Consts.MetricsPrefix + "_DSL_ReceiveBlocks", "The number of receibed Blocks");
        private readonly Gauge _SeverelyErroredSecsGauge = Metrics.CreateGauge(Consts.MetricsPrefix + "_DSL_SeverelyErroredSecs", "The number of severly errored Secs");
        private readonly Gauge _TransmitBlocksGauge = Metrics.CreateGauge(Consts.MetricsPrefix + "_DSL_TransmitBlocks", "The number of Transmit Blocks");
        private readonly TRClient<WANDSLInterfaceConfigClient> _client;

        #endregion


        public DSLCollector(TRClient<WANDSLInterfaceConfigClient> client)
        {
            _client = client;
        }

        public async Task CollectAsync()
        {
            var service = await _client.Get().ConfigureAwait(false);

            var info = await service.GetInfoAsync().ConfigureAwait(false);

            _EnabledGauge.Set(info.Enabled ? 1 : 0);
            _DownstreamAttenuationGauge.Set(info.DownstreamAttenuation);
            _UpstreamAttenuationGauge.Set(info.UpstreamAttenuation);
            _DownstreamCurrentRateGauge.Set(info.DownstreamCurrentRate);
            _DownstreamMaxRateGauge.Set(info.DownstreamMaxRate);
            _DownstreamNoiseMarginGauge.Set(info.DownstreamNoiseMargin);
            _DownstreamPowerGauge.Set(info.DownstreamPower);
            _UpstreamCurrentRateGauge.Set(info.UpstreamCurrentRate);
            _UpstreamMaxRateGauge.Set(info.UpstreamMaxRate);
            _UpstreamNoiseMarginGauge.Set(info.UpstreamNoiseMargin);
            _UpstreamPowerGauge.Set(info.UpstreamPower);

            var stats = await service.GetStatisticsTotalAsync().ConfigureAwait(false);

            _ATUCCRCErrorsGauge.Set(stats.ATUCCRCErrors);
            _ATUCFECErrorsGauge.Set(stats.ATUCFECErrors);
            _ATUCHECErrorsGauge.Set(stats.ATUCHECErrors);
            _CellDelinGauge.Set(stats.CellDelin);
            _CRCErrorsGauge.Set(stats.CRCErrors);
            _ErroredSecsGauge.Set(stats.ErroredSecs);
            _FECErrorsGauge.Set(stats.FECErrors);
            _HECErrorsGauge.Set(stats.HECErrors);
            _InitErrorsGauge.Set(stats.InitErrors);
            _InitTimeoutsGauge.Set(stats.InitTimeouts);
            _LinkRetrainGauge.Set(stats.LinkRetrain);
            _LossOfFramingGauge.Set(stats.LossOfFraming);
            _ReceiveBlocksGauge.Set(stats.ReceiveBlocks);
            _SeverelyErroredSecsGauge.Set(stats.SeverelyErroredSecs);
            _TransmitBlocksGauge.Set(stats.TransmitBlocks);
        }
    }
}
