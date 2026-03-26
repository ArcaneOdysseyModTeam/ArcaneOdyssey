using ArcaneOdyssey.Buffs.DOT;
using ArcaneOdyssey.Buffs.MagicMarks;
using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Imbues.Magic.Lost;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace ArcaneOdyssey.Imbues.Magic.Normal
{
	public class GlassMagic : AOMagic
	{
		public override float Aura => .2f;
		public override void RegisterMutations()
		{
			RegisterMutation<PrismMagic>();
			RegisterMutation<SoundMagic>();
			RegisterMutation<SlashMagic>();
		}
		public override bool Special => true;
		public override float? DashResist => 1.05f;
		public override SoundStyle? ImbueSound => SoundID.Shatter;
		public override Color ImbueColour => new(255, 255, 255);
		public override float ImbueSpeed => 1f;
		public override float ImbueSize => 1.053f;
		public override float ImbueDamage => 1f;
		public override float ScrollSpeed => 1f;
		public override float ScrollSize => 1.1f;
		public override float ScrollDamage => 0.9f;
		public override Debuff[] ImbueDebuffs => [Debuff.Create<AOBleed>()];
		public override SynergyEffects Effects => new(
			[ // these are debuffs cleared on hit
			
			],
			[
				
				Synergy.Create<Corroding>(1.05f),
				Synergy.Create<Crystallized>(0.92f),
				Synergy.Create<FreezingEffect>(1.075f),
				Synergy.Create<SandyEffect>(1.1f),
				
				Synergy.Create<Melting>(1.05f),
			]
			);
		public override void SpawningEffects(Rectangle area, Vector2 direction)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Glass, direction.X * 0.4f, direction.Y * 0.4f, Scale: area.RelativeScale());
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
				Dust.NewDust(position, 0, 0, DustID.Glass, (Main.rand.NextFloat() - 0.5f) * (15f * intensity), (Main.rand.NextFloat() - 0.5f) * (15f * intensity), Scale: intensity);
			}
		}
		public override void KillEffects(Rectangle area, Entity source = null)
		{
			for (int n = 0; n < 30; n++)
			{
				Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Glass, 2f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 2f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), Scale: area.RelativeScale());
			}
			SoundEngine.PlaySound(ImbueSound, area.Center());
		}
	}
}