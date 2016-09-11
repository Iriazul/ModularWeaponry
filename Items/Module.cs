using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace ModularWeaponry.Items
{
	public abstract class Module:ModItem
	{
		public ItemTag itemTag=ItemTag.None;
		//public ItemTag Allow=ItemTag.None;
		//public ItemTag Needs=ItemTag.None;
		//public ItemTag Block=ItemTag.None;
		public ModuleData modData;
		public byte maximum=255;
		public string display;
		public Color? textClr;
		
		public abstract	void Initialize();
		public virtual	void UpdateStats(Item item,byte level){}
		public virtual	void UpdateEquip(Item item,Player player,byte level){}
		public virtual	bool OnShootProj(Item item,Player player,Projectile projectile,byte level){return true;}
		public virtual	void OnHitEffect(Item item,Player player,Projectile projectile,NPC npc,byte level){}
		
		public override bool Autoload(ref string name,ref string texture,IList<EquipType> equips)
		{
			if(name=="Module"){return false;}
			if(!mod.TextureExists("Items/"+name)){texture="ModularWeaponry/Items/PlaceHolder";}
			return true;
		}
		
		public override sealed void SetDefaults()
		{
			item.width=Main.itemTexture[item.type].Width;
			item.height=Main.itemTexture[item.type].Height;
			modData=new ModuleData(Main.itemName[item.type]);
			Initialize();
			if(modData.name!=Main.itemName[item.type])
			{
				Module template=(Module)mod.GetItem(modData.name);
				display=template.display;
				textClr=template.textClr;
			}
			else
			{
				if(display==null)
				{
					display=item.name;
				}
			}
			item.toolTip2=this.GetCompatibilityString();
		}
		
		public string GetCompatibilityString()
		{
			string r="[Compatibilities]";
			if(itemTag.HasFlag(ItemTag.Any)){r+="\nAll items";}
			else
			{
				if(itemTag.HasFlag(ItemTag.Weap))
				{
					if(itemTag.HasFlag(ItemTag.Damage)||((itemTag&ItemTag.Damage)==0)){r+="\nAll weapons";}
					else
					{
						if(itemTag.HasFlag(ItemTag.Melee)){r+="\nMelee weapons";}
						if(itemTag.HasFlag(ItemTag.Range)){r+="\nRanged weapons";}
						if(itemTag.HasFlag(ItemTag.Magic)){r+="\nMagic weapons";}
					}
				}
				if(itemTag.HasFlag(ItemTag.Tool)){r+="\nAll tools";}
				else
				{
					if(itemTag.HasFlag(ItemTag.Hand)){r+="\nAll hand tools";}
					else
					{
						if(itemTag.HasFlag(ItemTag.Pick))	{r+="\nPickaxes";}
						if(itemTag.HasFlag(ItemTag.Axe))	{r+="\nAxes";}
						if(itemTag.HasFlag(ItemTag.Hammer))	{r+="\nHammers";}
					}
					if(itemTag.HasFlag(ItemTag.Elec)){r+="\nAll electric tools";}
					else
					{
						if(itemTag.HasFlag(ItemTag.Drill))	{r+="\nDrills";}
						if(itemTag.HasFlag(ItemTag.Saw))	{r+="\nChainsaws";}
						if(itemTag.HasFlag(ItemTag.Jack))	{r+="\nJackhammers";}
					}	
				}
				if(itemTag.HasFlag(ItemTag.Equip)){r+="\nAll equipment";}
				else
				{
					if(itemTag.HasFlag(ItemTag.Armor)){r+="\nAll armor";}
					else
					{
						if(itemTag.HasFlag(ItemTag.Head)){r+="\nAll headwear";}
						if(itemTag.HasFlag(ItemTag.Body)){r+="\nAll chestpieces";}
						if(itemTag.HasFlag(ItemTag.Legs)){r+="\nAll leggings";}
					}
					if(itemTag.HasFlag(ItemTag.Accessory)){r+="\nAll accessories";}
				}
			}
			return r;
		}
	}
	
	[Flags]
	public enum ItemTag:uint
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
		
		Damage=	ItemTag.Melee	|ItemTag.Range	|ItemTag.Magic,	//Anything that deals damage
		Mine=	ItemTag.Pick	|ItemTag.Drill,					//Pick or Drill
		Chop=	ItemTag.Axe		|ItemTag.Saw,					//Axe or Saw
		Pound=	ItemTag.Hammer	|ItemTag.Jack,					//Hammer of Jackhammer
		Hand=	ItemTag.Pick	|ItemTag.Axe	|ItemTag.Hammer,//Hand tools
		Elec=	ItemTag.Drill	|ItemTag.Saw	|ItemTag.Jack,	//Electric tools
		Tool=	ItemTag.Mine	|ItemTag.Chop	|ItemTag.Pound,	//Any tool
		Armor=	ItemTag.Head	|ItemTag.Body	|ItemTag.Legs,	//Any armor piece
		Equip=	ItemTag.Armor	|ItemTag.Accessory,				//Any equipable item
		None=	0x00000000,//Nothing
		Any=	0xFFFFFFFF//Anything
	}
}