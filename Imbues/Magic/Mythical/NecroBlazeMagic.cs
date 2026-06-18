using ArcaneOdyssey.Buffs.DOT;
using ArcaneOdyssey.Buffs.MagicMarks;
using ArcaneOdyssey.Buffs.Stuns;
using ArcaneOdyssey.Imbues.Base;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;


namespace ArcaneOdyssey.Imbues.Magic.Mythical
{
	public class NecroBlazeMagic : MagicType
	{
		public override float DashSpeed => 1.2f; // burst
		public override void SetStaticDefaults() 
		{ 
			base.SetStaticDefaults(); 
			ArcaneOdysseyMod.Sets.cold[Type] = false; 
		}
		public override SoundStyle? ImbueSound => SoundID.Item20;
		public override MagicCircleTypes CircleType => MagicCircleTypes.Solar;
		public override Color ImbueColour => Color.Black;
		public override Color ImbueColour2 => new(0, 200, 150);
		public override bool AnimatedColours => true;
		public override ImbuableTiers ImbuableTier => ImbuableTiers.Mythical;
		public override bool CanBeWet => false;
		public override float ScrollSpeed => 1f;
		public override float ScrollSize => 1.15f;
		public override bool Special => true;
		public override float ScrollDamage => .85f;

		public override Debuff[] ImbueDebuffs => [Debuff.Create<NecroFlame>()];
		public override Combo[] CombinedDebuffs => [Combo.Create<CharredEffect, Petrified>()];
		public override SynergyEffects Effects => new(
			[ // these are debuffs cleared on hit
				ClearBuff.Create<Bleeding>(),
				ClearBuff.Create<Soaked>(),
				ClearBuff.Create<FreezingEffect>(),
				ClearBuff.Create<SnowyEffect>(),
				ClearBuff.Create<CharredEffect>()
			],
			[
				Synergy.Create<Bleeding>(1.15f),
				Synergy.Create<CharredEffect>(1.01f),

				Synergy.Create<Corroding>(1.05f),
				Synergy.Create<Crystallized>(0.85f),
				Synergy.Create<FreezingEffect>(0.99f),
				Synergy.Create<SnowyEffect>(0.99f),
				Synergy.Create<Soaked>(0.99f),

				Synergy.Create<Melting>(1.05f),

				Synergy.Create<Poisoned>(1.05f),

				Synergy.Create<Burning>(1.1f),

				Synergy.Create<Flammable>(1.075f),
				Synergy.Create<SandyEffect>(0.98f),
				Synergy.Create<Scalding>(1.1f),
				Synergy.Create<SearedEffect>(1.1f)

			]
			);

		public override int BlastFrames => 3;

		public override void SpawningEffects(Rectangle area, Vector2 direction)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Wraith, direction.X * 0.5f, direction.Y * 0.5f, Scale: 1f * area.RelativeScale())];
				spawnedDust.noGravity = true;
				Dust spawnedDust2 = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Vortex, direction.X * 0.5f, direction.Y * 0.5f, Scale: 1.6f * area.RelativeScale())];
				spawnedDust2.noGravity = true;
			}
		}

		public override void LingeringEffects(Rectangle area, Vector2? direction = null, Entity source = null)
		{
			Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Wraith, Scale: 1.3f)];
			spawnedDust.noGravity = true;
			Dust spawnedDust2 = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Vortex, Scale: 2f)];
			spawnedDust2.noGravity = true;
		}

		public override void ExplosionEffects(Vector2 position, float intensity = 1f)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(position, 0, 0, DustID.Wraith, (Main.rand.NextFloat() - 0.5f) * (15f * intensity), (Main.rand.NextFloat() - 0.5f) * (15f * intensity), Scale: 2f * intensity)];
				spawnedDust.noGravity = true;
				Dust spawnedDust2 = Main.dust[Dust.NewDust(position, 0, 0, DustID.Vortex, (Main.rand.NextFloat() - 0.5f) * (15f * intensity), (Main.rand.NextFloat() - 0.5f) * (15f * intensity), Scale: 3f * intensity)];
				spawnedDust2.noGravity = true;
			}
		}

		public override void KillEffects(Rectangle area, Entity source = null)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Wraith, 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), Scale: 2f * area.RelativeScale())];
				spawnedDust.noGravity = true;
				Dust spawnedDust2 = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Vortex, 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), Scale: 4f * area.RelativeScale())];
				spawnedDust2.noGravity = true;
			}
			SoundEngine.PlaySound(ImbueSound, area.Center());
		}

		public override void RegisterMutations()
		{

		}
}
}