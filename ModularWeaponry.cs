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

namespace ModularWeaponry
{
	public class ModularWeaponry:Mod
	{
		public const byte MAX_MODULES=5;
		public static bool moduleInterfaceOpen;
		public static Mod mod;//ModularWeaponry
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
		public bool updateItemSlot=false;
		public Item[] itemModules;

		public override void PostDrawInterface(SpriteBatch spriteBatch)
		{
			//Initiallize item slot if neccessary
			if(item==null)item=new Item();			
			if(!Main.ingameOptionsWindow&&Main.playerInventory&&!Main.inFancyUI&&moduleInterfaceOpen)//Here we check if we're not in any UI and if the players' inventory is open.
			{
				Main.inventoryScale=0.9F;
				Vector2 centerDrawPos=new Vector2(110,280);
				if(MouseOverSlot(centerDrawPos))//Handle weapon Item.
				{
					Main.player[Main.myPlayer].mouseInterface=true;
					if (Main.mouseLeftRelease&&Main.mouseLeft)
					{
						if (Main.mouseItem.type==0||Main.mouseItem.IsCompatible())
						{
							ItemSlot.LeftClick(ref item,0);
							updateItemSlot=true;
						}
					}
					ItemSlot.MouseHover(ref item,0);
				}
				ItemSlot.Draw(spriteBatch,ref item,1,centerDrawPos);
				if(updateItemSlot)//Check if the item that is being modified has been switched out and if so, if it's not an 'empty' item.
				{
					if(item.type!=0)//If so, reset the 'itemModules' array and fill it with the appropriate items.
					{
						IInfo info=item.GetModInfo<IInfo>(this);
						if(info.modules==null)
						{
							info.modules=new string[MAX_MODULES];
						}
						itemModules=new Item[info.modules.Length];
						for(int i=0;i<itemModules.Length;++i)
						{
							itemModules[i]=new Item();
							itemModules[i].SetDefaults(info.modules[i]);
						}
					}
					updateItemSlot=false;
				}
				if(item.type!=0)//If there is actually an item being modified.
				{
					for(int i=0;i<itemModules.Length;++i)
					{
						Vector2 drawPos=new Vector2(centerDrawPos.X+(52*i),centerDrawPos.Y+52);
						if(MouseOverSlot(drawPos))
						{
							Main.player[Main.myPlayer].mouseInterface=true;
							if(Main.mouseLeftRelease&&Main.mouseLeft)
							{
								if(Main.mouseItem.type==0||item.CanEquipModule(Main.mouseItem))
								{
									ItemSlot.LeftClick(itemModules,0,i);
									item.UpdateModules(itemModules.ToStringArray());
								}
							}
							ItemSlot.MouseHover(itemModules,0,i);
						}
						ItemSlot.Draw(spriteBatch,itemModules,1,i,drawPos);
					}
				}
			}
			else if(!moduleInterfaceOpen&&item.type!=0)
			{
				// Drop the weapon if it was left in the item slot.
				for (int i=0;i<Main.item.Length;++i)
				{
					if(!Main.item[i].active||Main.item[i].type==0)
					{
						Main.item[i]=item;
						Main.item[i].position=Main.player[Main.myPlayer].Center;
						item=new Item();
						break;
					}
				}
			}
		}
		private bool MouseOverSlot(Vector2 pos){return Main.mouseX>=pos.X&&Main.mouseX<=pos.X+Main.inventoryBackTexture.Width*Main.inventoryScale&&(Main.mouseY>=pos.Y&&Main.mouseY<=pos.Y+Main.inventoryBackTexture.Height*Main.inventoryScale);}
	}

	public static class Extensions
	{
		public static bool IsModule(this Item item){return item.modItem is Module;}
		public static bool IsCompatible(this Item item){return item.GetTypes()!=0&&!item.IsModule();}
		public static bool CanEquipModule(this Item item,Item module){return module.IsModule()&&((item.GetTypes()&module.GetTypes())!=0);}
		
		public static string[] ToStringArray(this Item[] items)
		{
			string[] array=new string[items.Length];
			for(byte i=0;i<items.Length;i++){array[i]=items[i].type!=0?Main.itemName[items[i].type]:null;}
			return array;
		}
		
		public static void UpdateModules(this Item item,string[] modules=null)
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
				((Module)ModularWeaponry.mod.GetItem(module.name)).UpdateStats(item,module.level);
			}
		}
		
		public static ItemTag GetTypes(this Item item)
		{
			if(item.IsModule()){return((Module)item.modItem).itemTag;}
			ItemTag r=ItemTag.None;
			if(item.melee)			{r|=ItemTag.Melee;}
			if(item.ranged)			{r|=ItemTag.Range;}
			if(item.magic)			{r|=ItemTag.Magic;}
			if(item.useStyle==5)
			{
				if(item.pick>0)		{r|=ItemTag.Drill;}
				if(item.axe>0)		{r|=ItemTag.Saw;}
				if(item.hammer>0)	{r|=ItemTag.Jack;}
			}
			else
			{
				if(item.pick>0)		{r|=ItemTag.Pick;}
				if(item.axe>0)		{r|=ItemTag.Axe;}
				if(item.hammer>0)	{r|=ItemTag.Hammer;}
			}
			if(item.headSlot>-1)	{r|=ItemTag.Head;}
			if(item.bodySlot>-1)	{r|=ItemTag.Body;}
			if(item.legSlot>-1)		{r|=ItemTag.Legs;}
			//if(item.)		{r|=ItemTag.Wings;}
			if(item.accessory)		{r|=ItemTag.Accessory;}
			if(item.createTile>-1)	{r|=ItemTag.Tile;}
			if(item.createWall>-1)	{r|=ItemTag.Wall;}
			if(((r&ItemTag.Damage)!=0)&&!((r&ItemTag.Tool)!=0)){r|=ItemTag.Weap;}
			return r;
		}
	}
}
