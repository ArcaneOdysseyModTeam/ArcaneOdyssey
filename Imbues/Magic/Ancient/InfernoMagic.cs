using ArcaneOdyssey.Buffs.DOT;
using ArcaneOdyssey.Buffs.MagicMarks;
using ArcaneOdyssey.Buffs.Stuns;
using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Imbues.Magic.Lost;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;


namespace ArcaneOdyssey.Imbues.Magic.Ancient
{
	public class InfernoMagic : MagicType
	{
		public override void RegisterMutations()
		{
			RegisterDefaultMagic<FlareMagic>();
		}
		
		public override float DashSpeed => 1.2f; // burst
		public override void SetStaticDefaults() { base.SetStaticDefaults(); ArcaneOdysseyMod.Sets.cold[Type] = false; }
		public override SoundStyle? ImbueSound => SoundID.Item20;
		public override Color ImbueColour => Color.LightBlue;
		public override Color ImbueColour2 => Color.Blue;
		public override ColourTransitionStyle TransitionStyle => ColourTransitionStyle.Tangent;
		public override ImbuableTiers ImbuableTier => ImbuableTiers.Ancient;
		public override bool CanBeWet => false;
		public override float ImbueSpeed => 1f;
		public override float ImbueSize => 1.15f;
		public override float ImbueDamage => .85f;

		public override MagicCircleTypes CircleType => MagicCircleTypes.Solar;
		public override Debuff[] ImbueDebuffs => [Debuff.Create<VesuvianBurn>()]; // create custom debuff later
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
			if (Main.dedServ)
				return;
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.FireworkFountain_Blue, direction.X * 2f, direction.Y * 2f, Scale: area.RelativeScale())];
				spawnedDust.noGravity = true;
				Dust spawnedDust2 = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.BlueTorch, 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), Scale: 2.4f * area.RelativeScale())];
				spawnedDust2.noGravity = true;
			}
		}

		public override void LingeringEffects(Rectangle area, Vector2? direction = null, Entity source = null)
		{
			if (Main.dedServ)
				return;
			Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Shadowflame, Scale: 1.6f * area.RelativeScale());
			Dust spawnedDust = Dust.NewDustDirect(area.TopLeft(), area.Width, area.Height, DustID.FireworkFountain_Blue, Scale: 0.8f * area.RelativeScale());
			spawnedDust.noGravity = true;
		}
		public override void ExplosionEffects(Vector2 position, float intensity = 1f)
		{
			if (Main.dedServ)
				return;
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(position, 0, 0, DustID.FireworkFountain_Blue, (Main.rand.NextFloat() - 0.5f) * (15f * intensity), (Main.rand.NextFloat() - 0.5f) * (15f * intensity), Scale: 1.3f * intensity)];
				spawnedDust.noGravity = true;
				Dust spawnedDust2 = Main.dust[Dust.NewDust(position, 0, 0, DustID.BlueTorch, (Main.rand.NextFloat() - 0.5f) * (15f * intensity), (Main.rand.NextFloat() - 0.5f) * (15f * intensity), Scale: 2.8f * intensity)];
				spawnedDust2.noGravity = true;
			}
		}
		public override void KillEffects(Rectangle area, Entity source = null)
		{
			if (Main.dedServ)
				return;
			for (int n = 0; n < 10; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.FireworkFountain_Blue, 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), Scale: 2f * area.RelativeScale())];
				spawnedDust.noGravity = true;
				Dust spawnedDust2 = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.BlueTorch, 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), Scale: 2.8f * area.RelativeScale())];
				spawnedDust2.noGravity = true;
			}
			SoundEngine.PlaySound(ImbueSound, area.Center());
		}
	}
}