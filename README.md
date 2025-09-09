# Smart Regrowth
Adds the following features to the Staff of Regrowth and Axe of Regrowth:
- Can be autoselected
- Works with the smart cursor
- Automatically replants seeds in pots and planters

Only works on plants, does not affect behavior when placing grass on dirt. Also does not alter the behavior of the Axe of Regrowth to replant tree saplings.

# Mod Calls
This mod offers compatibility options using the following mod calls. For best results, send these in `ModSystem.PostSetupContent()`\
Please note that at this time, modded plants will only work with auto select and smart cursor. Replanting seeds only supports vanilla plants and will (or should) not affect plants added by mod calls

## AddHarvestablePlant
Adds an entry to the list of tiles that can be targeted by auto select and the smart cursor while holding the Staff/Axe of Regrowth

Example: `NeonQOL.Call("AddHarvestablePlant", ModContent.TileType<Tiles.CustomPlant>(), 0)`

### Argument 0
`string` - "AddHarvestablePlant". Indicates the type of mod call being sent
### Argument 1
`int` - TileID of your custom plant
### Argument 2
`int` - TileStyle. Optional, defaults to 0 if none given. Use this if your plant uses a specific style to determine whether it is harvestable

## DisableRegrowthConfigs
If, for any reason, you need to disable any of the three added features from this mod, you can do so with this mod call

Example: `NeonQOL.Call("DisableRegrowthConfigs", Mod, true, false, false)`

### Argument 0
`string` - "DisableRegrowthConfigs". Indicates the type of mod call being sent
### Argument 1
`Mod` - your mod's instance
### Argument 2
`bool` - DisableAutoSelect. Sending true will disable the ability of the Staff/Axe of Regrowth to be autoselected while holding the auto select key and hovering the mouse over a harvestable plant
### Argument 3
`bool` - DisableSmartCursor. Sending true will disable the ability of the smart cursor to automatically select harvestable plants while holding the Staff/Axe of Regrowth
### Argument 4
`bool` - DisableReplant. Sending true will disable the ability of the Staff/Axe of Regrowth to automatically replant seeds when harvesting a plant in a pot or planter