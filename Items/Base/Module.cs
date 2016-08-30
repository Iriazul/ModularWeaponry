using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ModularWeaponry.Items.Base
{
    public delegate bool ApplyModule(ref Item weapon);
    public class Module : ModItem
    {
        public int moduleType;
        public ApplyModule applyModule;
    }
}
