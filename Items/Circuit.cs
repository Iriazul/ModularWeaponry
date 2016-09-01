using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ModularWeaponry.Items.Base;

namespace ModularWeaponry.Items
{
	public class Circuit:Module
	{
		public static ItemType itemType=ItemType.Weap;
		public override void Initialize(ref ItemType itemType,ref ApplyStats applyStats,ref ApplyHitNPC applyHitNPC)
		{
			itemType=ItemType.Weap;
			applyHitNPC=delegate(Entity attacker,NPC npc)
			{
				npc.AddBuff(BuffID.Electrified,600);
			};
		}
		public override void SetDefaults()
		{
			item.name="Circuit";
			item.toolTip="";
			
			item.width=item.height=16;
		}
	}
}
