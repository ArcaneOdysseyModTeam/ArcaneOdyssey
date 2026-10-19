using Terraria.Enums;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ObjectData;

namespace ArcaneOdyssey.Tiles.SubworldHelpers
{
	public class SpawnSetter : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			TileID.Sets.IsValidSpawnPoint[Type] = true;
			TileID.Sets.DisableSmartCursor[Type] = true;
			TileID.Sets.InteractibleByNPCs[Type] = true;
			TileID.Sets.HasOutlines[Type] = true;
			MinPick = 100000;
			MineResist = 1000f;
			VanillaFallbackOnModDeletion = TileID.Tombstones;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.newTile.Origin = new(0, 1);
			TileObjectData.newTile.StyleLineSkip = 2;
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.Direction = TileObjectDirection.PlaceRight;
			TileObjectData.addAlternate(1);
			TileObjectData.addTile(Type);

			AddMapEntry(Color.DarkGray);
		}

		public override bool RightClick(int i, int j)
		{
			if (Main.LocalPlayer.SpawnPos().ToVector2().Distance(new Vector2(i, j)) < 2)
			{
				Main.LocalPlayer.RemoveSpawn();
			}
			else
			{
				Main.LocalPlayer.ChangeSpawn(i, j);
			}
			return true;
		}

		public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings) => true;

		public override bool CanExplode(int i, int j) => false;

		public override bool CanKillTile(int i, int j, ref bool blockDamaged) => false;

		public override bool CreateDust(int i, int j, ref int type) => false;

		public override void NearbyEffects(int i, int j, bool closer)
		{
			if (closer && !Main.dedServ)
			{
				var player = Main.LocalPlayer;
				var tile = AOUtils.GetTile(new Point(i, j));
				if (player.SpawnPos().ToVector2().Distance(new Vector2(i, j)) < 2)
				{
					if (tile.TileFrameX < 18 * 2)
						tile.TileFrameX += 18 * 2;
				}
				else
				{
					if (tile.TileFrameX >= 18 * 2)
					{
						tile.TileFrameX -= 18 * 2;
					}
				}
			}
		}

#if VSDEBUGMODE
		public override void MouseOverFar(int i, int j)
		{
			Player player = Main.LocalPlayer;
			player.noThrow = 2;
			player.cursorItemIconText = PrettyPrintName();
			player.cursorItemIconID = -1;
			player.cursorItemIconEnabled = true;
		}
#endif
	}
}
