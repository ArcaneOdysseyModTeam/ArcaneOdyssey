using ArcaneOdyssey.Buffs.DOT;
using ArcaneOdyssey.Buffs.MagicMarks;
using ArcaneOdyssey.Buffs.Stuns;
using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Imbues.Magic.Normal;
using ArcaneOdyssey.Projectiles.Magic.Effects;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;


namespace ArcaneOdyssey.Imbues.Magic.Lost
{
	public class PoisonLightningMagic : MagicType
	{
		public override float Aura => .8f;
		public override bool ImmuneDash => true; // instant
		public override SoundStyle? ImbueSound => SoundID.DD2_LightningBugZap with { Volume = 2.25f };
		public override Color ImbueColour => Color.Purple;
		public override Color ImbueColour2 => new(105, 0, 105, 255);
		public override ColourTransitionStyle TransitionStyle => ColourTransitionStyle.Smooth;
		public override float ImbueSize => 1.15f;
		public override float ImbueDamage => 0.9f;
		public override float ScrollSpeed => 1.4f;
		public override ImbuableTiers ImbuableTier => ImbuableTiers.Lost;
		public override Debuff[] ImbueDebuffs => [Debuff.Create<ElectrifiedToxins>(), Debuff.Create<Paralyzed>(60, 25)];
		public override Combo[] CombinedDebuffs => [Combo.Create<Soaked, Paralyzed>()];
		public override SynergyEffects Effects => new(
			[ // these are debuffs cleared on hit
				ClearBuff.Create<Petrified>(), // petrified
				ClearBuff.Create<CharredEffect>(),
				ClearBuff.Create<SandyEffect>(),
				ClearBuff.Create<Bleeding>(),
				ClearBuff.Create<Frozen>()
			],
			[
				Synergy.Create<Bleeding>(1.075f),

				Synergy.Create<Burning>(.99f),
				Synergy.Create<Scalding>(0.9f),
				Synergy.Create<FreezingEffect>( 1.2f), // frozen
				Synergy.Create<Bleeding>(1.2f), // bleeding
				 // scalding
				 // melting/hellfire
				Synergy.Create<Melting>(1.075f),
				 // venom acid
				Synergy.Create<Corroding>(1.075f),
				Synergy.Create<Soaked>( 1.05f), //
				Synergy.Create<Flammable>(0.98f),
				Synergy.Create<Scorched>(1.15f),
				Synergy.Create<Crystallized>(1.075f),
				Synergy.Create<SearedEffect>(1.15f)
			]
			);

		public override MagicCircleTypes CircleType => MagicCircleTypes.Draconic;

		public override int BlastFrames => 6;

		public override void SpawningEffects(Rectangle area, Vector2 direction)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Cloud, direction.X * 0.4f, direction.Y * 0.4f, 0, Color.Purple, 3f * area.RelativeScale())];
				spawnedDust.noGravity = true;
				Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.WitherLightning, direction.X * 0.2f, direction.Y * 0.2f, Scale: 1.2f * area.RelativeScale());
			}
		}

		public override void LingeringEffects(Rectangle area, Vector2? direction = null, Entity source = null)
		{
			Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Cloud, 0f, 0f, 0, Color.Purple, 2f * area.RelativeScale())];
			spawnedDust.noGravity = true;
			float waveVal = 10f * MathF.Abs((float)Main.GameUpdateCount % 5 % 10f - 2.5f) - 12.5f;
			if (source is Projectile projectile && projectile.extraUpdates > 0)
			{
				waveVal = 10f * MathF.Abs(((float)Main.GameUpdateCount + (float)projectile.numUpdates) % 5 % 10f - 2.5f) - 12.5f;
			}
			Vector2 baseVec = new(0f, waveVal);
			Dust spawnedDust2 = Dust.NewDustPerfect(area.Center() + baseVec.RotatedBy(direction.GetValueOrDefault(Vector2.One).ToRotation()), DustID.CrystalPulse, Vector2.Zero, Scale: 1.2f);
			spawnedDust2.noGravity = true;
			Lighting.AddLight(area.Center(), 2, 1, 2);
		}
		public override void ExplosionEffects(Vector2 position, float intensity = 1f)
		{
			Projectile.NewProjectile(Item.GetSource_FromThis(), position, Vector2.Zero, ModContent.ProjectileType<LightningBurst>(), 0, 0, ai0: intensity * 1.6f);
		}
		public override void KillEffects(Rectangle area, Entity source = null)
		{
			for (int n = 0; n < 15; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Cloud, 5f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 5f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 0, Color.Purple, 3f * area.RelativeScale())];
				spawnedDust.noGravity = true;
				Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.WitherLightning, 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), Scale: 1.2f * area.RelativeScale());
			}
			if (source is Projectile projectile && Main.myPlayer == projectile.owner)
			{
				var proj = Projectile.NewProjectileDirect(projectile.GetSource_FromThis(), new(area.X + area.Width * Main.rand.NextFloat(), area.Y + area.Height * Main.rand.NextFloat()), Vector2.Zero, ModContent.ProjectileType<PoisonCloud>(), 15 * (AOUtils.BossesKilled + 1), 0f);
				proj.scale *= projectile.Hitbox.RelativeScale(max: 2f);
				proj.Hitbox = proj.Hitbox.Scaled(projectile.Hitbox.RelativeScale(max: 2f));
				proj.netUpdate = true;
				Projectile.NewProjectile(source.GetSource_FromThis(), area.Center(), Vector2.Zero, ModContent.ProjectileType<LightningBurst>(), 0, 0, proj.owner, ai0: area.RelativeScale(AetherExplosion.SpriteSize) * 1.5f);
			}
			SoundEngine.PlaySound(ImbueSound, area.Center());
		}

		public override void RegisterMutations()
		{
			RegisterDefaultMagic<PoisonMagic>();
		}
	}
}