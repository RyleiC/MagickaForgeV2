Magicka Forge is a tool written in C# for creating modded XNB files for the 2011 game Magicka by Arrowhead. (Requires the .NET 8.0 Framework)
The tool uses JSON files as an aim to make modding more accessible and quicker than if the files were manually edited using hexidecimal editors with decompressed XNB files.

A XNB can be generated by dragging a valid JSON file onto the executable or manually typing in the file path in the console. If you use a folder instead of a file all JSON files in the folder will be compiled instead!
Likewise, JSONs can be generated from *decompressed* magicka XNB files in the same process.

XNBs generated by Magicka Forge should be placed inside Magicka/Content/Data/items/Wizard or Magicka/Content/Data/items/NPC for items or Magicka/Content/Data/characters after which will allow them to be be loaded into the vanilla game just like any other item.
When creating a character XNB ensure that you're using the Legacy Magicka prompt should you be playing on common game rollbacks like version 1.5.1.0, **please note that this tool is made with 1.5.1.0 in mind, not the modern release of magicka!**

Magicka Forge v2 was programmed by Rylei C. on 15-10-2024.