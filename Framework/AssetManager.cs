using RoenCore.Models;
using StardewModdingAPI.Events;
using StardewValley;
namespace RoenCore.Framework;
internal class AssetManager
{
    static Dictionary<string, NpcModel> NpcListData = null!;
    public static Dictionary<string, NpcModel> NpcData
    {
        get
        {
            if (NpcListData == null)
            {
                NpcListData = Game1.content.Load<Dictionary<string, NpcModel>>("Aviroen.RoenCore/NpcList");
            }
            return NpcListData;
        }
    }
    internal static void OnAssetRequested(AssetRequestedEventArgs e)
    {
        if (e.NameWithoutLocale.IsEquivalentTo("Aviroen.RoenCore/NpcList"))
        {
            e.LoadFrom(() => new Dictionary<string, NpcModel>(), AssetLoadPriority.Exclusive);
        }
    }
    internal static void OnAssetInvalidated(AssetsInvalidatedEventArgs e)
    {
        foreach (var name in e.NamesWithoutLocale)
        {
            if (name.IsEquivalentTo("Aviroen.RoenCore/NpcList"))
            {
                NpcListData = null!;
            }
        }
    }
}
