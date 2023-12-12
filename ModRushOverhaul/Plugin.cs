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
        public static PluginConfig cfg;

        private static Plugin Instance;

        void Awake() {

            if(Instance == null) Instance = this;
            mls = BepInEx.Logging.Logger.CreateLogSource(GUID);
            cfg = new PluginConfig(this);

            harmony.PatchAll(typeof(Plugin));
            mls.LogWarning("The Company...");
            if (cfg.usePremiumBatterys) {
                harmony.PatchAll(typeof(BatteryItemsPatch));
                mls.LogWarning("...bought better Batteries.");
            }
            if (cfg.keepItemsOnTeleport) {

                harmony.PatchAll(typeof(ShipTeleporterPatch));
                mls.LogWarning("...upgraded the Inverse Teleporter.");
            }
            if (cfg.useStaminaRework) {

                harmony.PatchAll(typeof(PlayerControllerBPatch));
                mls.LogWarning("...got you a Gym subscription.");
            }
            harmony.PatchAll(typeof(ItemDropshipPatch));
            mls.LogWarning("...now pays Amazon Premium.");
            harmony.PatchAll(typeof(RoundManagerPatch));
            if (cfg.dontLoseScrap)
                mls.LogWarning("...insured all found scrap");
            if (cfg.longerDay) {

                harmony.PatchAll(typeof(TimeOfDayPatch));
                mls.LogWarning("God made the day longer");
            }
            if (cfg.openDoorsFaster) {

                harmony.PatchAll(typeof(DoorLockPatch));
                mls.LogWarning("You learned how to open doors quickly");
            }
            harmony.PatchAll(typeof(TerminalPatch));
            mls.LogWarning("New Terminal commands \"curval\" and \"quota\"");
            
        }
    }
}