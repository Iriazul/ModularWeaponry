using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace ModularWeaponry.Items
{
	public class Talisman:Module
	{
		public override void Initialize()
		{
			item.name="Talisman";
			item.toolTip="Enchants melee and ranged weapons to make them deal magic damage and use mana rather then ammo";
			itemTag=ItemTag.Weap|ItemTag.Melee|ItemTag.Range;
			textClr=Color.Gold;
			maximum=20;
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
					case 10:case 14:
					{
						if(item.damage<20){item.shoot=20;item.shootSpeed=10f;item.useSound=12;extraLevels=2;}//Space gun
						else{if(item.damage<40){item.shoot=88;item.shootSpeed=17f;item.useSound=12;extraLevels=2;}//Laser Rifle
							else{if(item.damage<60){item.shoot=260;item.shootSpeed=15f;item.useSound=12;extraLevels=1;}//Heatray
								else{if(item.damage<80){item.shoot=440;item.shootSpeed=20f;item.useSound=91;extraLevels=1;}//Laser Machinegun
									else{/*if(item.damage<100)*/{item.shoot=645;item.shootSpeed=10f;item.useSound=88;extraLevels=0;}//Lunar Flare
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
}