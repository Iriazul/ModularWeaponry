using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace ModularWeaponry.Items
{
	public class Circuit:Module
	{
		public override void Initialize()
		{
			item.name="Circuit";
			item.toolTip="";
			itemTag=ItemTag.Weap;
			textClr=Color.Lime;
			maximum=20;
		}
		public override void OnHitEffect(Item item,Player player,Projectile projectile,NPC npc,byte level)
		{
			npc.AddBuff(BuffID.Electrified,120*level);
		}
	}
	public class ToxicSalve:Module
	{
		public override void Initialize()
		{
			item.name="Toxic Salve";
			item.toolTip="";
			itemTag=ItemTag.Weap;
			textClr=Color.YellowGreen;
			maximum=20;
		}
		public override void OnHitEffect(Item item,Player player,Projectile projectile,NPC npc,byte level)
		{
			npc.AddBuff(BuffID.Venom,120*level);
		}
	}
	public class VampiricCharm:Module
	{
		public override void Initialize()
		{
			item.name="Vampiric Charm";
			item.toolTip="Converts damage dealt to health";
			itemTag=ItemTag.Weap;
			textClr=Color.YellowGreen;
			maximum=20;
		}
		public override void OnHitEffect(Item item,Player player,Projectile projectile,NPC npc,byte level)
		{
			if(projectile!=null)
			{
				player.statLife+=(int)(0.1*projectile.damage*(3*Math.Log(level)+1));
			}
			else
			{
				player.statLife+=(int)(0.1*player.GetWeaponDamage(item)*(3*Math.Log(level)+1));
			}
		}
	}
}