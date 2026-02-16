using ArcaneOdyssey.Content.Buffs.DOT;
using ArcaneOdyssey.Content.Buffs.MagicMarks;
using ArcaneOdyssey.Content.Buffs.Stuns;
using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Imbues.Magic.Lost;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Imbues.Magic.Normal
{
	public class WindMagic : AOMagic
	{
		public override void RegisterMutations()
		{
			RegisterMutation<BlizzardMagic>();
			RegisterMutation<CloudMagic>();
			RegisterMutation<GravityMagic>();
			RegisterMutation<SlashMagic>();
			RegisterMutation<HeatMagic>();
			RegisterMutation<SoundMagic>();
			RegisterMutation<StormMagic>();
		}
		public override float DashSpeed => 1.5f; // instant
		public override float KBMulti => 2f;
		public override SoundStyle? ImbueSound => SoundID.Dig;
		public override Color ImbueColour => new(255, 255, 255, 255);
		public override float AOImbueSpeed => 1.175f;
		public override float AOImbueSize => 1.15f;
		public override float AOImbueDamage => .9f;
		public override float AOScrollSpeed => 1.35f;
		public override float AOScrollSize => 1.2f;
		public override float AOScrollDamage => .825f;
		public override CombinedDebuff[] CombinedDebuffs => [new(ModContent.BuffType<SnowyEffect>(), ModContent.BuffType<AOFrozen>()), new(ModContent.BuffType<FreezingEffect>(), ModContent.BuffType<AOFrozen>())];
		public override SynergyEffects Effects => new(
			[
				BuffID.OnFire,
				ModContent.BuffType<FreezingEffect>(),
				ModContent.BuffType<CharredEffect>(),
				BuffID.Venom,
				ModContent.BuffType<SandyEffect>(),
				BuffID.Wet,
				ModContent.BuffType<SnowyEffect>(),
				ModContent.BuffType<AOScalding>(),
				BuffID.Oiled
			],
			[
				new(ModContent.BuffType<Crystallized>(),0.9f),
				new(BuffID.OnFire,0.9f),
				new(ModContent.BuffType<CharredEffect>(),1.125f),
				new(ModContent.BuffType<Singed>(), .9f),
				new(ModContent.BuffType<FreezingEffect>(),1.1f),
				new(BuffID.Poisoned,0.9f),
				new(ModContent.BuffType<SandyEffect>(),0.9f),
				new(BuffID.ShadowFlame,1.15f),
				new(BuffID.Wet,0.9f),
				new(BuffID.Oiled,0.98f),
				new(ModContent.BuffType<AOScalding>(),0.9f),
				new(ModContent.BuffType<SearedEffect>(),1.15f)
			]
			);
		public override void SpawningEffects(Rectangle area, Vector2 direction)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.BubbleBurst_White, direction.X * 2f, direction.Y * 2f, Scale: 3f * area.RelativeScale())];
				spawnedDust.noGravity = true;
			}
		}
		public override void LingeringEffects(Rectangle area, Vector2? direction = null, Entity source = null)
		{
			Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.BubbleBurst_White, Scale: area.RelativeScale())];
			spawnedDust.noGravity = true;
		}
		public override void ExplosionEffects(Vector2 position, float intensity = 1f)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(position, 0, 0, DustID.BubbleBurst_White, (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize * intensity), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize * intensity), Scale: 3f * intensity)];
				spawnedDust.noGravity = true;
			}
		}
		public override void KillEffects(Rectangle area, Entity source = null)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.BubbleBurst_White, 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), Scale: 3f * area.RelativeScale())];
				spawnedDust.noGravity = true;
			}
			SoundEngine.PlaySound(ImbueSound, area.Center());
		}
	}
}
