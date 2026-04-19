using System;
using HarmonyLib;

namespace ServerTools.Patches
{
    [HarmonyPatch(typeof(Missile))]
    public class MissilePatch
    {
        public static Action<Missile> OnAwakeMissile;
        [HarmonyPatch("Awake")]
        [HarmonyPostfix]
        public static void OnEnablePostfix(Missile __instance)
        {
            OnAwakeMissile?.Invoke(__instance);
        }
    }
}
