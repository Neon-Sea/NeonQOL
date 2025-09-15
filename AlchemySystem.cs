using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NeonQOL
{
    internal class AlchemySystem : ModSystem
    {
        public static AlchemySystem instance;
        public static List<HarvestablePlant> AllHarvestablePlants = [];
        public static List<string> ModsDisablingAutoSelect = [];
        public static List<string> ModsDisablingSmartCursor = [];
        public static List<string> ModsDisablingReplant = [];
        private readonly AlchemyConfig config = ModContent.GetInstance<AlchemyConfig>();

        public override void Load()
        {
            instance = this;
            // vanilla plants and their blooming conditions since vanilla treats plants oddly
            AllHarvestablePlants.Add(new HarvestablePlant(TileID.ImmatureHerbs, TileID.MatureHerbs, ItemID.Daybloom, ItemID.DaybloomSeeds, 0, 0, () => Main.dayTime, () => Main.dayTime, () => Main.dayTime));
            AllHarvestablePlants.Add(new HarvestablePlant(TileID.ImmatureHerbs, TileID.MatureHerbs, ItemID.Moonglow, ItemID.MoonglowSeeds, 1, 1, () => !Main.dayTime, () => !Main.dayTime, () => !Main.dayTime));
            AllHarvestablePlants.Add(new HarvestablePlant(TileID.ImmatureHerbs, TileID.BloomingHerbs, ItemID.Blinkroot, ItemID.BlinkrootSeeds, 2, 2));
            AllHarvestablePlants.Add(new HarvestablePlant(TileID.ImmatureHerbs, TileID.MatureHerbs, ItemID.Deathweed, ItemID.DeathweedSeeds, 3, 3, () => !Main.dayTime && (Main.bloodMoon || Main.moonPhase == 0), () => !Main.dayTime && (Main.bloodMoon || Main.moonPhase == 0), () => !Main.dayTime && (Main.bloodMoon || Main.moonPhase == 0)));
            AllHarvestablePlants.Add(new HarvestablePlant(TileID.ImmatureHerbs, TileID.MatureHerbs, ItemID.Waterleaf, ItemID.WaterleafSeeds, 4, 4, () => Main.raining || Main.cloudAlpha > 0f, () => Main.raining || Main.cloudAlpha > 0f, () => Main.raining || Main.cloudAlpha > 0f));
            AllHarvestablePlants.Add(new HarvestablePlant(TileID.ImmatureHerbs, TileID.MatureHerbs, ItemID.Fireblossom, ItemID.FireblossomSeeds, 5, 5, () => !Main.raining && Main.dayTime && Main.time > 40500.00, () => !Main.raining && Main.dayTime && Main.time > 40500.00, () => !Main.raining && Main.dayTime && Main.time > 40500.00));
            AllHarvestablePlants.Add(new HarvestablePlant(TileID.ImmatureHerbs, TileID.BloomingHerbs, ItemID.Shiverthorn, ItemID.ShiverthornSeeds, 6, 6));
        }

        public override void Unload()
        {
            AllHarvestablePlants = null;
            ModsDisablingAutoSelect = null;
            ModsDisablingSmartCursor = null;
            ModsDisablingReplant = null;
            instance = null;
        }

        // check each plant in the list along with its condition for replanting
        public static HarvestablePlant CheckForValidRegrowthReplant(int type, int tileStyle)
        {
            return AllHarvestablePlants.Find(plant =>
            {
                if (plant.ItemTypePlantList == null && type == plant.TileTypeBlooming && plant.CondReplant() && tileStyle == plant.TileStyleBlooming)
                {
                    return true;
                }
                else if (plant.ItemTypePlantList != null && type == plant.TileTypeBlooming && plant.CondReplant())
                {
                    foreach (int i in plant.TileStyleBloomingList)
                    {
                        if (tileStyle == i)
                        {
                            return true;
                        }
                    }
                    return false;
                }
                else return false;
            }
            );
        }

        // check each plant in the list along with its condition for auto select
        public static HarvestablePlant CheckForValidRegrowthAutoSelect(int type, int tileStyle)
        {
            return AllHarvestablePlants.Find(plant =>
            {
                if (plant.ItemTypePlantList == null && type == plant.TileTypeBlooming && plant.CondAutoSelect() && tileStyle == plant.TileStyleBlooming)
                {
                    return true;
                }
                else if (plant.ItemTypePlantList != null && type == plant.TileTypeBlooming && plant.CondAutoSelect())
                {
                    foreach (int i in plant.TileStyleBloomingList)
                    {
                        if (tileStyle == i)
                        {
                            return true;
                        }
                    }
                    return false;
                }
                else return false;
            }
            );
        }

        // check each plant in the list along with its condition for smart cursor
        public static HarvestablePlant CheckForValidRegrowthSmartCursor(int type, int tileStyle)
        {
            return AllHarvestablePlants.Find(plant =>
            {
                if (plant.ItemTypePlantList == null && type == plant.TileTypeBlooming && plant.CondSmartCursor() && tileStyle == plant.TileStyleBlooming)
                {
                    return true;
                }
                else if (plant.ItemTypePlantList != null && type == plant.TileTypeBlooming && plant.CondSmartCursor())
                {
                    foreach (int i in plant.TileStyleBloomingList)
                    {
                        if (tileStyle == i)
                        {
                            return true;
                        }
                    }
                    return false;
                }
                else return false;
            }
            );
        }

        public object Call(params object[] args)
        {
            try
            {
                // unfortunate if else chain to build mod calls, would love to improve on this if i can find a better way to deal with a list of unknown length containing objects of unknown type
                if (args.Length == 0)
                {
                    Mod.Logger.Error("Call Error: No arguments provided for mod call");
                }
                string message = args[0] as string;
                if (message == "AddHarvestablePlant")
                {
                    if (args.Length < 6)
                    {
                        Mod.Logger.Error("Call Error: Insufficient arguments provided for AddHarvestablePlant");
                        return false;
                    }
                    string callingMod = "Unknown Mod";
                    if (args[1] is Mod mod)
                    {
                        callingMod = mod.DisplayName;
                    }
                    int typeImmature = (int)args[2];
                    int typeBlooming = (int)args[3];
                    if (typeImmature < TileID.Count || typeBlooming < TileID.Count)
                    {
                        Mod.Logger.Error("Call Error: Provided TileID is a vanilla tile");
                        return false;
                    }
                    else if (args.Length == 6)
                    {
                        if (args[4] is int plantDrops && args[5] is int seedDrops)
                        {
                            AllHarvestablePlants.Add(new HarvestablePlant(typeImmature, typeBlooming, plantDrops, seedDrops));
                            Mod.Logger.Info("New plant with tile name " + ModContent.GetModTile(typeBlooming).Name + " added by " + callingMod);
                            return true;
                        }
                        else
                        {
                            Mod.Logger.Error("Could not add plant, there was a problem with the provided values");
                            return false;
                        }
                    }
                    else if (args.Length == 7)
                    {
                        if (args[4] is int plantDrops && args[5] is int seedDrops && args[6] is int tileStyleImmature)
                        {
                            AllHarvestablePlants.Add(new HarvestablePlant(typeImmature, typeBlooming, plantDrops, seedDrops, tileStyleImmature));
                            Mod.Logger.Info("New plant with tile name " + ModContent.GetModTile(typeBlooming).Name + " added by " + callingMod);
                            return true;
                        }
                        else
                        {
                            Mod.Logger.Error("Could not add plant, there was a problem with the provided values");
                            return false;
                        }
                    }
                    else if (args.Length == 8)
                    {
                        if (args[4] is int plantDrops && args[5] is int seedDrops && args[6] is int tileStyleImmature && args[7] is int tileStyleBlooming)
                        {
                            AllHarvestablePlants.Add(new HarvestablePlant(typeImmature, typeBlooming, plantDrops, seedDrops, tileStyleImmature, tileStyleBlooming));
                            Mod.Logger.Info("New plant with tile name " + ModContent.GetModTile(typeBlooming).Name + " added by " + callingMod);
                            return true;
                        }
                        else if (args[4] is int[] plantDropsList && args[5] is int[] seedDropsList && args[6] is int[] tileStyleImmatureList && args[7] is int[] tileStyleBloomingList)
                        {
                            AllHarvestablePlants.Add(new HarvestablePlant(typeImmature, typeBlooming, [.. plantDropsList], [.. seedDropsList], [.. tileStyleImmatureList], [.. tileStyleBloomingList]));
                            Mod.Logger.Info("New plants with tile name " + ModContent.GetModTile(typeBlooming).Name + " added by " + callingMod);
                            return true;
                        }
                        else
                        {
                            Mod.Logger.Error("Could not add plant, there was a problem with the provided values");
                            return false;
                        }
                    }
                    else if (args.Length == 9)
                    {
                        if (args[8] is not Func<bool> condReplant)
                        {
                            Mod.Logger.Error("Could not add plant, there was a problem with the provided values");
                            return false;
                        }
                        else if (args[4] is int plantDrops && args[5] is int seedDrops && args[6] is int tileStyleImmature && args[7] is int tileStyleBlooming)
                        {
                            AllHarvestablePlants.Add(new HarvestablePlant(typeImmature, typeBlooming, plantDrops, seedDrops, tileStyleImmature, tileStyleBlooming, condReplant));
                            Mod.Logger.Info("New plant with tile name " + ModContent.GetModTile(typeBlooming).Name + " added by " + callingMod);
                            return true;
                        }
                        else if (args[4] is int[] plantDropsList && args[5] is int[] seedDropsList && args[6] is int[] tileStyleImmatureList && args[7] is int[] tileStyleBloomingList)
                        {
                            AllHarvestablePlants.Add(new HarvestablePlant(typeImmature, typeBlooming, [.. plantDropsList], [.. seedDropsList], [.. tileStyleImmatureList], [.. tileStyleBloomingList], condReplant));
                            Mod.Logger.Info("New plants with tile name " + ModContent.GetModTile(typeBlooming).Name + " added by " + callingMod);
                            return true;
                        }
                        else
                        {
                            Mod.Logger.Error("Could not add plant, there was a problem with the provided values");
                            return false;
                        }
                    }
                    else if (args.Length == 10)
                    {
                        if (args[8] is not Func<bool> condReplant || args[9] is not Func<bool> condAutoSelect)
                        {
                            Mod.Logger.Error("Could not add plant, there was a problem with the provided values");
                            return false;
                        }
                        else if (args[4] is int plantDrops && args[5] is int seedDrops && args[6] is int tileStyleImmature && args[7] is int tileStyleBlooming)
                        {
                            AllHarvestablePlants.Add(new HarvestablePlant(typeImmature, typeBlooming, plantDrops, seedDrops, tileStyleImmature, tileStyleBlooming, condReplant, condAutoSelect));
                            Mod.Logger.Info("New plant with tile name " + ModContent.GetModTile(typeBlooming).Name + " added by " + callingMod);
                            return true;
                        }
                        else if (args[4] is int[] plantDropsList && args[5] is int[] seedDropsList && args[6] is int[] tileStyleImmatureList && args[7] is int[] tileStyleBloomingList)
                        {
                            AllHarvestablePlants.Add(new HarvestablePlant(typeImmature, typeBlooming, [.. plantDropsList], [.. seedDropsList], [.. tileStyleImmatureList], [.. tileStyleBloomingList], condReplant, condAutoSelect));
                            Mod.Logger.Info("New plants with tile name " + ModContent.GetModTile(typeBlooming).Name + " added by " + callingMod);
                            return true;
                        }
                        else
                        {
                            Mod.Logger.Error("Could not add plant, there was a problem with the provided values");
                            return false;
                        }
                    }
                    else if (args.Length == 11)
                    {
                        if (args[8] is not Func<bool> condReplant || args[9] is not Func<bool> condAutoSelect || args[10] is not Func<bool> condSmartCursor)
                        {
                            Mod.Logger.Error("Could not add plant, there was a problem with the provided values");
                            return false;
                        }
                        else if (args[4] is int plantDrops && args[5] is int seedDrops && args[6] is int tileStyleImmature && args[7] is int tileStyleBlooming)
                        {
                            AllHarvestablePlants.Add(new HarvestablePlant(typeImmature, typeBlooming, plantDrops, seedDrops, tileStyleImmature, tileStyleBlooming, condReplant, condAutoSelect, condSmartCursor));
                            Mod.Logger.Info("New plant with tile name " + ModContent.GetModTile(typeBlooming).Name + " added by " + callingMod);
                            return true;
                        }
                        else if (args[4] is int[] plantDropsList && args[5] is int[] seedDropsList && args[6] is int[] tileStyleImmatureList && args[7] is int[] tileStyleBloomingList)
                        {
                            AllHarvestablePlants.Add(new HarvestablePlant(typeImmature, typeBlooming, [.. plantDropsList], [.. seedDropsList], [.. tileStyleImmatureList], [.. tileStyleBloomingList], condReplant, condAutoSelect, condSmartCursor));
                            Mod.Logger.Info("New plants with tile name " + ModContent.GetModTile(typeBlooming).Name + " added by " + callingMod);
                            return true;
                        }
                        else
                        {
                            Mod.Logger.Error("Could not add plant, there was a problem with the provided values");
                            return false;
                        }
                    }
                    else
                    {
                        Mod.Logger.Error("Could not add plant, there was a problem with the provided values");
                        return false;
                    }
                }    
                else if (message == "DisableRegrowthConfigs")
                {
                    if (args.Length < 2)
                    {
                        throw new Exception("Smart Regrowth Call Error: No arguments provided for DisableRegrowthConfigs");
                    }
                    if (args[1] is not Mod callingMod)
                    {
                        throw new Exception("Smart Regrowth Call Error: Invalid mod argument provided for DisableRegrowthConfigs");
                    }
                    bool disableReplant = Convert.ToBoolean(args[2]);
                    bool disableAutoSelect = args.Length > 3 && Convert.ToBoolean(args[3]);
                    bool disableSmartCursor = args.Length > 4 && Convert.ToBoolean(args[4]);

                    if (disableAutoSelect)
                    {
                        ModsDisablingAutoSelect.Add(callingMod.DisplayName);
                    }

                    if (disableSmartCursor)
                    {
                        ModsDisablingSmartCursor.Add(callingMod.DisplayName);
                    }

                    if (disableReplant)
                    {
                        ModsDisablingReplant.Add(callingMod.DisplayName);
                    }

                    config.AutoSelect = !disableAutoSelect && config.AutoSelect;
                    config.SmartCursor = !disableSmartCursor && config.SmartCursor;
                    config.Replant = !disableReplant && config.Replant;
                    config.SaveChanges(pendingConfig: null, status: null, silent: true, broadcast: false);

                    return true;
                }
                else
                {
                    Mod.Logger.Error("Smart Regrowth Call Error: Unknown Message: " + message);
                }
            }
            catch (Exception e)
            {
                Mod.Logger.Error("Smart Regrowth Call Error: " + e.StackTrace + e.Message);
            }
            return false;
        }
    }
}
