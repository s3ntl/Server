using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BepInEx.Configuration;
using BepInEx.Logging;
using NuclearOption.Networking;
using ServerTools.Services;


namespace ServerTools.Commands
{
    public class SendBalance : PermissionConfigurableCommand
    {
        public SendBalance(ConfigFile config) : base(config)
        {
        }

        public override string Name { get; } = "sendbalance";

        public override string Description { get; } = "sending balance to user.";

        public override string Usage { get; } = "send <player_steamID/username> <sum>";

        public override PermissionLevel PermissionLevelDefault { get; } = PermissionLevel.Everyone;

        public override bool Execute(Player player, string[] args)
        {
            Plugin.logger.LogWarning("Executing sendbalance");

            if (!Int32.TryParse(args[args.Length - 1], out var amount))
            {
                ChatService.SendServerMessage("No amount", player);
                return false;
            }

            List<string> list = new List<string>();

            for (int i = 0; i < args.Length - 1; i++)
            {
                list.Add(args[i]);
            }

            string[] identity = list.ToArray();

            if (!PlayerService.TryGetPlayer(identity, out Player targetPlayer))
            {
                ChatService.SendServerMessage("No player with such name or id founded", targetPlayer);
                return false;
            }

            if (amount < 0)
            {
                ChatService.SendServerMessage("amount must be greater than zero", targetPlayer);
                return false;
            }

            if (player == targetPlayer)
            {
                ChatService.SendServerMessage($"Cant send balance to yourself", targetPlayer);
                return false;
            }

            player.AddAllocation(-amount);
            targetPlayer.AddAllocation(amount);
            ChatService.SendServerMessage($"Player {player.GetPlayerNameWithoutID()} sent you ${amount}kk", targetPlayer);
            ChatService.SendServerMessage($"You sent ${amount}kk to {targetPlayer.GetPlayerNameWithoutID()}", player);

            return true;

        }

        public override bool Validate(Player player, string[] args)
        {
            if (args.Length >= 2)
            {
                Plugin.logger.LogWarning($"Sendbalance validated");
                return true;
            }
            Plugin.logger.LogError($"Sendbalance not validated");
            return false;
        }
    }
}
