using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using WebSocketSharp.Server;

namespace ServerTools.IPC
{
    public static class IPCService
    {
        private static WebSocketServer? _server;
        private static readonly ConcurrentDictionary<string, WebSocketSessionManager> _channels = new();

        public static void Start(int port)
        {
            _server = new WebSocketServer($"ws://0.0.0.0:{port}");
            _server.AddWebSocketService<StatsWebSocketBehavior>("/stats");
            _server.AddWebSocketService<CommandWebSocketBehavior>("/cmd");
            _server.Start();

            _channels["/stats"] = _server.WebSocketServices["/stats"].Sessions;
            _channels["/cmd"] = _server.WebSocketServices["/cmd"].Sessions;

        }

        public static void Stop()
        {
            _server?.Stop();
        }

        
        public static void Broadcast(string channel, string message)
        {
            if (_channels.TryGetValue(channel, out var sessions))
            {
                foreach (var session in sessions.Sessions)
                    session.Context.WebSocket.Send(message);
            }
        }

    }
}
