using ArcaneOdyssey.AOPlayers;
using ArcaneOdyssey.Items.Base;
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
			Item.reuseDelay = 2;
			Item.useStyle = ItemUseStyleID.Thrust;
			Item.shoot = ModContent.ProjectileType<SanguineThrow>();
			Item.shootSpeed = 10f * Speed;
			Item.autoReuse = true;
		}

		private bool canshoot;

		private bool canSwing;

		public override bool CanUseItem(Player player)
		{
			canSwing = !canSwing;
			if (!canSwing && player.ArcaneOdyssey().OnCooldown<SanguineThrowCooldown>())
			{
				canshoot = false;
				Item.useStyle = ItemUseStyleID.Thrust;
				Item.noMelee = false;
				Item.noUseGraphic = false;
				return canSwing;
			}
			if (!player.ArcaneOdyssey().OnCooldown<SanguineThrowCooldown>())
			{
				canSwing = true;
				canshoot = true;
				Item.useStyle = ItemUseStyleID.Swing;
				Item.noMelee = true;
				Item.noUseGraphic = true;
				return true;
			}
			return base.CanUseItem(player);
		}

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ArcaneOdysseyMod.Sets.dagger[Type] = true;
		}

		public override bool CanShoot(Player player) => canshoot;

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			canshoot = false;
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
