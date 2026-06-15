using ArcaneOdyssey.Buffs.DOT;
using ArcaneOdyssey.Buffs.MagicMarks;
using ArcaneOdyssey.Buffs.Stuns;
using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Imbues.Magic.Normal;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Imbues.Magic.Lost
{
	public class BlizzardMagic : MagicType
	{
		public override float Aura => .9f;
		public override float? DashResist => 1.075f;
		public override void SetStaticDefaults() { base.SetStaticDefaults(); ArcaneOdysseyMod.Sets.cold[Type] = true; }
		public override SoundStyle? ImbueSound => SoundID.Dig;
		public override Color ImbueColour => Color.DarkGray;
		public override Color ImbueColour2 => Color.White;
		public override ColourTransitionStyle TransitionStyle => ColourTransitionStyle.Tangent;
		public override ImbuableTiers ImbuableTier => ImbuableTiers.Lost;
		public override float ScrollSpeed => .925f;
		public override float ScrollSize => 1.15f;
		public override float ScrollDamage => 1f;
		public override Debuff[] ImbueDebuffs => [Debuff.Create<SnowyEffect>()];
		public override Combo[] CombinedDebuffs => [Combo.Create<Soaked, Frozen>(), Combo.Create<FreezingEffect, Frozen>()];
		public override SynergyEffects Effects => new(
			[ // these are debuffs cleared on hit
				ClearBuff.Create<Burning>(),
				ClearBuff.Create<CharredEffect>(),
				ClearBuff.Create<Corroding>(),
				ClearBuff.Create<Soaked>(),
				ClearBuff.Create<Flammable>(),
				ClearBuff.Create<FreezingEffect>(),
				ClearBuff.Create<Melting>(),
				ClearBuff.Create<Scorched>(),
				ClearBuff.Create<Scalding>(),
				ClearBuff.Create<SearedEffect>()
			],
			[
				Synergy.Create<Crystallized>(0.8f),
				Synergy.Create<Bleeding>(1.05f),
				Synergy.Create<Burning>(.9f),
				Synergy.Create<CharredEffect>(0.8f),
				Synergy.Create<Corroding>(.9f),
				Synergy.Create<FreezingEffect>(1.1f),
				Synergy.Create<Melting>(.9f),
				Synergy.Create<Scorched>(0.8f),
				Synergy.Create<Soaked>(1.1f),
				Synergy.Create<SearedEffect>(0.8f)
			]
			);

		public override int BlastFrames => 4;

		public override void SpawningEffects(Rectangle area, Vector2 direction)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Snow, direction.X * 2f, direction.Y * 2f, Scale: 3f * area.RelativeScale())];
				spawnedDust.noGravity = true;
			}
		}

		public override MagicCircleTypes CircleType => MagicCircleTypes.Ornamental;

		public override void LingeringEffects(Rectangle area, Vector2? direction = null, Entity source = null)
		{
			Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Snow, Scale: area.RelativeScale());
		}

		public override void ExplosionEffects(Vector2 position, float intensity = 1f)
		{
			for (int n = 0; n < 2; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(position, 0, 0, DustID.SnowBlock, (Main.rand.NextFloat() - 0.5f) * (15f * intensity), (Main.rand.NextFloat() - 0.5f) * (15f * intensity), Scale: 3f * intensity)];
				spawnedDust.noGravity = true;
			}
			Dust.NewDustDirect(position, 0, 0, DustID.Snow, (Main.rand.NextFloat() - 0.5f) * (15f * intensity), (Main.rand.NextFloat() - 0.5f) * (15f * intensity), Scale: 3f * intensity).noGravity = true;
		}

		public override void KillEffects(Rectangle area, Entity source = null)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.SnowBlock, 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), Scale: 3f * area.RelativeScale())];
				spawnedDust.noGravity = true;
			}
			SoundEngine.PlaySound(ImbueSound, area.Center());
		}

		public static Asset<Texture2D> trail;

		public override void AutoStaticDefaults()
		{
			base.AutoStaticDefaults();

			ModContent.RequestIfExists(Texture.Replace(Name, AttackPrefix + "Trail"), out trail);
		}

		public override void RegisterMutations()
		{
			RegisterDefaultMagic<SnowMagic>();
		}
	}
}
