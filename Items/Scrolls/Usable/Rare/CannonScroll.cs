using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Items.Base;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Items.Scrolls.Usable.Rare
{
	public class CannonScroll : RareScroll
	{
		public override bool CanHaveMagic => true;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.damage = 23;
			Item.mana = 30;
			Item.DamageType = DamageClass.Magic;
			Item.shootSpeed = 7f;
			Item.useTime = Item.useAnimation = 20;
			Item.shoot = ProjectileID.WoodenArrowFriendly; // does not actually shoot
			Item.channel = true;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			AOMagic.CreateMagicCircle(Item, player, Imbue, damage);
			return false;
		}

		public override bool CanUseItem(Player player) => base.CanUseItem(player) && player.ownedProjectileCounts[Imbue.GetSkill("Cannon")] < 1;
		
	}
}
