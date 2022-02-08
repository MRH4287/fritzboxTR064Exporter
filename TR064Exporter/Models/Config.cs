using System;
using System.Collections.Generic;
using System.Text;

namespace TR064Exporter.Models
{
    public class Config
    {
        public string Ip { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public int? Port { get; set; }

        public int? Timeout { get; set; }

        public string LokiEndpoint { get; set; }

        public string LokiJobName { get; set; }

    }
}
