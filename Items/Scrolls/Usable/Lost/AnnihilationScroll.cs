using ArcaneOdyssey.AOPlayers;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Projectiles.Magic;
using ArcaneOdyssey.Imbues.Base;

namespace ArcaneOdyssey.Items.Scrolls.Usable.Lost
{
	public class AnnihilationScroll : LostScroll
	{
		public override bool CanHaveMagic => true;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.damage = 60;
			Item.mana = 200;
			Item.useTime = Item.useAnimation = 40;
			Item.DamageType = DamageClass.Magic;
			Item.shoot = ModContent.ProjectileType<AnnihilationSpell>(); // does not actually shoot
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			player.ArcaneOdyssey()?.StartDash(new Annihilation(this), -2, Imbue, false);
			AOMagic.CreateMagicCircle(Item, player, Imbue, damage);
			return false;
		}

		public override bool CanUseItem(Player player) => base.CanUseItem(player) && player.ownedProjectileCounts[ModContent.ProjectileType<AnnihilationSpell>()] < 1;
	}

	public class Annihilation(AnnihilationScroll scroll) : DashSystem(scroll.Item)
	{
		public override bool Immune => false;

		public override bool LocksPlayer => true;

		public override float DashSpeed => 23;

		public override int Cooldown => 0;

		public override int DashMax => 10;

		public override bool ContactDamage => false;

		public override bool OnHit(Player player, Entity target) => false;

		public override void OnEnd(Player player)
		{
			int damage = 0;
			if (source is Item item)
			{
				damage = item.damage;
			}
			scroll.ActivateAbility(player);
			AOUtils.ShootProjectile(source.GetSource_ItemUse(player), player.Center, player.DirectionTo(Main.MouseWorld) * 10, ModContent.ProjectileType<AnnihilationSpell>(), damage, Knockback, player.whoAmI, Imbue, SecondImbue, true);
		}
	}
}
