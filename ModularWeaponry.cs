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

        public Item weapon;
        public Item module;
        public override void PostDrawInterface(SpriteBatch spriteBatch)
        {
            if (weapon == null) weapon = new Item();
            if (module == null) module = new Item();

            // Here we check if we're not in any UI and if the players' inventory is open.            
            if (!Main.ingameOptionsWindow && Main.playerInventory && !Main.inFancyUI && moduleInterfaceOpen)
            {
                Main.inventoryScale = 0.9F;

                // Handle weapon Item.
                Vector2 drawPos = new Vector2(110, 280);
                this.HandleUIItem(spriteBatch, ref weapon, drawPos);
                // Handle module Item.
                drawPos = new Vector2(110 + 52, 280);
                this.HandleUIItem(spriteBatch, ref module, drawPos, true);

                drawPos = new Vector2(110 + 102, 288);
                bool hover = false;
                if (Main.mouseX > drawPos.X && Main.mouseX < drawPos.X + 30 &&
                            Main.mouseY > drawPos.Y && Main.mouseY < drawPos.Y + 30 && !PlayerInput.IgnoreMouseInterface)
                {
                    Main.player[Main.myPlayer].mouseInterface = true;

                    hover = true;
                }

                // Click on craft button
                if(hover && Main.mouseLeftRelease && Main.mouseLeft)
                {
                    if (weapon.type != 0 && module.type != 0)
                    {
                        if (((Module)module.modItem).applyModule(ref weapon))
                        {
                            module = new Item();
                        }
                    }
                }
                // Draw craft button.
                spriteBatch.Draw(Main.craftToggleTexture[hover ? 1 : 0], drawPos, Color.White);
            }
            else if (!moduleInterfaceOpen && (weapon.type != 0 || module.type != 0))
            {
                // Drop any items that were left in the item spots
            }
        }

        private void HandleUIItem(SpriteBatch spriteBatch, ref Item item, Vector2 drawPos, bool module = false)
        {
            if (Main.mouseX >= drawPos.X && (double)Main.mouseX <= (double)drawPos.X + (double)Main.inventoryBackTexture.Width * (double)Main.inventoryScale && (Main.mouseY >= drawPos.Y && (double)Main.mouseY <= (double)drawPos.Y + (double)Main.inventoryBackTexture.Height * (double)Main.inventoryScale))
            {
                Main.player[Main.myPlayer].mouseInterface = true;

                if (Main.mouseLeftRelease && Main.mouseLeft)
                {
                    if (Main.mouseItem.type == 0 || (module && Main.mouseItem.IsModule()) || (!module && Main.mouseItem.damage > 0))
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
