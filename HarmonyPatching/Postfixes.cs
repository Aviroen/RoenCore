using HarmonyLib;
using StardewValley.Characters;
using StardewValley.Events;
using StardewValley.Locations;
using StardewValley;
using StardewModdingAPI;
using StardewValley.GameData.Characters;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI.Events;
using StardewValley.GameData.WildTrees;
using Microsoft.Xna.Framework;
using StardewValley.Menus;
using System.Reflection.Metadata.Ecma335;
using xTile.Layers;

namespace RoenCore.HarmonyPatching;
[HarmonyPatch]
public class Postfixes
{
    internal static readonly string TileProp_Doors = $"{RoenCore.Manifest}_Doors";
    internal static readonly string tileSheetRef = $"{RoenCore.Manifest}/CustomDoor_texture";

    private static Texture2D LoadCustomDoorTexture(GameLocation location)
    {
        return Game1.content.Load<Texture2D>(location.GetData().CustomFields[tileSheetRef]);
    }

    [HarmonyPatch(typeof(InteriorDoor), "ResetLocalState")]
    public static void Postfix(InteriorDoor __instance)
    {
        bool flicker = false;
        bool flip = false;

        Microsoft.Xna.Framework.Rectangle sourceRect = default(Microsoft.Xna.Framework.Rectangle);
        if (__instance.Tile == null)
            return;
        if (__instance.Tile.Properties.ContainsKey(TileProp_Doors))
        {
            __instance.Sprite = new TemporaryAnimatedSprite("Maps\\" + tileSheetRef, sourceRect, 100f, 4, 1, new Vector2(__instance.Position.X, __instance.Position.Y - 2) * 64f, flicker, flip, (float)((__instance.Position.Y + 1) * 64 - 12) / 10000f, 0f, Color.White, 4f, 0f, 0f, 0f)
            {

            };
        }
    }


    [HarmonyPriority(Priority.Last)]
    [HarmonyPatch(typeof(Utility), "pickPersonalFarmEvent")]
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

    [HarmonyPriority(Priority.Last)]
    [HarmonyPatch(typeof(GameLocation), "resetLocalState")]
    	public virtual Microsoft.Xna.Framework.Rectangle getMugShotSourceRect()
	{
		return this.GetData()?.MugShotSourceRect ?? new Microsoft.Xna.Framework.Rectangle(0, (this.Age == 2) ? 4 : 0, 16, 24);
	}
         
    //you can use vector2.distance(light.position, player.position)  to find your radius
    [HarmonyPriority(Priority.Last)]
    [HarmonyPatch(typeof(NPC), "getMugShotSourceRect")]
    public static void Postfix(ref Rectangle __result, NPC __instance)
    {
        Texture2D? customTexture = null;
        if ((__instance.GetData().CustomFields?.TryGetValue("Aviroen.MugShot", out string? textureName) ?? false))
        {
            customTexture = Game1.content.Load<Texture2D>(textureName);
            //new Microsoft.Xna.Framework.Rectangle(0, ((NpcAge)2) ? 4 : 0, 16, 24) ?? false;
            __result = new Rectangle(0, 0, customTexture.Width, customTexture.Height);
        }
    }
    */
}
