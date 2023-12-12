using BepInEx;

namespace ModRushOverhaul {
    public class PluginConfig {

        public readonly bool keepItemsOnTeleport;

        public readonly bool usePremiumBatterys;

        public readonly bool useStaminaRework;

        public readonly bool dontLoseScrap;

        public readonly bool longerDay;

        public readonly bool openDoorsFaster;

        public readonly float dropshipTimeToArrive;

        public PluginConfig(Plugin BindingPlugin) {
            keepItemsOnTeleport = BindingPlugin.Config.Bind<bool>("ModRush Overhaul", "mroKeepItemsOnTeleport", true, "Do you want to keep your items when teleported?\ntrue = yes / false = no").Value;
            usePremiumBatterys = BindingPlugin.Config.Bind<bool>("ModRush Overhaul", "mroUsePremiumBatterys", true, "Should the Company buy premium batteries?\ntrue = yes / false = no").Value;
            useStaminaRework = BindingPlugin.Config.Bind<bool>("ModRush Overhaul", "mroUseStaminaRework", true, "You want to use a Gym subscription?\ntrue = yes / false = no").Value;
            dontLoseScrap = BindingPlugin.Config.Bind<bool>("ModRush Overhaul", "mroDontLoseScrap", false, "Should the Company insure all found scrap?\ntrue = yes / false = no").Value;
            longerDay = BindingPlugin.Config.Bind<bool>("ModRush Overhaul", "mroLongerDay", true, "Please God to make the day longer?\ntrue = yes / false = no").Value;
            openDoorsFaster = BindingPlugin.Config.Bind<bool>("ModRush Overhaul", "mroOpenDoorsFaster", false, "Do you want to learn how to open doors?\ntrue = yes / false = no").Value;
            dropshipTimeToArrive = BindingPlugin.Config.Bind<float>("ModRush Overhaul", "mroDropshipTimeToArrive", 8f, "Which type of Amazon Premium shoud the Company pay?\nHow fast will the Dropship arrive in seconds:").Value;
        }
    }
}
