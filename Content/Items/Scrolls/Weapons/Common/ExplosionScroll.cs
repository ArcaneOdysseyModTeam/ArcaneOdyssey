using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Imbues.Relics;
using ArcaneOdyssey.Content.Projectiles.Helpers;
using ArcaneOdyssey.Content.Projectiles.Magic;
using ArcaneOdyssey.Content.Projectiles.Relics;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Scrolls.Weapons.Common
{
	public class ExplosionScroll : CommonScroll
	{
		public override bool CanHaveMagic => true;
		public override bool CanHaveRelic => true;
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.damage = 50;
			Item.reuseDelay = 60;
			Item.channel = true;
			Item.DamageType = DamageClass.Magic;
			Item.UseSound = SoundID.Item84;
			Item.mana = 100;
			Item.shoot = ModContent.ProjectileType<ExplosionSpell>();
		}

		public override void UpdateInventory(Player player)
		{
			base.UpdateInventory(player);
			if (Imbue is SpiritEnergy)
			{
				Item.DamageType = OracleDamage.Instance;
			}
			else
			{
				Item.DamageType = DamageClass.Magic;
			}
		}

		public override void ModifyManaCost(Player player, ref float reduce, ref float mult)
		{
			if (Imbue is SpiritEnergy)
				mult *= 0;
		}

		public override bool AltFunctionUse(Player player) => true;

		public override bool CanUseItem(Player player) => base.CanUseItem(player) && player.ownedProjectileCounts[Item.shoot] < 1 && player.ArcaneOdyssey().myCircle == null;
		
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			if (Imbue is AOMagic)
			{
				AOMagic.CreateMagicCircle(Item, player, Imbue, damage);
				Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
			}
			else
			{
				Projectile.NewProjectile(source, player.MountedCenter, Vector2.Zero, ModContent.ProjectileType<RotatingMagicCircle>(), 0, 0f, player.whoAmI, 0, player.altFunctionUse);
				Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<SpiritExplosion>(), damage, knockback, player.whoAmI);
			}
			return false;
		}
	}
}
