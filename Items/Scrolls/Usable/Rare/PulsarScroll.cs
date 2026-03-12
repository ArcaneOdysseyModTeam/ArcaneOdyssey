using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Items.Base;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Items.Scrolls.Usable.Rare
{
	public class PulsarScroll : RareScroll
	{
		public override bool CanHaveMagic => true;
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.damage = 40;
			Item.DamageType = DamageClass.Magic;
			Item.UseSound = SoundID.Item84;
			Item.mana = 50;
			Item.shoot = ProjectileID.WoodenArrowFriendly;
		}

		public override bool AltFunctionUse(Player player) => true;

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			AOMagic.CreateMagicCircle(Item, player, Imbue, damage);
			return false;
		}
	}
}
