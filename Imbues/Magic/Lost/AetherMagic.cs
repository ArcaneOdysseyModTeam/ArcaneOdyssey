using ArcaneOdyssey.Buffs.DOT;
using ArcaneOdyssey.Buffs.MagicMarks;
using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Imbues.Magic.Ancient;
using ArcaneOdyssey.Projectiles.Magic.Effects;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Imbues.Magic.Lost
{
	public class AetherMagic : AOMagic
	{
		public override void RegisterMutations()
		{
			RegisterMutation<IonMagic>();
		}
		
		public override float DashSpeed => 1.4f; // instant
		public override SoundStyle? ImbueSound => SoundID.Item9;
		public override Color ImbueColour => Color.Lerp(Color.LightYellow, Color.Yellow, Math.Abs(MathF.Tan(AOUtils.UpdateCount)));
		public override bool? Cold => false;
		public override bool CanBeWet => false;
		public override float AOScrollSpeed => 1.25f;
		public override float AOScrollSize => 1.2f;
		public override float AOScrollDamage => .95f;
		public override AOImbuableTier ImbuableTier => AOImbuableTier.Lost;
		public override Debuff[] ImbueDebuffs => [Debuff.Create<CharredEffect>(), Debuff.Create<BlindedEffect>(60 * 5)];
		public override SynergyEffects Effects => new(
			[ // these are debuffs cleared on hit
				ClearBuff.Create<FreezingEffect>(),
				ClearBuff.Create<SnowyEffect>(),
				ClearBuff.Create<Soaked>()
			],
			[
				Synergy.Create<AOBleed>(1.01f),
				Synergy.Create<AOBurning>(1.125f),
				Synergy.Create<Corroding>(1.075f),
				Synergy.Create<FreezingEffect>(1.01f),
				Synergy.Create<Melting>(1.075f),
				Synergy.Create<SnowyEffect>(0.99f),
				Synergy.Create<Scorched>(1.15f),
				Synergy.Create<Soaked>(0.99f),
				Synergy.Create<Flammable>(1.075f),
				Synergy.Create<SandyEffect>(0.99f),
				Synergy.Create<Scalding>(1.125f),
				Synergy.Create<SearedEffect>(1.15f),
				Synergy.Create<Crystallized>(1.075f),
				Synergy.Create<DrainedEffect>(0.8f)
			]
			);


		public override void SpawningEffects(Rectangle area, Vector2 direction)
		{
			if (Main.dedServ)
				return;
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.YellowStarDust, direction.X * 0.2f, direction.Y * 0.2f, Scale: 3f * area.RelativeScale())];
				spawnedDust.noGravity = true;
				Dust spawnedDust2 = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.YellowTorch, direction.X * 0.2f, direction.Y * 0.2f, Scale: 3f * area.RelativeScale())];
				spawnedDust2.noGravity = true;
			}
		}

		public override void LingeringEffects(Rectangle area, Vector2? direction = null, Entity source = null)
		{
			Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.YellowStarDust, Scale: area.RelativeScale());
			Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.YellowTorch, Scale: 2f * area.RelativeScale())];
			spawnedDust.noGravity = true;
			Lighting.AddLight(area.Center(), 2, 2, 0);
		}

		public override void ExplosionEffects(Vector2 position, float intensity = 1f)
		{
			//for (int n = 0; n < 3; n++)
			//{
			//	Dust spawnedDust = Main.dust[Dust.NewDust(position, 0, 0, DustID.YellowStarDust, (Main.rand.NextFloat() - 0.5f) * (25f * intensity * AOScrollSize), (Main.rand.NextFloat() - 0.5f) * (25f * intensity * AOScrollSize), Scale: 3f * intensity)];
			//	spawnedDust.noGravity = true;
			//	Dust spawnedDust2 = Main.dust[Dust.NewDust(position, 0, 0, DustID.YellowTorch, (Main.rand.NextFloat() - 0.5f) * (25f * intensity * AOScrollSize), (Main.rand.NextFloat() - 0.5f) * (25f * intensity * AOScrollSize), Scale: 3f * intensity)];
			//	spawnedDust2.noGravity = true;
			//}
			Projectile.NewProjectile(Item.GetSource_FromThis(), position, Vector2.Zero, ModContent.ProjectileType<AetherExplosion>(), 0, 0, ai0: 2f * intensity);
		}

		public override void KillEffects(Rectangle area, Entity source = null)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.YellowStarDust, 28f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 28f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), Scale: 3f * area.RelativeScale())];
				spawnedDust.noGravity = true;
				Dust spawnedDust2 = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.YellowTorch, 28f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 28f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), Scale: 3f * area.RelativeScale())];
				spawnedDust2.noGravity = true;
			}
			SoundEngine.PlaySound(ImbueSound, area.Center());
			if (source is Projectile projectile && projectile.ModProjectile is not AetherExplosion)
			{
				if (projectile.owner == Main.myPlayer && AetherExplosion.Count < 4)
				{
					Projectile.NewProjectile(projectile.GetSource_FromThis(), area.Center(), Vector2.Zero, ModContent.ProjectileType<AetherExplosion>(), projectile.damage / 4, 0, projectile.owner);
				}
			}
		}

		public override bool PreEffects(Entity entity = null)
		{
			if (entity is Projectile projectile)
				if (projectile.ModProjectile is AetherExplosion)
					return false;
			return base.PreEffects(entity);
		}
	}
}
