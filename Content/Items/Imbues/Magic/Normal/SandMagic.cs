using ArcaneOdyssey.Content.Buffs.DOT;
using ArcaneOdyssey.Content.Buffs.MagicMarks;
using ArcaneOdyssey.Content.Items.Base;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Imbues.Magic.Normal
{
	public class SandMagic : AOMagic
	{
		public override float? DashResist => 1.1f;
		public override SoundStyle? ImbueSound => SoundID.Dig;
		public override Color ImbueColour => new(255, 255, 60, 255);
		public override bool CanBeWet => false;
		public override float AOImbueSpeed => 0.975f;
		public override float AOImbueSize => 1.053f;
		public override float AOImbueDamage => 1.05f;
		public override float AOScrollSpeed => 0.95f;
		public override float AOScrollSize => 1.1f;
		public override float AOScrollDamage => 0.975f;
		public override AODebuffRequirement[] ImbueDebuffs => [new(ModContent.BuffType<SandyEffect>(), 60 * 10)];
		public override SynergyEffects Effects => new(
			[ // these are debuffs cleared on hit
				BuffID.Wet,
				BuffID.Oiled
			],
			[
				new(ModContent.BuffType<AOBleed>(),1.1f),
				new(BuffID.OnFire,1.125f),
				new(ModContent.BuffType<CharredEffect>(),1.01f),
				new(BuffID.Venom,1.075f),
				new(ModContent.BuffType<Crystallized>(),0.8f),
				new(BuffID.OnFire3,1.075f),
				new(BuffID.Wet,0.8f),
				new(BuffID.Oiled,0.9f),
				new(ModContent.BuffType<Singed>(), 1.1f),
				new(ModContent.BuffType<AOScalding>(),1.125f)
			]
			);

		public override void SpawningEffects(Rectangle area, Vector2 direction)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Sand, direction.X * 2f, direction.Y * 2f, Scale: 3f * area.RelativeScale())];
				spawnedDust.noGravity = true;
			}
		}

		public override void LingeringEffects(Rectangle area, Vector2? direction = null, Entity source = null)
		{
			Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Sand, Scale: 1f * area.RelativeScale());
		}

		public override void ExplosionEffects(Vector2 position, float intensity = 1f)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(position, 0, 0, DustID.Sand, (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize * intensity), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize * intensity), Scale: 3f * intensity)];
				spawnedDust.noGravity = true;
			}
		}

		public override void KillEffects(Rectangle area, Entity source = null)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Sand, 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), Scale: 3f * area.RelativeScale())];
				spawnedDust.noGravity = true;
			}
			SoundEngine.PlaySound(ImbueSound, area.Center());
		}
	}
}