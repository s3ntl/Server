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
    public class SpawnCommand : PermissionConfigurableCommand
    {
        public SpawnCommand(ConfigFile config) : base(config)
        {
        }

        public override string Name { get; } = "spawn";

        public override string Description { get; } = "/spawn <PlayerName/ID> <weaponName> <count> <OwnerID>";

        public override string Usage { get; } = "";

        public override PermissionLevel PermissionLevelDefault { get; } = PermissionLevel.Admin;

        public override bool Execute(Player player, string[] args)
        {
            string[] identificator = new string[1];
            identificator[0] = args[0];
            string missileName = args[1];
            string count = args[2];

            string[] owner = new string[1];
            if (args.Length > 3)
            {
                owner[0] = args[3];
            }
            
            if (!PlayerService.TryGetPlayer(identificator, out Player target)) return false;
            if (!Int32.TryParse(count, out int result)) return false;
            if (!PlayerService.TryGetPlayer(owner, out Player ownerObject)) Plugin.logger.LogInfo("No owner");
            Plugin.IPCLog("Starting missile spawn", this);
            CustomSpawner.SpawnMissilesAtPlayer(missileName, result, target, ownerObject);
            return true;
        }

        public override bool Validate(Player player, string[] args)
        {
            if (args.Length < 3) return false;
            return true;
        }
    }
}
