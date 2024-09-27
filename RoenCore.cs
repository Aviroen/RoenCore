using System;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using HarmonyLib;
using StardewValley;
using StardewValley.Characters;
using StardewValley.Events;
using StardewValley.Locations;
using System.Reflection;
using StardewValley.TerrainFeatures;

namespace RoenCore
{
    /// <summary>The mod entry point.</summary>
    public class RoenCore : Mod
    {
        internal static IModHelper ModHelper { get; set; } = null!;
        internal static IMonitor ModMonitor { get; set; } = null!;
        internal static Harmony Harmony { get; set; } = null!;
        internal static IManifest Manifest { get; set; } = null!;

        public override void Entry(IModHelper helper)
        {
            ModHelper = helper;
            ModMonitor = Monitor;
            Harmony = new Harmony(ModManifest.UniqueID);
            Manifest = ModManifest;

            helper.Events.GameLoop.GameLaunched += this.OnGameLaunched;
            helper.Events.Input.ButtonPressed += this.OnButtonPressed;

            Harmony.PatchAll();
            
            Harmony.Patch(
      original: AccessTools.Method(typeof(Utility), nameof(Utility.pickPersonalFarmEvent)),
      postfix: new HarmonyMethod(typeof(Postfixes), nameof(Postfixes.Postfix))  // assumes main mod class is called ModEntry
    );
            

        }

        private void OnGameLaunched(object? sender, GameLaunchedEventArgs e)
        {
            Postfixes.Initialize(ModManifest);
        }
        private void OnButtonPressed(object? sender, ButtonPressedEventArgs e)
        {
            if (!Context.IsWorldReady)
                return;

            if (e.Button == SButton.F5)
            {
                //
            }
        }
    }
}