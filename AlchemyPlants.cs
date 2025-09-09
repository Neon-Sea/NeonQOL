using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace NeonQOL
{
	internal class AlchemyPlants : GlobalTile
	{
        private readonly AlchemyConfig config = ModContent.GetInstance<AlchemyConfig>();
        public override bool AutoSelect(int i, int j, int type, Item item)
		{
			//make so the staff or axe of regrowth can be auto selected for blooming herbs when holding shift (or whatever key auto select is set to)
			if (!config.AutoSelect)
			{
				return false;
			}
            Player player = Main.player[Main.myPlayer];
            Tile tile = Framing.GetTileSafely(i, j);
            //check if tile is within reach, then check if tile is a harvestable plant
            if (player.position.X / 16f - (float)Player.tileRangeX - (float)item.tileBoost <= (float)Player.tileTargetX &&
                (player.position.X + (float)player.width) / 16f + (float)Player.tileRangeX + (float)item.tileBoost - 1f >= (float)Player.tileTargetX &&
                player.position.Y / 16f - (float)Player.tileRangeY - (float)item.tileBoost <= (float)Player.tileTargetY &&
                (player.position.Y + (float)player.height) / 16f + (float)Player.tileRangeY + (float)item.tileBoost - 2f >= (float)Player.tileTargetY &&
				AlchemySystem.CheckForValidRegrowthTarget(type, TileObjectData.GetTileStyle(tile)))
			{
				if (item.type == ItemID.StaffofRegrowth || item.type == ItemID.AcornAxe) //pick whichever of the two it finds first
				{
					return true;
				}
				else if (!player.HasItem(ItemID.StaffofRegrowth) && !player.HasItem(ItemID.AcornAxe) && item.pick > 0) // only select pick if player has neither of the regrowth items
				{
					return true;
				}
				else return false;
			}
			return false;
		}

		public override void KillTile(int i, int j, int type, ref bool fail, ref bool effectOnly, ref bool noItem)
		{
            //KillTile override for blooming herbs, makes seeds replant if holding staff or axe and adjusts seed drops accordingly
            if (!config.Replant)
			{  
				return; 
			}
            if (type == TileID.MatureHerbs || type == TileID.BloomingHerbs)
			{
				Player player = Main.player[Player.FindClosest(new Vector2(i * 16, j * 16), 16, 16)];
				Tile targetTile = Framing.GetTileSafely(i, j);
				int targetStyle = TileObjectData.GetTileStyle(targetTile);
				if (WorldGen.IsHarvestableHerbWithSeed(type, targetStyle) && (player.HeldItem.type == ItemID.StaffofRegrowth || player.HeldItem.type == ItemID.AcornAxe))
				{
					Tile baseTile = Framing.GetTileSafely(i, j + 1);
					bool onPlanter = (baseTile.TileType == TileID.ClayPot || baseTile.TileType == TileID.PlanterBox);
					int plantDrop;
					int seedDrop;
					if (targetStyle != 6)
					{
						plantDrop = 313 + targetStyle;
						seedDrop = 307 + targetStyle;
					}
					else
					{
						plantDrop = ItemID.Shiverthorn;
						seedDrop = ItemID.ShiverthornSeeds;
					}
					EntitySource_TileBreak eSource = new(i, j);
					Rectangle tileRectangle = new(i * 16, j * 16, 16, 16);
					int item;
					if (Main.netMode != NetmodeID.MultiplayerClient || ((targetStyle ==  6 || targetStyle == 2) && player.whoAmI == Main.myPlayer))
					{
						item = Item.NewItem(eSource, tileRectangle, plantDrop, Main.rand.Next(1, 3));
						if (Main.netMode != NetmodeID.SinglePlayer)
						{
							NetMessage.SendData(MessageID.SyncItem, -1, Main.myPlayer, null, item, 1f);
						}
						item = Item.NewItem(eSource, tileRectangle, seedDrop, Main.rand.Next(onPlanter ? 0 : 1, onPlanter ? 5 : 6));
						if (Main.netMode != NetmodeID.SinglePlayer)
						{
							NetMessage.SendData(MessageID.SyncItem, -1, Main.myPlayer, null, item, 1f);
						}
					}
					if (onPlanter)
					{
						fail = true;
						targetTile.TileType = TileID.ImmatureHerbs;
						if (Main.netMode == NetmodeID.MultiplayerClient && ((targetStyle == 6 || targetStyle == 2) && player.whoAmI == Main.myPlayer))
                        {
							NetMessage.SendTileSquare(-1, i, j);
                        }
					}
					noItem = true;
				}
			}
		}
	}
}
