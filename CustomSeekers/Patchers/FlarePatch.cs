using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using UnityEngine;

namespace ImprovedMissiles.Patchers
{
    [HarmonyPatch(typeof(IRFlare))]
    public class FlarePatch
    {
        [HarmonyPatch("OnEnable")]
        [HarmonyPrefix]
        public static void OnEnablePrefix(IRFlare __instance)
        {

        }
    }
}
