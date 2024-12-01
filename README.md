**Magicka Forge** is a toolset that can be used to create XNB content for the 2011 game Magicka by Arrowhead Studios.\
Magicka Forge was written by Rylei C, 06-08-2024.

# Features
The toolkit can both compile & decompile XNBs to JSON files, both allowing you to add in new content as well as view and tinker with pre-existing ones.

As of now, the tool supports items, characters & to a lesser extent, levels.
Characters & Items may essentially be anything without the bounds of the programming, so I recommend looking at other items to see how the developers did certain things.

# Instructions
To compile a JSON to an XNB, simply drag and drop the file onto the desired compiler and it should complete compilation swiftly.
For the case of characters, make sure to check which version of Magicka you wish to compile to, as modern & rollbacked characters are not compatible and *will* crash if not appropriately assigned.

These may all be loaded into the game like any other file via placing them in their respective content folders.
- (Items) Magicka/Content/Data/items/NPC *or* Magicka/Content/Data/items/Wizard
- (Characters) Magicka/Content/Data/Characters
- (Levels) Magicka/Content/Levels/*
