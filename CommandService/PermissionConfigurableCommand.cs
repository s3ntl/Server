using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BepInEx.Configuration;
using NuclearOption.Networking;

namespace ServerTools.Commands
{
    public abstract class PermissionConfigurableCommand : ICommand
    {
        public abstract string Name { get; }

        public abstract string Description { get; }

        public abstract string Usage { get; }

        public abstract PermissionLevel PermissionLevelDefault { get; }

        public abstract bool Execute(Player player, string[] args);
        

        public abstract bool Validate(Player player, string[] args);
        
        private ConfigEntry<PermissionLevel> PermissionLevelConfig { get; }
        
        public PermissionLevel PermissionLevel
        {
            get
            {
                return this.PermissionLevelConfig.Value;
            }
        }

        protected PermissionConfigurableCommand(ConfigFile config)
        {
            this.PermissionLevelConfig = config.Bind<PermissionLevel>("Commands",
                this.Name, this.PermissionLevelDefault,
                "The permission level required to execute the " + this.Name + " command.");
        }
    }
}
