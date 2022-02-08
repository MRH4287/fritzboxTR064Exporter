using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TR064Exporter.Clients;

namespace TR064Exporter.Collectors
{
    class HomeAutomationCollector : ICollector
    {
        private readonly TRClient<HomeAutomationClient> _client;

        public HomeAutomationCollector(TRClient<HomeAutomationClient> client)
        {
            _client = client;
        }

        public async Task CollectAsync()
        {
            var service = await _client.Get().ConfigureAwait(false);

            await service.Test().ConfigureAwait(false);

            
        }
    }
}
