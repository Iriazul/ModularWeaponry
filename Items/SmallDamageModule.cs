using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ModularWeaponry.Items.Base;

namespace ModularWeaponry.Items
{
	public class SmallDamageModule:Module
	{
		public override void Initialize(ref ItemType itemType,ref ApplyStats applyStats,ref ApplyHitNPC applyHitNPC)
		{
			itemType=ItemType.Weap|ItemType.Tool;
			applyStats=delegate(Item item)
			{
				item.damage=(int)(item.damage*1.1+1);
			};
		}
		public override void SetDefaults()
		{
			item.name="Small Damage Module";
			item.toolTip="";
			
			item.width=item.height=16;
		}
	}
}
