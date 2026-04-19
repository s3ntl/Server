using System;
using System.Globalization;
using System.Linq;
using Mirage;
using NuclearOption.Networking;



namespace ServerTools.Utils
{
    public static class PlayerUtils
    {
        public static Player GetPlayer(this INetworkPlayer player)
        {
            NetworkIdentity identity = player.Identity;
            return (identity != null) ? identity.GetComponent<Player>() : null;
        }
        public static bool TryFindPlayer(string identifier, out Player playerObject)
        {
           


           // if (PlayerUtils.TryFindPlayerByName(identifier, out playerObject)) return true;
           // if (PlayerUtils.TryFindPlayerBySteamID(identifier, out playerObject)) return true;

            playerObject = null;
            return false;
        }

        public static bool TryFindPlayerByName(string playerName, out Player playerObject)
        {
            INetworkPlayer networkPlayer = NetworkManagerNuclearOption.i.Server.
                AuthenticatedPlayers.FirstOrDefault(delegate (INetworkPlayer p)
            {
                Player player = p.GetPlayer();
                return string.Equals((player != null) ? player.PlayerName : null, playerName, StringComparison.CurrentCultureIgnoreCase);
            });
            playerObject = ((networkPlayer != null) ? networkPlayer.GetPlayer() : null);
            return playerObject != null;
        }

        public static bool TryFindPlayerBySteamID(string steamID, out Player playerObject)
        {
            
            INetworkPlayer matchingPlayer = NetworkManagerNuclearOption.i.Server.AuthenticatedPlayers
                .FirstOrDefault(p =>
                    p.GetPlayer()?.SteamID.ToString().Equals(steamID, StringComparison.OrdinalIgnoreCase) ?? false);

            playerObject = matchingPlayer?.GetPlayer();
            return playerObject != null;
        }

        public static bool TryGetPlayerIDOrNameAndSum(string[] args, out string identifier, out float value)
        {
            identifier = "";
            value = 0f;

            if (args.Length < 2)
            {
                return false;
            }

            if (long.TryParse(args[0], out _))
            {
                identifier = args[0];
                return float.TryParse(args[1], NumberStyles.Float, CultureInfo.InvariantCulture, out value);
            }
            else
            {
                value = float.Parse(args[args.Length - 1], NumberStyles.Float, CultureInfo.InvariantCulture);
                identifier = string.Join(" ", args.Take(args.Length - 1));
                return true;
            }
        } 

        public static void SetPlayerName(Player player, string name) //не работает короче
        {
            string newName = name.SanitizeRichText(32).ReplaceCharactersNotInFont(GameAssets.i.playerNameFont);

            var method = Utils.ReflectionUtils.CreatePrivateMethodDelegate(typeof(Action<string>), "set_Network<PlayerName> k__BackingField", player);

            method.DynamicInvoke(name);
            
        }
    }
}
