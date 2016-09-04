using System;
using Microsoft.Xna.Framework;
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
			//Block=IType.None;
			item.rare=-12;
		}
		public override void UpdateStats(Item item,byte level){Main.NewText("UpdateStats (Level:"+level+")");}
		public override void UpdateEquip(Item item,Player player,byte level)
		{
			if((clock+=level)>=60)
			{
				Item heldItem=player.inventory[player.selectedItem];
				if(heldItem!=null)
				{
					if(heldItem.type!=0)
					{
						IInfo info=heldItem.GetModInfo<IInfo>(mod);
						if(info!=null)
						{
							info.Print();
						}else{Main.NewText("Info is null");}
					}else{Main.NewText("Item is blank");}
				}else{Main.NewText("Item is null");}
				clock=0;
			}
		}
		public override bool OnShootProj(Item item,Player player,Projectile projectile,byte level){Main.NewText("OnShootProj (Level: "+level+")");return true;}
		public override void OnHitEffect(Item item,Entity attacker,NPC npc,byte level){Main.NewText("OnHitEffect (Level:"+level+")");}
	}
	
	public class Circuit:Module
	{
		public override void Initialize()
		{
			item.name="Circuit";
			item.toolTip="";
			iType=IType.Weap;
			ModuleColor=Color.Lime;
		}
		public override void OnHitEffect(Item item,Entity attacker,NPC npc,byte level)
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
			ModuleColor=Color.Blue;
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
			ModuleColor=Color.Red;
		}
		public override void UpdateStats(Item item,byte level)
		{
			item.damage=(int)Math.Ceiling(item.damage*(1+0.05*level));
		}
	}
    public class DamageModule : Module
    {
        public override void Initialize()
        {
            item.name = "Damage Module";
            item.toolTip = "";
            iType = IType.Weap | IType.Tool;
            ModuleColor = Color.Red;
        }
        public override void UpdateStats(Item item, byte level)
        {
            item.damage = (int)Math.Ceiling(item.damage * (1 + 0.1 * level));
        }
    }
    public class SuperiorDamageModule : Module
    {
        public override void Initialize()
        {
            item.name = "Superior Damage Module";
            item.toolTip = "";
            iType = IType.Weap | IType.Tool;
            ModuleColor = Color.Red;
        }
        public override void UpdateStats(Item item, byte level)
        {
            item.damage = (int)Math.Ceiling(item.damage * (1 + 0.15 * level));
        }
    }

	public class Talisman:Module
	{
		public override void Initialize()
		{
			item.name="Talisman";
			item.toolTip="Enchants melee and ranged weapons to make them deal magic damage and use mana rather then ammo";
			//Needs=IType.Weap;
			//Block=IType.Magic;
			//Allow=IType.Melee|IType.Range;
			iType=IType.Weap|IType.Melee|IType.Range;
		}
		public override void UpdateStats(Item item,byte level)
		{
			if(item.melee)
			{
				item.melee=false;
				item.magic=true;
				return;
			}
			if(item.ranged)
			{
				item.ranged=false;
				item.magic=true;
				item.useAmmo=0;
				byte extraLevels=0;
				switch(item.shoot)
				{
					//Bows and repeating crossbows
					case 1:break;
					//Guns
					case 10:
					case 14:
					{
						if(item.damage<20){item.shoot=20;item.shootSpeed=10f;item.useSound=12;extraLevels=2;}//Space gun
						else{
							if(item.damage<40){item.shoot=88;item.shootSpeed=17f;item.useSound=12;extraLevels=2;}//Laser Rifle
							else{
								if(item.damage<60){item.shoot=260;item.shootSpeed=15f;item.useSound=12;extraLevels=1;}//Heatray
								else{
									if(item.damage<80){item.shoot=440;item.shootSpeed=20f;item.useSound=91;extraLevels=1;}//Laser Machinegun
									else{
										/*if(item.damage<100)*/{item.shoot=645;item.shootSpeed=10f;item.useSound=88;extraLevels=0;}//Lunar Flare
										//else{item.shoot=632;item.shootSpeed=30f;item.useSound=13;extraLevels=0;}//Last Prism
									}
								}
							}
						}
						break;
					}
				}
				item.mana=item.useTime*2/(level+extraLevels);
				item.damage+=extraLevels*2;
				return;
			}
		}
	}
	public class ToxicSalve:Module
	{
		public override void Initialize()
		{
			item.name="Toxic Salve";
			item.toolTip="";
			iType=IType.Weap;
			ModuleColor=Color.YellowGreen;
		}
		public override void OnHitEffect(Item item,Entity attacker,NPC npc,byte level)
		{
			npc.AddBuff(BuffID.Venom,120*level);
		}
	}
    public class VampiricCharm : Module
    {
        public override void Initialize()
        {
            item.name = "Vampiric Charm";
            item.toolTip = "Converts damage dealt to health";
            iType = IType.Weap;
            ModuleColor = Color.YellowGreen;
        }
        public override void OnHitEffect(Item item, Entity attacker, NPC npc, byte level)
        {
            if (attacker is Projectile)
            {
                Main.player[((Projectile)attacker).owner].statLife += (int)((0.1 * ((Projectile)attacker).damage) * (3 * Math.Log(level) + 1));
            }
            else
            {
                Main.player[((Projectile)attacker).owner].statLife += (int)((0.1 * ((Projectile)attacker).damage) * (3 * Math.Log(level) + 1));
            }
        }
    }
}
