using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ModularWeaponry.Items.Base;

namespace ModularWeaponry.Items
{
	public class OverClocker : Module
	{
		public override void InitializeActions(ref ApplyStats applyStats,ref ApplyHitNPC applyHitNPC)
		{
			applyStats=delegate(ref Item item)
			{
				//IInfo info=item.GetModInfo<IInfo>(mod);
				item.useTime=(int)(item.useTime*0.9);
				item.useAnimation=(int)(item.useAnimation*0.9);
			};
		}
		public override void SetDefaults()
		{
			item.name = "OverClocker";
			item.width = item.height = 16;
			itemType=ItemType.Weap|ItemType.Tool;
		}
	}
}
