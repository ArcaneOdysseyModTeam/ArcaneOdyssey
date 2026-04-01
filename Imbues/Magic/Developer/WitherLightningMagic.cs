using ArcaneOdyssey.Imbues.Magic.Ancient;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;

namespace ArcaneOdyssey.Imbues.Magic.Developer
{
	public class WitherLightningMagic : DeathMagic
	{
		public override float ScrollDamage => base.ScrollDamage * 1.5f;
		public override float ScrollSpeed => base.ScrollSpeed * 1.5f;
		public override float ScrollSize => base.ScrollSize * 1.5f;

		public override ImbuableTiers ImbuableTier => ImbuableTiers.Developer;

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
	}
}
