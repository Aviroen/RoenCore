using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewValley;
using RoenCore.Framework;
using RoenCore.Models;

namespace RoenCore.Patches;

internal class Events
{
    private static IModRegistry ModRegistry { get; set; } = null!;
    private static IMonitor Monitor { get; set; } = null!;
    private static IModHelper Helper { get; set; } = null!;
    public static void Initialize(IModRegistry registry, IMonitor monitor, IModHelper helper)
    {
        ModRegistry = registry;
        Monitor = monitor;
        Helper = helper;
    }
    public static void command_LargeFrame(Event @event, string[] args, EventContext context)
    {
        //name, frame, width, height
        if (!ArgUtility.TryGet(args, 1, out var actorName, out var error, allowBlank: true, "actorName") 
            || !ArgUtility.TryGetInt(args, 2, out var frame, out error, "frame") 
            || !ArgUtility.TryGetInt(args, 3, out var width, out error, "width") 
            || !ArgUtility.TryGetInt(args, 4, out var height, out error, "height"))
        {
            context.LogErrorAndSkip(error);
            return;
        }
        if (!@event.IsFarmerActorId(actorName, out var farmerNumber))
        {
            bool isOptionalNpc;
            NPC n = @event.getActorByName(actorName, out isOptionalNpc);
            if (n == null)
            {
                context.LogErrorAndSkip("no NPC found with name '" + actorName + "'", isOptionalNpc);
                return;
            }
            n.Sprite.SpriteWidth = width;
            n.Sprite.SpriteHeight = height;
            n.Sprite.CurrentFrame = frame / 2;
        }
        @event.CurrentCommand++;
        @event.Update(context.Location, context.Time);
    }
    /*
    public static void command_playerControl(Event @event, string[] args, EventContext context)
    {
        //event id, true/false, int in ms for timer, npc for host, string for host
        bool isOptionalNpc;
        Game1.player.CanMove = true;
        Game1.viewportFreeze = false;
        Game1.forceSnapOnNextViewportUpdate = true;
        Game1.player.canOnlyWalk = false;
        Game1.globalFade = false;
        if (!ArgUtility.TryGet(args, 1, out var playerControlEventID, out var error, allowBlank: false, "string playerControlEventID")
            || !ArgUtility.TryGetBool(args, 2, out var timerBool, out error, "string timer bool")
            || !ArgUtility.TryGetInt(args, 3, out var timer, out error, "string timer")
            || !ArgUtility.TryGet(args, 4, out var npc, out error, allowBlank: false, "string NPC Name")
            || !ArgUtility.TryGet(args, 5, out var hostMessage, out error, allowBlank: false, "string host message"))
        {
            context.LogErrorAndSkip(error);
            return;
        }
        NPC n = @event.getActorByName(npc, out isOptionalNpc);
        @event.festivalHost = n;
        @event.hostMessageKey = hostMessage;
        if (timerBool == true)
        {
            @event.festivalTimer = timer;
        }
        @event.CurrentCommand++;
    }
    */
    public static void command_TempAct(Event @event, string[] args, EventContext context)
    {
        //{{ModId}} x y width height
        if (!ArgUtility.TryGet(args, 1, out var actorName, out var error, allowBlank: false, "string NpcName")
            || !ArgUtility.TryGetRectangle(args, 2, out Rectangle rectangle, out error, "rectangle"))
        {
            context.LogErrorAndSkip(error);
            return;
        }
        bool isOptionalNpc;
        NPC n = @event.getActorByName(actorName, out isOptionalNpc);

        var NpcModelDataAsset = Game1.content.Load<Dictionary<string, NpcModel>>("Aviroen.RoenCore/NpcList");
        foreach (var entry in NpcModelDataAsset)
        {
            if (NpcModelDataAsset.TryGetValue(actorName, out var InvitedList))
            {
                int x = Game1.random.Next(rectangle.X, rectangle.X + rectangle.Width);
                int y = Game1.random.Next(rectangle.Y, rectangle.Y + rectangle.Height);
                @event.addActor(entry.Key, x, y, 0, context.Location);
                @event.CurrentCommand++;
            }
        }
    }
}
