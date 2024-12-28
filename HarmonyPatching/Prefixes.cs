using StardewValley;
using HarmonyLib;
using StardewValley.Buildings;
using StardewValley.GameData.Buildings;
using System.Runtime.CompilerServices;

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
    [HarmonyPatch(typeof(InteriorDoor), "ResetLocalState")]
    public static bool Prefix(GameLocation __instance)
    {

        if (__instance.GetData().CustomFields?.TryGetValue("Aviroen.Door", out string? doorBase) ?? false)
        {
            string[] array = ArgUtility.SplitBySpace(doorBase);
            int.TryParse(array[1], out int tileX);
            int.TryParse(array[2], out int tileY);
            int.TryParse(array[3], out int doorWidth);
            int.TryParse(array[4], out int doorHeight);
            bool.TryParse(array[5], out bool doorFlip);
            Microsoft.Xna.Framework.Rectangle sourceRect = default(Microsoft.Xna.Framework.Rectangle);
            sourceRect = new Microsoft.Xna.Framework.Rectangle(tileX, tileY, doorWidth, doorHeight);


        }
        if (__instance.GetData().CustomFields?.TryGetValue("Aviroen.DoorAnimation", out string? doorAnim) ?? false)
        {
            string[] array2 = ArgUtility.SplitBySpace(doorAnim);
        }
        return true;
    }
 * brain fry hot hot fry
 * should i just fucking use reflection for the rest of StardewValley.InteriorDoor.ResetLocalState
 * am i really that desperate
 * i don't wanna copy paste the whole method but i also have no idea what i'm doing here
 * time to english
 * split by space:
 * "Aviroen.Doors": "texture tilesheetpos-x tilesheetpos-y width height",
 * "Aviroen.DoorAnimation": "texture tilesheetpos-x tilesheetpos-y width height"
 * array[0] = texture
 * english my way out of a bag again
 * need to reflect
 */
}
