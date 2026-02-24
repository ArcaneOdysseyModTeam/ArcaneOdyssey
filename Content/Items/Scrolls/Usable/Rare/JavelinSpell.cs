using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Projectiles.Magic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Scrolls.Usable.Rare
{
	public class JavelinSpell : RareScroll
	{
		public override string Texture => AOUtils.GetTexture<ArrayScroll>();
		public override bool CanHaveMagic => true;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.damage = 55;
			Item.mana = 45;
			Item.channel = true;
			Item.useTime = Item.useAnimation = 20;
			Item.DamageType = DamageClass.Magic;
			Item.shoot = ModContent.ProjectileType<Javelin>();
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			AOMagic.CreateMagicCircle(Item, player, Imbue, damage);
			return true;
		}

		public override bool CanUseItem(Player player) => base.CanUseItem(player) && player.ownedProjectileCounts[Item.shoot] < 1;
	}
}
