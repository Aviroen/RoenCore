using StardewModdingAPI;
using StardewValley;

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
        string actorName;
        int frame;
        int width;
        int height;
        string error;
        if (!ArgUtility.TryGet(args, 1, out actorName, out error, allowBlank: true, "actorName") || !ArgUtility.TryGetInt(args, 2, out frame, out error, "frame") || !ArgUtility.TryGetInt(args, 3, out width, out error, "width") || !ArgUtility.TryGetInt(args, 4, out height, out error, "height"))
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
}
