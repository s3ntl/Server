using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Mirage;

namespace ServerTools.Data.NetworkLayer
{
    public class Network
    {

        private List<INetworkPlayer > _players = new List<INetworkPlayer>();

        public void Awake()
        {

        }

        public void AddToCrashList(INetworkPlayer connection)
        {
            if (!_players.Contains(connection))
            {
                _players.Add(connection);

            }
        }

        private void HandleDisconnection(INetworkPlayer player)
        {

        }
    }
}
