using ArcaneOdyssey.Buffs.DOT;
using ArcaneOdyssey.Buffs.MagicMarks;
using ArcaneOdyssey.Imbues.Base;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace ArcaneOdyssey.Imbues.Magic.Lost
{
	public class DiamondMagic : AOMagic
	{
		public override float Aura => 1.5f;
		public override float? DashResist => 1.6f;
		public override float AOScrollSpeed => .65f;
		public override float AOScrollSize => 1.2f;
		public override float AOScrollDamage => 1.2f;
		public override Color ImbueColour => Color.Lerp(new(0, 210, 217), new(158, 252, 255), Math.Abs(MathF.Sin(AOUtils.UpdateCount)));
		public override AOImbuableTier ImbuableTier => AOImbuableTier.Lost;
		public override SoundStyle? ImbueSound => SoundID.Shatter;
		public override Debuff[] ImbueDebuffs => [Debuff.Create<AOBleed>()];
		public override SynergyEffects Effects => new(
			[ // these are debuffs cleared on hit
			
			],
			[
				Synergy.Create<FreezingEffect>(1.01f),
				Synergy.Create<AOBleed>(1.01f),
				Synergy.Create<Corroding>(1.01f),
				Synergy.Create<Melting>(1.075f),
				Synergy.Create<SandyEffect>(1.125f),
				Synergy.Create<Crystallized>(1.125f)
			]
			);
		public override void SpawningEffects(Rectangle area, Vector2 direction)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.GemSapphire, direction.X * 0.4f, direction.Y * 0.4f, Scale: area.RelativeScale());
			}
		}

		public override void LingeringEffects(Rectangle area, Vector2? direction = null, Entity source = null)
		{
			Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.SilverFlame, Scale: area.RelativeScale())];
			spawnedDust.noGravity = true;
			spawnedDust.noLight = true;
		}
		public override void ExplosionEffects(Vector2 position, float intensity = 1f)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust.NewDust(position, 0, 0, DustID.GemSapphire, (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize * intensity), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize * intensity), Scale: 3f * intensity);
			}
		}

		public override void KillEffects(Rectangle area, Entity source = null)
		{
			for (int n = 0; n < 30; n++)
			{
				Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.GemSapphire, 2f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 2f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), Scale: area.RelativeScale());
			}
			SoundEngine.PlaySound(ImbueSound, area.Center());
		}
	}
}