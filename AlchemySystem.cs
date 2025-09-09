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
        public static List<Mod> ModsDisablingAutoSelect = [];
        public static List<Mod> ModsDisablingSmartCursor = [];
        public static List<Mod> ModsDisablingReplant = [];
        private readonly AlchemyConfig config = ModContent.GetInstance<AlchemyConfig>();

        public override void Load()
        {
            instance = this;
            AllHarvestablePlants.Add(new HarvestablePlant(TileID.MatureHerbs, 0)); //Daybloom
            AllHarvestablePlants.Add(new HarvestablePlant(TileID.MatureHerbs, 1)); //Moonglow
            AllHarvestablePlants.Add(new HarvestablePlant(TileID.BloomingHerbs, 2)); //Blinkroot
            AllHarvestablePlants.Add(new HarvestablePlant(TileID.MatureHerbs, 3)); //Deathweed
            AllHarvestablePlants.Add(new HarvestablePlant(TileID.MatureHerbs, 4)); //Waterleaf
            AllHarvestablePlants.Add(new HarvestablePlant(TileID.MatureHerbs, 5)); //Fireblossom
            AllHarvestablePlants.Add(new HarvestablePlant(TileID.BloomingHerbs, 6)); //Shiverthorn
        }

        public override void Unload()
        {
            AllHarvestablePlants = null;
            ModsDisablingAutoSelect = null;
            ModsDisablingSmartCursor = null;
            ModsDisablingReplant = null;
            instance = null;
        }

        public static bool CheckForValidRegrowthTarget(int type, int tileStyle)
        {
            bool isHarvestablePlant = AllHarvestablePlants.Any(plant => plant.Type == type && tileStyle == plant.TileStyle);
            if (isHarvestablePlant && type == TileID.MatureHerbs)
            {                
                switch (tileStyle)
                {
                    case 0: //Daybloom
                        {
                            if (!Main.dayTime)
                                isHarvestablePlant = false;
                            break;
                        }
                    case 1: //Moonglow
                        {
                            if (Main.dayTime)
                                isHarvestablePlant = false;
                            break;
                        }
                    case 3: //Deathweed
                        {
                            if (!(!Main.dayTime && (Main.bloodMoon || Main.moonPhase == 0)))
                                isHarvestablePlant = false;
                            break;
                        }
                    case 4: //Waterleaf
                        {
                            if (!(Main.raining || Main.cloudAlpha > 0f))
                                isHarvestablePlant = false;
                            break;
                        }
                    case 5: //Fireblossom
                        {
                            if (!(!Main.raining && Main.dayTime && Main.time > 40500.00))
                                isHarvestablePlant = false;
                            break;
                        }
                    default:
                        {
                            isHarvestablePlant = false;
                            break;
                        }
                }
            }
            return isHarvestablePlant;
        }

        //string "AddHarvestablePlant", int TileID, int TileStyle
        //string "DisableRegrowthConfigs", Mod, bool AutoSelect, bool SmartCursor, bool Replant
        public object Call(params object[] args)
        {
            try
            {
                if (args.Length == 0)
                {
                    throw new ArgumentException("No arugments provided for Smarth Regrowth mod call");
                }
                string message = args[0] as string;
                if (message == "AddHarvestablePlant")
                {
                    if (args.Length < 2)
                    {
                        throw new ArgumentException("No arguments provided for AddHarvestablePlant");
                    }
                    int type = Convert.ToInt32(args[1]);
                    int tileStyle = args.Length > 2 ? Convert.ToInt32(args[2]) : 0;
                    if (type < TileID.Count )
                    {
                        throw new ArgumentException("Provided TileID is a vanilla tile");
                    }
                    AllHarvestablePlants.Add(new HarvestablePlant(type, tileStyle));
                    return true;
                }
                else if (message == "DisableRegrowthConfigs")
                {
                    if (args.Length < 2)
                    {
                        throw new ArgumentException("No arguments provided for DisableRegrowthConfigs");
                    }
                    if (args[1] is not Mod callingMod)
                    {
                        throw new Exception("Invalid mod argument provided for DisableRegrowthConfigs");
                    }
                    bool disableAutoSelect = Convert.ToBoolean(args[2]);
                    bool disableSmartCursor = args.Length > 3 && Convert.ToBoolean(args[3]);
                    bool disableReplant = args.Length > 4 && Convert.ToBoolean(args[4]);

                    if (disableAutoSelect)
                    {
                        ModsDisablingAutoSelect.Add(callingMod);
                    }

                    if (disableSmartCursor)
                    {
                        ModsDisablingSmartCursor.Add(callingMod);
                    }

                    if (disableReplant)
                    {
                        ModsDisablingReplant.Add(callingMod);
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
