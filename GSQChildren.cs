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
using System.Reflection;
using System.Runtime.CompilerServices;

namespace GSQChildren
{
    /// <summary>The mod entry point.</summary>
    internal sealed class ModEntry : Mod
    {
        internal static IModHelper ModHelper { get; set; } = null!;
        internal static IMonitor ModMonitor { get; set; } = null!;
        internal static Harmony Harmony { get; set; } = null!;
        internal static IManifest Manifest { get; set; } = null!;

        public override void Entry(IModHelper helper)
        {
            ModHelper = helper;
            ModMonitor = Monitor;
            Harmony = new Harmony(ModManifest.UniqueID);
            Manifest = ModManifest;

            Harmony.PatchAll();

            helper.Events.GameLoop.GameLaunched += this.OnGameLaunched;
        }

        private void OnGameLaunched(object? sender, GameLaunchedEventArgs e)
        {
            
        }
        [HarmonyPatch(typeof(Utility), nameof(Utility.pickPersonalFarmEvent))] //nope i'm lost i have no idea what i'm doing
        class Patch
        {
            static void Postfix(ref FarmEvent __result)
            {
                NPC npcSpouse = Game1.player.getSpouse();
                bool isMarriedOrRoommates = Game1.player.isMarriedOrRoommates();
                if (isMarriedOrRoommates)
                {
                    bool? flag = npcSpouse?.canGetPregnant();
                    if (flag.HasValue && flag.GetValueOrDefault() && Game1.player.currentLocation == Game1.getLocationFromName(Game1.player.homeLocation) && GameStateQuery.CheckConditions(npcSpouse.GetData()?.SpouseWantsChildren))
                    {
                        __result = new QuestionEvent(1);
                    }
                }
                if (isMarriedOrRoommates && Game1.player.team.GetSpouse(Game1.player.UniqueMultiplayerID).HasValue && Game1.player.GetSpouseFriendship().NextBirthingDate == null)
                {
                    long spouseID = Game1.player.team.GetSpouse(Game1.player.UniqueMultiplayerID).Value;
                    if (Game1.otherFarmers.TryGetValue(spouseID, out var farmerSpouse))
                    {
                        Farmer spouse = farmerSpouse;
                        if (spouse.currentLocation == Game1.player.currentLocation && (spouse.currentLocation == Game1.getLocationFromName(spouse.homeLocation) || spouse.currentLocation == Game1.getLocationFromName(Game1.player.homeLocation)) && playersCanGetPregnantHere(spouse.currentLocation as FarmHouse))
                        {
                            __result = new QuestionEvent(3);
                        }
                    }
                }
            }
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

        //NOTE TO SELF, READD "UTILITY" IN FRONT OF PLAYERSCANGETPRENGANTHERE AND YEET THE SECOND FUNCTION FOR 1.6.9
    }
}