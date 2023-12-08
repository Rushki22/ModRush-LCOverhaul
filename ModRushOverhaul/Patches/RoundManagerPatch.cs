using HarmonyLib;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ModRushOverhaul.Patches {

    [HarmonyPatch(typeof(RoundManager))]
    internal class RoundManagerPatch {

        private static List<GameObject> spawnedSyncedObjects = new List<GameObject>();

        // Overwrites the original method to not lose all scrap when all players die in a round
        [HarmonyPatch("DespawnPropsAtEndOfRound")]
        [HarmonyPrefix]
        private static bool dontLoseScrapWhenAllDead(RoundManager __instance, ref bool despawnAllItems) {

            if (Plugin.cfg.dontLoseScrap) { 
                if (!__instance.IsServer)
                    return false;

                GrabbableObject[] array = Object.FindObjectsOfType<GrabbableObject>();
                for (int i = 0; i < array.Length; i++) {

                    if (despawnAllItems || (!array[i].isHeld && !array[i].isInShipRoom)) {

                        if (array[i].isHeld && array[i].playerHeldBy != null) array[i].playerHeldBy.DropAllHeldItems();

                        array[i].gameObject.GetComponent<NetworkObject>().Despawn();
                    }

                    else array[i].scrapPersistedThroughRounds = true;
                    if (spawnedSyncedObjects.Contains(array[i].gameObject)) spawnedSyncedObjects.Remove(array[i].gameObject);
                }

                GameObject[] array2 = GameObject.FindGameObjectsWithTag("TemporaryEffect");

                for (int j = 0; j < array2.Length; j++)
                    Object.Destroy(array2[j]);

                return false;
            } else return true;
        }

        // Make enemys more likely to spawn
        [HarmonyPatch("PlotOutEnemiesForNextHour")]
        [HarmonyPrefix]
        private static void spawnEnemysFaster(RoundManager __instance) {
            if (Plugin.cfg.longerDay) {
                __instance.currentLevel.spawnProbabilityRange = 4.5f;
                __instance.currentLevel.maxEnemyPowerCount = 12;
            }
        }
    }
}
