using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Projectiles.Relics;
using ArcaneOdyssey.Skills.Base;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Items.Scrolls.Attacks.Common
{
	public class EffervescenceRite : CommonScroll
	{
		public override bool MetConditions() => AOUtils.BossesKilled > 0;
		public override bool CanHaveRelic => true;

		public override ModSkill Skill => ModContent.GetInstance<EffervescenceSkill>();
	}

	public class EffervescenceSkill : AttackSkill
	{
		public override int Damage => 50;

		public override int Time => 40;

		public override int Shoot => ModContent.ProjectileType<Effervescence>();

		public override int Scroll => ModContent.ItemType<EffervescenceRite>();

		public override bool Attack(Player player, Imbuable imbue, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int damage, float knockback)
		{
			ActivateAbility(player, imbue);
			return true;
		}

		public override bool PreActivate(Player player, Imbuable imbue) => player.ownedProjectileCounts[Shoot] < 1;
	}
}
