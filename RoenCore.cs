using System.Reflection;
using HarmonyLib;
using RoenCore.Framework;
using RoenCore.Patches;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Triggers;

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
            
            Events.Initialize(helper.ModRegistry, Monitor, helper);
            TriggerActions.Initialize(helper.ModRegistry);

            Event.RegisterCommand("Aviroen.Large", Events.command_LargeFrame); //name, frame, width, height
            //Event.RegisterCommand("Aviroen.Festival", Events.command_playerControl); //event id, true/false, int in ms for timer, npc for host, string for host
            Event.RegisterCommand("Aviroen.AddActors", Events.command_TempAct);//{{ModId}} x y width height

            helper.Events.Content.AssetRequested += static (_, e) => AssetManager.OnAssetRequested(e);
            helper.Events.Content.AssetsInvalidated += static (_, e) => AssetManager.OnAssetInvalidated(e);

            Harmony.PatchAll(Assembly.GetExecutingAssembly());

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