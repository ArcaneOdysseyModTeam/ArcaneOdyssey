using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Materials;
using ArcaneOdyssey.Content.Projectiles.Base;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey.Content.Items.Scrolls
{
	public class BlastScroll : EmptyMagicScroll
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
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item84;
			Item.mana = 5;
			Item.shootSpeed = 10;
			Item.shoot = ProjectileID.WoodenArrowFriendly; // does not actually shoot
		}

		public override void ScrollRecipe()
		{
			CreateRecipe().AddIngredient<EmptyMagicScroll>().AddIngredient(ItemID.WandofSparking).Register();
		}

		public override bool AltFunctionUse(Player player)
		{
			return CanUseItem(player);
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			AOPlayer playah = player.ArcaneOdyssey();
			if (playah.imbue.Skills.TryGetValue(typeof(BlastSpell), out type))
			{
				Projectile.NewProjectile(source, position, velocity * playah.imbue.AOScrollSpeed, type, (int)Math.Round(damage * (player.altFunctionUse != 2 ? 1 : .75f)), knockback, player.whoAmI, ai2: player.altFunctionUse);
				return false;
			}
			else
			{
				return true; // shoots the wooden arrow if the blast isnt found
			}
		}
	}
}
