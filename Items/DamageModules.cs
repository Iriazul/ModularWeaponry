using System;
using Microsoft.Xna.Framework;
using Terraria;

namespace ModularWeaponry.Items
{
	public class DamageModule1:Module
	{
		public override void Initialize()
		{
			display="Damage Module";
			item.name="Small Damage Module";
			item.toolTip="";
			itemTag=ItemTag.Weap|ItemTag.Tool;
			textClr=Color.Red;
			maximum=20;
		}
		public override void UpdateStats(Item item,byte level)
		{
			item.damage=(int)Math.Ceiling(item.damage*(1+0.05*level));
		}
	}
	public class DamageModule2: Module
	{
		public override void Initialize()
		{
			item.name="Average Damage Module";
			item.toolTip="";
			itemTag=ItemTag.Weap|ItemTag.Tool;
			modData=new ModuleData("DamageModule1",2);
		}
	}
	public class DamageModule3: Module
	{
		public override void Initialize()
		{
			item.name="Superior Damage Module";
			item.toolTip="";
			itemTag=ItemTag.Weap|ItemTag.Tool;
			modData=new ModuleData("DamageModule1",3);
		}
	}
}
