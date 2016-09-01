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
		private static IInfo tempItemInfo;
		
		public override void PreReforge(Item item)
		{
			tempItemInfo=item.GetModInfo<IInfo>(mod);
		}
		
		public override void PostReforge(Item item)
		{
			if(tempItemInfo!=null)
			{
				IInfo info=item.GetModInfo<IInfo>(mod);
				info.modules=tempItemInfo.modules;
				tempItemInfo=null;
			}
		}
		
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
				for(byte i=0;i<info.modules.Length;++i)
				{
					info.modules[i]=(ushort)mod.ItemType(splitModules[i]);
				}
				item.UpdateStats((ushort[])info.modules.Clone());
			}
			catch(Exception e){}
		}
		
		public override void OnHitNPC(Item item,Player player,NPC npc,int damage,float knockBack,bool crit)
		{
			ushort[] modules=(ushort[])item.GetModInfo<IInfo>(mod).modules.Clone();
			if(modules!=null)
			{
				for(byte i=0;i<modules.Length;i++)
				{
					if(modules[i]!=0)
					{
						HitNPC hitNPC;
						if(Module.onHitNPC.TryGetValue(Main.itemName[modules[i]],out hitNPC))
						{
							byte quantity=1;
							for(byte i2=(byte)(i+1);i2<modules.Length;i++)
							{
								if(modules[i2]==modules[i])
								{
									quantity++;
									modules[i2]=0;
								}
							}
							hitNPC(player,npc,quantity);
						}
					}
				}
			}
		}
	}
	
	public class GProjectile:GlobalProjectile
	{
		public override bool PreAI(Projectile projectile)
		{
			PInfo projInfo=projectile.GetModInfo<PInfo>(mod);
			if(projInfo.check)
			{
				if(projectile.friendly)
				{
					projInfo.check=false;
					Player player=Main.player[projectile.owner];
					IInfo itemInfo=player.inventory[player.selectedItem].GetModInfo<IInfo>(mod);
					if(itemInfo.modules!=null)
					{
						projInfo.hitEffects=itemInfo.modules;
					}
				}
			}
			return true;
		}
		public override void OnHitNPC(Projectile projectile,NPC npc,int damage,float knockback,bool crit)
		{
			ushort[] hitEffects=projectile.GetModInfo<PInfo>(mod).hitEffects;
			if(hitEffects!=null)
			{
				for(byte i=0;i<hitEffects.Length;i++)
				{
					if(hitEffects[i]!=0)
					{
						HitNPC hitNPC;
						if(Module.onHitNPC.TryGetValue(Main.itemName[hitEffects[i]],out hitNPC))
						{
							byte quantity=1;
							for(byte i2=(byte)(i+1);i<hitEffects.Length;i++)
							{
								if(hitEffects[i2]==hitEffects[i])
								{
									quantity++;
									hitEffects[i2]=0;
								}
							}
							hitNPC(projectile,npc,quantity);
						}
					}
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
		public bool check=true;
		public ushort[] hitEffects;
	}
}