using ArcaneOdyssey.NPCs.Idle;
using ArcaneOdyssey.Tiles.Bronze;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ObjectData;

namespace ArcaneOdyssey.Tiles.Subworlds
{
	public class TalosGuardSpawner : ModTile
	{
		public override string Texture => ArcaneOdysseyMod.DevMode ? AOUtils.GetTexture<BronzeBarTile>() : AOUtils.BlankTileTexture;
		public override void SetStaticDefaults()
		{
			TileID.Sets.BlockMergesWithMergeAllBlockOverride[Type] = false;
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			TileID.Sets.AvoidedByMeteorLanding[Type] = true;
			if (!ArcaneOdysseyMod.DevMode)
			{
				MinPick = 100000;
				MineResist = 1000f;
			}
			VanillaFallbackOnModDeletion = TileID.LihzahrdBrick;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.newTile.LavaPlacement = LiquidPlacement.Allowed;
			TileObjectData.newTile.HookPostPlaceMyPlayer = ModContent.GetInstance<GuardSpawnerTileEntity>().Generic_HookPostPlaceMyPlayer;
			TileObjectData.addTile(Type);
			if (ExternalModSupport.HasFargos)
			{
				ExternalModSupport.Fargos.Call("AddIndestructibleTileType", Type);
			}
		}

		public override bool CanExplode(int i, int j) => false;

		public override bool CanKillTile(int i, int j, ref bool blockDamaged) => ArcaneOdysseyMod.DevMode;

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

	public class GuardSpawnerTileEntity : ModTileEntity
	{
		public override bool IsTileValidForEntity(int x, int y)
		{
			var tile = AOUtils.GetTile(x, y);
			return tile.HasTile && tile.TileType == ModContent.TileType<TalosGuardSpawner>();
		}

		public override void Update()
		{
			if (!DownedBosses.DownedArgos)
			{
				if (myGuard < 0 || !Main.npc[myGuard].active || Main.npc[myGuard].type != ModContent.NPCType<IdleGuard>())
				{
					myGuard = NPC.NewNPC(new EntitySource_TileEntity(this), Position.ToWorldCoordinates().X.Round(), Position.ToWorldCoordinates().Y.Round(), ModContent.NPCType<IdleGuard>());
					var npc = Main.npc[myGuard];
					npc.Bottom = Position.ToWorldCoordinates(0, 16);
					npc.spriteDirection = Main.rand.NextBool().ToDirectionInt();
				}
			}
		}

		public int myGuard = -1;
	}
}
