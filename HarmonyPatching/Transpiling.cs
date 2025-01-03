﻿using HarmonyLib;
using StardewModdingAPI;
using StardewValley;
using StardewValley.TokenizableStrings;
using System.Reflection;
using System.Reflection.Emit;

namespace RoenCore.HarmonyPatching;
[HarmonyPatch]
public class Transpiling
{
    /*
    [HarmonyPatch(typeof(InteriorDoor), nameof(InteriorDoor.ResetLocalState))]
    public static IEnumerable<CodeInstruction> Door_Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        CodeMatcher matcher = new(instructions);
        MethodInfo tileSheet = AccessTools.PropertyGetter(typeof(InteriorDoor), nameof(InteriorDoor.ResetLocalState));
        MethodInfo modifyInfo = AccessTools.Method(typeof(Transpiling), nameof(Transpiling.CustomDoorAnim));

        matcher.MatchStartForward(
            new CodeMatch(OpCodes));

        return matcher.InstructionEnumeration();
    }
    public static void CustomDoorAnim(string doorTexture)
    {
        this.Sprite = new TemporaryAnimatedSprite("LooseSprites\\Cursors", sourceRect, 100f, 4, 1, new Vector2(x, y - 2) * 64f, flicker: false, flip, (float)((y + 1) * 64 - 12) / 10000f, 0f, Color.White, 4f, 0f, 0f, 0f)
        {
            holdLastFrame = true,
            paused = true
        };
    }
    
    [HarmonyPatch(typeof(GameLocation), nameof(GameLocation.ShowLockedDoorMessage))]
    public static IEnumerable<CodeInstruction> Door_Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        CodeMatcher matcher = new(instructions);
        MethodInfo genderCheck = AccessTools.PropertyGetter(typeof(GameLocation), nameof(GameLocation.ShowLockedDoorMessage));
        MethodInfo modifyInfo = AccessTools.Method(typeof(Transpiling), nameof(Transpiling.SetNonBinaryDoor));

        matcher.MatchStartForward(
            new CodeMatch(OpCodes.Br_S),
            new CodeMatch(OpCodes.Ldstr, "Male")
            )
            .ThrowIfNotMatch($"Wrong entry point.")
            .RemoveInstructions(4)
            .Insert(
            new CodeInstruction(OpCodes.Call, modifyInfo));

        return matcher.InstructionEnumeration();
    }
    public static void SetNonBinaryDoor(string[] action)
    {
        Gender ownerGender = Gender.Female;
        string[] ownerNames = new string[(action.Length == 2) ? 1 : 2];
        string checkString = Game1.content.LoadString("Strings\\Locations:DoorUnlock_NotFriend_" + ownerNames);
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
        if (checkString is not null)
        {
            string lockedDoorMessage = ((ownerNames.Length <= 1) ? Game1.content.LoadString("Strings\\Locations:DoorUnlock_NotFriend_" +
            (ownerNames[0])) : Game1.content.LoadString("Strings\\Locations:DoorUnlock_NotFriend_Couple", ownerNames[0], ownerNames[1]));
            Game1.drawObjectDialogue(lockedDoorMessage);
        }
        else 
        {
            string lockedDoorMessage = ((ownerNames.Length <= 1) ? Game1.content.LoadString("Strings\\Locations:DoorUnlock_NotFriend_" +
            ((ownerGender == Gender.Male) ? "Male" : (ownerGender == Gender.Female) ? "Female" : "Undefined"), ownerNames[0]) : Game1.content.LoadString("Strings\\Locations:DoorUnlock_NotFriend_Couple", ownerNames[0], ownerNames[1]));
            Game1.drawObjectDialogue(lockedDoorMessage);
        }
    }
    public virtual void ShowLockedDoorMessage(string[] action)
    {
        Gender ownerGender = Gender.Female;
        string[] ownerNames = new string[action.Length - 1]; // seriously
        string? ownerInternalName = null; // newly declared local

        for (int i = 0; i < ownerNames.Length; i++)
        {
            var key = action[i + 1];
            NPC owner = Game1.getCharacterFromName(key);
            if (owner is not null)
            {
                ownerNames[i] = owner.displayName;
                ownerInternalName = owner.Name;
                ownerGender = owner.Gender;
            }
            else if (NPC.TryGetData(key, out var data))
            {
                ownerNames[i] = TokenParser.ParseText(data.DisplayName);
                ownerInternalName = owner.Name;
                ownerGender = owner.Gender;
            }
            else
            {
                // log error
                return;
            }
        }

        var lockedDoorMessage = (ownerNames.Length <= 1) ?
            (Game1.content.LoadStringReturnNullIfNotFound("Strings\\Locations:DoorUnlock_NotFriend_" + ownerInternalName) ?? 
            (Game1.content.LoadString("Strings\\Locations:DoorUnlock_NotFriend_" + ownerGender.ToString().Format(ownerNames[0]))) :
            Game1.content.LoadString("Strings\\Locations:DoorUnlock_NotFriend_Couple", ownerNames);
        Game1.drawObjectDialogue(lockedDoorMessage);
    }
    */
}
/*
 * Btw gender is ldloc.0 here, bc it's a local
 * Don't match for local cus it can change without warning
 * Don't attempt to match for labels either
 * some thought process to go through with this
 * if new CodeMatch(OpCodes.Call, Gender.Male), is where i want to add the additional things after the fact, do i just create the rest of the string? what happens with the rest of the string
 * original:
 * string lockedDoorMessage = ((ownerNames.Length <= 1) ? Game1.content.LoadString("Strings\\Locations:DoorUnlock_NotFriend_" + ((ownerGender == Gender.Male) ? "Male" : "Female"), ownerNames[0]) : Game1.content.LoadString("Strings\\Locations:DoorUnlock_NotFriend_Couple", ownerNames[0], ownerNames[1]));
 * from this point, would it call from the "Male" **INSERT MY IL HERE**? do i have to account for the rest of the line, or does it throw out the rest or do i have to force it to throw out the rest?
 * if i have to throw out the rest, where and how do i do that and what does that look like in ilspy
 * 
IL_0090: ldsfld class StardewValley.LocalizedContentManager StardewValley.Game1::content
IL_0095: ldstr "Strings\\Locations:DoorUnlock_NotFriend_" // first playing card
IL_009a: ldloc.0
IL_009b: brfalse.s IL_00a4 //checks if gender loaded with ldloc.0 is false female = 1, nb = 2

IL_009d: ldstr "Female"
IL_00a2: br.s IL_00a9

IL_00a4: ldstr "Male" = 2nd playing card on top of the stack

IL_00a9: call string [System.Runtime]System.String::Concat(string, string)
IL_00ae: ldloc.1
IL_00af: ldc.i4.0
IL_00b0: ldelem.ref
IL_00b1: callvirt instance string StardewValley.LocalizedContentManager::LoadString(string, object)
IL_00b6: stloc.2

IL_00b7: ldloc.2
IL_00b8: call void StardewValley.Game1::drawObjectDialogue(string)
IL_00bd: ret
} // end of method GameLocation::ShowLockedDoorMessage

where is what exactly and what am i looking at
is that Gender.Male the ldstr "Male"
*/