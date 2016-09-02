using System;
using System.Reflection;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ModularWeaponry.Items.Base
{
	public delegate void Stats(Item item,byte level);
	public delegate void Equip(Item item,Player player,byte level);
	public delegate void HitEffect(Entity attacker,NPC npc,byte level);
	
	public class Module:ModItem
	{
		public static Dictionary<ushort,Stats>	updateStats=new Dictionary<ushort,Stats>();
		public static Dictionary<ushort,Equip>	updateEquip=new Dictionary<ushort,Equip>();
		public static Dictionary<ushort,HitEffect>	onHitEffect=new Dictionary<ushort,HitEffect>();
		
		public IType iType=IType.None;
		
		public virtual void Initialize(){}//(ref IType iType,ref Stats stats,ref Equip equip,ref HitNPC hitNPC){}
		
		public virtual void UpdateStats(Item item,byte level){}
		public virtual void UpdateEquip(Item item,Player player,byte level){}
		public virtual void OnHitEffect(Entity attacker,NPC npc,byte level){}
		
		public override bool Autoload(ref string name,ref string texture,IList<EquipType> equips)
		{
			if(name=="Module"){return false;}
			//Stats stats=null;
			//Equip equip=null;
			//HitNPC hitNPC=null;
			//Initialize(ref _iType,ref stats,ref equip,ref hitNPC);
			//if(stats!=null){updateStats.Add(name,stats);}
			//if(equip!=null){updateEquip.Add(name,equip);}
			//if(hitNPC!=null){onHitNPC.Add(name,hitNPC);}
			return true;
		}
		public override sealed void SetDefaults()
		{
			if(!updateStats.ContainsKey((ushort)item.type)){updateStats.Add((ushort)item.type,UpdateStats);}
			if(!updateEquip.ContainsKey((ushort)item.type)){updateEquip.Add((ushort)item.type,UpdateEquip);}
			if(!onHitEffect.ContainsKey((ushort)item.type)){onHitEffect.Add((ushort)item.type,OnHitEffect);}
			Initialize();
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