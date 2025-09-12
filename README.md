# Smart Regrowth
Adds the following features to the Staff of Regrowth and Axe of Regrowth:
- Can be autoselected
- Works with the smart cursor
- Automatically replants seeds in pots and planters

Only works on plants, does not affect behavior when placing grass on dirt. Also does not alter the behavior of the Axe of Regrowth to replant tree saplings.

# Mod Calls
This mod offers compatibility options using the following mod calls. For best results, send these in `ModSystem.PostSetupContent()`

## AddHarvestablePlant
Adds an entry to the list of tiles that you would like to function with this mod

### Argument 0
`string` - "AddHarvestablePlant". Indicates the type of mod call being sent
### Argument 1
`Mod` - Your mod's instance
### Argument 2
`int` - TileID of your custom plant in its initial (freshly planted) stage.
### Argument 3
`int` - TileID of your custom plant in its harvestable stage.\
Can be the same as above e.g. if your plant uses styles to represent growth stages
### Argument 4
`int` or `int[]` - ItemID of the herb that your custom plant drops.\
Also accepts a list if your tile represents more than one plant
### Argument 5
`int` or `int[]` - ItemID of the seed that your custom plant drops.\
If a list is provided, it must be the same length and order as the one used above
### Argument 6
`int` or `int[]` - Optional, TileStyle of your custom plant in its initial (freshly planted) stage. Defaults to 0 if none given.\
Use this if your plant uses a specific style to determine whether it is harvestable.\
If a list is provided, it must be the same length and order as the ones used above
### Argument 7
`int` or `int[]` - Optional, TileStyle of your custom plant in its harvestable stage. Defaults to 0 if none given.\
If a list is provided, it must be the same length and order as the ones used above
### Argument 8
`Func<bool>` - Optional, replant condition. If the provided function returns true then the provided plant will be automatically replanted by the Staff/Axe of Regrowth, and vice versa if false.\
Useful for example if you want to gate this mod's interaction with your plant behind certain progression flags, or just for testing purposes.
### Argument 9
`Func<bool>` - Optional, auto select condition. If the provided function returns true then the provided plant will auto select the Staff/Axe of Regrowth when holding auto select, and vice versa if it returns false.\
Useful for example if you want to gate this mod's interaction with your plant behind certain progression flags, or just for testing purposes.
### Argument 10
`Func<bool>` - Optional, smart cursor condition. If the provided function returns true then the Staff/Axe of regrowth will treat the provided plant as a valid smart cursor target, and vice versa if it returns false.\
Useful for example if you want to gate this mod's interaction with your plant behind certain progression flags, or just for testing purposes.

### Examples
```C#
//This is an example of the fewest required arguments to add a plant
NeonQOL.Call("AddHarvestablePlant",
             Mod,
             ModContent.TileType<Tiles.CoolPlantImmature>(),
             ModContent.TileType<Tiles.CoolPlantBlooming>(),
             ModContent.ItemType<CoolPlantFlower>(),
             ModContent.ItemType<CoolPlantSeed>());
```

```C#
//This is an example of a tile that represents multiple plants and growth stages, and uses a special condition for replanting
NeonQOL.Call("AddHarvestablePlant",
             Mod,
             ModContent.TileType<Tiles.CustomPlants>(),
             ModContent.TileType<Tiles.CustomPlants>(),
             new int[] { ModContent.ItemID<Items.CustomPlant1>(),ModContent.ItemID<Items.CustomPlant2>(),ModContent.ItemID<Items.CustomPlant3>() },
             new int[] { ModContent.ItemID<Items.CustomPlantSeed1>(),ModContent.ItemID<Items.CustomPlantSeed2>(),ModContent.ItemID<Items.CustomPlantSeed3>() },
             new int[] { 0,3,6 }
             new int[] { 2,5,8 }
             () => Main.hardMode); //Will only work on these plants in hardmode
```

## DisableRegrowthConfigs
If the `Func<bool>` arguments from the above call are not sufficient and/or you need to outright disable any of the three added features from this mod for compatibility or balancing reasons, you can do so with this mod call

Example: `NeonQOL.Call("DisableRegrowthConfigs", Mod, true, false, false);`

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