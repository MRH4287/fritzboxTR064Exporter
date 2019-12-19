using PS.FritzBox.API;
using PS.FritzBox.API.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TR064Exporter.Clients
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1054:URI-Parameter dürfen keine Zeichenfolgen sein", Justification = "As in the API")]
    public class VoipClient : FritzTR64Client
    {
        public VoipClient(ConnectionSettings connectionSettings) : base(connectionSettings)
        {
        }

        
        public VoipClient(string url, int timeout) : base(url, timeout)
        {
        }

        public VoipClient(string url, int timeout, string username) : base(url, timeout, username)
        {
        }

        public VoipClient(string url, int timeout, string username, string password) : base(url, timeout, username, password)
        {
        }

        protected override string ControlUrl => "/upnp/control/x_voip";

        protected override string RequestNameSpace => "urn:dslforum-org:service:X_VoIP:1";

        public async Task CallNumber(string number)
        {
            await InvokeAsync("X_AVM-DE_DialNumber",
                new PS.FritzBox.API.SOAP.SoapRequestParameter("NewX_AVM-DE_PhoneNumber", number)
             ).ConfigureAwait(false);
        }

        public async Task HangUp()
        {
            await InvokeAsync("X_AVM-DE_DialHangup").ConfigureAwait(false);
        }
    }
}
