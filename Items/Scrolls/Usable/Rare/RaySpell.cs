using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Projectiles.Magic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Items.Scrolls.Usable.Rare
{
	public class RaySpell : RareScroll
	{
		public override bool MetConditions() => NPC.downedMechBossAny;
		public override bool CanHaveMagic => true;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.mana = 12;
			Item.DamageType = DamageClass.Magic;
			Item.shootSpeed = 7f;
			Item.channel = true;
			Item.damage = 22;
			Item.useTime = Item.useAnimation = 5;
			Item.knockBack = 1f;
			Item.shoot = ModContent.ProjectileType<MagicRay>();
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			Imbuable.CreateMagicCircle(Item, player, Projectiles.MagicCircleMode.Barrage, false);
			ActivateAbility(player);
			return true;
		}

		public override bool CanUseItem(Player player) => base.CanUseItem(player) && player.ownedProjectileCounts[Item.shoot] < 1;

		public override void ModifyManaCost(Player player, ref float reduce, ref float mult)
		{
			mult = ApplySpeed(mult, true);
		}
	}
}
