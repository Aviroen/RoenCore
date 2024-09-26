using System;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using HarmonyLib;
using StardewValley;
using StardewValley.Characters;
using StardewValley.Events;
using StardewValley.Extensions;
using StardewValley.Locations;
using static StardewValley.Utility;

namespace GSQChildren
{
    /// <summary>The mod entry point.</summary>
    internal sealed class ModEntry : Mod
    {
        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            helper.Events.GameLoop.DayEnding += GameLoop_DayEnding;
        }

        private void GameLoop_DayEnding(object? sender, DayEndingEventArgs e)
        {
            
        }
        [HarmonyPostfix]
        public static FarmEvent pickPersonalFarmEvent()
        {
            Random r = Utility.CreateRandom(Game1.stats.DaysPlayed, Game1.uniqueIDForThisGame / 2, 470124797.0, Game1.player.UniqueMultiplayerID);
            if (Game1.weddingToday)
            {
                return null;
            }
            NPC npcSpouse = Game1.player.getSpouse();
            bool isMarriedOrRoommates = Game1.player.isMarriedOrRoommates();
            if (isMarriedOrRoommates && Game1.player.GetSpouseFriendship().DaysUntilBirthing <= 0 && Game1.player.GetSpouseFriendship().NextBirthingDate != null)
            {
                if (npcSpouse != null)
                {
                    return new BirthingEvent();
                }
                long spouseID = Game1.player.team.GetSpouse(Game1.player.UniqueMultiplayerID).Value;
                if (Game1.otherFarmers.ContainsKey(spouseID))
                {
                    return new PlayerCoupleBirthingEvent();
                }
            }
            else
            {
                if (isMarriedOrRoommates)
                {
                    bool? flag = npcSpouse?.canGetPregnant();
                    if (flag.HasValue && flag.GetValueOrDefault() && Game1.player.currentLocation == Game1.getLocationFromName(Game1.player.homeLocation) && GameStateQuery.CheckConditions(npcSpouse.GetData()?.SpouseWantsChildren))
                    {
                        return new QuestionEvent(1);
                    }
                }
                if (isMarriedOrRoommates && Game1.player.team.GetSpouse(Game1.player.UniqueMultiplayerID).HasValue && Game1.player.GetSpouseFriendship().NextBirthingDate == null && r.NextDouble() < 0.05)
                {
                    long spouseID = Game1.player.team.GetSpouse(Game1.player.UniqueMultiplayerID).Value;
                    if (Game1.otherFarmers.TryGetValue(spouseID, out var farmerSpouse))
                    {
                        Farmer spouse = farmerSpouse;
                        if (spouse.currentLocation == Game1.player.currentLocation && (spouse.currentLocation == Game1.getLocationFromName(spouse.homeLocation) || spouse.currentLocation == Game1.getLocationFromName(Game1.player.homeLocation)) && playersCanGetPregnantHere(spouse.currentLocation as FarmHouse))
                        {
                            return new QuestionEvent(3);
                        }
                    }
                }
            }
            if (r.NextBool())
            {
                return new QuestionEvent(1);
            }
            return new SoundInTheNightEvent(2);
        }
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

        /*********
        ** Private methods
        *********/
        /// <summary>Raised after the player presses a button on the keyboard, controller, or mouse.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event data.</param>
        /*you just patch pickPersonalFarmEvent
which is in StardewValley.Utility 
pickPersonalFarmEvent returns a FarmEvent to whatever calls it, and the QuestionEvent it returns for adoption/pregnancy s one of those, so you would just intercept the function after it finishes to go "you're trying to return the UFO sound in the middle of the night event? sorry, actually, you're going to return a pregnancy/adoption event bc i said so"
(actually it would be the "dogs" nightly event not the ufo but w/e the concept is the same)
        most of the code is already written for you from the original pickPersonalFarmEvent, you can just copy it but without the RNG check into your postfix
         */
    }
}