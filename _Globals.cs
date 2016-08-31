using System;
using System.IO;
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
		
		public override void OnHitNPC(Item item,Player player,NPC npc,int damage,float knockBack,bool crit)
		{
			IInfo info=item.GetModInfo<IInfo>(mod);
			if(info!=null)
			{
				foreach(ushort type in info.modules)
				{
					if(Module.onHitNPC.ContainsKey(Main.itemName[type]))
					{
						Module.onHitNPC[Main.itemName[type]](ref item,ref player,ref npc);
					}
				}
			}
		}
	}

	public class IInfo : ItemInfo
	{
		public ushort[] modules;
	}
}
