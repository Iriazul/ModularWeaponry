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
				writeString+=Main.itemName[info.modules[i]]+";";
			}
			writer.Write(writeString);
		}
		public override void LoadCustomData(Item item, BinaryReader reader)
		{
			string[] splitModules=reader.ReadString().Split(';');
			if(splitModules.Length<=1){return;}
			IInfo info=item.GetModInfo<IInfo>(mod);
			info.modules=new ushort[splitModules.Length-1];
			for(byte i=0;i<info.modules.Length;++i)
			{
				info.modules[i]=(ushort)mod.ItemType(splitModules[i]);
			}
			item.UpdateModules();
		}
		
		public override void ModifyTooltips(Item item,List<TooltipLine> tooltips)
		{
			IInfo info=item.GetModInfo<IInfo>(mod);
			if(info.compact!=null)
			{
				TooltipLine moduleToolTip=new TooltipLine(mod,"Modules","[Modules]");
				moduleToolTip.overrideColor=Color.Gray;
				tooltips.Add(moduleToolTip);
				foreach(ModuleData module in info.compact)
				{
					TooltipLine line=new TooltipLine(mod,"",Main.itemName[module.type]+" [Level "+module.level+"]");
					line.overrideColor=Module.moduleColor[module.type];
					tooltips.Add(line);
				}
			}
		}
		
		public override void UpdateEquip(Item item,Player player)
		{
			Stack<ModuleData> compact=item.GetModInfo<IInfo>(mod).compact;
			if(compact!=null)
			{
				foreach(ModuleData module in compact)
				{
                    try { Module.updateEquip[module.type](item, player, module.level); } catch (Exception e) { Main.NewText("Exception in UpdateEquip: " + e.Message); }
                }
			}
		}
		
		public override void OnHitNPC(Item item,Player player,NPC npc,int damage,float knockBack,bool crit)//For melee weapons
		{
			Stack<ModuleData> compact=item.GetModInfo<IInfo>(mod).compact;
			if(compact!=null)
			{
				foreach(ModuleData module in compact)
				{
					Module.onHitEffect[module.type](item,player,npc,module.level);
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
				projInfo.check=false;
				if(projectile.friendly)
				{
					Player player=Main.player[projectile.owner];
					IInfo itemInfo=player.inventory[player.selectedItem].GetModInfo<IInfo>(mod);
					if(itemInfo.compact!=null)
					{
						projInfo.modules=itemInfo.compact;
						projInfo.item=player.inventory[player.selectedItem];
						bool killProjectile=false;
						foreach(ModuleData module in itemInfo.compact)
						{
							if(!Module.onShootProj[module.type](projInfo.item,player,projectile,module.level)){killProjectile=true;}
						}
						if(killProjectile)
						{
							projectile.hide=true;
							projectile.active=false;
						}
					}
				}
			}
			return true;
		}
		public override void OnHitNPC(Projectile projectile,NPC npc,int damage,float knockback,bool crit)//For projectile based weapons
		{
			PInfo info=projectile.GetModInfo<PInfo>(mod);
			if(info.modules!=null)
			{
				foreach(ModuleData module in info.modules)
				{
					Module.onHitEffect[module.type](info.item,projectile,npc,module.level);
				}
			}
		}
	}
		
	public class IInfo:ItemInfo
	{
		public ushort[] modules;
		public Stack<ModuleData> compact;
		
		public void UpdateIInfo()
		{
			if(this.modules==null){this.compact=null;return;}
			ushort[] temp=(ushort[])this.modules.Clone();
			this.compact=new Stack<ModuleData>();
			for(byte i=0;i<temp.Length;i++)
			{
				if(temp[i]!=0)
				{
					byte level=1;
					for(byte i2=(byte)(i+1);i2<temp.Length;i2++)
					{
						if(temp[i2]==temp[i])
						{
							level++;
							temp[i2]=0;
						}
					}
					this.compact.Push(new ModuleData(temp[i],level));
				}
			}
			if(this.compact.Count==0){this.compact=null;}
		}
	}
	
	public class ModuleData
	{
		public ushort type;	//ID of module
		public byte level;	//How many of that type
		public ModuleData(ushort type,byte level)
		{
			this.type=type;
			this.level=level;
		}
	}
	
	public class PInfo:ProjectileInfo
	{
		public bool check=true;
		public Stack<ModuleData> modules;
		public Item item;
	}
}