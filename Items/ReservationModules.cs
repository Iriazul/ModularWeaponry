using System;
using Microsoft.Xna.Framework;
using Terraria;

namespace ModularWeaponry.Items
{
	public class AmmoReservation1:Module
	{
		public override void Initialize()
		{
			item.name="Ammo Reservation";
			item.toolTip="Chance to not consume ammo.";
			itemTag=ItemTag.Weap|ItemTag.Tool;
			textClr=Color.Yellow;
			maximum=20;
		}
		public override void UpdateStats(Item item,byte level)
		{
			
		}
	}
}