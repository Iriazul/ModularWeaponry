using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ModularWeaponry.Items.Base;

namespace ModularWeaponry.Items
{
	public class Circuit : Module
	{
		public override void InitializeActions(ref ApplyStats applyStats,ref ApplyHitNPC applyHitNPC)
		{
			applyHitNPC=delegate(ref Item item,ref Player player,ref NPC npc)
			{
				//IInfo info=item.GetModInfo<IInfo>(mod);
				
			};
		}
		public override void SetDefaults()
		{
			item.name = "Circuit";
			item.width = item.height = 16;
			itemType=ItemType.Melee;
		}
	}
}
