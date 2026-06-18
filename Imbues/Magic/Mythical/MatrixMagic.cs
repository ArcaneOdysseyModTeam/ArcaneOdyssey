using ArcaneOdyssey.Imbues.Base;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace ArcaneOdyssey.Imbues.Magic.Mythical
{
	public sealed class MatrixMagic : MagicType
	{
		public static float MatrixSize = 1, MatrixSpeed = 1, MatrixDamage = 1;
		public override bool ImmuneDash => true; // instant
		public override SoundStyle? ImbueSound => SoundID.DD2_LightningBugZap with { Volume = 2.25f };
		public override Color ImbueColour => Color.Turquoise;
		public override Color ImbueColour2 => Color.White;
		public override bool AnimatedColours => true;
		public override ImbuableTiers ImbuableTier => ImbuableTiers.Mythical;
		public override float ScrollSpeed => MatrixSpeed;
		public override float ScrollSize => MatrixSize;
		public override float ScrollDamage => MatrixDamage;
		public static Debuff[] MatrixDebuffs = [];
		public static Combo[] MatrixCombos = [];
		public override Debuff[] ImbueDebuffs => MatrixDebuffs;
		public override Combo[] CombinedDebuffs => MatrixCombos;
		public static SynergyEffects MatrixEffects = new();
		public static ImbueGimmick MatrixGimmick = null;
		public override ImbueGimmick Gimmick => MatrixGimmick is not BarGimmick ? MatrixGimmick : null;
		public override int BlastFrames => 6;

		public override SynergyEffects Effects => MatrixEffects;

		public override void SpawningEffects(Rectangle area, Vector2 direction)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.UltraBrightTorch, direction.X * 0.2f, direction.Y * 0.2f, Scale: 1.2f * area.RelativeScale());
			}
		}

		public override void LingeringEffects(Rectangle area, Vector2? direction = null, Entity source = null)
		{// WAHT IS  THIS IM SO CONFUSED
			float waveVal = 10f * MathF.Abs((float)Main.GameUpdateCount % 5 % 10f - 2.5f) - 12.5f;
			if (source is Projectile projectile && projectile.extraUpdates > 0)
			{
				waveVal = 10f * MathF.Abs(((float)Main.GameUpdateCount + (float)projectile.numUpdates) % 5 % 10f - 2.5f) - 12.5f;
			}
			Vector2 baseVec = new(0f, waveVal);
			Dust spawnedDust = Dust.NewDustPerfect(area.Center() + baseVec.RotatedBy(direction.GetValueOrDefault(Vector2.One).ToRotation()), DustID.Vortex, Vector2.Zero, Scale: 1.2f);
			spawnedDust.noGravity = true;
			Lighting.AddLight(area.Center(), 2, 0, 0);
			Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.UltraBrightTorch, Scale: .4f * area.RelativeScale());
		}

		public override void ExplosionEffects(Vector2 position, float intensity = 1f)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust dust = Dust.NewDustDirect(position, 0, 0, DustID.Firework_Green, (Main.rand.NextFloat() - 0.5f) * (13f * intensity), (Main.rand.NextFloat() - 0.5f) * (13f * intensity), Scale: 2.3f * intensity);
				dust.noGravity = true;
			}
		}

		public override void KillEffects(Rectangle area, Entity source = null)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.UltraBrightTorch, 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), Scale: 2.5f * area.RelativeScale());
			}
			SoundEngine.PlaySound(ImbueSound, area.Center());
		}

		public override void RegisterMutations()
		{
			
		}

		public override MagicCircleTypes CircleType => MagicCircleTypes.Reminiscent;
	}
}
