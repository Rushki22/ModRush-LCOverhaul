using HarmonyLib;

namespace ModRushOverhaul.Patches {

    // Slows down the day
    [HarmonyPatch(typeof(TimeOfDay))]
    internal class TimeOfDayPatch {

        [HarmonyPatch("Start")]
        [HarmonyPostfix]
        private static void longerDay(TimeOfDay __instance) { __instance.globalTimeSpeedMultiplier = 0.95f; } // -5%
    }
}