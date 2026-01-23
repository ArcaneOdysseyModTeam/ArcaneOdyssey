using ArcaneOdyssey.Content.Buffs.MagicMarks;
using ArcaneOdyssey.Content.Items.Base;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Imbues.Magic.Normal
{
	public class LightMagic : AOMagic
	{
		public override float DashSpeed => 1.5f; // instant
		public override SoundStyle? ImbueSound => SoundID.Item9;
		public override Color ImbueColour => new(255, 255, 0, 255);
		public override float AOImbueSpeed => 1.3f;
		public override float AOImbueSize => 0.946f;
		public override float AOImbueDamage => 0.9f;
		public override float AOScrollSpeed => 1.6f;
		public override float AOScrollSize => 1f;
		public override float AOScrollDamage => 0.87f;
		public override AODebuffRequirement[] ImbueDebuffs => [new(ModContent.BuffType<BlindedEffect>(), 60 * 5)];
		public override SynergyEffects Effects => new(
			[ // these are debuffs cleared on hit
				
			],
			[
				new(ModContent.BuffType<Crystallized>(),1.075f),
				new(ModContent.BuffType<DrainedEffect>(),0.8f)
			]
			);
		public override void SpawningEffects(Rectangle area, Vector2 direction)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.YellowStarDust, direction.X * 0.2f, direction.Y * 0.2f, Scale: 3f * area.RelativeScale())];
				spawnedDust.noGravity = true;
				Dust spawnedDust2 = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.YellowTorch, direction.X * 0.2f, direction.Y * 0.2f, Scale: 3f * area.RelativeScale())];
				spawnedDust2.noGravity = true;
			}
		}
		public override void LingeringEffects(Rectangle area, Vector2? direction = null, Entity source = null)
		{
			Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.YellowStarDust, Scale: area.RelativeScale())];
			Dust spawnedDust2 = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.YellowTorch, Scale: 2f * area.RelativeScale())];
			spawnedDust2.noGravity = true;
		}
		public override void ExplosionEffects(Vector2 position, float intensity = 1f)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(position, 0, 0, DustID.YellowStarDust, (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize * intensity), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize * intensity), Scale: 3f * intensity)];
				spawnedDust.noGravity = true;
				Dust spawnedDust2 = Main.dust[Dust.NewDust(position, 0, 0, DustID.YellowTorch, (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize * intensity), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize * intensity), Scale: 3f * intensity)];
				spawnedDust2.noGravity = true;
			}
		}
		public override void KillEffects(Rectangle area, Entity source = null)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.YellowStarDust, 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), Scale: 3f * area.RelativeScale())];
				spawnedDust.noGravity = true;
				Dust spawnedDust2 = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.YellowTorch, 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), Scale: 3f * area.RelativeScale())];
				spawnedDust2.noGravity = true;
			}
			SoundEngine.PlaySound(ImbueSound, area.Center());
		}
	}
}