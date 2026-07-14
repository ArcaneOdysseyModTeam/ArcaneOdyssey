using ArcaneOdyssey.Buffs.DOT;
using ArcaneOdyssey.Buffs.MagicMarks;
using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Skills.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace ArcaneOdyssey.Imbues.FightingStyles.Normal
{
	public class IronLeg : FightingStyle
	{
		public override float Aura => 1.5f;
		public override float? DashResist => 1.35f;

		public override Color ImbueColour => Color.LightGray;
		public override SoundStyle? ImbueSound => SoundID.Item99;

		public override float ImbueDamage => 1.125f;
		public override float ImbueSpeed => 0.75f;
		public override float ImbueSize => 1.1f;
		public override float ScrollDamage => .95f;
		public override float ScrollSize => 1.1f;
		public override float ScrollSpeed => 0.75f;

		public override Debuff[] ImbueDebuffs => [Debuff.Create<Bleeding>()];
		public override SynergyEffects Effects => new(
			[
				ClearBuff.Create<FreezingEffect>()
			],
			[
				Synergy.Create<Crystallized>(1.05f),
				Synergy.Create<FreezingEffect>(1.2f),
				Synergy.Create<SandyEffect>(1.1f),
				Synergy.Create<Melting>(1.1f),
				Synergy.Create<Corroding>(1.1f)
			]
		);
		public override void SpawningEffects(Rectangle area, Vector2 direction)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Mercury, direction.X * 0.4f, direction.Y * 0.4f, Scale: area.RelativeScale());
			}
		}

		public override void LingeringEffects(Rectangle area, Vector2? direction = null, Entity source = null)
		{
			Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.SilverFlame, Scale: area.RelativeScale())];
			spawnedDust.noGravity = true;
			spawnedDust.noLight = true;
		}

		public override void ExplosionEffects(Vector2 position, float intensity = 1f)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust.NewDust(position, 0, 0, DustID.Mercury, (Main.rand.NextFloat() - 0.5f) * (15f * intensity), (Main.rand.NextFloat() - 0.5f) * (15f * intensity), Scale: 2f * intensity);
			}
		}
		public override void KillEffects(Rectangle area, Entity source = null)
		{
			for (int n = 0; n < 30; n++)
			{
				Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Mercury, 2f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 2f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), Scale: area.RelativeScale());
			}
			SoundEngine.PlaySound(ImbueSound, area.Center());
		}
		public override void AddRecipes()
		{
			CreateRecipe().AddIngredient<BasicCombat>().AddRecipeGroup(RecipeGroupID.IronBar, 15).AddOnCraftCallback(BasicCombat.ReuseSkills).Register();
		}
	}

	public class ILegKick : StrikeSkill
	{
		public override int UseStyleID => ItemUseStyleID.HiddenAnimation;

		public override void AttackStats(Player player, Imbuable imbue, ref Vector2 position, ref Vector2 velocity, ref int damage, ref float knockback)
		{
			base.AttackStats(player, imbue, ref position, ref velocity, ref damage, ref knockback);
			position.Y += Player.defaultHeight / 2f;
		}
	}
}
