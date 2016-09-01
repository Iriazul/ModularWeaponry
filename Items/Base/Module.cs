using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ModularWeaponry.Items.Base
{
	
	public delegate void ApplyStats(Item item);
	public delegate void ApplyHitNPC(Entity attacker,NPC npc);
	
	public class Module:ModItem
	{
		public static Dictionary<string,ApplyStats> updateStats=new Dictionary<string,ApplyStats>();
		public static Dictionary<string,ApplyHitNPC> onHitNPC=new Dictionary<string,ApplyHitNPC>();
		
		private static ItemType _itemType;
		
		public ItemType GetItemType(){return _itemType;}
		public virtual void Initialize(ref ItemType itemType,ref ApplyStats applyStats,ref ApplyHitNPC applyHitNPC){}
		
		public override bool Autoload(ref string name,ref string texture,IList<EquipType> equips)
		{
			ApplyStats applyStats=null;
			ApplyHitNPC applyHitNPC=null;
			Initialize(ref _itemType,ref applyStats,ref applyHitNPC);
			if(applyStats!=null){updateStats.Add(name,applyStats);}
			if(applyHitNPC!=null){onHitNPC.Add(name,applyHitNPC);}
			return name!="Module";
		}
	}
	
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
		Drill=	1<<6,
		Saw=	1<<7,//Chainsaw
		Jack=	1<<8,//Jackhammer
		
		//Armor
		Head=	1<<9,
		Chest=	1<<10,
		Legs=	1<<11,
		Wings=	1<<12,
		
		Accessory=1<<13,
		
		Tile=	1<<14,
		Wall=	1<<15,
		
		Weap=	ItemType.Melee	|ItemType.Range	|ItemType.Magic,	//Any weapon
		Mine=	ItemType.Pick	|ItemType.Drill,//Pick or Drill
		Chop=	ItemType.Axe	|ItemType.Saw,	//Axe or Saw
		Pound=	ItemType.Hammer	|ItemType.Jack,	//Hammer of Jackhammer
		Tool=	ItemType.Mine	|ItemType.Chop	|ItemType.Pound,//Any tool
		Armor=	ItemType.Head	|ItemType.Chest	|ItemType.Legs,	//Any armor piece
		Equip=	ItemType.Armor	|ItemType.Accessory,//Any equipable item
		Any=	0xFFFF,//Anything
		None=	0x0000 //Nothing
	}
}