using PS.FritzBox.API.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TR064Exporter
{
    class TRClient<TClient>
        where TClient: FritzTR64Client
    {
        private readonly Connection _connection;


        public TRClient(Connection connection)
        {
            _connection = connection;
        }

        public Task<TClient> Get()
            => _connection.GetService<TClient>();
    }
}
