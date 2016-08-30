using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ModularWeaponry.Items
{
    public class GItem : GlobalItem
    {
        public override bool NeedsCustomSaving(Item item)
        {
            IInfo info = item.GetModInfo<IInfo>(mod);
            return info.modulesInstalled != null;
        }

        public override void SaveCustomData(Item item, BinaryWriter writer)
        {
            IInfo info = item.GetModInfo<IInfo>(mod);

            string writeString = "";
            for (int i = 0; i < info.modulesInstalled.Length; ++i)
            {
                writeString += info.modulesInstalled[i] + ";";
            }
            writer.Write(writeString);
        }
        public override void LoadCustomData(Item item, BinaryReader reader)
        {
            IInfo info = item.GetModInfo<IInfo>(mod);
            info.modulesInstalled = new int[3];

            string[] splitModules = reader.ReadString().Split(';');
            for (int i = 0; i < info.modulesInstalled.Length; ++i)
            {
                info.modulesInstalled[i] = int.Parse(splitModules[i]);
            }
        }
    }

    public class IInfo : ItemInfo
    {
        public int[] modulesInstalled;

        public int GetEmptyModule()
        {
            for(int i = 0; i < modulesInstalled.Length; ++i)
            {
                if (modulesInstalled[i] == 0)
                    return i;
            }
            return -1;
        }
    }
}
