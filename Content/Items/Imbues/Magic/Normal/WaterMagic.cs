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
	public class WaterMagic : AOMagic
	{
		public override float Aura => .8f;
		public override void RegisterMutations()
		{
			RegisterMutation<CloudMagic>();
			RegisterMutation<LunarMagic>();
			RegisterMutation<OilMagic>();
			RegisterMutation<StormMagic>();
		}
		public override float DashSpeed => 1.2f; // burst
		public override bool? Cold => true;
		public override Color ImbueColour => new(0, 30, 255);
		public override float AOImbueSpeed => 1f;
		public override float AOImbueSize => 1.22f;
		public override float AOImbueDamage => 0.975f;
		public override float AOScrollSpeed => 1f;
		public override float AOScrollSize => 1.25f;
		public override float AOScrollDamage => 0.9f;
		public override SoundStyle? ImbueSound => SoundID.Splash;
		public override Debuff[] ImbueDebuffs => [new(BuffID.Wet, 60 * 10)];
		public override SynergyEffects Effects => new(
			[ // these are debuffs cleared on hit
				ClearBuff.Create < AOBurning >(),
				ClearBuff.Create < CharredEffect >(),
				ClearBuff.Create < Corroding >(),
				ClearBuff.Create < Melting >(),
				new(BuffID.Oiled),
				ClearBuff.Create < Singed >(),
				ClearBuff.Create < Scalding >(),
				ClearBuff.Create < SearedEffect >()
			],
			[
				new(ModContent.BuffType<Crystallized>(),0.85f),
				new(ModContent.BuffType<AOBleed>(),1.05f),
				new(BuffID.OnFire,0.8f),
				Synergy.Create<AOBurning>(.8f),
				new(ModContent.BuffType<CharredEffect>(),0.9f),
				new(BuffID.Venom,0.9f),
				Synergy.Create<Corroding>(.9f),
				new(ModContent.BuffType<FreezingEffect>(),1.075f),
				new(BuffID.OnFire3,0.9f),
				Synergy.Create<Melting>(.9f),
				new(BuffID.Oiled,0.98f),
				new(ModContent.BuffType<SandyEffect>(),0.8f),
				new(BuffID.ShadowFlame,0.7f),
				new(ModContent.BuffType<SnowyEffect>(),1.1f),
				new(ModContent.BuffType<SearedEffect>(),0.7f),
				new(ModContent.BuffType<Singed>(), 0.8f),
			]
		);

		public override void SpawningEffects(Rectangle area, Vector2 direction)
		{
			for (int n = 0; n < 3; n++)

			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Water, direction.X * 2f, direction.Y * 2f, Scale: 3f * area.RelativeScale())];
				spawnedDust.noGravity = true;
			}
		}

		public override void LingeringEffects(Rectangle area, Vector2? direction = null, Entity source = null)
		{
			Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Water, Scale: 1.2f * area.RelativeScale());
		}
		public override void ExplosionEffects(Vector2 position, float intensity = 1f)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(position, 0, 0, DustID.Water, (Main.rand.NextFloat() - 0.5f) * (35f * intensity * AOScrollSize), (Main.rand.NextFloat() - 0.5f) * (35f * intensity * AOScrollSize), Scale: 3f * intensity)];
				spawnedDust.noGravity = true;
			}
		}
		public override void KillEffects(Rectangle area, Entity source = null)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Water, 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), Scale: 3f * area.RelativeScale())];
				spawnedDust.noGravity = true;
			}
			SoundEngine.PlaySound(ImbueSound, area.Center());
		}
	}
}