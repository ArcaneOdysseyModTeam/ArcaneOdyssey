using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Projectiles.Weapons;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Weapons
{
	public class CommodoreKaiSabre : AORangedOrMeleeWeapon
	{
		public override float AOSpeed => 1.1f;
		public override float AOSize => 1.1f;
		public override float AODamage => .925f;
		public override int AOValue => 200;
		public override AORarities AORarity => AORarities.Uncommon;
		public override AOItemTiers AOWeaponTier => AOItemTiers.Good;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.width = 52;
			Item.height = 54;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.shoot = ModContent.ProjectileType<KatanaSlash>();
		}
	}
}
