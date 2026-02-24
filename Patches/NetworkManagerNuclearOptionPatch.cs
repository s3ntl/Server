using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using Mirage;
using NuclearOption.Networking;

namespace ServerTools.Patches
{
    [HarmonyPatch(typeof(NetworkManagerNuclearOption))]
    public class NetworkManagerNuclearOptionPatch
    {
        public static Action<INetworkPlayer> onPlayerConnected;
        public static Action<INetworkPlayer> onPlayerDisconnected;
        [HarmonyPatch("LogServerConnected")]
        [HarmonyPrefix]
        public static void LogServerConnectedPatch(INetworkPlayer player)
        {
            Plugin.logger.LogInfo($"LogServerConnected: {player}");

            onPlayerConnected?.Invoke(player);
        }


        [HarmonyPrefix]
        [HarmonyPatch("LogServerDisconnected")]
        public static void LogServerDisconnected(INetworkPlayer player)
        {
            onPlayerDisconnected?.Invoke(player);
            Plugin.logger.LogInfo($"LogServerDisconnected: {player}");
        }

    }
}
