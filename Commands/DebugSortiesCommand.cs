using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using BepInEx.Configuration;
using NuclearOption.Networking;

namespace ServerTools.Commands
{
    internal class DebugSortiesCommand : PermissionConfigurableCommand
    {
        public DebugSortiesCommand(ConfigFile config) : base(config)
        {
        }

        public override string Name { get; } = "showsorties";

        public override string Description { get; } = "";

        public override string Usage { get; } = "";

        public override PermissionLevel PermissionLevelDefault { get; } = PermissionLevel.Owner;

        public override bool Execute(Player player, string[] args)
        {
            //NS.Plugin.ShowSorties();
            return true;
        }

        public override bool Validate(Player player, string[] args)
        {
            return true;
        }
    }
}
