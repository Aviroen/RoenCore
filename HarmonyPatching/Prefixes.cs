using StardewValley;
using HarmonyLib;
using StardewValley.Buildings;
using StardewValley.GameData.Buildings;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using StardewValley.GameData.WildTrees;

namespace RoenCore.HarmonyPatching;
[HarmonyPatch]
public class Prefixes
{
    /// <summary>Get the building's data from <see cref="F:StardewValley.Game1.buildingData" />, if found.</summary>
    [HarmonyPriority(Priority.Last)]
    [HarmonyPatch(typeof(Farm), "addCrows")]
    public static bool Prefix(Farm __instance)
    {
        foreach (Building building in __instance.buildings)
        {
            if (building.GetData() is BuildingData data && (data.CustomFields?.TryGetValue("Aviroen.AtraAntiCrow", out string? customString) ?? false))
            {
                if (bool.TryParse(customString, result: out bool stringActivated) && stringActivated)
                {
                    return false;
                }
            }
        }
        return true;
    }
    /*
     
    */
}
