using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ModularWeaponry.Items.Base;

namespace ModularWeaponry.Items
{
	public class OverClocker:Module
	{
		public override void InitializeActions(ref ApplyStats applyStats,ref ApplyHitNPC applyHitNPC)
		{
			applyStats=delegate(Item item)
			{
				item.useTime=(int)(item.useTime*0.9);
				item.useAnimation=(int)(item.useAnimation*0.9);
			};
		}
		public override void SetDefaults()
		{
			item.name="Overclocker";
			item.toolTip="";
			itemType=ItemType.Weap|ItemType.Tool;
			
			item.width=item.height=16;
		}
	}
}
