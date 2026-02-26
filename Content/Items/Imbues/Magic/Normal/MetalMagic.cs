using ArcaneOdyssey.Content.Buffs.DOT;
using ArcaneOdyssey.Content.Buffs.MagicMarks;
using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Imbues.Magic.Lost;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace ArcaneOdyssey.Content.Items.Imbues.Magic.Normal
{
	public class MetalMagic : AOMagic
	{
		public override float Aura => 1.4f;
		public override void RegisterMutations()
		{
			RegisterMutation<DiamondMagic>();
			RegisterMutation<FrostmetalMagic>();
			RegisterMutation<SlashMagic>();
		}
		public override bool Special => true;
		public override float? DashResist => 1.5f;
		public override SoundStyle? ImbueSound => SoundID.Item99;
		public override Color ImbueColour => new(100, 100, 100);
		public override float AOImbueSpeed => 0.825f;
		public override float AOImbueSize => 1.158f;
		public override float AOImbueDamage => 1.1f;
		public override float AOScrollSpeed => 0.65f;
		public override float AOScrollSize => 1.2f;
		public override float AOScrollDamage => 1.025f;
		public override Debuff[] ImbueDebuffs => [Debuff.Create<AOBleed>()];
		public override SynergyEffects Effects => new(
			[ // these are debuffs cleared on hit
				ClearBuff.Create<FreezingEffect>()
			],
			[
				
				Synergy.Create<Corroding>(1.05f),
				Synergy.Create<Crystallized>(1.05f),
				Synergy.Create<FreezingEffect>(1.02f),
				
				Synergy.Create<Melting>(1.05f),
				Synergy.Create<SandyEffect>(1.1f)

			]
			);
		public override void SpawningEffects(Rectangle area, Vector2 direction)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Mercury, direction.X * 0.4f, direction.Y * 0.4f, Scale: area.RelativeScale())];
			}
		}

		public override void LingeringEffects(Rectangle area, Vector2? direction = null, Entity source = null)
		{
			Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.SilverFlame, Scale: 2f * area.RelativeScale())];
			spawnedDust.noGravity = true;
			spawnedDust.noLight = true;
		}
		public override void ExplosionEffects(Vector2 position, float intensity = 1f)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(position, 0, 0, DustID.Mercury, (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize * intensity), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize * intensity), Scale: 2f * intensity)];
			}
		}
		public override void KillEffects(Rectangle area, Entity source = null)
		{
			for (int n = 0; n < 30; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Mercury, 2f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 2f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), Scale: area.RelativeScale())];
			}
			SoundEngine.PlaySound(ImbueSound, area.Center());
		}
	}
}