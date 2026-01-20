using ArcaneOdyssey.Content.Buffs.DOT;
using ArcaneOdyssey.Content.Buffs.MagicMarks;
using ArcaneOdyssey.Content.Buffs.Stuns;
using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Imbues.Magic.Normal;
using ArcaneOdyssey.VFX.Dusts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Imbues.Magic.Lost
{
	public class FlareMagic : AOMagic
	{
		public override float DashSpeed => 1.2f; // burst
		public override Color ImbueColour => new(255, 0, 0);
		public override SoundStyle? ImbueSound => SoundID.Item20;
		public override bool? Cold => false;
		public override bool CanBeWet => false;
		public override float AOScrollSpeed => 1f;
		public override float AOScrollSize => 1.1f;
		public override float AOScrollDamage => .925f;

		public override AODebuffRequirement[] ImbueDebuffs => [new(ModContent.BuffType<Singed>(), 60 * 5)];
		public override AOImbuableTier ImbuableTier => AOImbuableTier.Lost;

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
				new(ModContent.BuffType<Singed>(), 1.1f),
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

		public override void SpawningEffects(Entity projectile)
		{
			for (int n = 0; n < 2; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, ModContent.DustType<FlareDust>(), projectile.velocity.X * 2f, projectile.velocity.Y * 2f, Alpha: (255 * .75f).Round())];
				spawnedDust.noGravity = true;
			}
		}

		public override void LingeringEffects(Entity projectile)
		{
			for (int n = 0; n < 2; n++)
			{
				var spawnedDust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, ModContent.DustType<FlareDust>(), Alpha: (255 * .75f).Round());
				spawnedDust.noGravity = true;
			}
		}

		public override void ExplosionEffects(Entity projectile)
		{
			for (int n = 0; n < 6; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(projectile.Center, 0, 0, ModContent.DustType<FlareDust>(), (Main.rand.NextFloat() - 0.5f) * (30f * AOScrollSize), (Main.rand.NextFloat() - 0.5f) * (30f * AOScrollSize), Alpha: (255 * .75f).Round())];
				spawnedDust.noGravity = true;
			}
		}

		public override void KillEffects(Entity projectile)
		{
			for (int n = 0; n < 20; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, ModContent.DustType<FlareDust>(), 8f * (Main.rand.NextFloat() - 0.5f), 8f * (Main.rand.NextFloat() - 0.5f), Alpha: (255 * .75f).Round())];
				spawnedDust.noGravity = true;
			}
			SoundEngine.PlaySound(ImbueSound, projectile.Center);
		}

		public override void AddRecipes()
		{
			CreateLostRecipe(typeof(FireMagic));
		}
	}
}