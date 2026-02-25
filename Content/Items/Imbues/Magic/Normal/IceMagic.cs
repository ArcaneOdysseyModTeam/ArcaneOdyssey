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
	public class IceMagic : AOMagic
	{
		public override float Aura => 1.1f;
		public override void RegisterMutations()
		{
			RegisterMutation<BlizzardMagic>();
			RegisterMutation<FrostmetalMagic>();
		}
		public override bool Special => true;
		public override float? DashResist => 1.3f;
		public override bool? Cold => true;
		public override SoundStyle? ImbueSound => SoundID.Item27;
		public override Color ImbueColour => new(30, 200, 255, 255);
		public override bool CanBeWet => false;
		public override float AOImbueSpeed => .925f;
		public override float AOImbueSize => 1.15f;
		public override float AOImbueDamage => 1.05f;
		public override float AOScrollSpeed => 0.85f;
		public override float AOScrollSize => 1.2f;
		public override float AOScrollDamage => 0.975f;
		public override Debuff[] ImbueDebuffs => [new(ModContent.BuffType<FreezingEffect>(), 60 * 10), new(ModContent.BuffType<AOFrozen>(), 60, 33)];
		public override Combo[] CombinedDebuffs => [new(BuffID.Wet, ModContent.BuffType<AOFrozen>())];



		public override SynergyEffects Effects => new(
			[ // these are debuffs cleared on hit
				new(BuffID.Wet),
				ClearBuff.Create < AOBleed >(),
				new(BuffID.OnFire),
				ClearBuff.Create < AOBurning >(),
				new(BuffID.Venom),
				ClearBuff.Create < Corroding >(),
				new(BuffID.OnFire3),
				ClearBuff.Create < Melting >(),
				new(BuffID.ShadowFlame),
				ClearBuff.Create < CharredEffect >()
			],
			[ // synergies
				new(ModContent.BuffType<AOBleed>(), 1.2f), // bleeding
				new(ModContent.BuffType<AOFrozen>(), 1.1f), // frozen
				new(ModContent.BuffType<FreezingEffect>(), 1.1f), // freezing
				new(BuffID.Wet, 1.1f), // (add stunning later!)
				new(BuffID.OnFire, .9f), // burning
				Synergy.Create<AOBurning>(.9f),
				new(BuffID.Oiled,1.03f),
				new(ModContent.BuffType<CharredEffect>(), .9f), // charred
				new(BuffID.OnFire3, .8f), // scorched
				Synergy.Create<Melting>(.8f),
				new(BuffID.ShadowFlame, 0.8f),
				new(ModContent.BuffType<SnowyEffect>(), 1.1f),
				new(ModContent.BuffType<Crystallized>(),1.075f),
				new(ModContent.BuffType<SearedEffect>(),0.8f),
				new(ModContent.BuffType<Singed>(), 0.85f)
			]
			);
		public override void SpawningEffects(Rectangle area, Vector2 direction)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.SnowflakeIce, direction.X * 0.5f, direction.Y * 0.5f, Scale: 3f * area.RelativeScale())];
				spawnedDust.noGravity = true;
				Dust spawnedDust2 = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Ice, direction.X * 0.5f, direction.Y * 0.5f, Scale: 2f * area.RelativeScale())];
			}
		}

		public override void LingeringEffects(Rectangle area, Vector2? direction = null, Entity source = null)
		{
			Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Ice, Scale: area.RelativeScale())];
		}
		public override void ExplosionEffects(Vector2 position, float intensity = 1f)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(position, 0, 0, DustID.SnowflakeIce, (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize * intensity), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize * intensity), Scale: 3f * intensity)];
				spawnedDust.noGravity = true;
				Dust spawnedDust2 = Main.dust[Dust.NewDust(position, 0, 0, DustID.Ice, (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize * intensity), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize * intensity), Scale: 2f * intensity)];
			}
		}
		public override void KillEffects(Rectangle area, Entity source = null)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.SnowflakeIce, 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), Scale: 3f * area.RelativeScale())];
				spawnedDust.noGravity = true;
				Dust spawnedDust2 = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Ice, 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), Scale: 2f * area.RelativeScale())];
			}
			SoundEngine.PlaySound(ImbueSound, area.Center());
		}
	}
}
