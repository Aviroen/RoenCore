using ContentPatcher;
using Microsoft.Xna.Framework.Graphics;
using RoenCore.Models;
using RoenCore;
using StardewModdingAPI.Events;
using StardewValley;
namespace RoenCore.Framework;
internal class AssetManager
{
    //static Dictionary<string, NpcModel> NpcListData = null!;
    //public static Dictionary<string, NpcModel> NpcData
    //{
    //    get
    //    {
    //        if (NpcListData == null)
    //        {
    //            NpcListData = Game1.content.Load<Dictionary<string, NpcModel>>("Aviroen.RoenCore/NpcList");
    //        }
    //        return NpcListData;
    //    }
    //}
    internal static void OnAssetRequested(AssetRequestedEventArgs e)
    {
        //if (e.NameWithoutLocale.IsEquivalentTo("Aviroen.RoenCore/NpcList"))
        //{
        //    e.LoadFrom(() => new Dictionary<string, NpcModel>(), AssetLoadPriority.Exclusive);
        //}
        if (e.Name.IsEquivalentTo("Maps/Aviroen_TileSheet"))
        {
            e.LoadFromModFile<Texture2D>("Assets/TileSheet.png", AssetLoadPriority.Low);
        }
    }
    internal static void OnAssetInvalidated(AssetsInvalidatedEventArgs e)
    {
        //foreach (var name in e.NamesWithoutLocale)
        //{
        //    if (name.IsEquivalentTo("Aviroen.RoenCore/NpcList"))
        //    {
        //        RoenCore.ModMonitor.Log($"Loaded asset Aviroen.RoenCore/NpcList invalidated, reloading.");
        //        NpcListData = null!;
        //    }
        //}
    }
    //internal void OnGameLaunched(object? sender, GameLaunchedEventArgs e)
    //{
    //    var api = this.Helper.ModRegistry.GetApi<IContentPatcherAPI>("Pathoschild.ContentPatcher");
    //}
}
