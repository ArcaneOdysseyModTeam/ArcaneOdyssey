using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace ArcaneOdyssey.Tiles
{
	[LegacyName("TitleMusicBoxTile")]
	public class WhiteEyesPlushTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileObsidianKill[Type] = true;
			TileID.Sets.HasOutlines[Type] = true;
			TileID.Sets.DisableSmartCursor[Type] = true;
			TileID.Sets.InteractibleByNPCs[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.Origin = new Point16(0, 1);
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.newTile.Direction = TileObjectDirection.PlaceLeft;
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.Direction = TileObjectDirection.PlaceRight;
			TileObjectData.addAlternate(1);
			TileObjectData.addTile(Type);

			AddMapEntry(Color.IndianRed, Lang.GetItemName(TileLoader.GetItemDropFromTypeAndStyle(Type)));
		}

		public static readonly SoundStyle YippeeSound = new(ArcaneOdysseyMod.InternalName + "/Sounds/ElfPetYippee") { MaxInstances = 0 };

		public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings) => true;

		public override bool CreateDust(int i, int j, ref int type) => false;

		public override void MouseOver(int i, int j)
		{
			Player player = Main.LocalPlayer;
			player.noThrow = 2;
			player.cursorItemIconID = TileLoader.GetItemDropFromTypeAndStyle(Type);
			player.cursorItemIconEnabled = true;
		}

		public override bool RightClick(int i, int j)
		{
			SoundEngine.PlaySound(YippeeSound, new Vector2(i, j).ToWorldCoordinates());
			return true;
		}
	}
}
