using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;

namespace NeonQOL
{
	public class AlchemyPlants : GlobalTile
	{
		public override bool AutoSelect(int i, int j, int type, Item item)
		{
			Player player = Main.player[Main.myPlayer];
			//make so the staff of regrowth can be auto selected for blooming herbs when holding shift (or whatever key auto select is set to)
			if (type == 83 || type == 84)
			{
				int targetStyle = Framing.GetTileSafely(i, j).TileFrameX / 18;
				bool isBloomingPlant = type == 84;
				switch (targetStyle)
				{
					case 0:
						{
							if (Main.dayTime)
								isBloomingPlant = true;
							break;
						}
					case 1:
						{
							if (!Main.dayTime)
								isBloomingPlant = true;
							break;
						}
					case 3:
						{
							if (!Main.dayTime && (Main.bloodMoon || Main.moonPhase == 0))
								isBloomingPlant = true;
							break;
						}
					case 4:
						{
							if (Main.raining || Main.cloudAlpha > 0f)
								isBloomingPlant = true;
							break;
						}
					case 5:
						{
							if (!Main.raining && Main.dayTime && Main.time > 40500.00)
								isBloomingPlant = true;
							break;
						}
					default:
						break;
				}
				if (isBloomingPlant &&
				player.position.X / 16f - (float)Player.tileRangeX - (float)item.tileBoost <= (float)Player.tileTargetX &&
				(player.position.X + (float)player.width) / 16f + (float)Player.tileRangeX + (float)item.tileBoost - 1f >= (float)Player.tileTargetX &&
				player.position.Y / 16f - (float)Player.tileRangeY - (float)item.tileBoost <= (float)Player.tileTargetY &&
				(player.position.Y + (float)player.height) / 16f + (float)Player.tileRangeY + (float)item.tileBoost - 2f >= (float)Player.tileTargetY)
				{
					if (item.type == ItemID.StaffofRegrowth)
					{
						return true;
					}
					else if (!player.HasItem(ItemID.StaffofRegrowth) && item.pick > 0) // prioritize staff over pick for plants
					{
						return true;
					}
					else return false;
				}
				else return false;
			}
			else return false;
		}

