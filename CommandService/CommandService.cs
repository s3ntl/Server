using System;
using System.Collections.Generic;
using System.Linq;

using NuclearOption.Networking;

namespace ServerTools.Commands
{
    public static class CommandService
    {
        private static readonly List<ICommand> commands = new List<ICommand>();
        public static void RegisterChatCommand(ICommand command)
        {
            commands.Add(command);
        }

        public static IEnumerable<ICommand> GetChatCommands() 
        {
            return commands.AsReadOnly();
        }

        public static PermissionLevel GetPermissionLevel(Player player)
        {
           // Plugin.logger.LogInfo("Trying to get permission level");
            PermissionLevel level;
           // Plugin.logger.LogInfo("checking owner");
            if (Plugin.Owner.Value == player.SteamID.ToString())
            {
                level =  PermissionLevel.Owner;
            }
           // Plugin.logger.LogInfo("checking admin");
            else if (Plugin.AdminsIDS.Contains(player.SteamID.ToString()))
            {
                level = PermissionLevel.Admin;
            }
           // Plugin.logger.LogInfo("checking moderator");
             else if (Plugin.ModeratorsIDS.Contains(player.SteamID.ToString()))
            {
                level = PermissionLevel.Moderator;
            }

            else level = PermissionLevel.Everyone;
           // Plugin.logger.LogInfo($"level: {level.ToString()}");
            return level;
        }

        public static bool TryGetCommand(string commandName, out ICommand command)
        {
            Plugin.IPCLog($"[CommandService] Trying to get command {commandName}. Total commands in list: {commands.Count}" +
                $"\nCommand 1: {commands[0].Name}");
            command = commands.Find((ICommand c) => string.Equals(c.Name,commandName, StringComparison.CurrentCultureIgnoreCase));
            return command != null;
        }

        public static bool TryExecuteCommand(Player player, string commandName, string[] args)
        {
            string line = "";
            foreach (string arg in args)
            {
                line += arg + " ";
            }
            Plugin.IPCLog($"[CommandService] Trying to execute command {commandName}, args {line}");
            ICommand command;
            if (!TryGetCommand(commandName, out command))
            {
                Plugin.IPCLog($"[CommandService] Command {commandName} not found.");
                return false;
            }
            if (GetPermissionLevel(player) < command.PermissionLevelDefault)
            {
                Plugin.IPCLog($"[CommandService] Player {player.PlayerName} tried to execute {commandName} with no permissions");
                return false;
            }
            if (!command.Validate(player, args))
            {
                Plugin.IPCLog($"[CommandService] Command {commandName} by player {player.PlayerName} not validated. Args: {args}");
                return false;
            }
            Plugin.IPCLog($"[CommandService] Begining execute command {commandName}");
            if (command.Execute(player, args))
            {
                Plugin.IPCLog($"[CommandService] Command {commandName} executed by {player.PlayerName}. Args: {args}");
                return true;
            }
            Plugin.IPCLog($"[CommandService] Unknown error, command: {commandName}, args: {args}");
            return false;
        }
        public static bool TryExecuteCommand(Player player, string message)
        {
            
            string[] array = message.Split(new char[] { ' ' }, StringSplitOptions.None);
            string commandName = array[0];
            string[] args = array.Skip(1).ToArray<string>();
            return TryExecuteCommand(player, commandName, args);
        }
    }
}
