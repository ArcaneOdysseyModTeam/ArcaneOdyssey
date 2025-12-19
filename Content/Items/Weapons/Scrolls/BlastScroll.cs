using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Materials;
using ArcaneOdyssey.Content.Projectiles.Relics;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Weapons.Scrolls
{
	public class BlastScroll : Scroll
	{
		public override bool CanHaveMagic => true;
		public override bool CanHaveRelic => true;
		public override void SetStaticDefaults()
		{
			ItemID.Sets.ItemsThatAllowRepeatedRightClick[Type] = true;
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.useTime = 15;
			Item.useAnimation = 15;
			Item.damage = 10;
			Item.mana = 15;
			Item.channel = true;
			Item.DamageType = DamageClass.Magic;
			Item.shoot = ModContent.ProjectileType<SpiritBlast>(); // does not actually shoot usually
			Item.shootSpeed = 7f;
		}

		public override void AddRecipes()
		{
			CreateRecipe().AddIngredient<EmptyScroll>().AddIngredient(ItemID.WandofSparking).Register();
		}

		public override bool AltFunctionUse(Player player) => Imbue is AOMagic;

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			if (Imbue?.CreateChargingEffect(Item, player) is null && player.ownedProjectileCounts[type] < 3)
			{
				Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
			}
			return false;
		}
	}
}
