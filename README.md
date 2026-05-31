Adds an event command: Aviroen.Large

Aviroen.Large takes 4 arguments

<npc_name> <frame_int> <width_int> <height_int>

When you're scripting events you use it like so:

```Aviroen.Large Abigail 18 32 32```

This makes it so Abigail will use her 18th frame (starting from 0) and will now be 32 width and 32 height.

If you need to return the NPC back to normal size:

```Aviroen.Large Abigail 0 16 32```

So Abigail will be on frame 0, at 16 width, and 32 height.
