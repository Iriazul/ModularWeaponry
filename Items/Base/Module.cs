using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ModularWeaponry.Items.Base
{
	public delegate void ApplyModule(ref Item weapon);
	public class Module:ModItem
	{
		public ApplyModule applyModule;
		
		[Flags]
		public enum ItemType:ushort
		{
			//Damage type
			Melee=	1<<0,
			Range=	1<<1,
			Magic=	1<<2,
			
			//Tile collection
			Pick=	1<<3,
			Axe=	1<<4,
			Hammer=	1<<5,
			
			//Use style
			Swing=	1<<6,
			Point=	1<<7,
			Thrust=	1<<8,
			
			//Armor
			Head=	1<<9,
			Chest=	1<<10,
			Legs=	1<<11,
			
			Accessory=	1<<12,
			
			Block=	1<<13,
			
			Weap=	ItemType.Melee	|ItemType.Range	|ItemType.Magic,	//Any weapon
			Tool=	ItemType.Hand	|ItemType.Elec,	//Any tool
			Mine=	ItemType.Pick	|ItemType.Drill,//Pick or Drill
			Chop=	ItemType.Axe	|ItemType.Saw,	//Axe or Saw
			Pound=	ItemType.Hammer	|ItemType.Jack,	//Hammer of Jackhammer
			Armor=	ItemType.Head	|ItemType.Chest	|ItemType.Legs,	//Any armor piece
			Equip=	ItemType.Armor	|ItemType.Accessory,//Any equipable item
			Any=	0xFFFF,//Anything
			None=	0x0000 //Nothing?
		}
		
		public static ItemType GetItemTypes(Item item)
		{
			ItemType r=ItemType.None;
			if(item.melee)		{r+=ItemType.Melee;}
			if(item.ranged)		{r+=ItemType.Range;}
			if(item.magic)		{r+=ItemType.Magic;}
			if(item.pick>0)		{r+=ItemType.Pick;}
			if(item.axe>0)		{r+=ItemType.;}
			if(item.hammer>0)	{r+=ItemType.;}
			//if(item.)		{r+=ItemType.;}
			
		}
		
		public override bool Autoload(ref string name,ref string texture,IList<EquipType> equips)
		{
			mod.actions.Add(this.Type,applyModule);
			return true;
		}
	}
}