using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Projectiles.Magic;
using Terraria.DataStructures;

namespace ArcaneOdyssey.Items.Scrolls.Usable.Common
{
	public class BarrageSpell : CommonScroll
	{
		public override bool MetConditions() => NPC.downedBoss2;
		public override bool CanHaveMagic => true;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.damage = 5;
			Item.mana = 5;
			Item.DamageType = DamageClass.Magic;
			Item.shootSpeed = 7;
			Item.channel = true;
			Item.useTime = Item.useAnimation = 10;
			Item.shoot = ProjectileID.WoodenArrowFriendly; // does not actually shoot
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			Imbuable.CreateMagicCircle(Item, player, Projectiles.MagicCircleMode.Barrage, false, ModContent.ProjectileType<BlastSpell>(), spread: ApplySpeed(MathHelper.PiOver4 / 2f));
			return false;
		}

		public override void ModifyManaCost(Player player, ref float reduce, ref float mult)
		{
			mult = ApplySpeed(mult, true);
		}
	}
}
