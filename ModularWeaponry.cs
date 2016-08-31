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
	public class ModularWeaponry : Mod
	{
		public static bool moduleInterfaceOpen;
		public static ModularWeaponry mod;
		public ModularWeaponry()
		{
			mod=this;
			this.Properties = new ModProperties()
			{
				Autoload = true,
				AutoloadGores = true,
				AutoloadSounds = true
			};
		}

		public Item item;
		public Item lastItem;
		public Item[] itemModules;

		public override void PostDrawInterface(SpriteBatch spriteBatch)
		{
			if (item == null) item = new Item();
			if (lastItem == null) lastItem = new Item();

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
						IInfo lastInfo = lastItem.GetModInfo<IInfo>(this);
						for (int i = 0; i < itemModules.Length; ++i)
						{
							lastInfo.modules[i] = (ushort)itemModules[i].type;
						}
					}

					try
					{
						if (item.type > 0)
						{
							// If so, reset the 'itemModules' array and fill it with the appropriate items.
							IInfo info = item.GetModInfo<IInfo>(this);
							if (info.modules == null)
								info.modules = new ushort[5];
							
							itemModules = new Item[info.modules.Length];
							for (int i = 0; i < itemModules.Length; ++i)
							{
								itemModules[i] = new Item();
								itemModules[i].SetDefaults(info.modules[i]);
							}
						}
					}
					catch (Exception e)
					{
						Main.NewText(e.Message);
					}
				}

				// If there is actually an item being modified.
				if(item.type > 0)
				{
					for (int i = 0; i < itemModules.Length; ++i)
					{
						drawPos = new Vector2(110 + (52 * i), 280 + 52);
						if (Main.mouseX >= drawPos.X && Main.mouseX <= drawPos.X + Main.inventoryBackTexture.Width * Main.inventoryScale && (Main.mouseY >= drawPos.Y && Main.mouseY <= drawPos.Y + Main.inventoryBackTexture.Height * Main.inventoryScale))
						{
							Main.player[Main.myPlayer].mouseInterface = true;

							if (Main.mouseLeftRelease && Main.mouseLeft)
							{
								if (Main.mouseItem.type == 0 || (Main.mouseItem.IsModule()&&((item.GetTypes()&Main.mouseItem.GetTypes())>0)))
								{
									ItemSlot.LeftClick(itemModules, 0, i);
									item.UpdateStats(itemModules);
								}
							}
							ItemSlot.MouseHover(itemModules, 0, i);
						}
						ItemSlot.Draw(spriteBatch, itemModules, 1, i, drawPos);
					}
				}

				lastItem = item;
			}
			else if (!moduleInterfaceOpen && item.type != 0)
			{
				// Drop the weapon if it was left in the item slot.
				IInfo info = item.GetModInfo<IInfo>(this);
				for(int i = 0; i < itemModules.Length; ++i)
				{
					info.modules[i] = (ushort)itemModules[i].type;
				}

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

			lastItem = item;
		}

		private void HandleUIItem(SpriteBatch spriteBatch, ref Item item, Vector2 drawPos)
		{
			if (Main.mouseX >= drawPos.X && Main.mouseX <= drawPos.X + Main.inventoryBackTexture.Width * Main.inventoryScale && (Main.mouseY >= drawPos.Y && Main.mouseY <= drawPos.Y + Main.inventoryBackTexture.Height * Main.inventoryScale))
			{
				Main.player[Main.myPlayer].mouseInterface = true;

				if (Main.mouseLeftRelease && Main.mouseLeft)
				{
					//if (Main.mouseItem.type == 0)
					//{
						ItemSlot.LeftClick(ref item, 0);
						//item.UpdateStats(itemModules);
					//}
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
		
		public static void UpdateStats(this Item item,Item[] itemModules)
		{
			Item temp=item.Clone();
			item.SetDefaults(item.type);
			item.Prefix(temp.prefix);
			item.GetModInfo<IInfo>(ModularWeaponry.mod).modules=temp.GetModInfo<IInfo>(ModularWeaponry.mod).modules;
			foreach(Item module in itemModules)
			{
				if(module!=null&&module.type>0)
				{
					Module.updateStats[Main.itemName[module.type]](ref item);
				}
			}
		}
		
		public static ItemType GetTypes(this Item item)
		{
			if(item.IsModule()){return((Module)item.modItem).itemType;}
			ItemType r=ItemType.None;
			if(item.melee)			{r|=ItemType.Melee;}
			if(item.ranged)			{r|=ItemType.Range;}
			if(item.magic)			{r|=ItemType.Magic;}
			if(item.useStyle==5)
			{
				if(item.pick>0)		{r|=ItemType.Drill;}
				if(item.axe>0)		{r|=ItemType.Saw;}
				if(item.hammer>0)	{r|=ItemType.Jack;}
			}
			else
			{
				if(item.pick>0)		{r|=ItemType.Pick;}
				if(item.axe>0)		{r|=ItemType.Axe;}
				if(item.hammer>0)	{r|=ItemType.Hammer;}
			}
			if(item.headSlot>-1)	{r|=ItemType.Head;}
			if(item.bodySlot>-1)	{r|=ItemType.Chest;}
			if(item.legSlot>-1)		{r|=ItemType.Legs;}
			//if(item.)		{r|=ItemType.Wings;}
			if(item.accessory)		{r|=ItemType.Accessory;}
			if(item.createTile>-1)	{r|=ItemType.Tile;}
			if(item.createWall>-1)	{r|=ItemType.Wall;}
			return r;
		}
	}
}
