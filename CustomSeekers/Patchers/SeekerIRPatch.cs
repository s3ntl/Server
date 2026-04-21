using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using ImprovedMissiles.States;
using ImprovedMissiles.Utils;

namespace ImprovedMissiles.Patchers
{
    [HarmonyPatch(typeof(IRSeeker))]
    public class SeekerIRPatch
    {
        [HarmonyPatch("Initialize")]
        [HarmonyPostfix]
        public static void Init(IRSeeker __instance, Unit target, GlobalPosition aimpoint)
        {
            CustomIRSeekerTest seeker = new CustomIRSeekerTest(__instance);
            if (seeker == null)
            {
                Plugin.logger.LogInfo("seeker is null");
            }
            
        }
        [HarmonyPatch("IRSeeker_OnTargetFlare")]
        [HarmonyPrefix]
        public static bool IRSeeker_OnTargetFlarePrefix(IRSeeker __instance, IRSource source)
        {
            MissileManager.GetBehaviour(__instance).IR_Seeker_OnTargetFlare(source);
            return false;
        }

        [HarmonyPatch("OnDestroy")]
        [HarmonyPostfix]
        public static void OnDestroyPostfix(IRSeeker __instance)
        {
            //потом очистку памяти сделаю
        }
    }
}
