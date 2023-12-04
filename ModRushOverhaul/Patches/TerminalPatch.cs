using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ModRushOverhaul.Patches {

    [HarmonyPatch(typeof(Terminal))]
    internal class TerminalPatch {

        [HarmonyPostfix]
        [HarmonyPatch("ParsePlayerSentence")]
        private static void displayShipValue(ref Terminal __instance, ref TerminalNode __result) {

            // Get player input
            string s = __instance.screenText.text.Substring(__instance.screenText.text.Length - __instance.textAdded);

            // Displays the current total scrap value inside the ship at 100% buying rate
            if (s.ToLower() == "curval") {

                List<GrabbableObject> items = getShipItems();
                
                int sum = calculateScrapInShipValue(items);
                TerminalNode terminalNode = new TerminalNode();
                terminalNode.displayText = $"Total scrap value inside the ship: ${sum}\n";
                terminalNode.clearPreviousText = true;
                __result = terminalNode;
            }
            else if (s.ToLower() == "quota") { // Displays what you need to sell to meet the profit quota precisely

                List<GrabbableObject> items = getShipItems();

                int quota = TimeOfDay.Instance.profitQuota;
                int profit = TimeOfDay.Instance.quotaFulfilled;
                int daysLeft = TimeOfDay.Instance.daysUntilDeadline;
                float buyingRate = StartOfRound.Instance.companyBuyingRate;
                int sum = (int)(calculateScrapInShipValue(items) * buyingRate);

                TerminalNode terminalNode = new TerminalNode();
                // Check if scrap value sum is lower than quota
                if (sum < quota) {

                    if (daysLeft <= 0) // No days left ;(
                        terminalNode.displayText = 
                            $"You do not meet the profit quota.\n\n" +
                            $"Happy disciplinary process! :)\n\n\n\n" +
                            $"PS: I recommend restarting now\n";
                    else if (daysLeft > 0) // Displays differnece between scrap value and quota
                        terminalNode.displayText = 
                            $"Current quota: ${profit} / ${quota}\n" +
                            $"Buying at {Mathf.RoundToInt(buyingRate * 100f)}%\n\n" +
                            $"You do not meet the profit quota.\n\n" +
                            $"Your current scrap value at buying rate is ${sum}\n" +
                            $"Difference: ${(quota - profit) - sum}\n\n" +
                            $"I believe in you!\n";
                } else { // Lists the items needed to sell

                    terminalNode.displayText = 
                        $"Current quota: ${profit} / ${quota}\n" +
                        $"Buying at {Mathf.RoundToInt(buyingRate * 100f)}%\n\n" +
                        $"Note: All values calculated including the buying rate.\n\n" +
                        $"To meet the profit quota, sell the following items:\n\n";

                    GrabbableObject lastItem = null;
                    float value = 0;
                    foreach (GrabbableObject item in items.OrderByDescending(item => item.scrapValue)) {

                        int scrapValue = (int)(item.scrapValue * buyingRate);
                        if (scrapValue != 0) {
                            if ((quota - profit) - value >= scrapValue) {

                                terminalNode.displayText += $"- {item.itemProperties.itemName}: ${scrapValue}\n";
                                value += scrapValue;
                            } else {
                                lastItem = item; // Note cheapest item in list
                                continue;
                            } 
                        } else continue;

                        if (value >= quota - profit) break;
                    }

                    string displayLastItem = "";
                    if (value != quota - profit) { // If the displayed scrap value is not enough to reach quota, display the noted cheapest item
                        terminalNode.displayText += $"\nThis item overshoots quota, but is the cheapest you own: {lastItem.itemProperties.itemName} (${(int)(lastItem.scrapValue * buyingRate)})\n";
                        displayLastItem = $"(+ ${(int)(lastItem.scrapValue * buyingRate)})";
                    }
                    
                    terminalNode.displayText += $"\nSelling value: ${value} {displayLastItem}\n";
                }

                terminalNode.clearPreviousText = true;
                __result = terminalNode;
            } else {
                return;
            }
        }

        // Calculates the total scrap value inside the ship
        private static int calculateScrapInShipValue(List<GrabbableObject> list) {
            return list.Sum((GrabbableObject item) => item.scrapValue);
        }

        // Get all items inside the ship and puts them in a list
        private static List<GrabbableObject> getShipItems() {

            List<GrabbableObject> list = (
                from obj in GameObject.Find("/Environment/HangarShip").GetComponentsInChildren<GrabbableObject>()
                where obj.scrapValue != 0 && obj.name != "ClipboardManual" && obj.name != "StickyNoteItem"
                select obj
            ).ToList();

            return list;
        }
    }
}
