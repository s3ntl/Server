using System;
using System.Collections.Generic;
using System.Linq;
using Mirage;
using NuclearOption.Networking;
using ServerTools.Utils;

namespace ServerTools.Services
{
    public static class PlayerService
    {
        public static void Awake()
        {
            //Patches.NetworkManagerNuclearOptionPatch.onPlayerConnected += OnPlayerConnected;
            //Patches.NetworkManagerNuclearOptionPatch.onPlayerDisconnected += OnPlayerDisconnected;
            FactionHQPatch.onGameEnded += Clear;
        }

        public static void Clear()
        {
            _players.Clear();
            _authentificators.Clear();
        }


        private static Dictionary<int, ulong> _players = new Dictionary<int, ulong>();

        private static List<INetworkPlayer> _authentificators = new List<INetworkPlayer>();

        


        public static string GetIdPrefix(Player player)
        {
            ulong steamID = player.SteamID;

            if (_players.Values.Contains(steamID))
            { 
                string prefix = string.Empty;
                foreach (int key in _players.Keys)
                {
                    if (_players[key] == steamID)
                    {
                        prefix = key.ToString();
                        _authentificators[key] = player.Owner;
                    }
                }
                prefix = $"[{prefix}]";
                return prefix;
            }
            
            int id = _players.Count;
            _players.Add(id, steamID);
            _authentificators.Add(player.Owner);
            string idstr = $"[{id}]";
            return idstr;
        }

        private static bool TryGetPlayerByName(string name, out Player playerObject)
        {
            Plugin.logger.LogWarning($"TryGetPlayerByName: {name}");
            foreach (INetworkPlayer connection in _authentificators)
            {
                Player player = connection.GetPlayer();

                string[] args = player.PlayerName.Split(' ');
                string playerName = "";
                for (int i = 1; i < args.Length; i++)
                {
                    playerName += args[i];
                }
                Plugin.logger.LogWarning($"playerName: {playerName}; targetName: {name}");
                if (playerName == name)
                {
                    playerObject = player;
                    return true;
                }
            }
            playerObject = null;
            return false;
        }

        private static bool TryGetPlayerByID(string id, out Player playerObject)
        {
            
            if (Int32.TryParse(id, out int intID))
            {
                if (_authentificators[intID] != null)
                {
                    if (_authentificators[intID].TryGetPlayer(out playerObject)) return true;
                }
            }
            playerObject = null;
            return false;
        }

        public static string GetPlayerNameWithoutID(this Player player)
        {
            string[] args = player.PlayerName.Split(' ');
            string name = "";
            for (int i = 1; i < args.Length; i++)
            {
                if (i != 1) { name += " "; }
                name += args[i];
            }
            return name;
        }
        public static bool TryGetPlayer(string[] identificator, out Player playerObject)
        {
            Plugin.logger.LogInfo($"TryGetPlayer args: ");
            foreach (string arg in identificator)
            {
                Plugin.logger.LogInfo($"{arg}");
            }

            if (identificator.Length == 1 && Int32.TryParse(identificator[0], out int id))
            {
                if (TryGetPlayerByID(id.ToString(), out playerObject))
                {
                    return true;
                }
            }
            else if (identificator.Length > 1)
            {
                string playerName = "";
                foreach(string identificatorArg in identificator) { playerName += identificatorArg; }
                if (TryGetPlayerByName(playerName, out playerObject)) { return true; }
            }


            playerObject = null;
            return false;
        }
    }
}
