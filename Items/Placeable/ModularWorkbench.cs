using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ModularWeaponry.Items.Placeable
{
    public class ModularWorkbench : ModItem
    {
        public override void SetDefaults()
        {
            item.name = "Modular Workbench";
            item.width = item.height = 16;
            item.toolTip = "Used to apply modules on weapons";

            item.maxStack = 99;

            item.useStyle = 1;
            item.useTime = 15;
            item.useAnimation = 15;

            item.createTile = mod.TileType("ModularWorkbench");
        }
    }
}
