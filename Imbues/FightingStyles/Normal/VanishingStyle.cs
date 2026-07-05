using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Imbues.Gimmicks.Bars;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Imbues.FightingStyles.Normal
{
	public sealed class VanishingStyle : FightingStyleBarred
	{
		public override float Aura => 1.25f;
		public static bool HasYou => ModLoader.HasMod("YouBoss");

		public override Color ImbueColour => Color.Black;
		public override SoundStyle? ImbueSound => SoundID.Item64;
		public override Color DisplayColor => Color.White;

		public override ImbuableTiers ImbuableTier => !HasYou ? base.ImbuableTier : ImbuableTiers.Ancient;

		public override BarGimmick Bar => ModContent.GetInstance<VanishBar>();

		public override void SpawningEffects(Rectangle area, Vector2 direction)
		{
			for (int n = 0; n < 2; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Wraith, direction.X * 2f, direction.Y * 2f, Scale: 1.5f * area.RelativeScale())];
				spawnedDust.noGravity = true;
			}
		}

		public override void LingeringEffects(Rectangle area, Vector2? direction = null, Entity source = null)
		{
			Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Wraith, Scale: 2f * area.RelativeScale())];
			spawnedDust.noGravity = true;
		}

		public override void ExplosionEffects(Vector2 position, float intensity = 1f)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(position, 0, 0, DustID.Wraith, (Main.rand.NextFloat() - 0.5f) * (25f * intensity), (Main.rand.NextFloat() - 0.5f) * (25f * intensity), Scale: 3f * intensity)];
				spawnedDust.noGravity = true;
			}
		}

		public override void KillEffects(Rectangle area, Entity source = null)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Wraith, 4f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 4f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), Scale: 1.5f * area.RelativeScale())];
				spawnedDust.noGravity = true;
			}
			SoundEngine.PlaySound(ImbueSound, area.Center());
		}

		public override void AddRecipes()
		{
			if (!HasYou)
				CreateRecipe().AddIngredient<BasicCombat>().AddIngredient(ItemID.SoulofNight, 5).AddOnCraftCallback(BasicCombat.ReuseSkills).Register();
			else
				CreateRecipe().AddIngredient<BasicCombat>().AddIngredient(ModLoader.GetMod("YouBoss").Find<ModItem>("FirstFractal")).AddOnCraftCallback(BasicCombat.ReuseSkills).Register();
		}
	}
}
