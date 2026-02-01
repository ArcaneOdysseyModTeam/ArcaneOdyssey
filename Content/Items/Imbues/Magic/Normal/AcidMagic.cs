using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Buffs.MagicMarks;
using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using ArcaneOdyssey.Content.Buffs.DOT;
using Terraria.Audio;

namespace ArcaneOdyssey.Content.Items.Imbues.Magic.Normal
{
	public class AcidMagic : AOMagic
	{
		public override bool Special => true;
		public override float DashSpeed => 1.2f; // burst
		public override Color ImbueColour => new(245, 0, 240);
		public override float AOImbueSpeed => 0.925f;
		public override float AOImbueSize => 1f;
		public override float AOImbueDamage => 1f;
		public override float AOScrollSpeed => 1f;
		public override float AOScrollSize => 1.05f;
		public override float AOScrollDamage => 0.875f;
		public override SoundStyle? ImbueSound => SoundID.Splash;
		public override AODebuffRequirement[] ImbueDebuffs => [new(BuffID.Venom, 60 * 10)];
		public override SynergyEffects Effects => new(
			[ // these are debuffs cleared on hit
				ModContent.BuffType<FreezingEffect>(),
				ModContent.BuffType<SnowyEffect>(),
				ModContent.BuffType<SandyEffect>()
			],
			[
				new(ModContent.BuffType<AOBleed>(),1.075f),
				new(BuffID.OnFire,1.075f),
				new(ModContent.BuffType<CharredEffect>(),1.1f),
				new(ModContent.BuffType<FreezingEffect>(),1.2f),
				new(BuffID.OnFire3,1.05f),
				new(BuffID.Poisoned,1.05f),
				new(BuffID.ShadowFlame,1.1f),
				new(ModContent.BuffType<Singed>(), 1.1f),
				new(BuffID.Wet,0.9f),
				new(BuffID.Oiled,1.05f),
				new(ModContent.BuffType<Crystallized>(),0.9f),
				new(ModContent.BuffType<SandyEffect>(),0.99f),
				new(ModContent.BuffType<AOScalding>(),1.075f),
				new(ModContent.BuffType<SearedEffect>(),1.1f)
			]
			);



		public override void SpawningEffects(Rectangle area, Vector2 direction)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.UnholyWater, direction.X * 2f, direction.Y * 2f, Scale: 3f * area.RelativeScale())];
				spawnedDust.noGravity = true;
			}
		}

		public override void LingeringEffects(Rectangle area, Vector2? direction = null, Entity source = null)
		{
			Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Venom, Scale: area.RelativeScale());
			Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.UnholyWater, Scale: 1.6f * area.RelativeScale());
		}

		public override void ExplosionEffects(Vector2 position, float intensity = 1f)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust.NewDust(position, 0, 0, DustID.Venom, (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize * intensity), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize * intensity), Scale: intensity);
				Dust.NewDust(position, 0, 0, DustID.UnholyWater, (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize * intensity), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize * intensity), Scale: 3f * intensity);
			}
		}

		public override void KillEffects(Rectangle area, Entity source = null)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.UnholyWater, 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), Scale: 3f * area.RelativeScale())];
				spawnedDust.noGravity = true;
			}
			SoundEngine.PlaySound(ImbueSound, area.Center());
		}
	}
}