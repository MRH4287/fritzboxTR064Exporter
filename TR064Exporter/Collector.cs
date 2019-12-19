using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TR064Exporter.Collectors;

namespace TR064Exporter
{
    class Collector : IHostedService, IDisposable
    {
        private readonly IEnumerable<ICollector> _collectors;
        private readonly ILogger<Collector> _logger;
        private CancellationTokenSource _tokenSource = new CancellationTokenSource();
        private Task _worker;

        public Collector(IEnumerable<ICollector> collectors, ILogger<Collector> logger) 
        {
            _collectors = collectors;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _worker = Task.Run(Work);
            return Task.CompletedTask;
        }

        private async Task Work()
        {
            while (!_tokenSource.IsCancellationRequested)
            {
                foreach (var collector in _collectors)
                {
                    try
                    {
                        await collector.CollectAsync().ConfigureAwait(false);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"Error while executing collector '{collector.GetType().Name}'");
                    }

                }

                await Task.Delay(5000, _tokenSource.Token).ConfigureAwait(false);
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
