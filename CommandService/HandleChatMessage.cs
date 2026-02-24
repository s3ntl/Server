using HarmonyLib;
using Mirage;
using NuclearOption.Chat;
using NuclearOption.Networking;

namespace ServerTools.Commands
{
    [HarmonyPatch(typeof(ChatManager))]
    internal class HandleChatMessage
    {
        [HarmonyPatch("UserCode_CmdSendChatMessage_-456754112")]
        [HarmonyPrefix]
        public static bool ChatMessage(string message, bool allChat, INetworkPlayer sender)
        {
            Player player;
            if (!sender.TryGetPlayer(out player))
            {
                Plugin.logger.LogWarning("Player component is null");
                return true;
            }

            if (message.StartsWith(Plugin.CommandPrefix.Value.ToString()) && message.Length > 1)
            {
                //Plugin.logger.LogInfo("Trying to execute command..");
                if (CommandService.TryExecuteCommand(player, message.Substring(1)))
                {
                    return false;
                }
                return false;
            }

            if (!allChat) Plugin.logger.LogInfo($"Player {player.PlayerName} sent message: {message}");
            else Plugin.logger.LogInfo($"Player {player.PlayerName} sent message to faction chat: {message}");
            return true;
        }
    }
}
