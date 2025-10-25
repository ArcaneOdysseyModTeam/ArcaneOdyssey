using ArcaneOdyssey.Content.Items.Base;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Weapons.Scrolls
{
	public class PulsarScroll : MagicScroll
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.damage = 60;
            Item.DamageType = DamageClass.Magic;
            Item.UseSound = SoundID.Item84;
			Item.mana = 50;
            Item.shoot = ProjectileID.WoodenArrowFriendly;
		}

		public override void AddRecipes()
		{
			CreateRecipe().AddIngredient<BlastScroll>().AddIngredient(ItemID.ExplosivePowder, 5).Register();
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			AOMagic.CreateMagicCircle(Item, player, Item.ArcaneOdyssey().Imbue);
			return false;
		}
	}
}
