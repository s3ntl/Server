using ServerTools.Utils;
using BepInEx.Configuration;
using NuclearOption.Networking;
using ServerTools.Services;

namespace ServerTools.Commands
{
    public class BanCommand : PermissionConfigurableCommand
    {
        public BanCommand(ConfigFile config) : base(config)
        {
        }

        public override string Name { get; } = "ban";

        public override string Description { get; } = "";

        public override string Usage { get; } = "";

        public override PermissionLevel PermissionLevelDefault { get; } = PermissionLevel.Admin;

        public override bool Execute(Player player, string[] args)
        {
            string identifier = string.Join(" ", args);

            Player targetPlayer;

            if (!PlayerService.TryGetPlayer(args, out targetPlayer))
            {
                ChatService.SendServerMessage("Could not find a player with that ID or name.", player);
                return false;
            }

            if (targetPlayer == null)
            {
                ChatService.SendServerMessage("No such player is currently online.", player);
                return false;
            }

            ServerUtils.Ban(targetPlayer);
            return true;
        }

        public override bool Validate(Player player, string[] args)
        {
            if (args.Length < 1) return false;
            return true;
        }
    }
}
