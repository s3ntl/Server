using Cysharp.Threading.Tasks;
using Mirage;
using NuclearOption.Networking;
using Steamworks;
using UnityEngine;
using NuclearOption.DedicatedServer;

namespace ServerTools.Utils
{
    public static class ServerUtils
    {
        
        public static async UniTaskVoid Kick(Player player, string reason = "")
        {
            NuclearOption.Networking.NetworkManagerNuclearOption nm 
                = NuclearOption.Networking.NetworkManagerNuclearOption.i;

            
            if (!nm.Server.Active)
            {
                throw new MethodInvocationException("KickPlayerAsync called when server is not active");
            }

            INetworkPlayer conn = player.Owner;
            nm.Authenticator.OnKick(conn);
            Player localPlayer;
            string hostName = (GameManager.GetLocalPlayer<Player>(out localPlayer) ? localPlayer.PlayerName : "server");
            player.KickReason(reason, hostName);
            await UniTask.Delay(100);
            conn.Disconnect();
        }

        public static void Ban(Player player, string reason = "")
        {
            BanOffline(player.SteamID);
            Kick(player, reason).Forget();
        }

        public static void BanOffline(ulong id)
        {
            NetworkManagerNuclearOption.i.Authenticator.BanList.Add(new CSteamID(id), "mamy ebal");
            
        }
    }
}
