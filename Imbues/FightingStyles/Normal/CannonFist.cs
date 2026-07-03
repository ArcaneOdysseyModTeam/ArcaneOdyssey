using ArcaneOdyssey.Buffs.DOT;
using ArcaneOdyssey.Buffs.MagicMarks;
using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Imbues.Gimmicks.FightingStyle;
using ArcaneOdyssey.Skills.Base;
using ArcaneOdyssey.Skills.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Imbues.FightingStyles.Normal
{
	public class CannonFist : FightingStyle
	{
		public override ImbueGimmick Gimmick => ModContent.GetInstance<CannonFrenzy>();
		public override float Aura => .875f;
		public override Color ImbueColour => Color.Black;
		public override SoundStyle? ImbueSound => SoundID.Item14;

		public override AttackSkill DefaultAttack => ModContent.GetInstance<CannonFistSkill>();

		public override float ImbueDamage => 1.085f;
		
		public override float ImbueSize => 1.056f;
		public override float ScrollDamage => 0.7f;
		public override float ScrollSize => 1f;
		public override float ScrollSpeed => 1f;

		public override Debuff[] ImbueDebuffs => [Debuff.Create<Bleeding>()];
		public override SynergyEffects Effects => new(
			[],
			[
				Synergy.Create<Crystallized>(1.1f)
			]
		);

		public override void SpawningEffects(Rectangle area, Vector2 direction)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Ash, direction.X * 2f, direction.Y * 2f, Scale: 4f * area.RelativeScale())];
				spawnedDust.noGravity = true;
			}
		}

		public override void LingeringEffects(Rectangle area, Vector2? direction = null, Entity source = null)
		{
			Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Ash, Scale: 2f * area.RelativeScale())];
			spawnedDust.noGravity = true;
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
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Pixie, 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), Scale: 3f * area.RelativeScale())];
				spawnedDust.noGravity = true;
				Dust spawnedDust3 = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Pixie, 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), Scale: 3f * area.RelativeScale())];
				spawnedDust3.noGravity = true;
				Dust spawnedDust2 = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Ash, 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), Scale: 4f * area.RelativeScale())];
				spawnedDust2.noGravity = true;
			}
			SoundEngine.PlaySound(ImbueSound, area.Center());
		}

		public override void AddRecipes()
		{
			CreateRecipe().AddIngredient<BasicCombat>().AddIngredient(ItemID.Bomb, 15).Register();
		}
	}

	public class CannonFistSkill : StrikeSkill
	{
		public override int Shoot => ProjectileID.CannonballFriendly;

		public override float Knockback => 2f;

		public override float Speed => 8f;

		public override int UseStyleID => ItemUseStyleID.Swing;

		public override void AttackStats(Player player, Imbuable imbue, ref Vector2 position, ref Vector2 velocity, ref int damage, ref float knockback)
		{
			if (player.ConsumeItem(ItemID.Cannonball))
			{
				velocity *= 2;
				damage *= 2;
				knockback *= 2;
			}
		}
	}
}
