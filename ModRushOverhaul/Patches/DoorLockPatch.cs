using HarmonyLib;

namespace ModRushOverhaul.Patches {

    [HarmonyPatch(typeof(DoorLock))]
    internal class DoorLockPatch {

        // Opens doors quicker
        [HarmonyPatch("Awake")]
        [HarmonyPostfix]
        private static void instantUse(ref InteractTrigger ___doorTrigger) { ___doorTrigger.timeToHold = 0.0f; }
    }
}
