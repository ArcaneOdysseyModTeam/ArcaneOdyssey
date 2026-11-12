using ArcaneOdyssey.Biomes;
using ArcaneOdyssey.NPCs.Base;
using ArcaneOdyssey.NPCs.Enemy;
using ArcaneOdyssey.Tiles.Subworlds;
using Terraria.DataStructures;

namespace ArcaneOdyssey.NPCs.Idle
{
	public class IdleGuard : BaseNPC
	{
		public override string Texture => AOUtils.GetTexture<TalosGuard>();

		public override void SetDefaults()
		{
			base.SetDefaults();
			NPC.defense = 8;
			NPC.lifeMax = 75;
			NPC.width = 26;
			NPC.height = 58;
			NPC.damage = 25;
			DrawOffsetY = -2;
			NPC.knockBackResist = 0f;
			NPC.HitSound = SoundID.NPCHit4;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.friendly = false;
			NPC.immortal = true;
			NPC.noGravity = true;
			NPC.noTileCollide = true;
			NPC.CanBeReplacedByOtherNPCs = true;
			NPC.chaseable = false;
			SpawnModBiomes = [AOUtils.BiomeType<TalosBiome>()];
		}

		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[Type] = TalosGuard.Frames;
			NPCID.Sets.ImmuneToAllBuffs[Type] = true;
			NPCID.Sets.TeleportationImmune[Type] = true;
		}

		public override void OnHitByItem(Player player, Item item, NPC.HitInfo hit, int damageDone)
		{
			if (!DownedBosses.DownedArgos)
			{
				player.KillMe(PlayerDeathReason.ByCustomReason(Mod.CustomLocalization(ModContent.GetInstance<TalosCaughtTile>().GetLocalizationKey("Death"), player.name).ToNetworkText()), player.statLifeMax2 * 2, 0);
			}
		}

		public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
		{
			if (!DownedBosses.DownedArgos)
			{
				target.KillMe(PlayerDeathReason.ByCustomReason(Mod.CustomLocalization(ModContent.GetInstance<TalosCaughtTile>().GetLocalizationKey("Death"), target.name).ToNetworkText()), target.statLifeMax2 * 2, 0);
			}
		}

		public override void OnHitByProjectile(Projectile projectile, NPC.HitInfo hit, int damageDone)
		{
			if (!DownedBosses.DownedArgos)
			{
				if (projectile.friendly && !projectile.hostile)
				{
					if (projectile.TryGetOwner(out var player))
					{
						player.KillMe(PlayerDeathReason.ByCustomReason(Mod.CustomLocalization(ModContent.GetInstance<TalosCaughtTile>().GetLocalizationKey("Death"), player.name).ToNetworkText()), player.statLifeMax2 * 2, 0);
					}
				}
			}
		}

		public override bool CheckActive() => false;

		public override void AI()
		{
			if (DownedBosses.DownedArgos)
			{
				NPC.active = false;
			}
		}
	}
}
