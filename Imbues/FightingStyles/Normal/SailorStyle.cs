using ArcaneOdyssey.Buffs.DOT;
using ArcaneOdyssey.Buffs.MagicMarks;
using ArcaneOdyssey.Buffs.Stuns;
using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Imbues.Gimmicks.Bars;
using System;
using Terraria.Audio;

namespace ArcaneOdyssey.Imbues.FightingStyles.Normal
{
	public class SailorStyle : FightingStyleBarred
	{
		public override float Aura => .875f;
		public override float DashSpeed => BarValue > (BarMax / 2) ? 1.2f : 1f; // burst?
		public override void SetStaticDefaults() 
		{ 
			base.SetStaticDefaults(); 
			ArcaneOdysseyMod.Sets.cold[Type] = true; 
		}
		public override Color ImbueColour => Color.CornflowerBlue;
		public override SoundStyle? ImbueSound => SoundID.Splash;
		public override BarGimmick Bar => ModContent.GetInstance<SailorBar>();
		public override Color DisplayColor => Color.PaleVioletRed;
		public override Debuff[] ImbueDebuffs => [Debuff.Create<Soaked>()];
		public override SynergyEffects Effects => new(
			[
				ClearBuff.Create<SearedEffect>(),
				ClearBuff.Create<CharredEffect>(),
				ClearBuff.Create<Burning>(),
				ClearBuff.Create<Melting>(),
				ClearBuff.Create<Corroding>(),
				ClearBuff.Create<Scorched>(),
				ClearBuff.Create<Singed>(),
				ClearBuff.Create<Scalding>(),
				ClearBuff.Create<Petrified>()
			],
			[
				Synergy.Create<Crystallized>(1.1f),
				Synergy.Create<SnowyEffect>(1.1f),
				Synergy.Create<FreezingEffect>(1.075f),
				Synergy.Create<Bleeding>(1.05f),
				Synergy.Create<CharredEffect>(0.9f),
				Synergy.Create<Melting>(.9f),
				Synergy.Create<Singed>(0.8f),
				Synergy.Create<Corroding>(.9f),
				Synergy.Create<SearedEffect>(0.85f),
				Synergy.Create<Scorched>(0.85f),
				Synergy.Create<SandyEffect>(0.8f),
				Synergy.Create<Burning>(.8f),
			]
		);

		public override void SpawningEffects(Rectangle area, Vector2 direction)
		{
			BarValue -= BarMax / 100f;
			for (int n = 0; n < (int)Math.Max(Math.Round((float)BarValue / (BarMax / 3)), 1); n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Water, direction.X * 2f, direction.Y * 2f, Scale: LerpValue * area.RelativeScale())];
				spawnedDust.noGravity = true;
			}
		}

		public override void LingeringEffects(Rectangle area, Vector2? direction = null, Entity source = null)
		{
			for (int n = 0; n < (int)Math.Max(Math.Round((float)BarValue / (BarMax / 3 * 2)), 1); n++)
				Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Water, Scale: LerpValue * 2.2f * area.RelativeScale());
		}

		public override void ExplosionEffects(Vector2 position, float intensity = 1f)
		{
			for (int n = 0; n < (int)Math.Max(Math.Round((float)BarValue / (BarMax / 3)), 1); n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(position, 0, 0, DustID.Water, (Main.rand.NextFloat() - 0.5f) * (35f * intensity), (Main.rand.NextFloat() - 0.5f) * (35f * intensity), Scale: LerpValue * intensity)];
				spawnedDust.noGravity = true;
			}
		}
		public override void KillEffects(Rectangle area, Entity source = null)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Water, 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), Scale: LerpValue * 3f * area.RelativeScale())];
				spawnedDust.noGravity = true;
			}
			SoundEngine.PlaySound(ImbueSound, area.Center());
		}

		public override void AddRecipes()
		{
			var rec = CreateRecipe().AddIngredient<BasicCombat>();
			if (ExternalModSupport.HasThorium)
			{
				rec.AddIngredient(ExternalModSupport.Thorium.Find<ModItem>("DepthScale"), 5);
			}
			else
			{
				rec.AddIngredient(ItemID.Coral, 15);
			}
			rec.AddOnCraftCallback(BasicCombat.ReuseSkills).Register();
		}
	}
}
