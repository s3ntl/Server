using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BepInEx.Configuration;
using NuclearOption.Networking;
using ServerTools.Services;

namespace ServerTools.Commands
{
    public class SayCommand : PermissionConfigurableCommand
    {
        public SayCommand(ConfigFile config) : base(config)
        {
        }

        public override string Name { get; } = "say";

        public override string Description { get; } = "";

        public override string Usage { get; } = "";

        public override PermissionLevel PermissionLevelDefault { get; } = PermissionLevel.Moderator;

        public override bool Execute(Player player, string[] args)
        {
            string line = "";

            foreach (string arg in args)
            {
                line += arg + " ";

            }

            ChatService.SendServerMessage(line);

            return true;
        }

        public override bool Validate(Player player, string[] args)
        {
            if (args.Length > 0) { return true; } return false;
        }
    }
}
