using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp.Server;
using WebSocketSharp;

namespace ServerTools.IPC
{
    public class LogsWebSocketBehavior : WebSocketBehavior
    {
        protected override void OnMessage(MessageEventArgs e)
        {
            
            Plugin.logger.LogDebug($"Get data from client: {e.Data}");
        }

        protected override void OnOpen()
        {
            Plugin.logger.LogInfo("Client connected to logs channel");
            Send("Connection approved");
        }
    }
}
