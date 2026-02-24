using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BepInEx.Configuration;
using NuclearOption.Networking;
using ServerTools.Services;
using ServerTools.Utils;

namespace ServerTools.Commands
{
    public class KickCommand : PermissionConfigurableCommand
    {
        public KickCommand(ConfigFile config) : base(config)
        {
        }

        public override string Name { get; } = "kick";

        public override string Description { get; } = "";

        public override string Usage { get; } = "";

        public override PermissionLevel PermissionLevelDefault { get; } = PermissionLevel.Moderator;

        public override bool Execute(Player player, string[] args)
        {
            

            Player targetPlayer;

            if (!PlayerService.TryGetPlayer(args, out targetPlayer))
            {
                ChatService.SendServerMessage($"Cant find player with such name or ID", player);
                return false;
            }

            ServerUtils.Kick(targetPlayer).Forget();
            ChatService.SendServerMessage($"Player {targetPlayer.PlayerName} kicked successfuly", player);
            return true;
        }

        public override bool Validate(Player player, string[] args)
        {
            if (args.Length < 1)
            {
                return false;
            }
            return true;
        }
    }
}
