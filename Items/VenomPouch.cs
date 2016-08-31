using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ModularWeaponry.Items.Base;

namespace ModularWeaponry.Items
{
	public class VenomPouch : Module
	{
		public override void InitializeActions(ref ApplyStats applyStats,ref ApplyHitNPC applyHitNPC)
		{
			applyHitNPC=delegate(ref Item item,ref Player player,ref NPC npc)
			{
				npc.AddBuff(BuffID.Venom,10);
			};
		}
		public override void SetDefaults()
		{
			item.name = "VenomPouch";
			item.width = item.height = 16;
			itemType=ItemType.Melee;
		}
	}
}
