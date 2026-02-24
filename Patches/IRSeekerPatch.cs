using System.Runtime.InteropServices;
using HarmonyLib;
using NuclearOption.Networking;
using ServerTools.Utils;

namespace ServerTools.Patches
{
    //[HarmonyPatch(typeof(IRSeeker))]
    public class IRSeekerPatch
    {
        //[HarmonyPatch("IRSeeker_OnTargetFlare")]
        //[HarmonyPrefix]
        public static bool OnFlarePatch(IRSeeker __instance, IRSource source)
        {
            Missile missile = ReflectionUtils.ReadPrivateField<Missile>("missile", __instance);
            Unit targetUnit = ReflectionUtils.ReadPrivateField<Unit>("targetUnit", __instance);
            if (missile.owner.GetPlayer() != null)
            {
                Player player = missile.owner.GetPlayer();

            }
            return false;
        }
    }
}
