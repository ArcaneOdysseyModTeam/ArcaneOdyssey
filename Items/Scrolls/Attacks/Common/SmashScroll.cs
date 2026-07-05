using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Projectiles.Berserker;
using ArcaneOdyssey.Skills.Base;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Items.Scrolls.Attacks.Common
{
	public class SmashScroll : CommonScroll
	{
		public override bool MetConditions() => AOUtils.BossesKilled>0;
		public override bool CanHaveFS => true;
		public override ModSkill Skill => ModContent.GetInstance<SmashSkill>();
	}

	public class SmashSkill : AttackSkill
	{
		public override int Damage => 30;

		public override int Shoot => ModContent.ProjectileType<ShockwaveSmash>();

		public override int Scroll => ModContent.ItemType<SmashScroll>();

		public override float Speed => 5f;

		public override bool Attack(Player player, Imbuable imbue, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int damage, float knockback)
		{
			ActivateAbility(player, imbue);
			return true;
		}
	}
}
