using ArcaneOdyssey.Buffs.DOT;
using ArcaneOdyssey.Buffs.MagicMarks;
using ArcaneOdyssey.Buffs.Stuns;
using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Projectiles.Magic.Effects;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Imbues.Magic.Lost
{
	public class FrostmetalMagic : AOMagic
	{
		public override float Aura => 1.3f;
		public override ImbuableTiers ImbuableTier => ImbuableTiers.Lost;
		public override bool? Cold => true;
		public override float? DashResist => 1.45f;
		public override SoundStyle? ImbueSound => SoundID.Item27;
		public override Color ImbueColour => new(65, 150, 177);
		public override Color ImbueColour2 => new(100, 100, 100);
		public override ColourTransitionStyle TransitionStyle => ColourTransitionStyle.Tangent;
		public override float ScrollSpeed => 0.65f;
		public override float ScrollSize => 1.2f;
		public override float ScrollDamage => 1.2f;
		public override Combo[] CombinedDebuffs => [Combo.Create<Soaked, AOFrozen>()];
		public override Debuff[] ImbueDebuffs => [Debuff.Create<AOBleed>(), Debuff.Create<FreezingEffect>()];
		public override SynergyEffects Effects => new(
			[ // these are debuffs cleared on hit
				ClearBuff.Create<Soaked>(),
				ClearBuff.Create<AOBurning>(),
				ClearBuff.Create<Corroding>(),
				ClearBuff.Create<Melting>(),
				ClearBuff.Create<Scorched>(),
				ClearBuff.Create<CharredEffect>()
			],
			[
				Synergy.Create<Corroding>(1.05f),
				Synergy.Create<Melting>(1.05f),
				Synergy.Create<SandyEffect>(1.1f),
				Synergy.Create<AOFrozen>(1.1f), // frozen
				Synergy.Create<Soaked>( 1.1f), // (add stunning later!)
				Synergy.Create<AOBurning>(.9f),
				Synergy.Create<Flammable>(1.03f),
				Synergy.Create<CharredEffect>(.9f), // charred
				Synergy.Create<Scorched>( 0.8f),
				Synergy.Create<SnowyEffect>(1.1f),
				Synergy.Create<Crystallized>(1.075f),
				Synergy.Create<SearedEffect>(0.8f)

			]
			);

		public override void SpawningEffects(Rectangle area, Vector2 direction)
		{
			if (Main.dedServ)
				return;
			for (int n = 0; n < 10; n++)
			{
				Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Mercury, direction.X * 0.4f, direction.Y * 0.4f, Scale: 2f * area.RelativeScale());
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.SnowflakeIce, direction.X * 0.5f, direction.Y * 0.5f, Scale: area.RelativeScale())];
				spawnedDust.noGravity = true;
				Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Ice, direction.X * 0.5f, direction.Y * 0.5f, Scale: area.RelativeScale());
			}
		}

		public override void LingeringEffects(Rectangle area, Vector2? direction = null, Entity source = null)
		{
			if (Main.dedServ)
				return;
			Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.SilverFlame, Scale: 2f * area.RelativeScale())];
			spawnedDust.noGravity = true;
			Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Ice, Scale: area.RelativeScale());
		}

		public override void ExplosionEffects(Vector2 position, float intensity = 1f)
		{
			if (Main.dedServ)
				return;
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(position, 0, 0, DustID.SnowflakeIce, (Main.rand.NextFloat() - 0.5f) * (15f * ScrollSize * intensity), (Main.rand.NextFloat() - 0.5f) * (15f * ScrollSize * intensity))];
				spawnedDust.noGravity = true;
				Dust.NewDust(position, 0, 0, DustID.Ice, (Main.rand.NextFloat() - 0.5f) * (15f * ScrollSize * intensity), (Main.rand.NextFloat() - 0.5f) * (15f * ScrollSize * intensity), Scale: intensity);
				Dust.NewDust(position, 0, 0, DustID.Mercury, (Main.rand.NextFloat() - 0.5f) * (15f * ScrollSize * intensity), (Main.rand.NextFloat() - 0.5f) * (15f * ScrollSize * intensity), Scale: 2f * intensity);
			}
		}

		public override bool PreEffects(Entity entity = null)
		{
			if (entity is Projectile projectile)
				if (projectile.ModProjectile is FrostmetalShard)
					return false;
			return base.PreEffects(entity);
		}

		public override void KillEffects(Rectangle area, Entity source = null)
		{
			if (Main.dedServ)
				return;
			if (source is Projectile projectile && Main.myPlayer == projectile.owner)
			{
				for (int i = 0; i < 3; i++)
				{
					var angle = Main.rand.NextFloat(MathHelper.TwoPi).ToRotationVector2() * 7f;
					angle.Y *= 0.35f;
					if (Main.LocalPlayer.ownedProjectileCounts[ModContent.ProjectileType<FrostmetalShard>()] < 3)
					{
						var proj = Projectile.NewProjectileDirect(projectile.GetSource_FromThis(), area.Center(), angle, ModContent.ProjectileType<FrostmetalShard>(), projectile.damage / 6, projectile.knockBack / 6, projectile.owner);
						proj.frame = i;
					}
				}
			}
			for (int n = 0; n < 15; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.SnowflakeIce, 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), Scale: area.RelativeScale())];
				spawnedDust.noGravity = true;
				Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Ice, 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), Scale: area.RelativeScale());
				Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Mercury, 2f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 2f * (Main.rand.NextFloat() - 0.5f), Scale: 2f * area.RelativeScale());
			}
			SoundEngine.PlaySound(ImbueSound, area.Center());
		}
	}
}