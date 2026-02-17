using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Projectiles.Magic;
using ArcaneOdyssey.PlayerClasses;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Weapons.Scrolls
{
	public class AnnihilationScroll : Scroll
	{
		public override ScrollTier Tier => ScrollTier.Lost;
		public override bool CanHaveMagic => true;
		public override int AOValue => 2000;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.damage = 60;
			Item.mana = 200;
			Item.useTime = Item.useAnimation = 40;
			Item.DamageType = DamageClass.Magic;
			Item.shoot = ProjectileID.WoodenArrowFriendly; // does not actually shoot
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			player.ArcaneOdyssey()?.StartDash(new Annihilation(Item), -2, Imbue, false);
			AOMagic.CreateMagicCircle(Item, player, Imbue, damage);
			return false;
		}

		public override bool CanUseItem(Player player) => base.CanUseItem(player) && player.ownedProjectileCounts[ModContent.ProjectileType<AnnihilationSpell>()] < 1;
	}

	public class Annihilation(Entity source) : DashSystem(source)
	{
		public override bool Immune => false;

		public override bool AnyDirection => true;

		public override float DashSpeed => 23;

		public override int Cooldown => 0;

		public override int DashMax => 10;

		public override int Damage => 0;

		public override bool OnHit(Player player, Entity target) => false;

		public override void OnEnd(Player player)
		{
			int damage = 0;
			if (source is Item item)
			{
				damage = item.damage;
			}
			AOUtils.ShootProjectile(source.GetSource_ItemUse(player), player.Center, player.DirectionTo(Main.MouseWorld) * 10, ModContent.ProjectileType<AnnihilationSpell>(), damage, Knockback, player.whoAmI, Imbue, SecondImbue, true);
		}
	}
}
