using System;
using Microsoft.Xna.Framework;
using Terraria;

namespace ModularWeaponry.Items
{
	public class TestModule:Module
	{
		public static byte clock=0;
		public override void Initialize()
		{
			item.name="TestModule";
			item.toolTip="This module is to test the functionality of every hook";
			itemTag=ItemTag.Any;
			//Block=ItemTag.None;
			item.rare=-12;
			textClr=Color.Black;
			maximum=20;
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
		public override void OnHitEffect(Item item,Player player,Projectile projectile,NPC npc,byte level){Main.NewText("OnHitEffect (Level:"+level+")");}
	}
}
