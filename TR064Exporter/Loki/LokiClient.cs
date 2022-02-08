using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TR064Exporter.Models;

namespace TR064Exporter.Loki
{
    internal class LokiClient
    {
        private readonly HttpClient _httpClient;
        private readonly Config _config;
        private readonly ILogger<LokiClient> _logger;
        private Uri _uri;

        private Regex _logTimeRegex = new Regex(@".*?([0-9]+\.[0-9]+\.[0-9]+\s[0-9]+:[0-9]+:[0-9]+)\s?(.*)");

        public LokiClient(HttpClient httpClient, Config config, ILogger<LokiClient> logger)
        {
            _httpClient = httpClient;
            _config = config;
            _logger = logger;
            if (!string.IsNullOrWhiteSpace(_config.LokiEndpoint))
            {
                var baseUri = new Uri(_config.LokiEndpoint);
                _uri = new Uri(baseUri, "/loki/api/v1/push");
            }
        }

        public async Task PostLogs(IEnumerable<string> logs)
        {
            if (_uri == null)
            {
                throw new InvalidOperationException("Loki-Endpoint not set");
            }

            var filteredLogs = logs
                .Select(GetWithOffset)
                .Where(entry => (DateTimeOffset.UtcNow - entry.Item1).TotalHours <= 2)
                .Select(GetWithTimestamp)
                .ToList();

            if (filteredLogs.Count == 0)
            {
                return;
            }

            var request = GenerateRequestString(filteredLogs);
            var response = await _httpClient.PostAsync(_uri, new StringContent(request, Encoding.UTF8, "application/json"));

            if (!response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                _logger.LogWarning("Can't send logs to Loki: {0} -- {1}", response.ReasonPhrase, body);
            }
        }



        private (DateTimeOffset, string) GetWithOffset(string log)
        {
            var match = _logTimeRegex.Match(log);
            if (match.Success)
            {
                var time = DateTimeOffset.Parse(match.Groups[1].Value, System.Globalization.CultureInfo.GetCultureInfo("de-DE"));
                return (time, match.Groups[2].Value);
            }

            DateTimeOffset now = DateTimeOffset.UtcNow;
            return (now, log);
        }

        private (long, string) GetWithTimestamp((DateTimeOffset, string) input)
        {
            return (input.Item1.ToUnixTimeMilliseconds() * 1000000, input.Item2);
        }

        private string GenerateRequestString(IEnumerable<(long, string)> logs)
        {
            var values = logs.Select(kvp =>
            {
                return new string[] { kvp.Item1.ToString(System.Globalization.CultureInfo.InvariantCulture), kvp.Item2 };
            }).ToArray();

            var request = new Dictionary<string, object>
            {
                ["streams"] = new Dictionary<string, object>[]
                {
                    new Dictionary<string, object>
                    {
                        ["stream"] = new Dictionary<string, string>
                        {
                            ["job"] = _config.LokiJobName ?? "fritzbox"
                        },
                        ["values"] = values
                    }
                }
            };

            return System.Text.Json.JsonSerializer.Serialize(request);
        }
    }
}
