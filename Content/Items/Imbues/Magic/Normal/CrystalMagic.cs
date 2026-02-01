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
	public class CrystalMagic : AOMagic
	{
		public override bool Special => true;
		public override float? DashResist => 1.3f;
		public override Color ImbueColour => new(255, 0, 0);
		public override float AOImbueSpeed => 0.95f;
		public override float AOImbueSize => 1.11f;
		public override float AOImbueDamage => 1.025f;
		public override float AOScrollSpeed => 0.9f;
		public override float AOScrollSize => 1.15f;
		public override float AOScrollDamage => 1.05f;
		public override SoundStyle? ImbueSound => SoundID.Shatter;
		public override AODebuffRequirement[] ImbueDebuffs => [new(ModContent.BuffType<Crystallized>(), 60 * 5)];
		public override CombinedDebuff[] CombinedDebuffs => [];
		public override SynergyEffects Effects => new(
			[ // these are debuffs cleared on hit
			
			],
			[
				new(ModContent.BuffType<FreezingEffect>(), 1.01f),
				new(ModContent.BuffType<AOBleed>(), 1.01f),
				new(BuffID.Venom, 1.01f),
				new(BuffID.OnFire3, 1.075f),
				new(ModContent.BuffType<SandyEffect>(), 1.125f)
			]
			);


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
				Dust.NewDust(position, 0, 0, DustID.GemRuby, (Main.rand.NextFloat() - 0.5f) * (7f * intensity * AOScrollSize), (Main.rand.NextFloat() - 0.5f) * (7f * intensity * AOScrollSize), Scale: 2f * intensity);
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