using ArcaneOdyssey.Content.Buffs.MagicMarks;
using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Imbues.Magic.Normal;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Imbues.Magic.Lost
{
	public class DarknessMagic : AOMagic
	{
		public override AOImbuableTier ImbuableTier => AOImbuableTier.Lost;
		public override float DashSpeed => 1.2f; // burst
		public override SoundStyle? ImbueSound => SoundID.Item8;
		public override Color ImbueColour => Color.Black;
		public override float AOImbueSpeed => 1.3f;
		public override float AOImbueSize => 1.3f;
		public override float AOImbueDamage => 1.3f;
		public override AODebuffRequirement[] ImbueDebuffs => [new(ModContent.BuffType<DrainedEffect>(), (60 * 7.5f).Round())];
		public override SynergyEffects Effects => new(
			[ // these are debuffs cleared on hit
				
			],
			[
				new(BuffID.Confused,1.2f),
				new(ModContent.BuffType<Crystallized>(),0.7f),
				new(ModContent.BuffType<BlindedEffect>(),0.7f),
			]
			);

		public override void SpawningEffects(Rectangle area, Vector2 direction)
		{
			for (int n = 0; n < 2; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Wraith, direction.X * 2f, direction.Y * 2f, Scale: 3f * area.RelativeScale())];
				spawnedDust.noGravity = true;
				Dust spawnedDust2 = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.VampireHeal, direction.X * 2f, direction.Y * 2f, Scale: 3f * area.RelativeScale())];
				spawnedDust2.noGravity = true;
			}
		}

		public override void LingeringEffects(Rectangle area, Vector2? direction = null, Entity source = null)
		{
			if (Main.GameUpdateCount % 2 == 0)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Wraith, Scale: 2f * area.RelativeScale())];
				spawnedDust.noGravity = true;
				Dust spawnedDust2 = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.VampireHeal, Scale: 3f * area.RelativeScale())];
				spawnedDust2.noGravity = true;
			}
		}

		public override void ExplosionEffects(Vector2 position, float intensity = 1f)
		{
			for (int n = 0; n < 2; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(position, 0, 0, DustID.Wraith, (Main.rand.NextFloat() - 0.5f) * (35f * intensity * AOScrollSize), (Main.rand.NextFloat() - 0.5f) * (35f * intensity * AOScrollSize), Scale: 3f * intensity)];
				spawnedDust.noGravity = true;
				Dust spawnedDust2 = Main.dust[Dust.NewDust(position, 0, 0, DustID.VampireHeal, (Main.rand.NextFloat() - 0.5f) * (35f * intensity * AOScrollSize), (Main.rand.NextFloat() - 0.5f) * (35f * intensity * AOScrollSize), Scale: 3f * intensity)];
				spawnedDust2.noGravity = true;
			}
		}

		public override void KillEffects(Rectangle area, Entity source = null)
		{
			for (int n = 0; n < 6; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Wraith, 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), Scale: 3f * area.RelativeScale())];
				spawnedDust.noGravity = true;
				Dust spawnedDust2 = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.VampireHeal, 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), Scale: 3f * area.RelativeScale())];
				spawnedDust2.noGravity = true;
			}
			SoundEngine.PlaySound(ImbueSound, area.Center());
		}

		public override void AddRecipes()
		{
			CreateLostRecipe(typeof(ShadowMagic));
		}
	}
}
