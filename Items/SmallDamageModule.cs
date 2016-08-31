using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ModularWeaponry.Items.Base;

namespace ModularWeaponry.Items
{
	public class SmallDamageModule : Module
	{
		public override void InitializeActions(ref ApplyStats applyStats,ref ApplyHitNPC applyHitNPC)
		{
			applyStats=delegate(ref Item item)
			{
				//IInfo info=item.GetModInfo<IInfo>(mod);
				item.damage=(int)(item.damage*1.1);
			};
		}
		public override void SetDefaults()
		{
			item.name = "Small Damage Module";
			item.width = item.height = 16;
			itemType=ItemType.Weap|ItemType.Tool;
		}
	}
}
