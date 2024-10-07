using HarmonyLib;
using StardewModdingAPI;
using StardewValley;

namespace RoenCore.HarmonyPatching;
public class Prefixes
{
    public static Harmony PrefixesHarmony { get; set; } = null!;
    public static bool IsInitialized;

    public static void Initialize(IManifest manifest)
    {
        IsInitialized = true;
        PrefixesHarmony = new Harmony($"{manifest.UniqueID}_Prefixes");
    }
    [HarmonyPatch(typeof(Farm), nameof(Farm.addCrows))]
    internal static bool Scarecrow(Farm __instance)
    {
        return !__instance.buildings.Any(static building => building.buildingType.Value == "Aviroen.AtraAntiCrow");
    }
}
