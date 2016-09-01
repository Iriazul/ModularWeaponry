using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ModularWeaponry.Items.Base;

namespace ModularWeaponry.Items
{
	public class Circuit:Module
	{
		public override void Initialize(ref IType iType,ref Stats stats,ref HitNPC hitNPC)
		{
			iType=IType.Weap;
			hitNPC=delegate(Entity attacker,NPC npc,byte quantity)
			{
				npc.AddBuff(BuffID.Electrified,120*quantity);
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
		public override void Initialize(ref IType iType,ref Stats stats,ref HitNPC hitNPC)
		{
			iType=IType.Weap|IType.Tool;
			stats=delegate(Item item,byte quantity)
			{
				float multiplier=(float)Math.Pow(0.9f,quantity);
				item.useTime=(int)(item.useTime*multiplier);
				item.useAnimation=(int)(item.useAnimation*multiplier);
				if(item.useTime<1){item.useTime=1;}
				if(item.useAnimation<1){item.useAnimation=1;}
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
		public override void Initialize(ref IType iType,ref Stats stats,ref HitNPC hitNPC)
		{
			iType=IType.Weap|IType.Tool;
			stats=delegate(Item item,byte quantity)
			{
				item.damage=(int)Math.Ceiling(item.damage*(1+0.05*quantity));
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
		public override void Initialize(ref IType iType,ref Stats stats,ref HitNPC hitNPC)
		{
			iType=IType.Weap;
			hitNPC=delegate(Entity attacker,NPC npc,byte quantity)
			{
				npc.AddBuff(BuffID.Venom,120*quantity);
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