		public override void KillTile(int i, int j, int type, ref bool fail, ref bool effectOnly, ref bool noItem)
		{
			//KillTile override for blooming herbs, makes seeds replant if holding staff and adjusts seed drops accordingly
			if (type == TileID.MatureHerbs || type == TileID.BloomingHerbs)
			{
				Player player = Main.player[Player.FindClosest(new Vector2(i, j), 16, 16)];
				Tile targetTile = Framing.GetTileSafely(i, j);
				int targetStyle = targetTile.TileFrameX / 18;
				if (WorldGen.IsHarvestableHerbWithSeed(type, targetStyle) && player.HeldItem.type == ItemID.StaffofRegrowth)
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
						plantDrop = 2358;
						seedDrop = 2357;
					}
					EntitySource_TileBreak eSource = new(i, j);
					Rectangle tileRectangle = new(i * 16, j * 16, 16, 16);
					int item = Item.NewItem(eSource, tileRectangle, plantDrop, WorldGen.genRand.Next(1, 3));
					if (Main.netMode != NetmodeID.SinglePlayer)
						NetMessage.SendData(MessageID.SyncItem, -1, -1, null, item, 1f);
					item = Item.NewItem(eSource, tileRectangle, seedDrop, WorldGen.genRand.Next(onPlanter ? 0 : 1, onPlanter ? 5 : 6));
					if (Main.netMode != NetmodeID.SinglePlayer)
						NetMessage.SendData(MessageID.SyncItem, -1, -1, null, item, 1f);
					if (onPlanter)
					{
						fail = true;
						targetTile.TileType = TileID.ImmatureHerbs;
					}
					noItem = true;
				}
			}
		}
	}

	public class AlchemyPlayer : ModPlayer
	{
		public override bool PreItemCheck()
		{
			if (Player.HeldItem.type == ItemID.StaffofRegrowth)
				Cursor();
			return true;
		}
		public void Cursor()
		{
			//adapted from vanilla, smart cursor code
			if (Player.whoAmI != Main.myPlayer)
				return;
			Main.SmartCursorShowing = false;
			if (!Main.SmartCursorIsUsed)
				return;
			Item item = Player.inventory[Player.selectedItem];
			Vector2 mouse = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);
			if (Player.gravDir == -1f)
				mouse.Y = Main.screenPosition.Y + Main.screenHeight - Main.mouseY;
			int targetX = (int)MathHelper.Clamp(Player.tileTargetX, 10, Main.maxTilesX - 10);
			int targetY = (int)MathHelper.Clamp(Player.tileTargetY, 10, Main.maxTilesY - 10);
			Tile targetTile = Main.tile[targetX, targetY];
			bool disableCursor = TileID.Sets.DisableSmartCursor[targetTile.TileType];
			int tileBoost = item.tileBoost;
			int maxLeft = (int)(Player.position.X / 16f) - Player.tileRangeX - tileBoost + 1;
			int maxRight = (int)((Player.position.X + Player.width) / 16f) + Player.tileRangeX + tileBoost - 1;
			int maxUp = (int)(Player.position.Y / 16f) - Player.tileRangeY - tileBoost + 1;
			int maxDown = (int)((Player.position.Y + Player.height) / 16f) + Player.tileRangeY + tileBoost - 2;
			maxLeft = Utils.Clamp(maxLeft, 10, Main.maxTilesX - 10);
			maxRight = Utils.Clamp(maxRight, 10, Main.maxTilesX - 10);
			maxDown = Utils.Clamp(maxDown, 10, Main.maxTilesY - 10);
			maxUp = Utils.Clamp(maxUp, 10, Main.maxTilesY - 10);
			if (disableCursor && targetX >= maxLeft && targetX <= maxRight && targetY <= maxDown && targetY >= maxUp)
				return;
			List<Tuple<int, int>> potentialTargetTiles = new List<Tuple<int, int>>();
			for (int xCheck = maxLeft; xCheck <= maxRight; xCheck++)
			{
				for (int yCheck = maxUp; yCheck <= maxDown; yCheck++)
				{
					Tile checkTile = Main.tile[xCheck, yCheck];
					bool isBloomingPlant = checkTile.TileType == TileID.BloomingHerbs;
					if (checkTile.TileType == TileID.MatureHerbs)
					{
						switch (checkTile.TileFrameX / 18)
						{
							case 0:
								{
									if (Main.dayTime)
										isBloomingPlant = true;
									break;
								}
							case 1:
								{
									if (!Main.dayTime)
										isBloomingPlant = true;
									break;
								}
							case 3:
								{
									if (!Main.dayTime && (Main.bloodMoon || Main.moonPhase == 0))
										isBloomingPlant = true;
									break;
								}
							case 4:
								{
									if (Main.raining || Main.cloudAlpha > 0f)
										isBloomingPlant = true;
									break;
								}
							case 5:
								{
									if (!Main.raining && Main.dayTime && Main.time > 40500.00)
										isBloomingPlant = true;
									break;
								}
							default:
								break;
						}
					}
					if (isBloomingPlant)
						potentialTargetTiles.Add(new Tuple<int, int>(xCheck, yCheck));
				}
			}
			if (potentialTargetTiles.Count > 0)
			{
				float distanceToMouse = -1f;
				Tuple<int, int> currentTileCoords = potentialTargetTiles[0];
				for (int target = 0; target < potentialTargetTiles.Count; target++)
				{
					float distanceNew = Vector2.Distance(new Vector2(potentialTargetTiles[target].Item1, potentialTargetTiles[target].Item2) * 16f + Vector2.One * 8f, mouse);
					if (distanceToMouse == -1f || distanceNew < distanceToMouse)
					{
						distanceToMouse = distanceNew;
						currentTileCoords = potentialTargetTiles[target];
					}
				}
				if (Collision.InTileBounds(currentTileCoords.Item1, currentTileCoords.Item2, maxLeft, maxUp, maxRight, maxDown))
				{
					Main.SmartCursorX = (Player.tileTargetX = currentTileCoords.Item1);
					Main.SmartCursorY = (Player.tileTargetY = currentTileCoords.Item2);
					Main.SmartCursorShowing = true;
				}
				potentialTargetTiles.Clear();
			}
		}

	}
}
