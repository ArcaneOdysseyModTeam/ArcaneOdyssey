using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Projectiles.Weapons.Abilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Weapons
{
	public class ColossalGreatsword : AOWeapon
	{
		public override float AOSpeed => .65f;
		public override float AOSize => 1.2f;
		public override float AODamage => 1.15f;
		public override int AOValue => 250;
		public override AORarities AORarity => AORarities.Rare;
		public override AOItemTiers AOWeaponTier => AOItemTiers.Good;
		public override WeaponType WeaponsType => WeaponType.Strength;
		public override WeaponAbility? Ability => new(this, Color.PaleVioletRed);

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

		public override bool AltFunctionUse(Player player) => Imbue is not null;

		public override bool CanShoot(Player player) => player.AltUse() && player.ownedProjectileCounts[Item.shoot] < 1;
	}
}
