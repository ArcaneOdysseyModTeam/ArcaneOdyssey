using ArcaneOdyssey.Content.Buffs.DOT;
using ArcaneOdyssey.Content.Buffs.MagicMarks;
using ArcaneOdyssey.Content.Buffs.Stuns;
using ArcaneOdyssey.Content.Items.Base;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace ArcaneOdyssey.Content.Imbues.Magic.Developer
{
	public class VesuviusMagic : AOMagic
	{
		public override Color ImbueColour => new(0, 0, 255);
		public override float AOImbueSpeed => 1.2f;
		public override float AOImbueSize => 3f;
		public override float AOImbueDamage => 2f;
		public override float AOScrollSpeed => 1.2f;
		public override float AOScrollSize => 3f;
		public override float AOScrollDamage => 2f;
		public override AOImbuableTier ImbuableTier => AOImbuableTier.Developer;
		public override float? DashResist => 1.3f;
		public override SoundStyle? ImbueSound => SoundID.Item20;
		public override Debuff[] ImbueDebuffs => [Debuff.Create<VesuvianBurn>()];
		public override SynergyEffects Effects => new(
			[ // these are debuffs cleared on hit
				ClearBuff.Create<FreezingEffect>(), // freezing
				ClearBuff.Create<Petrified>(),
				ClearBuff.Create<Soaked>(),
				ClearBuff.Create<AOBleed>(),
				ClearBuff.Create<Corroding>(),
				ClearBuff.Create<FreezingEffect>(),
				ClearBuff.Create<SandyEffect>(),
				ClearBuff.Create<SnowyEffect>()
			],
			[
				Synergy.Create<Petrified>(2.2f), // petrified
				Synergy.Create<AOBleed>(2.15f), // bleeding
				Synergy.Create<AOBurning>(2.075f),
				Synergy.Create<Corroding>(2.1f),
				Synergy.Create<FreezingEffect>(1.95f),
				Synergy.Create<SnowyEffect>(1.99f),
				Synergy.Create<CharredEffect>(2.1f),
				Synergy.Create<SandyEffect>(1.99f),
				Synergy.Create<Soaked>(1.95f),
				Synergy.Create<Scorched>(2.1f),
				Synergy.Create<Flammable>(2.075f),
				Synergy.Create<Crystallized>(1.95f),
				Synergy.Create<Scalding>(2.075f)
			]
			);

		public override void SpawningEffects(Rectangle area, Vector2 direction)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.UltraBrightTorch, direction.X * 2f, direction.Y * 2f, 0, new Color(0, 0, 255, 0), 2.5f * area.RelativeScale())];
				spawnedDust.noGravity = true;
			}
		}

		public override void LingeringEffects(Rectangle area, Vector2? direction = null, Entity source = null)
		{
			Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.UltraBrightTorch, 0f, 0f, 0, new Color(0, 0, 255, 0), 1.2f);
			Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.SolarFlare, 0f, 0f, 0, Color.Blue, 1.2f);
			Lighting.AddLight(area.Center(), 1f, 0.19f, 0f);
		}
		public override void ExplosionEffects(Vector2 position, float intensity = 1f)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust.NewDust(position, 0, 0, DustID.UltraBrightTorch, (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize * intensity), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize * intensity), 0, new Color(0, 0, 255, 0), 2f * intensity);
				Dust.NewDust(position, 0, 0, DustID.SolarFlare, (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize * intensity), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize * intensity), 0, Color.Blue, 2f * intensity);
			}
		}

		public override void KillEffects(Rectangle area, Entity source = null)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.UltraBrightTorch, 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 0, new Color(0, 0, 255, 0), 3f * area.RelativeScale())];
				spawnedDust.noGravity = true;
			}
			SoundEngine.PlaySound(ImbueSound, area.Center());
		}
	}
}
