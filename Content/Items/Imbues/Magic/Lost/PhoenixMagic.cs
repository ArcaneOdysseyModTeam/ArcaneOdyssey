using ArcaneOdyssey.Content.Buffs.DOT;
using ArcaneOdyssey.Content.Buffs.MagicMarks;
using ArcaneOdyssey.Content.Buffs.Stuns;
using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Imbues.Magic.Normal;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Imbues.Magic.Lost
{
	public class PhoenixMagic : AOMagic
	{
		public override float DashSpeed => 1.2f; // burst
		public override bool? Cold => false;
		public override bool CanBeWet => false;
		public override SoundStyle? ImbueSound => SoundID.Item20;
		public override Color ImbueColour => new(0, 204, 255); // lerp between yellow and blue later
		public override AOImbuableTier ImbuableTier => AOImbuableTier.Lost;
		public override float AOImbueDamage => .95f;
		public override float AOImbueSpeed => 1.2f;
		public override float AOImbueSize => 1.3f;
		public override AODebuffRequirement[] ImbueDebuffs => [new(ModContent.BuffType<PhoenixHealing>(), 60 * 10),];

		public override CombinedDebuff[] CombinedDebuffs => [new(ModContent.BuffType<CharredEffect>(), ModContent.BuffType<AOPetrified>())];
		public override SynergyEffects Effects => new(
			[ // these are debuffs cleared on hit
				ModContent.BuffType<AOBleed>(),
				ModContent.BuffType<FreezingEffect>(),
				ModContent.BuffType<SnowyEffect>(),
				BuffID.Wet,
				ModContent.BuffType<CharredEffect>(),
				BuffID.Slimed
			],
			[
				new(ModContent.BuffType<AOBleed>(),1.15f),
				new(ModContent.BuffType<CharredEffect>(),1.01f),
				new(BuffID.Venom,1.05f),
				new(ModContent.BuffType<Crystallized>(),0.85f),
				new(ModContent.BuffType<FreezingEffect>(),0.99f),
				new(ModContent.BuffType<SnowyEffect>(),0.99f),
				new(BuffID.Wet,0.99f),
				new(BuffID.OnFire3,1.05f),
				new(BuffID.Poisoned,1.05f),
				new(BuffID.ShadowFlame,1.1f),
				new(BuffID.Slimed,1.075f),
				new(BuffID.Oiled,1.075f),
				new(ModContent.BuffType<SandyEffect>(),0.98f),
				new(ModContent.BuffType<AOScalding>(),1.1f),
				new(ModContent.BuffType<SearedEffect>(),1.1f)

			]
			);

		public override void LingeringEffects(Entity entity)
		{
			if (!Main.dedServ)
			{
				Dust.NewDust(entity.position, entity.width, entity.height, DustID.BlueTorch, Scale: 1.5f);
				Dust.NewDust(entity.position, entity.width, entity.height, DustID.YellowTorch, Scale: 1.5f);
			}
		}
		public override void SpawningEffects(Entity entity)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(entity.position, entity.width, entity.height, DustID.BlueTorch, entity.velocity.X * 2f, entity.velocity.Y * 2f, 0, default, 4f)];
				spawnedDust.noGravity = true;
				Dust spawnedDust2 = Main.dust[Dust.NewDust(entity.position, entity.width, entity.height, DustID.YellowTorch, 8f * (Main.rand.NextFloat() - 0.5f), 8f * (Main.rand.NextFloat() - 0.5f), 0, default, 4f)];
				spawnedDust2.noGravity = true;
			}
		}
		public override void ExplosionEffects(Entity projectile)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(projectile.Center, 0, 0, DustID.BlueFairy, (Main.rand.NextFloat() - 0.5f) * (22f * AOScrollSize), (Main.rand.NextFloat() - 0.5f) * (22f * AOScrollSize), 0, default, 5.5f)];
				spawnedDust.noGravity = true;
				Dust spawnedDust2 = Main.dust[Dust.NewDust(projectile.Center, 0, 0, DustID.YellowStarDust, (Main.rand.NextFloat() - 0.5f) * (22f * AOScrollSize), (Main.rand.NextFloat() - 0.5f) * (22f * AOScrollSize), 0, default, 5.5f)];
				spawnedDust2.noGravity = true;
			}
		}
		public override void KillEffects(Entity projectile)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.BlueTorch, 8f * (Main.rand.NextFloat() - 0.5f), 8f * (Main.rand.NextFloat() - 0.5f), 0, default, 5.5f)];
				spawnedDust.noGravity = true;
				Dust spawnedDust2 = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.YellowTorch, 8f * (Main.rand.NextFloat() - 0.5f), 8f * (Main.rand.NextFloat() - 0.5f), 0, default, 5.5f)];
				spawnedDust2.noGravity = true;
			}
			SoundEngine.PlaySound(ImbueSound, projectile.Center);
		}

		public override void AddRecipes()
		{
			CreateLostRecipe(typeof(FireMagic), typeof(PlasmaMagic),typeof(AshMagic),typeof(MagmaMagic),typeof(ExplosionMagic));
		}
	}
}
