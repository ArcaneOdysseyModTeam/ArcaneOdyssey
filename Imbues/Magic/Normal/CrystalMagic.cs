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
	public class CrystalMagic : MagicType
	{
		public override float Aura => 1.2f;
		public override void RegisterMutations()
		{
			RegisterMutation<DiamondMagic>();
			RegisterMutation<PrismMagic>();
		}
		public override bool Special => true;
		public override float? DashResist => 1.3f;
		public override Color ImbueColour => new(255, 0, 0);
		
		
		
		public override float ScrollSpeed => 0.9f;
		public override float ScrollSize => 1.15f;
		public override float ScrollDamage => 1.05f;
		public override SoundStyle? ImbueSound => SoundID.Shatter;
		public override Debuff[] ImbueDebuffs => [Debuff.Create<Crystallized>(60 * 5)];
		public override Combo[] CombinedDebuffs => [];
		public override SynergyEffects Effects => new(
			[ // these are debuffs cleared on hit
			
			],
			[
				Synergy.Create<FreezingEffect>(1.01f),
				Synergy.Create<Bleeding>(1.01f),

				Synergy.Create<Corroding>(1.01f),

				Synergy.Create<Melting>(1.075f),
				Synergy.Create<SandyEffect>(1.125f)
			]
			);

		public override int BlastFrames => 8;

		public override MagicCircleTypes CircleType => MagicCircleTypes.Ornamental;

		public override void SpawningEffects(Rectangle area, Vector2 direction)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.GemRuby, direction.X * 0.4f, direction.Y * 0.4f, Scale: area.RelativeScale());
			}
		}

		public override void LingeringEffects(Rectangle area, Vector2? direction = null, Entity source = null)
		{
			Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.GemRuby, Scale: area.RelativeScale())];
			spawnedDust.noGravity = true;
			spawnedDust.noLight = true;
		}

		public override void ExplosionEffects(Vector2 position, float intensity = 1f)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust.NewDustDirect(position, 0, 0, DustID.GemRuby, (Main.rand.NextFloat() - 0.5f) * (18f * intensity), (Main.rand.NextFloat() - 0.5f) * (18f * intensity), Scale: 2f * intensity).noGravity = true;
			}
		}

		public override void KillEffects(Rectangle area, Entity source = null)
		{
			for (int n = 0; n < 30; n++)
			{
				Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.GemRuby, 2f * (Main.rand.NextFloat() - 0.5f), 2f * (Main.rand.NextFloat() - 0.5f), Scale: area.RelativeScale());
			}
			SoundEngine.PlaySound(ImbueSound, area.Center());
		}
	}
}