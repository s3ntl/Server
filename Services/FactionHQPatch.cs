using System;
using HarmonyLib;
using NuclearOption.SavedMission.ObjectiveV2.Outcomes;

namespace ServerTools.Services
{
    [HarmonyPatch(typeof(FactionHQ))]
    public class FactionHQPatch
    {
        public static Action onGameEnded;
        [HarmonyPatch("DeclareEndGame")]
        [HarmonyPrefix]
        public static void DeclareEndGamePatch(EndType endType)
        {
            onGameEnded?.Invoke();
        }
    }
}
