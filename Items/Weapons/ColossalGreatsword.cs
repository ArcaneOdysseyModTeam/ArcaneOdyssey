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
	public class ColossalGreatsword : Weapon
	{
		public override float Speed => .65f;
		public override float Size => 1.2f;
		public override float Damage => 1.15f;
		public override int Value => 250;
		public override ItemRarities Rarity => ItemRarities.Rare;
		public override ItemTiers WeaponTier => ItemTiers.Good;
		public override Color Motif => Color.PaleVioletRed;

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ArcaneOdysseyMod.Sets.greatsword[Type] = true;
			ArcaneOdysseyMod.Sets.weaponType[Type] = WeaponType.Strength;
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.width = 86;
			Item.shootSpeed = 5;
			Item.height = 86;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTurn = true;
			Item.shoot = ModContent.ProjectileType<ColossalCleave>();
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			player.ArcaneOdyssey().SetCooldown<ColossalCleaveCooldown>();
			ActivateAbility(player, false);
			return true;
		}

		public override bool AltFunctionUse(Player player) => !player.ArcaneOdyssey().OnCooldown<ColossalCleaveCooldown>();

		public override bool CanShoot(Player player) => player.AltUse();
	}

	public class ColossalCleaveCooldown : DisplayedCooldown
	{
		public override string Texture => AOUtils.GetTexture<ColossalGreatsword>();

		public override int CooldownLength => 180;
	}
}
