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
	public class ExplosionMagic : AOMagic
	{
		public override void RegisterMutations()
		{
			RegisterMutation<AetherMagic>();
			RegisterMutation<HeatMagic>();
			RegisterMutation<PhoenixMagic>();
			RegisterMutation<SunMagic>();
			RegisterMutation<ShadowflameMagic>();
		}
		public override bool Special => true;
		public override float DashSpeed => 1.2f; // burst
		public override bool? Cold => false;
		public override Color ImbueColour => new(235, 146, 52);
		public override float AOImbueSpeed => 0.925f;
		public override bool CanBeWet => false;
		public override float AOImbueSize => 1.3f;
		public override float AOImbueDamage => 1f;
		public override float AOScrollSpeed => 0.85f;
		public override float AOScrollSize => 1.3f;
		public override float AOScrollDamage => 0.925f;
		public override SoundStyle? ImbueSound => SoundID.Item14;
		public override AODebuffRequirement[] ImbueDebuffs => [new(ModContent.BuffType<CharredEffect>(), 60 * 10)];
		public override SynergyEffects Effects => new(
			[ // these are debuffs cleared on hit
				ModContent.BuffType<FreezingEffect>(),
				ModContent.BuffType<SnowyEffect>(),
				BuffID.Wet
			],
			[
				new(ModContent.BuffType<AOBleed>(),1.01f),
				new(BuffID.OnFire,1.125f),
				new(BuffID.Venom,1.075f),
				new(ModContent.BuffType<Crystallized>(),1.075f),
				new(ModContent.BuffType<FreezingEffect>(),1.01f),
				new(BuffID.OnFire3,1.075f),
				new(ModContent.BuffType<SnowyEffect>(),0.99f),
				new(BuffID.ShadowFlame,1.15f),
				new(BuffID.Wet,0.99f),
				new(BuffID.Oiled,1.075f),
				new(ModContent.BuffType<Singed>(), 1.1f),
				new(ModContent.BuffType<SandyEffect>(),0.99f),
				new(ModContent.BuffType<AOScalding>(),1.125f),
				new(ModContent.BuffType<SearedEffect>(),1.15f)
			]
			);
		public override void SpawningEffects(Rectangle area, Vector2 direction)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Pixie, direction.X * 2f, direction.Y * 2f, Scale: 3f)];
				spawnedDust.noGravity = true;
				Dust spawnedDust3 = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Pixie, direction.X * 2f, direction.Y * 2f, Scale: 3f)];
				spawnedDust3.noGravity = true;
				Dust spawnedDust2 = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Ash, direction.X * 2f, direction.Y * 2f, Scale: 4f)];
				spawnedDust2.noGravity = true;
			}
		}
		public override void LingeringEffects(Rectangle area, Vector2? direction = null, Entity source = null)
		{
			Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Pixie, Scale: 1.6f)];
			spawnedDust.noGravity = true;
			Dust spawnedDust3 = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Pixie, Scale: 1.6f)];
			spawnedDust3.noGravity = true;
			Dust spawnedDust2 = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Ash, Scale: 2f)];
			spawnedDust2.noGravity = true;
		}
		public override void ExplosionEffects(Vector2 position, float intensity = 1f)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(position, 0, 0, DustID.Pixie, (Main.rand.NextFloat() - 0.5f) * (25f * intensity * AOScrollSize), (Main.rand.NextFloat() - 0.5f) * (25f * intensity * AOScrollSize), Scale: 3f * intensity)];
				spawnedDust.noGravity = true;
				Dust spawnedDust2 = Main.dust[Dust.NewDust(position, 0, 0, DustID.Pixie, (Main.rand.NextFloat() - 0.5f) * (25f * intensity * AOScrollSize), (Main.rand.NextFloat() - 0.5f) * (25f * intensity * AOScrollSize), Scale: 3f * intensity)];
				spawnedDust2.noGravity = true;
				Dust spawnedDust3 = Main.dust[Dust.NewDust(position, 0, 0, DustID.Ash, (Main.rand.NextFloat() - 0.5f) * (25f * intensity * AOScrollSize), (Main.rand.NextFloat() - 0.5f) * (25f * intensity * AOScrollSize), Scale: 4f * intensity)];
				spawnedDust3.noGravity = true;
			}
		}
		public override void KillEffects(Rectangle area, Entity source = null)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Pixie, 18f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 18f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), Scale: 3f * area.RelativeScale())];
				spawnedDust.noGravity = true;
				Dust spawnedDust3 = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Pixie, 18f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 18f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), Scale: 3f * area.RelativeScale())];
				spawnedDust3.noGravity = true;
				Dust spawnedDust2 = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Ash, 18f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 18f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), Scale: 4f * area.RelativeScale())];
				spawnedDust2.noGravity = true;
			}
			SoundEngine.PlaySound(ImbueSound, area.Center());
		}


	}
}