using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using ModRushOverhaul.Patches;

namespace ModRushOverhaul {

    [BepInPlugin(GUID, NAME, VERSION)]
    public class Plugin : BaseUnityPlugin {

        private const string GUID = "ModRush.Patcher";
        private const string NAME = "Modrush Overhaul";
        private const string VERSION = "1.0.0";

        private readonly Harmony harmony = new Harmony(GUID);

        internal ManualLogSource mls;

        private static Plugin Instance;

        void Awake() {

            if(Instance == null) Instance = this;
            mls = BepInEx.Logging.Logger.CreateLogSource(GUID);

            harmony.PatchAll(typeof(Plugin));
            mls.LogWarning("The Company...");
            harmony.PatchAll(typeof(BatteryItemsPatch));
            mls.LogWarning("...bought better Batterys.");
            harmony.PatchAll(typeof(ShipTeleporterPatch));
            mls.LogWarning("...upgraded the Inverse Teleporter.");
            harmony.PatchAll(typeof(PlayerControllerBPatch));
            mls.LogWarning("...got you a Gym subscribtion.");
            harmony.PatchAll(typeof(ItemDropshipPatch));
            mls.LogWarning("...now pays Amazon Premium.");
            harmony.PatchAll(typeof(TimeOfDayPatch));
            mls.LogWarning("God made the day longer");
            harmony.PatchAll(typeof(TerminalPatch));
            mls.LogWarning("New Terminal commands \"curval\" and \"quota\"");
        }
    }
}