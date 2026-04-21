using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BepInEx.Configuration;
using NuclearOption.Networking;
using NuclearOption.NetworkTransforms;
using ServerTools.IPC;
using ServerTools.Services;
using ServerTools.Utils;

namespace ServerTools.Commands
{
    public class TpTestCommand : PermissionConfigurableCommand
    {
        public TpTestCommand(ConfigFile config) : base(config)
        {
        }

        public override string Name { get; } = "tp";

        public override string Description { get; } = "";

        public override string Usage { get; } = "";

        public override PermissionLevel PermissionLevelDefault { get; } = PermissionLevel.Owner;

        public override bool Execute(Player player, string[] args)
        {
            if(!PlayerService.TryGetPlayer(args, out Player playerObject)) return false;
            try
            {
                if (player.Aircraft.TryGetComponent(out AircraftNetworkTransform netTransform))
                {
                    GlobalPosition pos = player.Aircraft.GlobalPosition();
                    pos.y = 10000;
                    NetworkTransformBase.NetworkSnapshot networkSnapshot = new NetworkTransformBase.NetworkSnapshot
                    {
                        globalPos = pos,


                    };
                    CallRpcResetClientAuthPosition(netTransform, networkSnapshot);


                }
                else
                {
                    IPCService.BroadcastChannel("/logs", "no networktransform component on aircraft");
                }

                return true;
            }
            catch (Exception ex)
            {
                IPCService.BroadcastChannel("/logs", $"exception: {ex.Message}");
                return false;
            }
        }

        public override bool Validate(Player player, string[] args)
        {
            if (args.Length < 1) return false;
            return true;
        }
        public static void CallRpcResetClientAuthPosition(AircraftNetworkTransform netTransform, NetworkTransformBase.NetworkSnapshot value)
        {
            if (netTransform == null)
                throw new ArgumentNullException(nameof(netTransform));

            var method = typeof(AircraftNetworkTransform).GetMethod("RpcResetClientAuthPosition",
                BindingFlags.NonPublic | BindingFlags.Instance);

            if (method == null)
                throw new MissingMethodException("RpcResetClientAuthPosition not found in AircraftNetworkTransform");

            method.Invoke(netTransform, new object[] { value });
        }
    }
}

