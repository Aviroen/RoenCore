using HarmonyLib;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Delegates;

namespace RoenCore.Patches;
internal class TriggerActions
    {
    private static IModRegistry ModRegistry { get; set; } = null!;
    public static void Initialize(IModRegistry registry)
    {
        ModRegistry = registry;
    }
}
