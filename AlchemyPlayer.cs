using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace NeonQOL
{
    public class AlchemyPlayer : ModPlayer
    {
        private readonly AlchemyConfig config = ModContent.GetInstance<AlchemyConfig>();

        public override bool PreItemCheck()
        {
            if (config.SmartCursor && (Player.HeldItem.type == ItemID.StaffofRegrowth || Player.HeldItem.type == ItemID.AcornAxe))
            {
                Cursor();
            }    
            return true;
        }

        private void Cursor()
        {
            //adapted from vanilla, smart cursor code
            if (Player.whoAmI != Main.myPlayer || !Main.SmartCursorIsUsed)
            {
                return;
            }
            Main.SmartCursorShowing = false;
            Item item = Player.inventory[Player.selectedItem];
            Vector2 mouse = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);
            if (Player.gravDir == -1f)
            {
                mouse.Y = Main.screenPosition.Y + Main.screenHeight - Main.mouseY;
            }
            // establish target location and boundaries of player's reach
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
            // disables smart cursor if the target tile is programmed to do so, BUT ONLY if it is in range, otherwise a random door on the edge of the screen will disable smart cursor
            if (disableCursor && targetX >= maxLeft && targetX <= maxRight && targetY <= maxDown && targetY >= maxUp)
            {
                return;
            }
            // check all tiles in range and add valid targets to a list
            List<Tuple<int, int>> potentialTargetTiles = [];
            for (int xCheck = maxLeft; xCheck <= maxRight; xCheck++)
            {
                for (int yCheck = maxUp; yCheck <= maxDown; yCheck++)
                {
                    Tile checkTile = Main.tile[xCheck, yCheck];
                    if (AlchemySystem.CheckForValidRegrowthSmartCursor(checkTile.TileType, TileObjectData.GetTileStyle(checkTile)) != null)
                        {
                        potentialTargetTiles.Add(new Tuple<int, int>(xCheck, yCheck));
                    }
                }
            }
            if (potentialTargetTiles.Count > 0)
            {
                // if there are valid targets, pick the one closest to the mouse
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
