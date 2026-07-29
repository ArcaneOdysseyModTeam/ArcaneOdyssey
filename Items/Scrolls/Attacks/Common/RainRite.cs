using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Projectiles.Relics;
using ArcaneOdyssey.Skills.Base;
using Terraria.DataStructures;

namespace ArcaneOdyssey.Items.Scrolls.Attacks.Common
{
	public class RainRite : CommonScroll
	{
		public override bool CanHaveRelic => true;

		public override ModSkill Skill => ModContent.GetInstance<RainSkill>();
	}

	public class RainSkill : AttackSkill
	{
		public override float Speed => 5f;

		public override int Damage => 18;

		public override int Shoot => ModContent.ProjectileType<SpiritRaincloud>();

		public override int Scroll => ModContent.ItemType<RainRite>();

		public override void AttackStats(Player player, Imbuable imbue, ref Vector2 position, ref Vector2 velocity, ref int damage, ref float knockback)
		{
			velocity = -Vector2.UnitY * velocity.Length();
			damage /= 10;
		}

		public override bool Attack(Player player, Imbuable imbue, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int damage, float knockback)
		{
			ActivateAbility(player, imbue);
			return true;
		}
	}
}
