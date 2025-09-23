using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Projectiles.Base;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using ArcaneOdyssey.Content.Items.Materials;
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey.Content.Items.Scrolls
{
	public class BlastScroll : MagicScroll
	{
		public override void SetStaticDefaults()
		{
			ItemID.Sets.ItemsThatAllowRepeatedRightClick[Type] = true;
		}

		public override void SetDefaultsScroll()
		{
			Item.useTime = 15;
			Item.useAnimation = 15;
			Item.damage = 10;
			Item.UseSound = SoundID.Item84;
			Item.mana = 5;
			Item.channel = true;
			Item.DamageType = DamageClass.Magic;
			Item.shootSpeed = 10;
			Item.shoot = ProjectileID.VortexLaser; // does not actually shoot
		}

		public override void ScrollRecipe()
		{
			CreateRecipe().AddIngredient<EmptyScroll>().AddIngredient(ItemID.WandofSparking).Register();
		}

		public override bool AltFunctionUse(Player player)
		{
			return CanUseItem(player);
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			AOMagic.CreateMagicCircle(Item, player, Item.ArcaneOdyssey().imbue);
			return false;
		}
	}
}
