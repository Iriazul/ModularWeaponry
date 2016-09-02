using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ModularWeaponry.Items.Base;

namespace ModularWeaponry.Items
{
	public class TestModule:Module
	{
		public static byte clock=0;
		public override void Initialize()
		{
			item.name="TestModule";
			item.toolTip="This module is to test the functionality of every hook";
			iType=IType.Any;
		}
		public override void UpdateStats(Item item,byte level){Main.NewText("UpdateStats (Level:"+level+")");}
		public override void UpdateEquip(Item item,Player player,byte level)
		{
			if((clock+=level)>=60)
			{
				Main.NewText("UpdateEquip (Level:"+level+")");
				clock=0;
			}
		}
		public override void OnHitEffect(Entity attacker,NPC npc,byte level){Main.NewText("OnHitEffect (Level:"+level+")");}
	}
	
	public class Circuit:Module
	{
		public override void Initialize()
		{
			item.name="Circuit";
			item.toolTip="";
			iType=IType.Weap;
		}
		public override void OnHitEffect(Entity attacker,NPC npc,byte level)
		{
			npc.AddBuff(BuffID.Electrified,120*level);
		}
	}
	public class OverClocker:Module
	{
		public override void Initialize()
		{
			item.name="Overclocker";
			item.toolTip="";
			iType=IType.Weap|IType.Tool;
		}
		public override void UpdateStats(Item item,byte level)
		{
			float multiplier=(float)Math.Pow(0.9f,level);
			item.useTime=(int)(item.useTime*multiplier);
			item.useAnimation=(int)(item.useAnimation*multiplier);
			if(item.useTime<1){item.useTime=1;}
			if(item.useAnimation<1){item.useAnimation=1;}
		}
	}
	public class SmallDamageModule:Module
	{
		public override void Initialize()
		{
			item.name="Small Damage Module";
			item.toolTip="";
			iType=IType.Weap|IType.Tool;
		}
		public override void UpdateStats(Item item,byte level)
		{
			item.damage=(int)Math.Ceiling(item.damage*(1+0.05*level));
		}
	}
	public class ToxicSalve:Module
	{
		public override void Initialize()
		{
			item.name="Toxic Salve";
			item.toolTip="";
			iType=IType.Weap;
		}
		public override void OnHitEffect(Entity attacker,NPC npc,byte level)
		{
			npc.AddBuff(BuffID.Venom,120*level);
		}
	}
}
