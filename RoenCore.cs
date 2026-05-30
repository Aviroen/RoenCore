using StardewModdingAPI;
using StardewModdingAPI.Events;
using HarmonyLib;
using StardewValley;
using RoenCore.Patches;

namespace RoenCore
{
    /// <summary>The mod entry point.</summary>
    public class RoenCore : Mod
    {
        internal static IModHelper ModHelper { get; set; } = null!;
        internal static IMonitor ModMonitor { get; set; } = null!;
        internal static Harmony Harmony { get; set; } = null!;
        internal static IManifest Manifest { get; set; } = null!;
        internal static HashSet<string> LoadedMods { get; set; } = [];

        public override void Entry(IModHelper helper)
        {
            ModHelper = helper;
            ModMonitor = Monitor;
            Harmony = new Harmony(ModManifest.UniqueID);
            Manifest = ModManifest;

            helper.Events.GameLoop.GameLaunched += this.OnGameLaunched;
            Event.RegisterCommand("Aviroen.Large", Events.command_LargeFrame);
            Events.Initialize(helper.ModRegistry, Monitor, helper);
            /*
            Harmony.Patch(
                original: AccessTools.Method(typeof(Utility), nameof(Utility.pickPersonalFarmEvent)),
                postfix: new HarmonyMethod(typeof(Postfixes), nameof(Postfixes.GSQBaby)));
            
            Harmony.Patch(
                original: AccessTools.Method(typeof(Farm), nameof(Farm.addCrows)),
                prefix: new HarmonyMethod(typeof(Prefixes), nameof(Prefixes.Prefix)));
            */
            Harmony.PatchAll();

        }

        private void OnGameLaunched(object? sender, GameLaunchedEventArgs e)
        {
            foreach (var mod in Helper.ModRegistry.GetAll())
            {
                if (Helper.ModRegistry.IsLoaded(mod.Manifest.UniqueID)) LoadedMods.Add(mod.Manifest.UniqueID);
            }

        }
    }
}