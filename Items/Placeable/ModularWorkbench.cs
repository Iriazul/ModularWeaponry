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
			item.name="Modular Workbench";
			item.toolTip="Used to apply modules to weapons and tools";
			item.width=15;
			item.height=14;
			item.maxStack=99;
			item.useStyle=1;
			item.useTime=15;
			item.useAnimation=15;
			item.consumable=true;
			item.createTile=mod.TileType("ModularWorkbench");
		}
	}
}
