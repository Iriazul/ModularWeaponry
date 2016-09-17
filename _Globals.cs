using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ModularWeaponry.Items;

namespace ModularWeaponry
{
	public class GItem:GlobalItem
	{
		/**************************\
		|*IO AND DATA PRESERVATION*|
		\**************************/
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
				foreach(string name in info.modules)
				{
					if(name!=null)
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
				if(i!=0){writeString+=";";}
				if(info.modules[i]!=null){writeString+=info.modules[i];}
			}
			writer.Write(writeString);
		}
		public override void LoadCustomData(Item item, BinaryReader reader)
		{
			string[] splitModules=reader.ReadString().Split(';');
			if(splitModules.Length<=1){return;}
			for(byte i=0;i<splitModules.Length;++i)
			{
				if(splitModules[i]==""){splitModules[i]=null;}
			}
			item.GetModInfo<IInfo>(mod).modules=splitModules;
			item.UpdateModules();
		}
		
		/***********\
		|*AESTHETICS*|
		\***********/
		public override void ModifyTooltips(Item item,List<TooltipLine> tooltips)
		{
			if(item.IsModule()){tooltips[0].overrideColor=((Module)item.modItem).textClr;return;}
			IInfo info=item.GetModInfo<IInfo>(mod);
			if(info!=null&&info.compact!=null)
			{
				TooltipLine moduleToolTip=new TooltipLine(mod,"","--[Modules]--");
				moduleToolTip.overrideColor=Color.Gray;
				tooltips.Add(moduleToolTip);
				foreach(ModuleData module in info.compact)
				{
					TooltipLine line=new TooltipLine(mod,"",((Module)mod.GetItem(module.name)).display+" [Level "+module.level+"]");
					line.overrideColor=((Module)mod.GetItem(module.name)).textClr;
					tooltips.Add(line);
				}
			}
		}
		
		/*********************\
		|*HOOK IMPLEMENTATION*|
		\*********************/
		public override void UpdateEquip(Item item,Player player)
		{
			if(item.type!=0)
			{
				IInfo info=item.GetModInfo<IInfo>(mod);
				if(info!=null&&info.compact!=null)
				{
					foreach(ModuleData module in info.compact)
					{
						((Module)mod.GetItem(module.name)).UpdateEquip(item,player,module.level);
					}
				}
			}
		}
		
		public override void OnHitNPC(Item item,Player player,NPC npc,int damage,float knockBack,bool crit)//For melee weapons
		{
			IInfo info=item.GetModInfo<IInfo>(mod);
			if(info!=null&&info.compact!=null)
			{
				foreach(ModuleData module in info.compact)
				{
					((Module)mod.GetItem(module.name)).OnHitEffect(item,player,null,npc,module.level);
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
					Item item=player.inventory[player.selectedItem];
					IInfo itemInfo=item.GetModInfo<IInfo>(mod);
					if(itemInfo.compact!=null)
					{
						projInfo.modules=itemInfo.compact;
						projInfo.item=item;
						projInfo.player=player;
						bool killProjectile=false;
						foreach(ModuleData module in itemInfo.compact)
						{
							if(!((Module)mod.GetItem(module.name)).OnShootProj(projInfo.item,player,projectile,module.level)){killProjectile=true;}
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
			PInfo projInfo=projectile.GetModInfo<PInfo>(mod);
			if(projInfo.modules!=null)
			{
				foreach(ModuleData module in projInfo.modules)
				{
					((Module)mod.GetItem(module.name)).OnHitEffect(projInfo.item,projInfo.player,projectile,npc,module.level);
				}
			}
		}
	}
		
	public class IInfo:ItemInfo
	{
		public string[] modules;
		public Stack<ModuleData> compact;
		
		public void UpdateIInfo()
		{
			if(modules==null){compact=null;return;}
			string[] temp=(string[])modules.Clone();
			compact=new Stack<ModuleData>();
			for(byte i=0;i<temp.Length;i++)
			{
				if(temp[i]!=null)
				{
					ModuleData modData=((Module)mod.GetItem(temp[i])).modData;
					byte level=modData.level;
					for(byte i2=(byte)(i+1);i2<temp.Length;i2++)
					{
						if(temp[i2]!=null)
						{
							ModuleData modData2=((Module)mod.GetItem(temp[i2])).modData;
							if(modData2.name==modData.name)
							{
								level+=modData2.level;
								temp[i2]=null;
							}
						}
					}
					if(level>((Module)mod.GetItem(modData.name)).maximum){level=((Module)mod.GetItem(modData.name)).maximum;}
					compact.Push(new ModuleData(modData.name,level));
				}
			}
			if(compact.Count<1){compact=null;}
		}
		public void Print()
		{
			if(modules!=null){
				if(modules.Length>0){
					string line="modules {"+modules[0];
					for(byte i=1;i<modules.Length;i++){line+=","+modules[i];}
					Main.NewText(line+"}");
				}else{Main.NewText("modules array is empty");}
			}else{Main.NewText("modules array is null");}
			if(compact!=null){
				if(compact.Count>0){
					ModuleData[] temp=compact.ToArray();
					string line="compact {["+temp[0].name+":"+temp[0].level;
					for(byte i=1;i<temp.Length;i++){line+="],["+temp[i].name+":"+temp[i].level;}
					Main.NewText(line+"]}");
				}else{Main.NewText("compact stack is empty");}
			}else{Main.NewText("compact stack is null");}
		}
	}
	
	public class ModuleData
	{
		public string name;
		public byte level;	//Strength or quantity
		public ModuleData(string name,byte level=1)
		{
			this.name=name;
			this.level=level;
		}
	}
	
	public class PInfo:ProjectileInfo
	{
		public bool check=true;
		public Stack<ModuleData> modules;
		public Item item;
		public Player player;
	}
}
