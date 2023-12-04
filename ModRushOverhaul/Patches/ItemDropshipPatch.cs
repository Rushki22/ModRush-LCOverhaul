using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ModRushOverhaul.Patches {

    [HarmonyPatch(typeof(ItemDropship))]
    internal class ItemDropshipPatch {

        // Important Assets
        private static StartOfRound playersManager;

        private static List<int> itemsToDeliver;

        private static Terminal terminalScript;
        private static List<int> orderedItemsFromTerminal;

        // Settings
        private static float howFast = 8; // sec
        private static float willStay = 60; // sec

        // Init Dropship
        [HarmonyPatch("Start")]
        [HarmonyPrefix]
        public static void initializeDropship(ItemDropship __instance) {

            playersManager = Object.FindObjectOfType<StartOfRound>();
            terminalScript = Object.FindObjectOfType<Terminal>();
            itemsToDeliver = (List<int>)Traverse.Create(__instance).Field("itemsToDeliver").GetValue();
        }

        // Update Timer
        [HarmonyPatch("Update")]
        [HarmonyPrefix]
        public static void dropshipUpdate(ItemDropship __instance) {

            if (__instance.IsServer && !__instance.deliveringOrder && terminalScript.orderedItemsFromTerminal.Count > 0 && !playersManager.shipHasLanded) {
                __instance.shipTimer += Time.deltaTime;
            }
        }

        // Some IL code stuff to hard to explain. But its Overwriting the code on specified place
        [HarmonyPatch("Update")]
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions) {

            List<CodeInstruction> codes = new List<CodeInstruction>(instructions);
            for (int i = 0; i < codes.Count; i++) {

                if (codes[i].opcode == OpCodes.Ldc_R4) {

                    if ((float)codes[i].operand == 20f) {
                        codes[i].operand = (float)willStay;
                    }
                    else if ((float)codes[i].operand == 40f) {
                        codes[i].operand = (float)(willStay + howFast);
                    }
                    else if ((float)codes[i].operand == 30f) {
                        codes[i].operand = (float)willStay;
                        break;
                    }
                }
            }
            return codes.AsEnumerable();
        }

        // Deliver items bought late game & Remove bought items from order list
        [HarmonyPatch("ShipLandedAnimationEvent")]
        [HarmonyPrefix]
        public static void addLateItemsServer(ItemDropship __instance) {

            if (__instance.IsServer && __instance.shipLanded && !__instance.shipDoorsOpened) {

                while (orderedItemsFromTerminal.Count > 0 && itemsToDeliver.Count < 12) {

                    itemsToDeliver.Add(orderedItemsFromTerminal[0]);
                    orderedItemsFromTerminal.RemoveAt(0);
                }
            }
        }

        // Get ordered items
        [HarmonyPatch(typeof(Terminal), "Start")]
        [HarmonyPrefix]
        public static void initializeTerminal(Terminal __instance) {
            orderedItemsFromTerminal = __instance.orderedItemsFromTerminal;
        }
    }
}