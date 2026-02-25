using ArcaneOdyssey.Content.Buffs.DOT;
using ArcaneOdyssey.Content.Buffs.MagicMarks;
using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Imbues.Magic.Lost;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Imbues.Magic.Normal
{
	public class WoodMagic : AOMagic
	{
		public override float Aura => 1.2f;
		public override void RegisterMutations()
		{
			RegisterMutation<OilMagic>();
			RegisterMutation<PlantMagic>();
			RegisterMutation<SlashMagic>();
			RegisterMutation<ThreadMagic>();
		}
		public override bool Special => true;
		public override float? DashResist => 1.3f;
		public override SoundStyle? ImbueSound => SoundID.Dig;
		public override Color ImbueColour => new(61, 33, 0, 255);
		public override float AOImbueSpeed => 0.9f;
		public override float AOImbueSize => 1.162f;
		public override float AOImbueDamage => 1.025f;
		public override float AOScrollSpeed => 0.8f;
		public override float AOScrollSize => 1.2f;
		public override float AOScrollDamage => 0.95f;
		public override Debuff[] ImbueDebuffs => [new(ModContent.BuffType<AOBleed>(), 60 * 10)];
		public override SynergyEffects Effects => new([],
			[
				new(BuffID.OnFire,1.1f),
				Synergy.Create<AOBurning>(1.1f),
				new(ModContent.BuffType<CharredEffect>(),1.1f),
				new(ModContent.BuffType<Singed>(), 1.1f),
				new(BuffID.Venom,1.05f),
				Synergy.Create<Corroding>(1.05f),
				new(BuffID.OnFire3,1.05f),
				Synergy.Create<Melting>(1.05f),
				new(ModContent.BuffType<SandyEffect>(),1.1f),
				new(BuffID.ShadowFlame,1.1f),
				new(ModContent.BuffType<Scalding>(),1.1f),
				new(ModContent.BuffType<SearedEffect>(),1.1f)
			]
			);

		public override void SpawningEffects(Rectangle area, Vector2 direction)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Pearlwood, direction.X * 0.2f, direction.Y * 0.2f, Scale: 1.5f * area.RelativeScale());
			}
		}

		public override void LingeringEffects(Rectangle area, Vector2? direction = null, Entity source = null)
		{
			Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Pearlwood, direction.GetValueOrDefault().X * 0.2f, direction.GetValueOrDefault().Y * 0.2f, Scale: 1f * area.RelativeScale());
		}

		public override void ExplosionEffects(Vector2 position, float intensity = 1f)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(position, 0, 0, DustID.Pearlwood, (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize * intensity), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize * intensity), Scale: 2.5f * intensity)];
				spawnedDust.noGravity = true;
			}
		}

		public override void KillEffects(Rectangle area, Entity source = null)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Pearlwood, 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), Scale: 2f * area.RelativeScale())];
				spawnedDust.noGravity = true;
			}
			SoundEngine.PlaySound(ImbueSound, area.Center());
		}
	}
}