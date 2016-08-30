using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ModularWeaponry
{
    public class GItem : GlobalItem
    {
        public override bool NeedsCustomSaving(Item item)
        {
            IInfo info = item.GetModInfo<IInfo>(mod);
            return info.modules != null;
        }

        public override void SaveCustomData(Item item,BinaryWriter writer)
        {
            IInfo info=item.GetModInfo<IInfo>(mod);
            string writeString="";
            for(byte i=0;i<info.modules.Length;++i)
            {
                writeString+=ItemLoader.GetItem(info.modules[i]).item.name+";";
            }
            writer.Write(writeString);
        }
        public override void LoadCustomData(Item item, BinaryReader reader)
        {
            IInfo info = item.GetModInfo<IInfo>(mod);
            info.modules = new ushort[5];
            string[] splitModules = reader.ReadString().Split(';');
            for (byte i=0;i<info.modules.Length;++i)
            {
                info.modules[i]=(ushort)mod.ItemType(splitModules[i]);
            }
        }
    }

    public class IInfo : ItemInfo
    {
        public ushort[] modules;
    }
}
