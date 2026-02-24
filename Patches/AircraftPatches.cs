using HarmonyLib;
using ServerTools.Services;
using UnityEngine;

namespace ServerTools.Patches
{
    [HarmonyPatch(typeof(Aircraft))]
    public class AircraftPatches
    {
        [HarmonyPatch("GetRadarReturn")]
        [HarmonyPrefix]
        public static void GetRadarReturn(Aircraft __instance, Vector3 source, Radar radar, Unit emitter, float dist, float clutter, ref RadarParams radarParams, bool triggerWarning)
        {
            radarParams = PersonalOppressionMode.GetRadarParams(__instance, radarParams, emitter);
        }
    }
}
