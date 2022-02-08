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

        private readonly IGauge _EnabledGauge = Metrics.DefaultFactory.CreateGauge(Consts.MetricsPrefix + "_DSL_Enabled", "Is the DSL enabled");
        private readonly IGauge _DownstreamAttenuationGauge = Metrics.DefaultFactory.CreateGauge(Consts.MetricsPrefix + "_DSL_DownstreamAttenuation", "The Downstream Attentuation");
        private readonly IGauge _UpstreamAttenuationGauge = Metrics.DefaultFactory.CreateGauge(Consts.MetricsPrefix + "_DSL_UpstreamAttenuation", "The Upstream Attentuation");
        private readonly IGauge _DownstreamCurrentRateGauge = Metrics.DefaultFactory.CreateGauge(Consts.MetricsPrefix + "_DSL_DownstreamCurrentRate", "The current Bitrate of the Downstream");
        private readonly IGauge _DownstreamMaxRateGauge = Metrics.DefaultFactory.CreateGauge(Consts.MetricsPrefix + "_DSL_DownstreamMaxRate", "The maximal Downstream");
        private readonly IGauge _DownstreamNoiseMarginGauge = Metrics.DefaultFactory.CreateGauge(Consts.MetricsPrefix + "_DSL_DownstreamNoiseMargin", "The Downstream Noise Margin");
        private readonly IGauge _DownstreamPowerGauge = Metrics.DefaultFactory.CreateGauge(Consts.MetricsPrefix + "_DSL_DownstreamPower", "The Downstream Power");
        private readonly IGauge _UpstreamCurrentRateGauge = Metrics.DefaultFactory.CreateGauge(Consts.MetricsPrefix + "_DSL_UpstreamCurrentRate", "The current Upstream Rate");
        private readonly IGauge _UpstreamMaxRateGauge = Metrics.DefaultFactory.CreateGauge(Consts.MetricsPrefix + "_DSL_UpstreamMaxRate", "The maximal Upstream Rate");
        private readonly IGauge _UpstreamNoiseMarginGauge = Metrics.DefaultFactory.CreateGauge(Consts.MetricsPrefix + "_DSL_UpstreamNoiseMargin", "The Upstream Noise Margin");
        private readonly IGauge _UpstreamPowerGauge = Metrics.DefaultFactory.CreateGauge(Consts.MetricsPrefix + "_DSL_UpstreamPower", "The Upstream Power");

        private readonly IGauge _ATUCCRCErrorsGauge = Metrics.DefaultFactory.CreateGauge(Consts.MetricsPrefix + "_DSL_ATUCCRCErrors", "The number of ATCCCRC Errors");
        private readonly IGauge _ATUCFECErrorsGauge = Metrics.DefaultFactory.CreateGauge(Consts.MetricsPrefix + "_DSL_ATUCFECErrors", "The number of ATUCFEC Errors");
        private readonly IGauge _ATUCHECErrorsGauge = Metrics.DefaultFactory.CreateGauge(Consts.MetricsPrefix + "_DSL_ATUCHECErrors", "The number of ATUCHEC Errors");
        private readonly IGauge _CellDelinGauge = Metrics.DefaultFactory.CreateGauge(Consts.MetricsPrefix + "_DSL_CellDelin", "The Cell Delin");
        private readonly IGauge _CRCErrorsGauge = Metrics.DefaultFactory.CreateGauge(Consts.MetricsPrefix + "_DSL_CRCErrors", "The number CRC errors");
        private readonly IGauge _ErroredSecsGauge = Metrics.DefaultFactory.CreateGauge(Consts.MetricsPrefix + "_DSL_ErroredSecs", "The Errored Secs");
        private readonly IGauge _FECErrorsGauge = Metrics.DefaultFactory.CreateGauge(Consts.MetricsPrefix + "_DSL_FECErrors", "The number FEC Errors");
        private readonly IGauge _HECErrorsGauge = Metrics.DefaultFactory.CreateGauge(Consts.MetricsPrefix + "_DSL_HECErrors", "The number ERC Errors");
        private readonly IGauge _InitErrorsGauge = Metrics.DefaultFactory.CreateGauge(Consts.MetricsPrefix + "_DSL_InitErrors", "The number of Init Errors");
        private readonly IGauge _InitTimeoutsGauge = Metrics.DefaultFactory.CreateGauge(Consts.MetricsPrefix + "_DSL_InitTimeouts", "The number Init Timouts");
        private readonly IGauge _LinkRetrainGauge = Metrics.DefaultFactory.CreateGauge(Consts.MetricsPrefix + "_DSL_LinkRetrain", "The Link Retrain");
        private readonly IGauge _LossOfFramingGauge = Metrics.DefaultFactory.CreateGauge(Consts.MetricsPrefix + "_DSL_LossOfFraming", "The Loss of Framing");
        private readonly IGauge _ReceiveBlocksGauge = Metrics.DefaultFactory.CreateGauge(Consts.MetricsPrefix + "_DSL_ReceiveBlocks", "The number of receibed Blocks");
        private readonly IGauge _SeverelyErroredSecsGauge = Metrics.DefaultFactory.CreateGauge(Consts.MetricsPrefix + "_DSL_SeverelyErroredSecs", "The number of severly errored Secs");
        private readonly IGauge _TransmitBlocksGauge = Metrics.DefaultFactory.CreateGauge(Consts.MetricsPrefix + "_DSL_TransmitBlocks", "The number of Transmit Blocks");
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
