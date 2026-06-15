using ArcaneOdyssey.Buffs.DOT;
using ArcaneOdyssey.Buffs.MagicMarks;
using ArcaneOdyssey.Buffs.Stuns;
using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Imbues.Gimmicks;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Imbues.FightingStyles.Normal
{
	public class PowderFist : FightingStyle
	{
		public override ImbueGimmick Gimmick => ModContent.GetInstance<PowderBurst>();
		public override float Aura => .875f;
		public override float DashSpeed => 1.2f;

		public override void SetStaticDefaults() { base.SetStaticDefaults(); ArcaneOdysseyMod.Sets.cold[Type] = false; }
		public override bool CanBeWet => false;
		public override Color ImbueColour => Color.DarkGray;
		public override SoundStyle? ImbueSound => SoundID.Item14;

		public override float ImbueDamage => Main.rand.NextFloat(0.85f, 1.17f);
		public override float ImbueSpeed => .9f;
		public override float ImbueSize => Main.rand.NextFloat(1.11f, 1.25f);
		public override float ScrollDamage => Main.rand.NextFloat(0.7f, .96f);
		public override float ScrollSize => Main.rand.NextFloat(1.05f, 1.19f);
		public override float ScrollSpeed => .9f;

		public override Combo[] CombinedDebuffs => [Combo.Create<Burning, Petrified>(), Combo.Create<Burning, Petrified>(), Combo.Create<Scalding, Petrified>(), Combo.Create<SearedEffect, Petrified>()];

		public override Debuff[] ImbueDebuffs => [Debuff.Create<CharredEffect>()];
		public override SynergyEffects Effects => new(
			[
				ClearBuff.Create<Soaked>(),
				ClearBuff.Create<FreezingEffect>(),
				ClearBuff.Create<Frozen>(),
				ClearBuff.Create<Paralyzed>(),
				ClearBuff.Create<Petrified>(),
				ClearBuff.Create<Scalding>(),
				ClearBuff.Create<Burning>(),
				ClearBuff.Create<SearedEffect>(),
			],
			[
				Synergy.Create<Petrified>(1.1f),
				Synergy.Create<Scalding>(1.1f),
				Synergy.Create<Burning>(1.1f),
				Synergy.Create<SearedEffect>(1.1f),
				Synergy.Create<Corroding>(1.1f),
				Synergy.Create<SandyEffect>(1.1f),
				Synergy.Create<Melting>(1.1f),
				Synergy.Create<Soaked>(.9f),
				Synergy.Create<FreezingEffect>(.9f),
				Synergy.Create<SnowyEffect>(.8f),
			]
		);

		public override void SpawningEffects(Rectangle area, Vector2 direction)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Pixie, direction.X * 2f, direction.Y * 2f, Scale: 3f * area.RelativeScale())];
				spawnedDust.noGravity = true;
				Dust spawnedDust3 = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Pixie, direction.X * 2f, direction.Y * 2f, Scale: 3f * area.RelativeScale())];
				spawnedDust3.noGravity = true;
				Dust spawnedDust2 = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Ash, direction.X * 2f, direction.Y * 2f, Scale: 4f * area.RelativeScale())];
				spawnedDust2.noGravity = true;
			}
		}

		public override void LingeringEffects(Rectangle area, Vector2? direction = null, Entity source = null)
		{
			Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Pixie, Scale: 1.6f * area.RelativeScale())];
			spawnedDust.noGravity = true;
			Dust spawnedDust3 = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Pixie, Scale: 1.6f * area.RelativeScale())];
			spawnedDust3.noGravity = true;
			Dust spawnedDust2 = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Ash, Scale: 2f * area.RelativeScale())];
			spawnedDust2.noGravity = true;
		}

		public override void ExplosionEffects(Vector2 position, float intensity = 1f)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(position, 0, 0, DustID.Pixie, (Main.rand.NextFloat() - 0.5f) * (15f * intensity), (Main.rand.NextFloat() - 0.5f) * (15f * intensity), Scale: 3f * intensity)];
				spawnedDust.noGravity = true;
				Dust spawnedDust2 = Main.dust[Dust.NewDust(position, 0, 0, DustID.Pixie, (Main.rand.NextFloat() - 0.5f) * (15f * intensity), (Main.rand.NextFloat() - 0.5f) * (15f * intensity), Scale: 3f * intensity)];
				spawnedDust2.noGravity = true;
				Dust spawnedDust3 = Main.dust[Dust.NewDust(position, 0, 0, DustID.Ash, (Main.rand.NextFloat() - 0.5f) * (15f * intensity), (Main.rand.NextFloat() - 0.5f) * (15f * intensity), Scale: 4f * intensity)];
				spawnedDust3.noGravity = true;
			}
		}

		public override void KillEffects(Rectangle area, Entity source = null)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Pixie, 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), Scale: 3f)];
				spawnedDust.noGravity = true;
				Dust spawnedDust3 = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Pixie, 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), Scale: 3f)];
				spawnedDust3.noGravity = true;
				Dust spawnedDust2 = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Ash, 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), Scale: 4f)];
				spawnedDust2.noGravity = true;
			}
			SoundEngine.PlaySound(ImbueSound, area.Center());
		}

		public override void AddRecipes()
		{
			CreateRecipe().AddIngredient<BasicCombat>().AddIngredient(ItemID.ExplosivePowder, 15).Register();
		}
	}
}
