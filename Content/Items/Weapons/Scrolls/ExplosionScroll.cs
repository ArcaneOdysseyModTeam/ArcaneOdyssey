using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Materials;
using ArcaneOdyssey.Content.Projectiles.Magic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Weapons.Scrolls
{
	public class ExplosionScroll : Scroll
	{
		public override bool CanHaveMagic => true;
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.useAnimation = Item.useTime = (ExplosionSpell.defaultMax - ExplosionSpell.defaultMin).Round();
			Item.damage = 50;
			Item.reuseDelay = 60;
			Item.channel = true;
			Item.DamageType = DamageClass.Magic;
			Item.UseSound = SoundID.Item84;
			Item.mana = 100;
			Item.shoot = ModContent.ProjectileType<ExplosionSpell>();
		}

		public override void AddRecipes()
		{
			CreateRecipe().AddIngredient<EmptyScroll>().AddIngredient(ItemID.Dynamite, 32).Register();
		}

		public override bool AltFunctionUse(Player player) => true;

		public override bool CanUseItem(Player player)
		{
			return base.CanUseItem(player) && player.ownedProjectileCounts[Item.shoot] < 1 && player.ArcaneOdyssey().myCircle == null;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			AOMagic.CreateMagicCircle(Item, player, Item.ArcaneOdyssey().Imbue);
			Projectile.NewProjectile(source, Main.MouseWorld, Vector2.Zero, type, damage, knockback * 1.5f, player.whoAmI);
			return false;
		}
	}
}
