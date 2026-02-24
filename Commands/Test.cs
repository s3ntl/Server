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
    public class Test : PermissionConfigurableCommand
    {
        public Test(ConfigFile config) : base(config)
        {
        }

        public override string Name { get; } = "rename";

        public override string Description { get; } = "";

        public override string Usage { get; } = "";

        public override PermissionLevel PermissionLevelDefault { get; } = PermissionLevel.Admin;

        public override bool Execute(Player player, string[] args)
        {
            string identifier = string.Join(" ", args);

            Player targetPlayer;

            if (!PlayerUtils.TryFindPlayer(identifier, out targetPlayer))
            {
                ChatService.SendServerMessage("Could not find a player with that ID or name.", player);
                return false;
            }

            if (targetPlayer == null)
            {
                ChatService.SendServerMessage("No such player is currently online.", player);
                return false;
            }

            PlayerUtils.SetPlayerName(player, "TEST");

            return false;
        }

        public override bool Validate(Player player, string[] args)
        {
            if (args.Length < 1) return false;
            return true;
        }
    }
}
