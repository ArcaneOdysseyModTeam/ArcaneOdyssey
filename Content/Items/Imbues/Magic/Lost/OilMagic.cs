using ArcaneOdyssey.Content.Items.Base;
using Terraria.ModLoader;
using Terraria.ID;
using ArcaneOdyssey.Content.Buffs.DOT;
using ArcaneOdyssey.Content.Buffs.MagicMarks;
using Terraria.Audio;
using Microsoft.Xna.Framework;
using Terraria;

namespace ArcaneOdyssey.Content.Items.Imbues.Magic.Lost
{
	public class OilMagic : AOMagic
	{
		public override float DashSpeed => 1.2f; // burst
		public override bool CanBeWet => false;
		public override Color ImbueColour => new(20, 20, 20);
		public override float AOImbueSpeed => 1.1f;
		public override float AOImbueSize => 1.22f;
		public override float AOImbueDamage => 1.28f;
		public override float AOScrollSpeed => 1.1f;
		public override float AOScrollSize => 1.25f;
		public override float AOScrollDamage => 1.28f;
		public override AOImbuableTier ImbuableTier => AOImbuableTier.Lost;
		public override SoundStyle? ImbueSound => SoundID.Splash;
		public override AODebuffRequirement[] ImbueDebuffs => [new(BuffID.Oiled, 60 * 10)];
		public override SynergyEffects Effects => new(
			[ // these are debuffs cleared on hit
			
			],
			[
				new(BuffID.OnFire,1.15f),
				new(BuffID.OnFire3,1.15f),
				new(BuffID.ShadowFlame,1.15f),
				new(ModContent.BuffType<AOBleed>(),1.1f),
				new(ModContent.BuffType<HeavyBleed>(),1.1f),
				new(ModContent.BuffType<SandyEffect>(),0.96f),
				new(ModContent.BuffType<SnowyEffect>(),0.96f),
				new(ModContent.BuffType<CharredEffect>(),1.05f),
				new(ModContent.BuffType<SearedEffect>(),1.1f)
			]
			);

		public override void SpawningEffects(Rectangle area, Vector2 direction)
		{
			for (int n = 0; n < 3; n++)

			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Water_Cavern, direction.X * 2f, direction.Y * 2f, 0, Color.Black, 3f * area.RelativeScale())];
				spawnedDust.noGravity = true;
			}
		}

		public override void LingeringEffects(Rectangle area, Vector2? direction = null, Entity source = null)
		{
			Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Water_Cavern, 0f, 0f, 0, Color.Black, 1.2f * area.RelativeScale());
		}

		public override void ExplosionEffects(Vector2 position, float intensity = 1f)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(position, 0, 0, DustID.Water_Cavern, (Main.rand.NextFloat() - 0.5f) * (35f * intensity * AOScrollSize), (Main.rand.NextFloat() - 0.5f) * (35f * intensity * AOScrollSize), 0, Color.Black, 3f * intensity)];
				spawnedDust.noGravity = true;
			}
		}
		public override void KillEffects(Rectangle area, Entity source = null)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Water_Cavern, 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 0, Color.Black, 3f * area.RelativeScale())];
				spawnedDust.noGravity = true;
			}
			SoundEngine.PlaySound(ImbueSound, area.Center());
		}
	}
}