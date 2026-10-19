using ArcaneOdyssey.NPCs;
using ArcaneOdyssey.Tiles.Bronze;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ObjectData;

namespace ArcaneOdyssey.Tiles.SubworldHelpers
{
	public class RowboatSpawnerTile : ModTile
	{
		public override string Texture => ArcaneOdysseyMod.DevMode ? AOUtils.GetTexture<BronzeBarTile>() : AOUtils.BlankTileTexture;
		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			TileID.Sets.AvoidedByMeteorLanding[Type] = true;
			MinPick = 100000;
			MineResist = 1000f;
			VanillaFallbackOnModDeletion = TileID.LihzahrdBrick;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.newTile.LavaPlacement = LiquidPlacement.Allowed;
			TileObjectData.newTile.HookPostPlaceMyPlayer = ModContent.GetInstance<RowboatSpawnerTileEntity>().Generic_HookPostPlaceMyPlayer;
			TileObjectData.addTile(Type);
			if (ExternalModSupport.HasFargos)
			{
				ExternalModSupport.Fargos.Call("AddIndestructibleTileType", Type);
			}
		}

		public override bool CanExplode(int i, int j) => false;

		public override bool CanKillTile(int i, int j, ref bool blockDamaged) => false;

		public override bool CreateDust(int i, int j, ref int type) => false;

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

	public class RowboatSpawnerTileEntity : ModTileEntity
	{
		public override bool IsTileValidForEntity(int x, int y)
		{
			var tile = AOUtils.GetTile(x, y);
			return tile.HasTile && tile.TileType == ModContent.TileType<RowboatSpawnerTile>();
		}

		public override void Update()
		{
			if (myRowboat < 0 || !Main.npc[myRowboat].active || Main.npc[myRowboat].type != ModContent.NPCType<RowboatNPC>())
			{
				myRowboat = NPC.NewNPC(new EntitySource_TileEntity(this), Position.ToWorldCoordinates().X.Round(), Position.ToWorldCoordinates().Y.Round(), ModContent.NPCType<RowboatNPC>());
			}
		}

		public int myRowboat = -1;
	}
}
