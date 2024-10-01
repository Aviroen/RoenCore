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
    //NOTE TO SELF, READD "UTILITY" IN FRONT OF PLAYERSCANGETPRENGANTHERE AND YEET THE SECOND FUNCTION FOR 1.6.9
    /*
    private static bool playersCanGetPregnantHere(FarmHouse farmHouse)
    {
        List<Child> kids = farmHouse.getChildren();
        if (farmHouse.cribStyle.Value <= 0)
        {
            return false;
        }
        if (farmHouse.getChildrenCount() < 2 && farmHouse.upgradeLevel >= 2 && kids.Count < 2)
        {
            if (kids.Count != 0)
            {
                return kids[0].Age > 2;
            }
            return true;
        }
        return false;
    }
     * 	public static bool isGreenRainDay()
	{
		return Utility.isGreenRainDay(Game1.dayOfMonth, Game1.season);
	}

	/// <summary>Get whether there's green rain scheduled on the given day.</summary>
	/// <param name="day">The day of month to check.</param>
	/// <param name="season">The season key to check.</param>
	public static bool isGreenRainDay(int day, Season season)
	{
		if (season == Season.Summer)
		{
			Random r = Utility.CreateRandom(Game1.year * 777, Game1.uniqueIDForThisGame);
			int[] possible_days = new int[8] { 5, 6, 7, 14, 15, 16, 18, 23 };
			return day == r.ChooseFrom(possible_days);
		}
		return false;
	}
     */
}
