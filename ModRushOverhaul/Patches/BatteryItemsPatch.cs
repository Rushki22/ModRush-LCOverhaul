using HarmonyLib;
using ModRushOverhaul.enums;

namespace ModRushOverhaul.Patches {
    internal class BatteryItemsPatch {
        
        // This is just an expansion for battary life-time on some items
        
        [HarmonyPatch(typeof(FlashlightItem), "ItemActivate")]
        [HarmonyPrefix]
        private static void flashlightBattery(ref FlashlightItem __instance) {

            Flashlights flashlightID = (Flashlights)__instance.flashlightTypeID;
            switch (flashlightID) {

                case Flashlights.ProFlashlight:
                    __instance.itemProperties.batteryUsage = 450;
                    break;

                case Flashlights.Flashlight:
                    __instance.itemProperties.batteryUsage = 210;
                    break;

                case Flashlights.LaserPointer:
                    __instance.itemProperties.batteryUsage = 275;
                    break;

                default:
                    break;
            }
        }

        [HarmonyPatch(typeof(WalkieTalkie), "ItemActivate")]
        [HarmonyPostfix]
        private static void walkieTalkieBattery(ref WalkieTalkie __instance) {
            __instance.itemProperties.batteryUsage = 500;
        }

        [HarmonyPatch(typeof(JetpackItem), "ItemActivate")]
        [HarmonyPrefix]
        private static void jetpackFuel(ref JetpackItem __instance) {
            __instance.itemProperties.batteryUsage = 180;
        }

        [HarmonyPatch(typeof(BoomboxItem), "ItemActivate")]
        [HarmonyPrefix]
        private static void hardbassLife(ref BoomboxItem __instance) {
            __instance.itemProperties.batteryUsage = 250;
        }
    }
}