using System;
using HarmonyLib;

namespace ServerTools.Patches
{
    [HarmonyPatch(typeof(Missile))]
    public class MissilePatch
    {
        public static Action<Missile> OnMissileAwake;
        [HarmonyPatch("Awake")]
        [HarmonyPostfix]
        public static void AwakePostfix(Missile __instance)
        {
            OnMissileAwake?.Invoke(__instance);
        }
    }
}
