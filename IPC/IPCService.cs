using System;
using System.Collections.Concurrent;
using WebSocketSharp.Server;

namespace ServerTools.IPC
{
    public static class IPCService
    {
        private static WebSocketServer? _server;
        private static readonly ConcurrentDictionary<string, WebSocketSessionManager> _channels = new();

        public static bool Start(int port)
        {
            try
            {
                _server = new WebSocketServer($"ws://0.0.0.0:{port}");
                _server.AddWebSocketService<StatsWebSocketBehavior>("/stats");
                _server.AddWebSocketService<CommandWebSocketBehavior>("/cmd");
                _server.AddWebSocketService<LogsWebSocketBehavior>("/logs");
                _server.AddWebSocketService<LogsWebSocketBehavior>("/AClogs");
                _server.AddWebSocketService<LogsWebSocketBehavior>("/ACFrameslogs");
                _server.AddWebSocketService<LogsWebSocketBehavior>("/weaponTest");
                _server.Start();

                _channels["/stats"] = _server.WebSocketServices["/stats"].Sessions;
                _channels["/cmd"] = _server.WebSocketServices["/cmd"].Sessions;
                _channels["/logs"] = _server.WebSocketServices["/logs"].Sessions;
                _channels["/AClogs"] = _server.WebSocketServices["/AClogs"].Sessions;
                _channels["/ACFrameslogs"] = _server.WebSocketServices["/ACFrameslogs"].Sessions;


                _channels["/weaponTest"] = _server.WebSocketServices["/weaponTest"].Sessions;

                Plugin.logger.LogInfo($"IPC Server started on port {port}");
                return true;
            }
            catch (Exception ex)
            {
                Plugin.logger.LogError($"Failed to start IPC Server: {ex.Message}");
                return false;
            }
        }

        public static void Stop()
        {
            if (_server != null)
            {
                _server.Stop();
                Plugin.logger.LogInfo("IPC Server stopped.");
            }
        }

        public static void BroadcastChannel(string channel, string message)
        {
            if (_server?.IsListening != true) return;
            if (_channels.TryGetValue(channel, out var sessions))
                sessions.Broadcast(message);
        }
    }
}