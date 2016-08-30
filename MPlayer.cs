using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ModularWeaponry
{
    public class MPlayer : ModPlayer
    {
        public int workbenchX, workbenchY;

        public override void PreUpdate()
        {
            if (ModularWeaponry.moduleInterfaceOpen)
            {
                if (player.chest != -1)
                {
                    ModularWeaponry.moduleInterfaceOpen = false;
                    return;
                }

                int tileX = (int)((player.position.X + player.width * 0.5) / 16.0);
                int tileY = (int)((player.position.Y + player.height * 0.5) / 16.0);
                if (tileX < this.workbenchX - Player.tileRangeX || tileX > workbenchX + Player.tileRangeX + 1 || tileY < workbenchY - Player.tileRangeY || tileY > workbenchY + Player.tileRangeY + 1)
                {
                    Main.PlaySound(11, -1, -1, 1);
                    ModularWeaponry.moduleInterfaceOpen = false;
                }
                else if (!Main.tile[this.workbenchX, this.workbenchY].active() || !Main.playerInventory)
                {
                    Main.PlaySound(11, -1, -1, 1);
                    ModularWeaponry.moduleInterfaceOpen = false;
                }
            }
        }
    }
}
