using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Projectiles.Abilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Items.Weapons
{
	public class CommodoreKaiSabre : Weapon
	{
		public override float Speed => 1.1f;
		public override float Size => 1.1f;
		public override float Damage => .925f;
		public override int Value => 200;
		public override ItemRarities Rarity => ItemRarities.Uncommon;
		public override ItemTiers WeaponTier => ItemTiers.Average;

		public override Color Motif => Color.Red;

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
