using HarmonyLib;
using StardewModdingAPI;

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
}
