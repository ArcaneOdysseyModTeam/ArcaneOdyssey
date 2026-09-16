using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Projectiles.Relics;
using ArcaneOdyssey.Skills.Base;
using Terraria.DataStructures;

namespace ArcaneOdyssey.Items.Scrolls.Attacks.Common
{
	public class HoundRite : CommonScroll
	{
		public override bool MetConditions() => AOUtils.BossesKilled>0;
		public override bool CanHaveRelic => true;

		public override ModSkill Skill => ModContent.GetInstance<HoundSkill>();
	}

	public class HoundSkill : AttackSkill
	{
		public override int Damage => 20;

		public override int Shoot => ModContent.ProjectileType<SpiritHound>();

		public override int Scroll => ModContent.ItemType<HoundRite>();

		public override int Time => 67;

		public override float Speed => 7f;

		public override bool Attack(Player player, Imbuable imbue, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int damage, float knockback)
		{
			ActivateAbility(player, imbue);
			return true;
		}
	}
}
