using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Projectiles.Berserker;
using ArcaneOdyssey.Skills.Base;
using Terraria.DataStructures;

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

		public override void AttackStats(Player player, Imbuable imbue, ref Vector2 position, ref Vector2 velocity, ref int damage, ref float knockback)
		{
			position += velocity.SafeNormalize(Vector2.Zero) * 10f;
		}
	}
}
