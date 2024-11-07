using StardewModdingAPI;
using StardewModdingAPI.Events;
using HarmonyLib;
using StardewValley.GameData.WildTrees;
using StardewValley;
using Microsoft.Xna.Framework;
using StardewValley.Menus;
using Microsoft.Xna.Framework.Graphics;

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
            helper.Events.Input.ButtonPressed += this.OnButtonPressed;
            helper.Events.GameLoop.DayStarted += this.OnDayStarted;
            helper.Events.World.TerrainFeatureListChanged += this.World_TerrainFeatureListChanged;
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
        private void OnButtonPressed(object? sender, ButtonPressedEventArgs e)
        {
            if (!Context.IsWorldReady)
                return;

            if (e.Button == SButton.F5)
            {
                //
            }
        }
        private void OnDayStarted(object? sender, DayStartedEventArgs e)
        {

        }
        private void World_TerrainFeatureListChanged(object? sender, TerrainFeatureListChangedEventArgs e)
        {
            //location.terrainFeatures.OnValueRemoved
            //Listen to the tree's isstump :P
        }
        /*
        public static LightSource? TreeGlow(WildTreeData treeData, string mapName, Vector2 pos)
        {
            if ((treeData.CustomFields?.TryGetValue("Aviroen.TreeGlow", out string? customString) ?? false))
            {
                //below code from chue https://github.com/Mushymato/StardewMods/blob/main/MiscMapActionsProperties/Framework/Tile/LightSpot.cs#L33-L51
                string[] args = ArgUtility.SplitBySpace(customString ?? "");
                if (!ArgUtility.TryGetOptionalFloat(args, 0, out float radius, out string error, defaultValue: 2f, name: "float radius") ||
                !ArgUtility.TryGetOptional(args, 1, out string colorStr, out error, defaultValue: "White", name: "string color") ||
                !ArgUtility.TryGetOptional(args, 2, out string textureStr, out error, defaultValue: "1", name: "string texture"))
                {
                    ModMonitor.Log(error, LogLevel.Error);
                    return null;
                }
            }
        }
        */
    }
}