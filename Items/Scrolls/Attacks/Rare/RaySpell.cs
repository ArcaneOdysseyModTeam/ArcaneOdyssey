using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Projectiles.Magic;
using ArcaneOdyssey.Skills.Base;
using Terraria.DataStructures;

namespace ArcaneOdyssey.Items.Scrolls.Attacks.Rare
{
	public class RaySpell : RareScroll
	{
		public override bool MetConditions() => NPC.downedMechBossAny;
		public override bool CanHaveMagic => true;
		public override ModSkill Skill => ModContent.GetInstance<RaySkill>();
	}

	public class RaySkill : AttackSkill
	{
		public override int Damage => 22;

		public override int Shoot => ModContent.ProjectileType<MagicRay>();

		public override int Scroll => ModContent.ItemType<RaySpell>();

		public override int ManaCost => 12;

		public override int Time => 5;

		public override float Knockback => 1f;
		public override bool Channel => true;
		public override float Speed => 7f;
		public override bool PreActivate(Player player, Imbuable imbue) => player.ownedProjectileCounts[Shoot] < 1;

		public override bool Attack(Player player, Imbuable imbue, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int damage, float knockback)
		{
			imbue.CreateMagicCircle(player, Projectiles.MagicCircleMode.Barrage, false);
			ActivateAbility(player, imbue);
			return true;
		}
	}
}
