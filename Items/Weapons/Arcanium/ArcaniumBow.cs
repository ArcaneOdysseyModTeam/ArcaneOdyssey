using ArcaneOdyssey.DamageClasses;
using ArcaneOdyssey.Items.Base;
using Terraria.Audio;

namespace ArcaneOdyssey.Items.Weapons.Arcanium
{
	public class ArcaniumBow : ArcaniumWeapon
	{
		public override string Texture => AOUtils.TerrariaItemTexture(ItemID.IronBow);

		public override ItemTiers WeaponTier => ItemTiers.Good;

		public override ItemRarities Rarity => ItemRarities.Rare;

		public override SoundStyle UseSound => SoundID.Item5;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.DamageType = ModContent.GetInstance<RangedArcaniumDamageClass>();
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.shoot = ProjectileID.WoodenArrowFriendly;
			Item.useAmmo = AmmoID.Arrow;
			Item.shootSpeed = 9f;
		}

		public override Vector2? HoldoutOffset() => new();
	}
}
