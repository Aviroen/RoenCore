using RoenCore.Models;
using StardewValley;
using RoenCore;
namespace RoenCore.Framework;

internal class Handler
{
    /*
     * i am not understanding a lick of wtf i'm doing here sorry
    public static string GetInvited(string entry)
    {
        List<string> invited = [.. AssetManager.NpcData[entry].InvitedList];
        if (invited.Count == 0)
        {
            return GetInvited(entry);
        }

        for (int i = 0; i < invited.Count; i++)
        {

        }
    }
    */
    private static Dictionary<string, NpcModel>? invitedList = null;
    public static Dictionary<string, NpcModel> data
    {
        get
        {
            if (invitedList == null)
            {
                invitedList = Game1.content.Load<Dictionary<string, NpcModel>>("Aviroen.RoenCore/NpcList");
            }
            RoenCore.ModMonitor.Log($"Loaded asset Aviroen.RoenCore/NpcList with {invitedList.Count} entries.");
            return invitedList!;
        }
        
    }
}

