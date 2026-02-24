using HarmonyLib;
using Mirage;

namespace ServerTools.Patches
{
    [HarmonyPatch(typeof(UnitCommand))]
    public class Patch
    {
        [HarmonyPatch("UserCode_CmdSetDestination_1791143641")]
        [HarmonyPrefix]
        public static bool CmdSetDestination(GlobalPosition waypoint, INetworkPlayer sender)
        {
            return DDOSProtection.ShouldBeExecuted(sender);
        }
    }
}
