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
	public class OverClocker:Module
	{
		public override void Initialize(ref ItemType itemType,ref ApplyStats applyStats,ref ApplyHitNPC applyHitNPC)
		{
			itemType=ItemType.Weap|ItemType.Tool;
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
			item.width=item.height=16;
		}
	}
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
	public class VenomPouch:Module
	{
		public override void Initialize(ref ItemType itemType,ref ApplyStats applyStats,ref ApplyHitNPC applyHitNPC)
		{
			itemType=ItemType.Weap;
			applyHitNPC=delegate(Entity attacker,NPC npc)
			{
				npc.AddBuff(BuffID.Venom,600);
			};
		}
		public override void SetDefaults()
		{
			item.name="Venom Pouch";
			item.toolTip="";
			item.width=item.height=16;
		}
	}
}
