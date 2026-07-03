using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Projectiles;
using ArcaneOdyssey.Projectiles.Magic;
using ArcaneOdyssey.Skills.Base;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Items.Scrolls.Usable.Common
{
	public class CannonScroll : CommonScroll
	{
		public override bool MetConditions() => AOUtils.BossesKilled > 0;
		public override bool CanHaveMagic => true;

		public override ModSkill Skill => ModContent.GetInstance<CannonSkill>();
	}

	public class CannonSkill : AttackSkill
	{
		public override int Damage => 23;

		public override bool Channel => true;

		public override int ManaCost => 30;

		public override int Time => 20;

		public override int Scroll => ModContent.ItemType<CannonScroll>();

		public override int Shoot => ModContent.ProjectileType<CannonSpell>();

		public override bool Attack(Player player, Imbuable imbue, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int damage, float knockback)
		{
			imbue.CreateMagicCircle(player, MagicCircleMode.Basic, false);
			Projectile.NewProjectile(source, player.MountedCenter + (player.SafeDirectionTo(Main.MouseWorld) * 94), velocity, Shoot, damage, knockback, player.whoAmI);
			return false;
		}

		public override bool PreActivate(Player player, Imbuable imbue) => player.ownedProjectileCounts[Shoot] < 1;
	}
}
