using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Projectiles.Relics;
using Terraria.DataStructures;

namespace ArcaneOdyssey.Items.Scrolls.Usable.Common
{
	public class RainRite : CommonScroll
	{
		public override bool CanHaveRelic => true;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.useTime = Item.useAnimation = 30;
			Item.damage = 18;
			Item.knockBack = 0f;
			Item.DamageType = DamageClass.Summon;
			Item.shoot = ModContent.ProjectileType<SpiritRaincloud>();
			Item.shootSpeed = 1f;
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			velocity = -Vector2.UnitY * 5f;
			damage /= 10;
		}

		public override bool CanUseItem(Player player) => base.CanUseItem(player) && player.ownedProjectileCounts[Item.shoot] < 1;

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			ActivateAbility(player);
			return true;
		}
	}
}
