using HarmonyLib;
using StardewValley.Characters;
using StardewValley.Events;
using StardewValley.Locations;
using StardewValley;
using StardewModdingAPI;

namespace RoenCore.HarmonyPatching;

public class Postfixes
{
    public static Harmony PostfixesHarmony { get; set; } = null!;
    internal static bool IsInitialized;

    internal static void Initialize(IManifest manifest)
    {
        IsInitialized = true;
        PostfixesHarmony = new Harmony($"{manifest.UniqueID}_Postfixes");
    }

    [HarmonyPatch(typeof(Utility), "pickPersonalFarmEvent")] //nope i'm lost i have no idea what i'm doing
    [HarmonyPostfix]
    public static void Postfix(ref FarmEvent __result)
    {
        NPC npcSpouse = Game1.player.getSpouse();
        bool isMarriedOrRoommates = Game1.player.isMarriedOrRoommates();
        if (npcSpouse != null && npcSpouse.GetData().CustomFields.TryGetValue("Aviroen.GSQBaby", out string? customString))
        {
            if (bool.TryParse(customString, result: out bool stringActivated) && stringActivated)
            {
                if (isMarriedOrRoommates)
                {
                    bool? flag = npcSpouse?.canGetPregnant();
                    if (flag.HasValue && flag.GetValueOrDefault() && Game1.player.currentLocation == Game1.getLocationFromName(Game1.player.homeLocation.Value) && GameStateQuery.CheckConditions(npcSpouse?.GetData()?.SpouseWantsChildren))
                    {
                        __result = new QuestionEvent(1);
                    }
                }
                if (isMarriedOrRoommates && Game1.player.team.GetSpouse(Game1.player.UniqueMultiplayerID).HasValue && Game1.player.GetSpouseFriendship().NextBirthingDate == null)
                {
                    long? spouseID = Game1.player.team.GetSpouse(Game1.player.UniqueMultiplayerID);
                    if (spouseID != null)
                    {
                        Game1.otherFarmers.TryGetValue((long)spouseID, out var farmerSpouse);
                        Farmer spouse = farmerSpouse;
                        if (spouse.currentLocation == Game1.player.currentLocation && (spouse.currentLocation == Game1.getLocationFromName(spouse.homeLocation.Value) || spouse.currentLocation == Game1.getLocationFromName(Game1.player.homeLocation.Value)) && Utility.playersCanGetPregnantHere(spouse.currentLocation as FarmHouse))
                        {
                            __result = new QuestionEvent(3);
                        }
                    }
                }
            }
        }
    }
    /*
     * The short description of short circuiting is that the minimal number of conditions will be evaluated from left to right

For example, if you have thingA() && thingB(), and thingA() returns false, thingB() will never even run.

Similarly, if you have thingA() || thingB() and thingA() returns true, thingB() will never run
     */
}
