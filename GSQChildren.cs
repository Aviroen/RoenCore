using System;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using HarmonyLib;
using StardewValley;
using StardewValley.Characters;
using StardewValley.Events;
using StardewValley.Locations;
using System.Reflection;
using StardewValley.TerrainFeatures;

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

            helper.Events.GameLoop.GameLaunched += this.OnGameLaunched;
            helper.Events.Input.ButtonPressed += this.OnButtonPressed;

            Harmony.Patch(
      original: AccessTools.Method(typeof(Utility), nameof(Utility.pickPersonalFarmEvent)),
      postfix: new HarmonyMethod(typeof(ModEntry), nameof(Postfix))  // assumes main mod class is called ModEntry
    );

        }

        private void OnGameLaunched(object? sender, GameLaunchedEventArgs e)
        {
            ModMonitor.Log($"Got past game launched.", LogLevel.Error);
        }
        private void OnButtonPressed(object? sender, ButtonPressedEventArgs e)
        {
            if (!Context.IsWorldReady)
                return;

            if (e.Button == SButton.F5)
            {
                //
            }
        }
        [HarmonyPatch(typeof(Utility), "pickPersonalFarmEvent")] //nope i'm lost i have no idea what i'm doing
        [HarmonyPostfix]
        static void Postfix(ref FarmEvent __result)
        {
            ModMonitor.Log($"We're inside the postfix", LogLevel.Error);
            NPC npcSpouse = Game1.player.getSpouse();
            bool isMarriedOrRoommates = Game1.player.isMarriedOrRoommates();
            if (npcSpouse != null && npcSpouse.GetData().CustomFields.TryGetValue("Aviroen.GSQBaby", out string customString))
            {
                Boolean.TryParse(customString, result: out bool stringActivated);
                if (stringActivated == true)
                {
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