using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Imbues.Gimmicks;
using ArcaneOdyssey.Imbues.Magic.Ancient;
using ArcaneOdyssey.Imbues.Magic.Lost;
using ArcaneOdyssey.Projectiles.Magic.Effects;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Imbues.Magic.Mythical
{
	public class WitherLightningMagic : DeathMagic
	{
		public override ImbueGimmick Gimmick => ModContent.GetInstance<InstantDeath>();
		public override float ScrollDamage => base.ScrollDamage * 1.5f;
		public override float ScrollSpeed => base.ScrollSpeed * 2.5f;
		public override float ScrollSize => base.ScrollSize * 1.5f;

		public override ImbuableTiers ImbuableTier => ImbuableTiers.Mythical;

		public override SynergyEffects Effects => base.Effects + AOUtils.CopySynergiesFromImbue<AncientLightningMagic>();

		public override int BlastFrames => 6;

		public override void LingeringEffects(Rectangle area, Vector2? direction = null, Entity source = null)
		{
			base.LingeringEffects(area, direction, source);

			float waveVal = 10f * MathF.Abs((float)Main.GameUpdateCount % 5 % 10f - 2.5f) - 12.5f;
			if (source is Projectile projectile && projectile.extraUpdates > 0)
			{
				waveVal = 10f * MathF.Abs(((float)Main.GameUpdateCount + (float)projectile.numUpdates) % 5 % 10f - 2.5f) - 12.5f;
			}
			Vector2 baseVec = new(0f, waveVal);
			Dust spawnedDust = Dust.NewDustPerfect(area.Center() + baseVec.RotatedBy(direction.GetValueOrDefault(Vector2.One).ToRotation()), DustID.Vortex, Vector2.Zero, Scale: 2f);
			spawnedDust.noGravity = true;
		}

		public override void KillEffects(Rectangle area, Entity source = null)
		{
			if (source is Projectile proj && Main.myPlayer == proj.owner)
				Projectile.NewProjectile(source.GetSource_FromThis(), area.Center(), Vector2.Zero, ModContent.ProjectileType<LightningBurst>(), 0, 0, proj.owner, ai0: area.RelativeScale(AetherExplosion.SpriteSize) * 1.5f);
		}

		public override void RegisterMutations()
		{

		}

		public override MagicCircleTypes CircleType => MagicCircleTypes.Tesla;
	}
}
