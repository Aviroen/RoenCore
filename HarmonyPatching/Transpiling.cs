using HarmonyLib;
using StardewValley.Characters;
using StardewValley.Events;
using StardewValley.Locations;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewModdingAPI;

namespace RoenCore.HarmonyPatching;
public class Transpiling
{
    public static Harmony TranspilingHarmony { get; set; } = null!;
    public static bool IsInitialized;

    public static void Initialize(IManifest manifest)
    {
        IsInitialized = true;
        TranspilingHarmony = new Harmony($"{manifest.UniqueID}_Transpilers");
    }
}
