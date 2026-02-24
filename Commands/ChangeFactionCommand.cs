using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BepInEx.Configuration;
using NuclearOption.Networking;

namespace ServerTools.Commands
{
    public class ChangeFactionCommand : PermissionConfigurableCommand
    {
        public ChangeFactionCommand(ConfigFile config) : base(config)
        {
        }

        public override string Name { get; } = "changefaction";

        public override string Description { get; } = "";

        public override string Usage { get; } = "";

        public override PermissionLevel PermissionLevelDefault { get; }

        public override bool Execute(Player player, string[] args)
        {
           



            return true;
        }

        public override bool Validate(Player player, string[] args)
        {
            if (args.Length < 1 ) return false;
            return true;
        }
    }
}
