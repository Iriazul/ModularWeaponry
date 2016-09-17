using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace ModularWeaponry.Tiles
{
	public class ModularWorkbench:ModTile
	{
		public override void SetDefaults()
		{
			Main.tileSolidTop[Type]=true;
			Main.tileFrameImportant[Type]=true;
			Main.tileNoAttach[Type]=true;
			Main.tileTable[Type]=true;
			Main.tileLavaDeath[Type]=true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x4);
			//Needs corrected height when standing on top
			TileObjectData.addTile(Type);
			AddMapEntry(new Color(200,200,200),"Modular Workbench");
			disableSmartCursor=true;
			adjTiles=new int[]{TileID.WorkBenches};
		}

		public override void MouseOver(int i, int j)
		{
			Player player = Main.player[Main.myPlayer];
	   
			player.showItemIcon2 = mod.ItemType("ModularWorkbench");

			player.noThrow = 2;
			player.showItemIcon = true;
		}
		public override void RightClick(int i, int j)
		{
			int left, top;
			this.GetBase(i, j, out left, out top);

			Main.mouseRightRelease = false;

			Player player = Main.player[Main.myPlayer];

			if (player.sign >= 0)
			{
				Main.PlaySound(11, -1, -1, 1);
				player.sign = -1;
				Main.editSign = false;
				Main.npcChatText = "";
			}
			else if (player.chest >= 0)
			{
				Main.PlaySound(11, -1, -1, 1);
				player.chest = -1;
				Recipe.FindRecipes();
			}

			MPlayer mp = player.GetModPlayer<MPlayer>(mod);

			if (ModularWeaponry.moduleInterfaceOpen)
			{
				ModularWeaponry.moduleInterfaceOpen = false;
				mp.workbenchX = mp.workbenchY = -1;				
				return;
			}

			ModularWeaponry.moduleInterfaceOpen = true;
			Main.playerInventory = true;
			mp.workbenchX = left;
			mp.workbenchY = top;

			Main.npcChatText = "";
			Main.PlaySound(10, -1, -1, 1);
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			Item.NewItem(i * 16, j * 16, 32, 16, mod.ItemType("ModularWorkbench"));
		}

		private void GetBase(int x, int y, out int left, out int top)
		{
			left = x;
			top = y;
			Tile tile = Main.tile[x, y];
			if (tile.frameX % 36 != 0)
			{
				left--;
			}
			if (tile.frameY != 0)
			{
				top--;
			}
		}
	}
}