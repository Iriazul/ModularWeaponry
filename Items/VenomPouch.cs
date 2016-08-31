using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ModularWeaponry.Items.Base;

namespace ModularWeaponry.Items
{
	public class VenomPouch:Module
	{
		public override void InitializeActions(ref ApplyStats applyStats,ref ApplyHitNPC applyHitNPC)
		{
			applyHitNPC=delegate(Entity attacker,NPC npc)
			{
				npc.AddBuff(BuffID.Venom,600);
			};
		}
		public override void SetDefaults()
		{
			item.name="Venom Pouch";
			item.toolTip="";
			itemType=ItemType.Weap;
			
			item.width=item.height=16;
		}
	}
}
