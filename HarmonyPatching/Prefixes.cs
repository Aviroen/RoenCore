using StardewValley;
using StardewValley.Buildings;
using StardewValley.GameData.Buildings;

namespace RoenCore.HarmonyPatching;
public class Prefixes
{
    /// <summary>Get the building's data from <see cref="F:StardewValley.Game1.buildingData" />, if found.</summary>

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
     * var data = building.GetData();
if (data.CustomFields is not null && data.CustomFields.TryGetValue("Aviroen.AtraAntiCrow", out string? customString))
  {
    if (bool.TryParse(customString, result: out bool stringActivated) && stringActivated)
    {
      return false;
    }
}
     * data.CustomFields is Dictionary<string, string> customFields && customFields.ContainsKey(thekey)
     * if (building.GetData() is BuildingData data && data.CustomFields?.TryGetValue("Aviroen.AtraAntiCrow", out string? customString))
    public static bool Prefix(ref Farm __instance)
    {
        
        if (!__instance.buildings.Any(Game1.buildingData.CustomFields.Value == "Aviroen.AtraAntiCrow"))
        {
            __instance = new Farm();
            return false;
        }
        else
        {
            return true;
        }
    }
    
    iterate list of buildings on the farm
        public static bool Prefix(Farm __instance)
    {
        return !__instance.buildings.Any(static building => building.buildingType.Value == "Aviroen.AtraAntiCrow");
    }
    
    public static void Scarecrow()
    {
        Building buildingData = new Building.GetData();
        if (building.GetData().CustomFields.TryGetValue("Aviroen.GSQBaby", out string? customString))
        {

        }
    }
    */
}
