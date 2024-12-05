using StardewValley;
using HarmonyLib;
using StardewValley.Buildings;
using StardewValley.GameData.Buildings;
using StardewValley.GameData.Characters;
using StardewValley.TokenizableStrings;
using System;
using System.Reflection.Metadata.Ecma335;

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
    [HarmonyPatch(typeof(LocalizedContentManager), nameof(LocalizedContentManager.LoadString), [typeof(string), typeof(object)])]
    public static bool Prefix(string path)
    {
        if (path == "Strings\\Locations:DoorUnlock_NotFriend_")
        {
            if (NPC.TryGetData(ownerKey, out var data))
            {
                string newPronouns = Game1.content.LoadString("Strings\\Locations:DoorUnlock_NotFriend_" + TokenParser.ParseText(data.DisplayName));
                if (newPronouns == null)
                {
                    return true;
                }
                Game1.drawObjectDialogue(newPronouns);
                return false;
            }
        }
        return true;
    }
    /*
     * you should check if the path looks like the doorunlock string and if it does get the NPC name from the end of it
you also need to make sure that newPronouns doesnt end up being null (and if it is null, return true)
    the path is going to be Strings\\Locations:DoorUnlock_NotFriend_Male if its a male character
since that is what ShowLockedDoorMessage is trying to load
     */
}
