using PS.FritzBox.API;
using PS.FritzBox.API.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TR064Exporter.Clients
{
    public class HomeAutomationClient : FritzTR64Client
    {
        public HomeAutomationClient(ConnectionSettings connectionSettings) : base(connectionSettings)
        {
        }

        public HomeAutomationClient(string url, int timeout) : base(url, timeout)
        {
        }

        public HomeAutomationClient(string url, int timeout, string username) : base(url, timeout, username)
        {
        }

        public HomeAutomationClient(string url, int timeout, string username, string password) : base(url, timeout, username, password)
        {
        }

        protected override string ControlUrl => "/upnp/control/x_homeauto";

        protected override string RequestNameSpace => "urn:X_AVM-DE_Homeauto-com:serviceId:X_AVM-DE_Homeauto1";

        public async Task Test()
        {
            var document = await InvokeAsync("GetInfo", null).ConfigureAwait(false);

            Console.WriteLine(document.ToString());

        }
    }
}
