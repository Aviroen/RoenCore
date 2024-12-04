using HarmonyLib;
using StardewModdingAPI;
using StardewValley;
using StardewValley.TokenizableStrings;
using System.Reflection;

namespace RoenCore.HarmonyPatching;
[HarmonyPatch]
public class Transpiling
{
    [HarmonyPatch(typeof(GameLocation), nameof(GameLocation.ShowLockedDoorMessage))]
    public static IEnumerable<CodeInstruction> door_Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        CodeMatcher matcher = new(instructions);
        MethodInfo genderCheck = AccessTools.PropertyGetter(typeof(GameLocation), nameof(GameLocation.ShowLockedDoorMessage));
        MethodInfo modifyInfo = AccessTools.Method(typeof(Transpiling), nameof(Transpiling.SetNonBinaryDoor));

        matcher.MatchStartForward(
            new CodeMatch(OpCodes.Call, Gender.Male),
            new CodeMatch(OpCodes.Add)
            )
            .ThrowIfNotMatch($"Wrong entry point.")
            .Advance(1)
            .Insert(
            new CodeInstruction(OpCodes),
            new CodeInstruction(OpCodes.Call, modifyInfo));

        return matcher.InstructionEnumeration();
    }
    public static void SetNonBinaryDoor(Gender gender, string[] action)
    {
        Gender ownerGender = Gender.Female;
        string[] ownerNames = new string[(action.Length == 2) ? 1 : 2];
        for (int i = 0; i < ownerNames.Length; i++)
        {
            string ownerKey = action[i + 1];
            NPC owner = Game1.getCharacterFromName(ownerKey);
            if (owner != null)
            {
                ownerNames[i] = owner.displayName;
                ownerGender = owner.Gender;
                continue;
            }
            if (NPC.TryGetData(ownerKey, out var data))
            {
                ownerNames[i] = TokenParser.ParseText(data.DisplayName);
                ownerGender = data.Gender;
                continue;
            }
        }

        string lockedDoorMessage = ((ownerNames.Length <= 1) ? Game1.content.LoadString("Strings\\Locations:DoorUnlock_NotFriend_" + ((ownerGender == Gender.Male) ? "Male" : (ownerGender == Gender.Female) ? "Female" : "Undefined"), ownerNames[0]) : Game1.content.LoadString("Strings\\Locations:DoorUnlock_NotFriend_Couple", ownerNames[0], ownerNames[1]));

        //string lockedDoorMessage = ((ownerNames.Length <= 1) ? Game1.content.LoadString("Strings\\Locations:DoorUnlock_NotFriend_" + ((ownerGender == Gender.Male) ? "Male" : (ownerGender == Gender.Female) ? "Female" : "Undefined"), ownerNames[0]) : Game1.content.LoadString("Strings\\Locations:DoorUnlock_NotFriend_Couple", ownerNames[0], ownerNames[1]));
    }
}
