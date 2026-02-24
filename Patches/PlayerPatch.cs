using System;
using HarmonyLib;
using NuclearOption.Networking;
using ServerTools.Services;

namespace ServerTools.Patches
{
    [HarmonyPatch(typeof(Player))]
    public static class PlayerPatch
    {
        public static Action<Player> onSetPlayerName;
        [HarmonyPatch("UserCode_CmdSetPlayerName_-1114485719")]
        [HarmonyPrefix]
        public static void CmdSetPlayerName(Player __instance, ref string playerName)
        {
            string idPrefix = PlayerService.GetIdPrefix(__instance);

            playerName = idPrefix + " " + playerName;

            onSetPlayerName?.Invoke(__instance);
        }
    }
}
