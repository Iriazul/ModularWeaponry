using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ModularWeaponry.Items.Base;

namespace ModularWeaponry.Items
{
	public class OverClocker : Module
	{
		public override void SetDefaults()
		{
			item.name = "OverClocker";
			item.width = item.height = 16;
			this.applyModule = delegate (ref Item weapon)
			{
				IInfo info = weapon.GetModInfo<IInfo>(mod);
				weapon.useTime *= 0.9;
			};
		}
	}
}
