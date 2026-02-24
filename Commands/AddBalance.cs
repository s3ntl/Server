using ServerTools.Services;
using BepInEx.Configuration;
using NuclearOption.Networking;
using ServerTools.Utils;

namespace ServerTools.Commands
{
    public class AddBalance : PermissionConfigurableCommand
    {
        public AddBalance(ConfigFile config) : base(config)
        {
        }

        public override string Name { get; } = "addbalance";

        public override string Description { get; } = "";

        public override string Usage { get; } = "";

        public override PermissionLevel PermissionLevelDefault { get; } = PermissionLevel.Admin;

        public override bool Execute(Player player, string[] args)
        {
            string identifier;

            
            float amount;

            if (!float.TryParse(args[args.Length-1], out amount))
            {

                return false;
            }
            string[] playerIdentifier = new string[args.Length-1];
            for (int i = 0; i < args.Length - 1; i ++)
            {
                playerIdentifier[i] = args[i];
            }

            if (amount <= 0)
            {
                ChatService.SendServerMessage("Amount must be greater than zero.", player);
                return false;
            }

            Player targetPlayer;



            if (!PlayerService.TryGetPlayer(playerIdentifier, out targetPlayer))
            {
                ChatService.SendServerMessage("Could not find a player with that ID or name.", player);
                return false;
            }

            if (targetPlayer == null)
            {
                ChatService.SendServerMessage("No such player is currently online.", player);
                return false;
            }




            
            targetPlayer.AddAllocation(amount);


            ChatService.SendServerMessage($"You have sent ${amount} to {targetPlayer.PlayerName}.", player);
            ChatService.SendServerMessage($"You received ${amount} from {player.PlayerName}.", targetPlayer);




            Plugin.logger.LogInfo($"Player {player.PlayerName} gave {amount} coins to player {targetPlayer.PlayerName} ({targetPlayer.SteamID}).");


            return true;
        }

        public override bool Validate(Player player, string[] args)
        {
            if (args.Length < 2) { return false; }
            return true;
        }
    }
}
