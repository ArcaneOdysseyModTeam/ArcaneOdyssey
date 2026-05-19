using ArcaneOdyssey.Buffs.DOT;
using ArcaneOdyssey.Buffs.MagicMarks;
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
	public class PrismMagic : MagicType
	{
		public override float Aura => .3f;
		public override float? DashResist => 1.15f;

		internal static readonly Color[] rainbowColors = [new Color(255, 71, 124), new Color(94, 61, 255), new Color(87, 219, 255), new Color(100, 255, 93)];

		public override SoundStyle? ImbueSound => SoundID.Shatter;

		public override Color ImbueColour => new(217, 0, 255);
		public override Color ImbueColour2 => new(0, 196, 52);
		public override ColourTransitionStyle TransitionStyle => ColourTransitionStyle.Smooth;

		public override float ScrollSpeed => 1.1f;
		public override float ScrollDamage => 1.125f;
		public override float ScrollSize => 1.15f;

		public override ImbuableTiers ImbuableTier => ImbuableTiers.Lost;
		public override Debuff[] ImbueDebuffs => [Debuff.Create<BlindedEffect>(60 * 3), Debuff.Create<Bleeding>()];

		public override SynergyEffects Effects => new(
			[ // these are debuffs cleared on hit
			
			],
			[
				Synergy.Create<Crystallized>(1.075f),
				Synergy.Create<DrainedEffect>(0.8f),

				Synergy.Create<Corroding>(1.05f),
				Synergy.Create<FreezingEffect>(1.075f),
				Synergy.Create<SandyEffect>(1.1f),

				Synergy.Create<Melting>(1.05f),
			]
			);

		public override MagicCircleTypes CircleType => MagicCircleTypes.Penta;

		public override int BlastFrames => 7;

		public override void SpawningEffects(Rectangle area, Vector2 direction)
		{
			if (Main.dedServ)
				return;
			int rainbowStep = (int)Main.GameUpdateCount;
			for (int n = 0; n < 3; n++)
			{
				Dust dust = Dust.NewDustDirect(area.TopLeft(), area.Width, area.Height, DustID.AncientLight, (Main.rand.NextFloat() - 0.5f) * 3f * area.RelativeScale(), (Main.rand.NextFloat() - 0.5f) * 3f * area.RelativeScale(), 0, rainbowColors[rainbowStep % 3], area.RelativeScale());
				dust.noGravity = true;
				rainbowStep++;
				Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Glass, Scale: area.RelativeScale());
			}
		}

		public override void LingeringEffects(Rectangle area, Vector2? direction = null, Entity source = null)
		{
			if (Main.dedServ)
				return;
			Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Glass, Scale: 0.5f * area.RelativeScale());
			if (source is Projectile projectile)
			{
				if (projectile.extraUpdates > 0)
				{
					Dust dust = Dust.NewDustDirect(area.TopLeft(), area.Width, area.Height, DustID.AncientLight, (Main.rand.NextFloat() - 0.5f) * 3f * area.RelativeScale(), (Main.rand.NextFloat() - 0.5f) * 3f * area.RelativeScale(), 0, rainbowColors[Math.Abs(projectile.numUpdates + Main.GameUpdateCount)/*Prevents issues with -1 updates, and also makes sure all colors are shown*/ % 3], 1.4f * area.RelativeScale());
					dust.noGravity = true;
				}
				else
				{
					Dust dust = Dust.NewDustDirect(area.TopLeft(), area.Width, area.Height, DustID.AncientLight, (Main.rand.NextFloat() - 0.5f) * 3f * area.RelativeScale(), (Main.rand.NextFloat() - 0.5f) * 3f * area.RelativeScale(), 0, rainbowColors[Main.GameUpdateCount % 3], 1.4f * area.RelativeScale());
					dust.noGravity = true;
				}
			}
			else
			{
				Dust dust = Dust.NewDustDirect(area.TopLeft(), area.Width, area.Height, DustID.AncientLight, (Main.rand.NextFloat() - 0.5f) * 3f * area.RelativeScale(), (Main.rand.NextFloat() - 0.5f) * 3f * area.RelativeScale(), 0, rainbowColors[Main.GameUpdateCount % 3], 1.4f * area.RelativeScale());
				dust.noGravity = true;
			}
		}

		public override void KillEffects(Rectangle area, Entity source = null)
		{
			if (Main.dedServ)
				return;
			int rainbowStep = (int)Main.GameUpdateCount;
			for (int n = 0; n < 10; n++)
			{
				Dust dust = Dust.NewDustDirect(area.TopLeft(), area.Width, area.Height, DustID.AncientLight, (Main.rand.NextFloat() - 0.5f) * 3f * area.RelativeScale(), (Main.rand.NextFloat() - 0.5f) * 3f * area.RelativeScale(), 0, rainbowColors[rainbowStep % 3], 2f * area.RelativeScale());
				dust.noGravity = true;
				rainbowStep++;
				Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Glass, Scale: 1.2f * area.RelativeScale());
			}
			SoundEngine.PlaySound(ImbueSound, area.Center());
			if (source is Projectile projectile && projectile.owner == Main.myPlayer && projectile.GetOwner().ownedProjectileCounts[ModContent.ProjectileType<PrismLinger>()] < 3)
				Projectile.NewProjectile(projectile.GetSource_FromThis(), area.Center(), Vector2.Zero, ModContent.ProjectileType<PrismLinger>(), projectile.damage / 6, 0, projectile.owner);
		}

		public override bool PreEffects(Entity entity = null)
		{
			if (entity is Projectile projectile)
				if (projectile.ModProjectile is PrismLinger)
					return false;
			return base.PreEffects(entity);
		}

		public override void ExplosionEffects(Vector2 position, float intensity = 1f)
		{
			if (Main.dedServ)
				return;
			int rainbowStep = (int)Main.GameUpdateCount;
			Dust.NewDustDirect(position, 0, 0, DustID.Glass, (Main.rand.NextFloat() - 0.5f) * (20f * intensity), (Main.rand.NextFloat() - 0.5f) * (20f * intensity), Scale: 1.5f * intensity).noGravity = true;
			for (int n = 0; n < 10; n++)
			{
				Dust dust = Dust.NewDustDirect(position, 0, 0, DustID.AncientLight, (Main.rand.NextFloat() - 0.5f) * (25f * intensity), (Main.rand.NextFloat() - 0.5f) * (25f * intensity), 0, rainbowColors[rainbowStep % 3], 2f * intensity);
				dust.noGravity = true;
				rainbowStep++;
			}
		}

		public override void RegisterMutations()
		{
			RegisterDefaultMagic<GlassMagic>();
		}
	}
}