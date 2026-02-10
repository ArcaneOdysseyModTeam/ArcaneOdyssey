using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Projectiles.Relics;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Weapons.Scrolls
{
	public class RainRite : Scroll
	{
		public override bool CanHaveRelic => true;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.useTime = Item.useAnimation = 30;
			Item.damage = 18;
			Item.knockBack = 0f;
			Item.DamageType = OracleDamage.Instance;
			Item.shoot = ModContent.ProjectileType<SpiritRaincloud>();
			Item.shootSpeed = 1f;
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			velocity = -Vector2.UnitY * 5f;
		}

		public override bool CanShoot(Player player) => player.ownedProjectileCounts[Item.shoot] < 1;

		public override void AddRecipes()
		{
			AddRecipe(ItemID.CrimsonRod);
			AddRecipe(ItemID.Vilethorn);
		}
	}
}
