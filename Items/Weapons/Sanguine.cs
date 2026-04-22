using ArcaneOdyssey.AOPlayers;
using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Items.Weapons.Bronze;
using ArcaneOdyssey.Projectiles.Abilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Items.Weapons
{
	public class Sanguine : Weapon
	{
		public override int Value => 125;

		public override ItemTiers WeaponTier => ItemTiers.Poor; // unfortunately a pre boss item

		public override Color Motif => Color.Red;

		public override Rarities Rarity => Rarities.Uncommon;

		public override float Damage => 1.1f;
		public override float Speed => 1.05f;
		public override float Size => .85f;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.height = Item.width = 44;
			Item.useStyle = ItemUseStyleID.Thrust;
			Item.shoot = ModContent.ProjectileType<SanguineThrow>();
			Item.shootSpeed = 10f * Speed;
			Item.autoReuse = true;
		}

		public override bool CanUseItem(Player player)
		{
			if (player.ArcaneOdyssey().OnCooldown<SanguineThrowCooldown>())
			{
				Item.noUseGraphic = false;
				Item.noMelee = false;
			}
			else
			{
				Item.noUseGraphic = true;
				Item.noMelee = true;
			}
			return base.CanUseItem(player);
		}

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ArcaneOdysseyMod.Sets.dagger[Type] = true;
		}

		public override bool CanShoot(Player player) => !player.ArcaneOdyssey().OnCooldown<SanguineThrowCooldown>();

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			ActivateAbility(player, true);
			player.ArcaneOdyssey().SetCooldown<SanguineThrowCooldown>();
			return true;
		}
	}

	public class SanguineThrowCooldown : DisplayedCooldown
	{
		public override string Texture => AOUtils.GetTexture<Sanguine>();

		public override int CooldownLength => 60;
	}
}
