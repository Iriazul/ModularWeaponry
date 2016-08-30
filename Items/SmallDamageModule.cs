using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using ModularWeaponry.Items.Base;

namespace ModularWeaponry.Items
{
    public class SmallDamageModule : Module
    {
        public override void SetDefaults()
        {
            item.name = "Small Damage Module";
            item.width = item.height = 16;

            this.moduleType = 1;
            this.applyModule = delegate (ref Item weapon)
            {
                IInfo info = weapon.GetModInfo<IInfo>(mod);
                int index = info.GetEmptyModule();

                if (index < 0) return false;

                return true;
            };
        }
    }
}
