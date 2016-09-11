using System;
using Microsoft.Xna.Framework;
using Terraria;

namespace ModularWeaponry.Items
{
	public class OverClocker:Module
	{
		public override void Initialize()
		{
			item.name="Overclocker";
			item.toolTip="";
			itemTag=ItemTag.Weap|ItemTag.Tool;
			textClr=Color.Blue;
			maximum=20;
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
}