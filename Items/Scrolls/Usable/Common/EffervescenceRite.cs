using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Projectiles.Relics;
using Terraria.DataStructures;

namespace ArcaneOdyssey.Items.Scrolls.Usable.Common
{
	public class EffervescenceRite : CommonScroll
	{
		public override bool MetConditions() => AOUtils.BossesKilled>0;
		public override bool CanHaveRelic => true;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.damage = 50;
			Item.useTime = Item.useAnimation = 40;
			Item.DamageType = DamageClass.Summon;
			Item.shoot = ModContent.ProjectileType<Effervescence>();
			Item.autoReuse = true;
			Item.shootSpeed = 1f;
		}

		public override bool CanUseItem(Player player) => base.CanUseItem(player) && player.ownedProjectileCounts[Item.shoot] < 1;

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			ActivateAbility(player);
			return true;
		}
	}
}
