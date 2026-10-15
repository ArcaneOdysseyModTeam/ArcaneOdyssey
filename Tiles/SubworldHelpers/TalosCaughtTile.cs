using ArcaneOdyssey.Tiles.Bronze;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ObjectData;

namespace ArcaneOdyssey.Tiles.SubworldHelpers
{
	public class TalosCaughtTile : ModTile
	{
		public override string Texture => ArcaneOdysseyMod.DevMode ? AOUtils.GetTexture<BronzeBarTile>() : AOUtils.BlankTileTexture;
		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Mod.CoolCustomLocalization(this.GetLocalizationKey("Death"), "{0} was caught by the guards.");
			Main.tileNoAttach[Type] = !ArcaneOdysseyMod.DevMode;
			TileID.Sets.AvoidedByMeteorLanding[Type] = true;
			TileID.Sets.NotReallySolid[Type] = true;
			Main.tileSolid[Type] = true;
			MinPick = 100000;
			MineResist = 1000f;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.newTile.LavaPlacement = LiquidPlacement.Allowed;
			TileObjectData.addTile(Type);
			VanillaFallbackOnModDeletion = TileID.LihzahrdBrick;
			TileID.Sets.NegatesFallDamage[Type] = true;
			if (ExternalModSupport.HasFargos)
			{
				ExternalModSupport.Fargos.Call("AddIndestructibleTileType", Type);
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

		public override bool CanExplode(int i, int j) => false;

		public override bool CanKillTile(int i, int j, ref bool blockDamaged) => false;

		public override bool CreateDust(int i, int j, ref int type) => false;

		public override void NearbyEffects(int i, int j, bool closer)
		{
			Main.tileSolid[Type] = !DownedBosses.DownedArgos;
		}

		public override void FloorVisuals(Player player)
		{
			if (!DownedBosses.DownedArgos)
			{
				player.KillMe(PlayerDeathReason.ByCustomReason(Mod.CustomLocalization(this.GetLocalizationKey("Death"), player.name).ToNetworkText()), player.statLifeMax2 * 2, 0);
			}
		}
	}
}
