using ArcaneOdyssey.Content.Buffs.DOT;
using ArcaneOdyssey.Content.Buffs.MagicMarks;
using ArcaneOdyssey.Content.Buffs.Stuns;
using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Imbues.Magic.Normal;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Imbues.Magic.Lost
{
	public class BlizzardMagic : AOMagic
	{
		public override float? DashResist => 1.075f;
		public override bool? Cold => true;
		public override SoundStyle? ImbueSound => SoundID.Dig;
		public override Color ImbueColour => new(255, 255, 255, 255);
		public override AOImbuableTier ImbuableTier => AOImbuableTier.Lost;
		public override float AOImbueSpeed => .925f;
		public override float AOImbueSize => 1.15f;
		public override float AOImbueDamage => 1f;
		public override AODebuffRequirement[] ImbueDebuffs => [new(ModContent.BuffType<SnowyEffect>(), 60 * 10)];
		public override CombinedDebuff[] CombinedDebuffs => [new(BuffID.Wet, ModContent.BuffType<AOFrozen>()), new(ModContent.BuffType<FreezingEffect>(), ModContent.BuffType<AOFrozen>())];
		public override SynergyEffects Effects => new(
			[ // these are debuffs cleared on hit
				BuffID.OnFire,
				ModContent.BuffType<CharredEffect>(),
				BuffID.Venom,
				BuffID.Wet,
				BuffID.Oiled,
				ModContent.BuffType<FreezingEffect>(),
				BuffID.OnFire3,
				BuffID.ShadowFlame,
				ModContent.BuffType<AOScalding>(),
				ModContent.BuffType<SearedEffect>()
			],
			[
				new(ModContent.BuffType<Crystallized>(),0.8f),
				new(ModContent.BuffType<AOBleed>(),1.05f),
				new(BuffID.OnFire,0.90f),
				new(ModContent.BuffType<CharredEffect>(),0.8f),
				new(BuffID.Venom,0.9f),
				new(ModContent.BuffType<FreezingEffect>(),1.1f),
				new(BuffID.OnFire3,0.9f),
				new(BuffID.ShadowFlame,0.8f),
				new(BuffID.Wet,1.1f),
				new(ModContent.BuffType<SearedEffect>(),0.8f)
			]
			);

		public override void SpawningEffects(Rectangle area, Vector2 direction)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Snow, direction.X * 2f, direction.Y * 2f, Scale: 3f * area.RelativeScale())];
				spawnedDust.noGravity = true;
			}
		}

		public override void LingeringEffects(Rectangle area, Vector2? direction = null, Entity source = null)
		{
			Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Snow, Scale: area.RelativeScale())];
		}

		public override void ExplosionEffects(Vector2 position, float intensity = 1f)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(position, 0, 0, DustID.SnowBlock, (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize * intensity), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize * intensity), Scale: 3f * intensity)];
				spawnedDust.noGravity = true;
			}
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
	}
}
