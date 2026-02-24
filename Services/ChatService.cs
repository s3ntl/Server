using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mirage.RemoteCalls;
using Mirage.Serialization;
using NuclearOption.Chat;
using NuclearOption.Networking;

namespace ServerTools.Services
{
    public static class ChatService
    {
        private static ChatManager chatManager;

        public static void Awake()
        {
            
        }

        public static void SendServerMessage(string message, Player targetPlayer = null)
        {
            chatManager = NetworkSceneSingleton<ChatManager>.i;

            PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
            writer.WriteString(message);
            writer.WriteBooleanExtension(true);

            if (targetPlayer != null) 
                ClientRpcSender.SendTarget(chatManager, 2, writer, Mirage.Channel.Reliable, targetPlayer.Owner);
            else ClientRpcSender.Send(chatManager, 2, writer, Mirage.Channel.Reliable, true);

            writer.Release();
        }

        
    }
}
