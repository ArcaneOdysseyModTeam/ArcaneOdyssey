using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Projectiles.Berserker;
using ArcaneOdyssey.Skills.Base;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Skills.Generic
{
	public class StrikeSkill : AttackSkill
	{
		public override int Damage => 15;

		public override int Shoot => ModContent.ProjectileType<BasicStrike>();

		public override int Scroll => 0;

		public override float Speed => 2f;

		public override float Knockback => 10f;

		public override bool Attack(Player player, Imbuable imbue, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int damage, float knockback) => true;
	}
}
