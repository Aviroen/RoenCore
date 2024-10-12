using StardewValley;
using HarmonyLib;
using StardewValley.Buildings;
using StardewValley.GameData.Buildings;

namespace RoenCore.HarmonyPatching;
public class Prefixes
{
    /// <summary>Get the building's data from <see cref="F:StardewValley.Game1.buildingData" />, if found.</summary>
    //[HarmonyAfter("")] //get rokugin's uniqueid
    public static bool Scarecrow(Farm __instance)
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
