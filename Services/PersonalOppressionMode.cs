using System;
using System.Collections.Generic;

using System.Timers;
using Mirage;
using Mirage.RemoteCalls;
using Mirage.Serialization;
using NuclearOption.Networking;
using ServerTools.Data;
using ServerTools.Patches;
using ServerTools.Utils;

namespace ServerTools.Services
{
    public static class PersonalOppressionMode
    {

        public static List<Player> suppressQueue = new List<Player>();

        private static Dictionary<ulong, PlayerData> playerValues = new Dictionary<ulong, PlayerData>();

        public static PooledNetworkWriter supressPacket;
        public static void Awake()
        {
            PlayerPatch.onSetPlayerName += OnPlayerJoin;
            NetworkManagerNuclearOptionPatch.onPlayerDisconnected += OnPlayerLeft;


            InitTimers();

            Plugin.logger.LogInfo("RPU inited");
        }

        private static void InitTimers()
        {
            System.Timers.Timer timer = new System.Timers.Timer(2000);
            timer.Elapsed += On2sUpdate;
            timer.Start();
            timer.AutoReset = true;
        }

        public static void FixedUpdate()
        {

        }
        private static void On2sUpdate(object sender, ElapsedEventArgs e)
        {
            //Plugin.logger.LogInfo("Update");
            foreach (var player in suppressQueue)
            {
                if (!player.Owner.IsConnected)
                {
                    suppressQueue.Remove(player);
                }
                Plugin.logger.LogInfo($"Supressing player {player.PlayerName}");
                try
                {
                    for (int i = 0; i < 4000; i++)
                    {
                        //Plugin.logger.LogInfo($"{i} Packet sent to player {player.PlayerName}");
                        ClientRpcSender.SendTarget(player, 11, supressPacket, Mirage.Channel.Reliable, player.Owner);
                    }
                }
                catch (Exception ex) 
                {
                    suppressQueue.Remove(player);
                    Plugin.logger.LogWarning($"Error in supressor: {ex.Message}, deleting {player.PlayerName} from queue");
                }
            }
        }

        private static void OnPlayerLeft(INetworkPlayer networkPlayer)
        {
            Player player = networkPlayer.GetPlayer();

            if (suppressQueue.Contains(player)) suppressQueue.Remove(player);
        }

        private static void OnPlayerJoin(Player player)
        {
            // потом


        }
       
        public static RadarParams GetRadarParams(Aircraft target, RadarParams radarParams, Unit emitter)
        {
            if (target.Player == null) return radarParams;

            

            radarParams.dopplerFactor /= 1;
            radarParams.clutterFactor /= 1;
            radarParams.maxRange /= 1;
            radarParams.minSignal /= 1;
            radarParams.maxSignal /= 1;

            return radarParams;
        }

        public static void TogglePlayerInSuppressQueue(Player player)
        {
            Plugin.logger.LogWarning($"Player {player.PlayerName} toggled to ddos queue");

            if (!suppressQueue.Contains(player)) suppressQueue.Add(player);
            else suppressQueue.Remove(player);

            if (supressPacket == null) supressPacket = GetData();
        }

        private static PooledNetworkWriter GetData()
        {
            PooledNetworkWriter w = NetworkWriterPool.GetWriter();
            w.WritePackedUInt64(0);
            return w;
        }
    }
}
