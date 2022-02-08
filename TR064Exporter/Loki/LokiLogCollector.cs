using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PS.FritzBox.API;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TR064Exporter.Models;

namespace TR064Exporter.Loki
{
    internal class LokiLogCollector: IHostedService, IDisposable
    {
        private readonly TRClient<DeviceInfoClient> _tRClient;
        private readonly LokiClient _loki;
        private readonly Config _config;

        private CancellationTokenSource _tokenSource = new CancellationTokenSource();
        private Task _worker;
        private HashSet<string> _knownLogs = new HashSet<string>();

        public LokiLogCollector(
            TRClient<DeviceInfoClient> tRClient,
            LokiClient loki,
            Config config
        )
        {
            _tRClient = tRClient;
            _loki = loki;
            _config = config;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(_config?.LokiEndpoint))
            {
                return Task.CompletedTask;
            }

            _worker = Task.Run(Work);
            return Task.CompletedTask;
        }

        private async Task Work()
        {
            while (!_tokenSource.IsCancellationRequested)
            {
                await HandleLogs();
                await Task.Delay(15000, _tokenSource.Token).ConfigureAwait(false);
            }
        }

        private async Task HandleLogs()
        {
            var client = await _tRClient.Get();
            var logs = await client.GetDeviceLogAsync();

            var logsHashSet = new HashSet<string>(logs);
            
            logsHashSet.ExceptWith(_knownLogs);
            _knownLogs.UnionWith(logsHashSet);

            if (logsHashSet.Count > 0)
            {
                await _loki.PostLogs(logsHashSet);
            }

            if (logsHashSet.Count > 100)
            {
                logsHashSet.Clear();
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _tokenSource.Cancel();
            return Task.CompletedTask;
        }

        #region IDisposable Support
        private bool disposedValue = false;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "I don't want to :)")]
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    try
                    {
                        _tokenSource?.Dispose();
                        _worker?.Dispose();
                    }
                    catch
                    {
                        // CatchAll
                    }
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
