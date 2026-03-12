using ArcaneOdyssey.Buffs.DOT;
using ArcaneOdyssey.Buffs.MagicMarks;
using ArcaneOdyssey.Buffs.Stuns;
using ArcaneOdyssey.Imbues.Base;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace ArcaneOdyssey.Imbues.Magic.Lost
{
	public class ThreadMagic : AOMagic
	{
		public override Color ImbueColour => Color.Lerp(Color.LightGray, Color.DarkGray, Math.Abs(MathF.Tan(AOUtils.UpdateCount)));
		public override SoundStyle? ImbueSound => SoundID.Grass;
		public override AOImbuableTier ImbuableTier => AOImbuableTier.Lost;

		public override float AOImbueDamage => .7f;
		public override float AOImbueSize => 1.15f;
		public override float AOImbueSpeed => .85f;

		public override Debuff[] ImbueDebuffs => [Debuff.Create<Tangled>(60 * 5)];

		public override SynergyEffects Effects => new([],
			[
				Synergy.Create<AOBurning>(.9f),
				 // burning
				Synergy.Create<CharredEffect>(.9f), // charred
				Synergy.Create<SearedEffect>(0.8f),
				
				Synergy.Create<Melting>(.95f),
				Synergy.Create<Scorched>( 0.8f),
			]);

		public override void SpawningEffects(Rectangle area, Vector2 direction)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Web, direction.X * 2f, direction.Y * 2f, Scale: 3f * area.RelativeScale())];
				spawnedDust.noGravity = true;
			}
		}

		public override void LingeringEffects(Rectangle area, Vector2? direction = null, Entity source = null)
		{
			Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Web, Scale: area.RelativeScale());
		}

		public override void ExplosionEffects(Vector2 position, float intensity = 1f)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(position, 0, 0, DustID.Web, (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize * intensity), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize * intensity), Scale: 3f * intensity)];
				spawnedDust.noGravity = true;
			}
		}

		public override void KillEffects(Rectangle area, Entity source = null)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Web, 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), Scale: 3f * area.RelativeScale())];
				spawnedDust.noGravity = true;
			}
			SoundEngine.PlaySound(ImbueSound, area.Center());
		}
	}
}
