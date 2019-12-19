using Microsoft.Extensions.Configuration;
using PS.FritzBox.API;
using PS.FritzBox.API.Base;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TR064Exporter.Models;

namespace TR064Exporter
{
    public class Connection
    {
        private readonly IPAddress _ip;
        private readonly string _baseAddress;
        private readonly DeviceFactory _deviceFactory = new DeviceFactory();
        private readonly string _username;
        private readonly string _password;
        private readonly int _timeout;
        private FritzDevice _device;

        public Connection(Config config)
        {
            if (config is null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            if (string.IsNullOrEmpty(config.Ip))
            {
                throw new ArgumentException("The Parameter ip needs to be set. Example: dotent run ip=192.168.1.1 username=admin password=test");
            }

            if (string.IsNullOrEmpty(config.Username))
            {
                throw new ArgumentException("The Parameter username needs to be set. Example dotent run ip=192.168.1.1 username=admin password=test");
            }

            if (string.IsNullOrEmpty(config.Password))
            {
                throw new ArgumentException("The Parameter password needs to be set. Example: dotent run ip=192.168.1.1 username=admin password=test");
            }

            var port = config.Port ?? 49000;
            var timeout = config.Timeout ?? 10;
            _ip = IPAddress.Parse(config.Ip);
            var urlBuilder = new UriBuilder("http", config.Ip, port);
            _baseAddress = urlBuilder.ToString();
            _username = config.Username;
            _password = config.Password;
            _timeout = timeout;
        }

        public Task<FritzDevice> Connect()
        {
            return _deviceFactory.CreateDeviceAsync(_ip);
        }

        public async Task<T> GetService<T>()
            where T : FritzTR64Client
        {
            _device ??= await Connect().ConfigureAwait(false);
            var config = new ConnectionSettings
            {
                BaseUrl = _baseAddress,
                Password = _password,
                UserName = _username,
                Timeout = _timeout
            };
            return await _device.GetServiceClient<T>(config).ConfigureAwait(false);
        }
    }
}
