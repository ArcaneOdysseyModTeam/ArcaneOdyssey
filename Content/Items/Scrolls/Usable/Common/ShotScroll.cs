using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Projectiles.Berserker;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Scrolls.Usable.Common
{
	public class ShotScroll : CommonScroll
	{
		public override bool CanHaveFS => true;
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.useTime = Item.useAnimation = 30;
			Item.damage = 50;
			Item.shoot = ModContent.ProjectileType<ShotTechnique>();
			Item.shootSpeed = 5f;
			Item.DamageType = DamageClass.Melee;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			ActivateAbility(player, type);
			return true;
		}
	}
}
