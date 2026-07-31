using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Projectiles.Berserker;
using ArcaneOdyssey.Skills.Base;
using Terraria.DataStructures;

namespace ArcaneOdyssey.Items.Scrolls.Attacks.Rare
{
	public class AxeTechnique : RareScroll
	{
		public override bool CanHaveFS => true;

		public override ModSkill Skill => ModContent.GetInstance<AxeSkill>();
	}

	public class AxeSkill : AttackSkill
	{
		public override int Damage => 50;

		public override int Time => 40;
		public override float Speed => 12f;

		public override int Shoot => ModContent.ProjectileType<AxeTechniqueProjectile>();

		public override int UseStyleID => ItemUseStyleID.Swing;

		public override int Scroll => ModContent.ItemType<AxeTechnique>();

		public override bool Attack(Player player, Imbuable imbue, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int damage, float knockback)
		{
			ActivateAbility(player, imbue);
			return true;
		}
	}
}
