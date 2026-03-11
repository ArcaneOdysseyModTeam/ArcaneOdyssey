using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Projectiles.Relics;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Scrolls.Usable.Common
{
	public class BlastScroll : CommonScroll
	{
		public override bool CanHaveRelic => true;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.useTime = Item.useAnimation = 67;
			Item.damage = 20;
			Item.DamageType = DamageClass.Summon;
			Item.shoot = ModContent.ProjectileType<SpiritBlast>();
			Item.shootSpeed = 7f;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			ActivateAbility(player);
			return true;
		}
	}
}
