using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.UI;
using Terraria.ID;
using Terraria.GameInput;
using Terraria.ModLoader;

using ModularWeaponry.Items.Base;
using ModularWeaponry.Items;

namespace ModularWeaponry
{
    public class ModularWeaponry : Mod
    {
        public static Dictionary<int, ApplyModule> modules;
        public static bool moduleInterfaceOpen;

        public ModularWeaponry()
        {
            this.Properties = new ModProperties()
            {
                Autoload = true,
                AutoloadGores = true,
                AutoloadSounds = true
            };
        }

        public override void Load()
        {
            modules = new Dictionary<int, ApplyModule>();

            modules.Add(1, delegate (ref Item weapon)
            {
                IInfo info = weapon.GetModInfo<IInfo>(this);
                int index = info.GetEmptyModule();

                if (index < 0) return false;

                info.modulesInstalled[index] = 1;

                return true;
            });
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
                if (lastItem != item && item.type > 0)
                {
                    // If so, reset the 'itemModules' array and fill it with the appropriate items.
                    IInfo info = item.GetModInfo<IInfo>(this);
                    itemModules = new Item[info.modules.Length];
                    for (int i = 0; i < itemModules.Length; ++i)
                    {
                        itemModules[i].SetDefaults(info.modules[i]);
                    }
                }

                // If there is actually an item being modified.
                if(item.type > 0)
                {
                    for(int i = 0; i < itemModules.Length; ++i)
                    {
                        drawPos = new Vector2(110 + (52 * i), 280 + 52);
                        if (Main.mouseX >= drawPos.X && Main.mouseX <= drawPos.X + Main.inventoryBackTexture.Width * Main.inventoryScale && (Main.mouseY >= drawPos.Y && Main.mouseY <= drawPos.Y + Main.inventoryBackTexture.Height * Main.inventoryScale))
                        {
                            Main.player[Main.myPlayer].mouseInterface = true;

                            if (Main.mouseLeftRelease && Main.mouseLeft)
                            {
                                if (Main.mouseItem.type == 0 || Main.mouseItem.IsModule())
                                {
                                    ItemSlot.LeftClick(itemModules, 0, i);
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
                    if (Main.mouseItem.type == 0 || Main.mouseItem.damage > 0)
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
    }
}
