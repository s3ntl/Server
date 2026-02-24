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
    public class BanCommandOffline : PermissionConfigurableCommand
    {
        public BanCommandOffline(ConfigFile config) : base(config)
        {
        }

        public override string Name { get; } = "banoffline";

        public override string Description { get; } = "";

        public override string Usage { get; } = "";

        public override PermissionLevel PermissionLevelDefault { get; } = PermissionLevel.Admin;

        public override bool Execute(Player player, string[] args)
        {
            string identifier = args[0];


            if (ulong.TryParse(identifier, out ulong id)) 
            { 
                ServerUtils.BanOffline(id);
                return true;
            }
            return false;
        }

        public override bool Validate(Player player, string[] args)
        {
            if (args.Length < 1) return false;
            return true;
        }
    }
}
