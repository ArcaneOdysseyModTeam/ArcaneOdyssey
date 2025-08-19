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
	public class BlastScroll : DefaultScroll
	{
        public override void SetStaticDefaults()
        {
			ItemID.Sets.ItemsThatAllowRepeatedRightClick[Type] = true;
        }

		public override void SetDefaultsScroll()
		{
			Item.useTime = 15;
			Item.useAnimation = 60;
			Item.damage = 10;
			Item.autoReuse = true;
			Item.mana = 2;
			Item.shootSpeed = 10;
			Item.shoot = ProjectileID.WoodenArrowFriendly; // does not actually shoot
		}

        public override void ScrollRecipe()
        {
			CreateRecipe().AddIngredient<DefaultScroll>().AddIngredient(ItemID.WandofSparking).Register();
        }

        public override bool AltFunctionUse(Player player)
        {
            return CanUseItem(player);
        }
        
        public override bool CanUseItem(Player player)
		{
			return player.GetModPlayer<AOPlayer>().imbue is not null;
		}

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            AOPlayer playah = player.GetModPlayer<AOPlayer>();
            AOMagic magic = playah.imbue;
			if (magic.Spells.TryGetValue(typeof(BlastSpell), out type))
			{
				Projectile.NewProjectile(source, position - new Vector2(0, 20), velocity * magic.AOMagicSpeed, type, (int)Math.Round(damage * (player.altFunctionUse != 2 ? 1 : .75f)), knockback, player.whoAmI, ai2: player.altFunctionUse);
                return false;
            }
			else
			{
				return true;
			}
        }
	}
}
