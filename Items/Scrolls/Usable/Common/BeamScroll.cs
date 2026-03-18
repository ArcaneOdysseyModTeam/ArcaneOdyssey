using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Projectiles.Magic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Items.Scrolls.Usable.Common
{
	public class BeamScroll : CommonScroll
	{
		public override bool CanHaveMagic => true;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.damage = 100;
			Item.channel = true;
			Item.InterruptChannelOnHurt = true;
			Item.mana = 30;
			Item.knockBack = 0f;
			Item.DamageType = DamageClass.Magic;
			Item.shoot = ModContent.ProjectileType<BeamSpell>(); // does not actually shoot
			Item.useAnimation = Item.useTime = 40;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			Imbuable.CreateMagicCircle(Item, player, Projectiles.MagicCircleMode.Basic, false, type);
			return false;
		}
	}
}
