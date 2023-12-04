using GameNetcodeStuff;
using HarmonyLib;
using UnityEngine;

namespace ModRushOverhaul.Patches {

    [HarmonyPatch(typeof(PlayerControllerB))]
    internal class PlayerControllerBPatch {

        // Save data
        private static float currentPlayerWeight;
        private static float newWeight;

        private static float playerSprintMeter;

        // Settings
        private static float drianMultipier = 0.7f; // -30%
        private static float regenMultipier = 1.2f; // +20%
        private static float weightMultipier = 0.9f; // -10%
        private static float jumpMultipier = 0.7f; // -30%

        // Start of stamina patch
        // Get Values
        [HarmonyPatch("Update")]
        [HarmonyPrefix]
        public static void getStamina(PlayerControllerB __instance) {

            if (__instance.IsOwner && __instance.isPlayerControlled) {

                playerSprintMeter = __instance.sprintMeter;
                currentPlayerWeight = __instance.carryWeight;

                newWeight = Mathf.Max(__instance.carryWeight * weightMultipier, 1f);

                __instance.carryWeight = newWeight;
            }
        }

        // Set Values
        [HarmonyPatch("Update")]
        [HarmonyPostfix]
        public static void setStamina(PlayerControllerB __instance) {

            if (__instance.IsOwner && __instance.isPlayerControlled) {

                float num = __instance.sprintMeter - playerSprintMeter;

                if (num < 0f)
                    __instance.sprintMeter = Mathf.Max(playerSprintMeter + num * drianMultipier, 0f);
                else if (num > 0f)
                    __instance.sprintMeter = Mathf.Min(playerSprintMeter + num * regenMultipier, 1f);

                __instance.carryWeight = currentPlayerWeight + (__instance.carryWeight - newWeight);
            }
        }

        // Get Values
        [HarmonyPatch("LateUpdate")]
        [HarmonyPrefix]
        public static void getLateUpdateStamina(PlayerControllerB __instance) {

            if (__instance.IsOwner && __instance.isPlayerControlled) {

                playerSprintMeter = __instance.sprintMeter;
                currentPlayerWeight = __instance.carryWeight;

                newWeight = Mathf.Max(__instance.carryWeight * weightMultipier, 1f);

                __instance.carryWeight = newWeight;
            }
        }

        // Set Values
        [HarmonyPatch("LateUpdate")]
        [HarmonyPostfix]
        public static void setLateUpdateStamina(PlayerControllerB __instance) {

            if (__instance.IsOwner && __instance.isPlayerControlled) {

                float num = __instance.sprintMeter - playerSprintMeter;

                if (num < 0f) __instance.sprintMeter = Mathf.Max(playerSprintMeter + num * drianMultipier, 0f);
                else if (num > 0f) __instance.sprintMeter = Mathf.Min(playerSprintMeter + num * regenMultipier, 1f);

                __instance.carryWeight = currentPlayerWeight + (__instance.carryWeight - newWeight);
            }
        }

        // Get Values
        [HarmonyPatch("Jump_performed")]
        [HarmonyPrefix]
        public static void getJumpStamina(PlayerControllerB __instance) {

            if (__instance.IsOwner && __instance.isPlayerControlled)
                playerSprintMeter = __instance.sprintMeter;
        }

        // Set Values
        [HarmonyPatch("Jump_performed")]
        [HarmonyPostfix]
        public static void setJumpStamina(PlayerControllerB __instance) {

            if (__instance.IsOwner && __instance.isPlayerControlled) {

                float num = __instance.sprintMeter - playerSprintMeter;

                if (num < 0f) __instance.sprintMeter = Mathf.Max(playerSprintMeter + num * jumpMultipier);
            }
        }
        // End of stamina patch
    }
}