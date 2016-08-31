using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ModularWeaponry.Items.Base;

namespace ModularWeaponry
{
	public class GItem : GlobalItem
	{
		public override bool NeedsCustomSaving(Item item)
		{
			IInfo info = item.GetModInfo<IInfo>(mod);
			if(info.modules!=null)
			{
				foreach(ushort type in info.modules)
				{
					if(type!=0)
					{
						return true;
					}
				}
			}
			return false;
		}

		public override void SaveCustomData(Item item,BinaryWriter writer)
		{
			IInfo info=item.GetModInfo<IInfo>(mod);
			string writeString="";
			for(byte i=0;i<info.modules.Length;++i)
			{
				if(info.modules[i]!=0)
				{
					writeString+=Main.itemName[info.modules[i]]+";";
				}
				else
				{
					writeString+=" ;";
				}
			}
			writer.Write(writeString);
		}
		public override void LoadCustomData(Item item, BinaryReader reader)
		{
			try
			{
			IInfo info = item.GetModInfo<IInfo>(mod);
			string[] splitModules = reader.ReadString().Split(';');
			info.modules = new ushort[splitModules.Length-1];
			Item[] itemModules=new Item[splitModules.Length-1];
			for(byte i=0;i<info.modules.Length;++i)
			{
				info.modules[i]=(ushort)mod.ItemType(splitModules[i]);
				ModItem temp=mod.GetItem(splitModules[i]);
				itemModules[i]=temp!=null?temp.item:new Item();
			}
			item.UpdateStats(itemModules);
			}
			catch(Exception e){}
		}
		
		public override bool Shoot(Item item,Player player,ref Vector2 position,ref float speedX,ref float speedY,ref int type,ref int damage,ref float knockBack)
		{
			IInfo itemInfo=player.inventory[player.selectedItem].GetModInfo<IInfo>(mod);
			if(itemInfo!=null)
			{
				Stack<ushort> temp=new Stack<ushort>();
				foreach(ushort module in itemInfo.modules)
				{
					Main.NewText("Apply?");
					if((module!=0)&&Module.onHitNPC.ContainsKey(Main.itemName[module]))
					{
						Main.NewText("Applied");
						temp.Push(module);
					}
				}
				if(temp.Count>0)
				{
					GProjectile.hitEffectForNextProjectile=temp;
				}
			}
			return true;
		}
		
		public override void OnHitNPC(Item item,Player player,NPC npc,int damage,float knockBack,bool crit)
		{
			IInfo info=item.GetModInfo<IInfo>(mod);
			if(info.modules!=null)
			{
				foreach(ushort type in info.modules)
				{
					ApplyHitNPC applyHitNPC;
					if(Module.onHitNPC.TryGetValue(Main.itemName[type],out applyHitNPC))
					{
						applyHitNPC(player,npc);
					}
				}
			}
		}
	}
	
	public class GProjectile:GlobalProjectile
	{
		public static Stack<ushort> hitEffectForNextProjectile;
		public override void SetDefaults(Projectile projectile)
		{
			if(hitEffectForNextProjectile!=null)
			{
				Main.NewText("Owned by Player");
				PInfo projInfo=projectile.GetModInfo<PInfo>(mod);
				projInfo.hitEffects=hitEffectForNextProjectile;
				hitEffectForNextProjectile=null;
			}
		}
		public override void OnHitNPC(Projectile projectile,NPC npc,int damage,float knockback,bool crit)
		{
			PInfo info=projectile.GetModInfo<PInfo>(mod);
			if(info.hitEffects!=null)
			{
				foreach(ushort type in info.hitEffects)
				{
					Main.NewText("Apply hit to NPC");
					Module.onHitNPC[Main.itemName[type]](projectile,npc);
				}
			}
		}
	}
	
	public class IInfo:ItemInfo
	{
		public ushort[] modules;
	}
	
	public class PInfo:ProjectileInfo
	{
		public Stack<ushort> hitEffects;
	}
}