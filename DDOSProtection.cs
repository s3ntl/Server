using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mirage;

namespace ServerTools
{
    public static class DDOSProtection
    {
        private static Dictionary<INetworkPlayer, DateTime> _commandingTimeMap = new Dictionary<INetworkPlayer, DateTime>();
        
        private static TimeSpan _interval = TimeSpan.FromSeconds(1); // make it configurable
        private const int _cost = 1;         //make it configurable
        private const bool _setError = true; //make it configurable

        public static bool ShouldBeExecuted(INetworkPlayer player)
        {
            if (_commandingTimeMap.ContainsKey(player))
            {
                if (DateTime.UtcNow - _commandingTimeMap[player] >= _interval)
                {
                    _commandingTimeMap[player] = DateTime.UtcNow;
                    if (_setError)
                    {
                        player.SetError(_cost, PlayerErrorFlags.None);
                    }
                    return true;
                }
                return false;
            }

            _commandingTimeMap.Add(player, DateTime.UtcNow);
            return false;
        }
    }
}
