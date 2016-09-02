using System;
using System.Reflection;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
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
		public static Dictionary<ushort,Color?>	moduleColor=new Dictionary<ushort,Color?>();
		public static Dictionary<ushort,Stats>	updateStats=new Dictionary<ushort,Stats>();
		public static Dictionary<ushort,Equip>	updateEquip=new Dictionary<ushort,Equip>();
		public static Dictionary<ushort,HitEffect>	onHitEffect=new Dictionary<ushort,HitEffect>();
		
		public IType iType=IType.None;
		public byte maxLevel=255;
		public Color? ModuleColor;
		
		public virtual void Initialize(){}
		public virtual void UpdateStats(Item item,byte level){}
		public virtual void UpdateEquip(Item item,Player player,byte level){}
		public virtual void OnHitEffect(Entity attacker,NPC npc,byte level){}
		
		public override bool Autoload(ref string name,ref string texture,IList<EquipType> equips)
		{
			return name!="Module";
		}
		public override sealed void SetDefaults()
		{
			item.width=Main.itemTexture[item.type].Width;
			item.height=Main.itemTexture[item.type].Height;
			Initialize();
			item.toolTip2=this.GetCompatibilityString();
			if(!moduleColor.ContainsKey((ushort)item.type)){moduleColor.Add((ushort)item.type,ModuleColor);}
			if(!updateStats.ContainsKey((ushort)item.type)){updateStats.Add((ushort)item.type,UpdateStats);}
			if(!updateEquip.ContainsKey((ushort)item.type)){updateEquip.Add((ushort)item.type,UpdateEquip);}
			if(!onHitEffect.ContainsKey((ushort)item.type)){onHitEffect.Add((ushort)item.type,OnHitEffect);}
		}
		public string GetCompatibilityString()
		{
			string r="";
			if(iType.HasFlag(IType.Any)){r+="\nAnything";}
			else
			{
				if(iType.HasFlag(IType.Weap))
				{
					if(iType.HasFlag(IType.Damage)){r+="\nAll weapons";}
					else
					{
						if(iType.HasFlag(IType.Melee)){r+="\nMelee weapons";}
						if(iType.HasFlag(IType.Range)){r+="\nRanged weapons";}
						if(iType.HasFlag(IType.Magic)){r+="\nMagic weapons";}
					}
				}
				if(iType.HasFlag(IType.Tool)){r+="\nAll tools";}
				else
				{
					if(iType.HasFlag(IType.Hand)){r+="\nAll hand tools";}
					else
					{
						if(iType.HasFlag(IType.Pick))	{r+="\nPickaxes";}
						if(iType.HasFlag(IType.Axe))	{r+="\nAxes";}
						if(iType.HasFlag(IType.Hammer))	{r+="\nHammers";}
					}
					if(iType.HasFlag(IType.Elec)){r+="\nAll electric tools";}
					else
					{
						if(iType.HasFlag(IType.Drill))	{r+="\nDrills";}
						if(iType.HasFlag(IType.Saw))	{r+="\nChainsaws";}
						if(iType.HasFlag(IType.Jack))	{r+="\nJackhammers";}
					}	
				}
				if(iType.HasFlag(IType.Equip)){r+="\nAll equipment";}
				else
				{
					if(iType.HasFlag(IType.Armor)){r+="\nAll armor";}
					else
					{
						if(iType.HasFlag(IType.Head)){r+="\nAll headwear";}
						if(iType.HasFlag(IType.Body)){r+="\nAll chestpieces";}
						if(iType.HasFlag(IType.Legs)){r+="\nAll leggings";}
					}
					if(iType.HasFlag(IType.Accessory)){r+="\nAll accessories";}
				}
			}
			return r==""?"":"Compatibilities:"+r;
		}
	}
	
	[Flags]
	public enum IType:uint
	{
		Weap=	1<<0,
		//Damage type
		Melee=	1<<1,
		Range=	1<<2,
		Magic=	1<<3,
		//Tile collection
		Pick=	1<<4,
		Axe=	1<<5,
		Hammer=	1<<6,
		Drill=	1<<7,
		Saw=	1<<8,//Chainsaw
		Jack=	1<<9,//Jackhammer
		//Armor
		Head=	1<<10,
		Body=	1<<11,
		Legs=	1<<12,
		
		
		Accessory=1<<13,
		//Wings=	1<<,
		
		Tile=	1<<14,
		Wall=	1<<15,
		
		Ammo=	1<<16,//Weapons that use ammo
		Mana=	1<<17,//Weapons that use mana
		
		Damage=	IType.Melee	|IType.Range|IType.Magic,	//Anything that deals damage
		Mine=	IType.Pick	|IType.Drill,				//Pick or Drill
		Chop=	IType.Axe	|IType.Saw,					//Axe or Saw
		Pound=	IType.Hammer|IType.Jack,				//Hammer of Jackhammer
		Hand=	IType.Pick	|IType.Axe	|IType.Hammer,	//Hand tools
		Elec=	IType.Drill	|IType.Saw	|IType.Jack,	//Electric tools
		Tool=	IType.Mine	|IType.Chop	|IType.Pound,	//Any tool
		Armor=	IType.Head	|IType.Body|IType.Legs,	//Any armor piece
		Equip=	IType.Armor	|IType.Accessory,			//Any equipable item
		None=	0x00000000,//Nothing
		Any=	0xFFFFFFFF//Anything
	}
}