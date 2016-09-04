using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.UI;
using Terraria.ID;
using Terraria.GameInput;
using Terraria.ModLoader;

using ModularWeaponry.Items;
using ModularWeaponry.Items.Base;

namespace ModularWeaponry
{
	public class ModularWeaponry:Mod
	{
		public const byte MAX_MODULES=5;
		public static bool moduleInterfaceOpen;
		public static ModularWeaponry mod;
		public ModularWeaponry()
		{
			mod=this;
			this.Properties=new ModProperties()
			{
				Autoload=true,
				AutoloadGores=true,
				AutoloadSounds=true
			};
		}

		public Item item;
		public Item lastItem;
		public Item[] itemModules;

		public override void PostDrawInterface(SpriteBatch spriteBatch)
		{
			//Initiallize items if neccessary
			if(item==null)item=new Item();
			if(lastItem==null)lastItem=new Item();

			// Here we check if we're not in any UI and if the players' inventory is open.			
			if (!Main.ingameOptionsWindow && Main.playerInventory && !Main.inFancyUI && moduleInterfaceOpen)
			{
				Main.inventoryScale = 0.9F;

				// Handle weapon Item.
				Vector2 drawPos = new Vector2(110, 280);
				this.HandleUIItem(spriteBatch, ref item, drawPos);

				// Check if the item that is being modified has been switched out and if so, if it's not an 'empty' item.
				if (lastItem != item)
				{
					// Set the modules for the last item correctly.
					if (lastItem.type != 0)
					{
						IInfo lastInfo=lastItem.GetModInfo<IInfo>(this);
						for (byte i=0;i<itemModules.Length;++i)
						{
							lastInfo.modules[i]=(ushort)itemModules[i].type;
						}
					}
					
					if(item.type!=0)
					{
						// If so, reset the 'itemModules' array and fill it with the appropriate items.
						IInfo info=item.GetModInfo<IInfo>(this);
						if(info.modules==null)
						{
							info.modules=new ushort[MAX_MODULES];
						}
						itemModules=new Item[info.modules.Length];
						for(int i=0;i<itemModules.Length;++i)
						{
							itemModules[i]=new Item();
							itemModules[i].SetDefaults(info.modules[i]);
						}
					}
				}

				// If there is actually an item being modified.
				if(item.type!=0)
				{
					for (int i = 0; i < itemModules.Length; ++i)
					{
						drawPos = new Vector2(110 + (52 * i), 280 + 52);
						if (Main.mouseX >= drawPos.X && Main.mouseX <= drawPos.X + Main.inventoryBackTexture.Width * Main.inventoryScale && (Main.mouseY >= drawPos.Y && Main.mouseY <= drawPos.Y + Main.inventoryBackTexture.Height * Main.inventoryScale))
						{
							Main.player[Main.myPlayer].mouseInterface = true;

							if (Main.mouseLeftRelease && Main.mouseLeft)
							{
								if(Main.mouseItem.type==0||item.CanEquipModule(Main.mouseItem))
								{
									ItemSlot.LeftClick(itemModules,0,i);
									item.UpdateModules(itemModules.ToTypeArray());
								}
							}
							ItemSlot.MouseHover(itemModules, 0, i);
						}
						ItemSlot.Draw(spriteBatch, itemModules, 1, i, drawPos);
					}
				}

				lastItem=item;
			}
			else if (!moduleInterfaceOpen && item.type != 0)
			{
				// Drop the weapon if it was left in the item slot.
				IInfo info = item.GetModInfo<IInfo>(this);
				info.modules=itemModules.ToTypeArray();

				for (int i = 0; i < 400; ++i)
				{
					if(!Main.item[i].active || Main.item[i].type == 0)
					{
						Main.item[i] = item;
						Main.item[i].position = Main.player[Main.myPlayer].Center;

						item = new Item();
						break;
					}
				}
			}
			lastItem=item;
		}

		private void HandleUIItem(SpriteBatch spriteBatch, ref Item item, Vector2 drawPos)
		{
			if (Main.mouseX >= drawPos.X && Main.mouseX <= drawPos.X + Main.inventoryBackTexture.Width * Main.inventoryScale && (Main.mouseY >= drawPos.Y && Main.mouseY <= drawPos.Y + Main.inventoryBackTexture.Height * Main.inventoryScale))
			{
				Main.player[Main.myPlayer].mouseInterface = true;

				if (Main.mouseLeftRelease && Main.mouseLeft)
				{
					if (Main.mouseItem.type==0||Main.mouseItem.IsCompatible())
					{
						ItemSlot.LeftClick(ref item, 0);
					}
				}
				ItemSlot.MouseHover(ref item, 0);
			}
			ItemSlot.Draw(spriteBatch, ref item, 1, drawPos);
		}
	}

	public static class Extensions
	{
		public static bool IsModule(this Item item)
		{
			return item.modItem is Module;
		}
		
		public static bool IsCompatible(this Item item)
		{
			return item.GetTypes()!=0&&!item.IsModule();
		}
		
		public static bool CanEquipModule(this Item item,Item module)
		{
			return module.IsModule()&&((item.GetTypes()&module.GetTypes())!=0);
		}
		
		public static ushort[] ToTypeArray(this Item[] items)
		{
			ushort[] array=new ushort[items.Length];
			for(byte i=0;i<items.Length;i++){array[i]=items!=null?(ushort)items[i].type:(ushort)0;}
			return array;
		}
		
		
		
		public static void UpdateModules(this Item item,ushort[] modules=null)
		{
			if(modules!=null){item.GetModInfo<IInfo>(ModularWeaponry.mod).modules=modules;}
			Item temp=item.Clone();
			item.SetDefaults(item.type);
			item.Prefix(temp.prefix);
			IInfo info=item.GetModInfo<IInfo>(ModularWeaponry.mod);
			info.modules=temp.GetModInfo<IInfo>(ModularWeaponry.mod).modules;
			info.UpdateIInfo();
			foreach(ModuleData module in info.compact)
			{
				Module.updateStats[module.type](item,module.level);
			}
		}
		
		public static IType GetTypes(this Item item)
		{
			if(item.IsModule()){return((Module)item.modItem).iType;}
			IType r=IType.None;
			if(item.melee)			{r|=IType.Melee;}
			if(item.ranged)			{r|=IType.Range;}
			if(item.magic)			{r|=IType.Magic;}
			if(item.useStyle==5)
			{
				if(item.pick>0)		{r|=IType.Drill;}
				if(item.axe>0)		{r|=IType.Saw;}
				if(item.hammer>0)	{r|=IType.Jack;}
			}
			else
			{
				if(item.pick>0)		{r|=IType.Pick;}
				if(item.axe>0)		{r|=IType.Axe;}
				if(item.hammer>0)	{r|=IType.Hammer;}
			}
			if(item.headSlot>-1)	{r|=IType.Head;}
			if(item.bodySlot>-1)	{r|=IType.Body;}
			if(item.legSlot>-1)		{r|=IType.Legs;}
			//if(item.)		{r|=IType.Wings;}
			if(item.accessory)		{r|=IType.Accessory;}
			if(item.createTile>-1)	{r|=IType.Tile;}
			if(item.createWall>-1)	{r|=IType.Wall;}
			if(((r&IType.Damage)!=0)&&!((r&IType.Tool)!=0)){r|=IType.Weap;}
			return r;
		}
	}
}
