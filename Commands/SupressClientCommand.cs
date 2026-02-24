using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BepInEx.Configuration;
using NuclearOption.Networking;
using ServerTools.Data;
using ServerTools.Services;

namespace ServerTools.Commands
{
    public class SupressClientCommand : PermissionConfigurableCommand
    {
        public SupressClientCommand(ConfigFile config) : base(config)
        {
        }

        public override string Name { get; } = "ddos";

        public override string Description { get; } = "";

        public override string Usage { get; } = "";

        public override PermissionLevel PermissionLevelDefault { get; } = PermissionLevel.Admin;

        public override bool Execute(Player player, string[] args)
        {
            if (!PlayerService.TryGetPlayer(args, out var playerObject)) return false;

            PersonalOppressionMode.TogglePlayerInSuppressQueue(playerObject);
            return true;
        }

        public override bool Validate(Player player, string[] args)
        {
            return true;
        }
    }
}
