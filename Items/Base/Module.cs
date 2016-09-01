using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ModularWeaponry.Items.Base
{
	
	public delegate void Stats(Item item,byte quantity);
	public delegate void HitNPC(Entity attacker,NPC npc,byte quantity);
	
	public class Module:ModItem
	{
		public static Dictionary<string,Stats> updateStats=new Dictionary<string,Stats>();
		public static Dictionary<string,HitNPC> onHitNPC=new Dictionary<string,HitNPC>();
		
		private static IType _iType;
		
		public IType GetIType(){return _iType;}
		public virtual void Initialize(ref IType iType,ref Stats stats,ref HitNPC hitNPC){}
		
		public override bool Autoload(ref string name,ref string texture,IList<EquipType> equips)
		{
			Stats stats=null;
			HitNPC hitNPC=null;
			Initialize(ref _iType,ref stats,ref hitNPC);
			if(stats!=null){updateStats.Add(name,stats);}
			if(hitNPC!=null){onHitNPC.Add(name,hitNPC);}
			return name!="Module";
		}
	}
	
	[Flags]
	public enum IType:uint
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
		
		Ammo=	1<<16,//Weapons that use ammo
		Mana=	1<<17,//Weapons that use mana
		
		Weap=	IType.Melee	|IType.Range	|IType.Magic,	//Any weapon
		Mine=	IType.Pick	|IType.Drill,					//Pick or Drill
		Chop=	IType.Axe	|IType.Saw,						//Axe or Saw
		Pound=	IType.Hammer|IType.Jack,					//Hammer of Jackhammer
		Tool=	IType.Mine	|IType.Chop		|IType.Pound,	//Any tool
		Armor=	IType.Head	|IType.Chest	|IType.Legs,	//Any armor piece
		Equip=	IType.Armor	|IType.Accessory,				//Any equipable item
		None=	0x00000000,//Nothing
		Any=	0xFFFFFFFF//Anything
	}
}