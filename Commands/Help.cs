using ServerTools.Services;
using BepInEx.Configuration;
using NuclearOption.Networking;

namespace ServerTools.Commands
{
    public class Help : PermissionConfigurableCommand
    {
        public Help(ConfigFile config) : base(config)
        {
        }

        public override string Name { get; } = "help";

        public override string Description { get; } = "";

        public override string Usage { get; } = "";

        public override PermissionLevel PermissionLevelDefault { get; } = PermissionLevel.Everyone;

        public override bool Execute(Player player, string[] args)
        {
            string message =  "";
            foreach(ICommand command in CommandService.GetChatCommands())
            {
                if (CommandService.GetPermissionLevel(player) < command.PermissionLevelDefault || command.Name == "help") continue;
                message += command.Name + " ";
            }
            ChatService.SendServerMessage(message, player);
            return true;
        }

        public override bool Validate(Player player, string[] args)
        {
            return true;
        }
    }
}
