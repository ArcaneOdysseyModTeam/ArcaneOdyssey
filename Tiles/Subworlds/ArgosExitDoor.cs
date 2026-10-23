using Terraria.ObjectData;

namespace ArcaneOdyssey.Tiles.Subworlds
{
	public class ArgosExitDoor : ModTile
	{
		public override void SetStaticDefaults()
		{
			if (!ArcaneOdysseyMod.DevMode)
			{
				MinPick = 100000;
				MineResist = 1000f;
			}
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = !ArcaneOdysseyMod.DevMode;
			TileID.Sets.DrawsWalls[Type] = true;
			TileID.Sets.AvoidedByMeteorLanding[Type] = true;
			TileID.Sets.NotReallySolid[Type] = true;
			Main.tileSolid[Type] = true;

			TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.newTile.StyleLineSkip = 2;
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.addAlternate(1);
			TileObjectData.addTile(Type);

			AddMapEntry(Color.DarkGray);
		}

		public override bool CanExplode(int i, int j) => false;

		public override bool CanKillTile(int i, int j, ref bool blockDamaged) => ArcaneOdysseyMod.DevMode;

		public override bool CreateDust(int i, int j, ref int type) => false;

		public static bool Open => DownedBosses.DownedArgos && !AOUtils.BossAlive;

		public override void NearbyEffects(int i, int j, bool closer)
		{
			Main.tileSolid[Type] = !Open;
			var tile = AOUtils.GetTile(new Point(i, j));
			if (closer)
			{
				if (!Open)
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
	}
}
