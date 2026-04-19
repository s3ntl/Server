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

        public override string Name { get; } = "test";

        public override string Description { get; } = "";

        public override string Usage { get; } = "";

        public override PermissionLevel PermissionLevelDefault { get; } = PermissionLevel.Admin;

        public override bool Execute(Player player, string[] args)
        {
            string factionName = args[0];
            FactionHQ faction = FactionRegistry.HqFromName(factionName);
            if (faction != null) faction.DeclareEndGame(NuclearOption.SavedMission.ObjectiveV2.Outcomes.EndType.Victory);

            return false;
        }

        public override bool Validate(Player player, string[] args)
        {
            if (args.Length != 1) return false;
            return true;
        }
    }
}
