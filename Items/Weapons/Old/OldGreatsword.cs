using ArcaneOdyssey.Items.Base;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;


namespace ArcaneOdyssey.Items.Weapons.Old
{
	public class OldGreatsword : Weapon
	{
		public override int Value => 40;
		public override float Size => 1.1f;
		public override float Speed => .9f;
		public override float Damage => 1.05f;
		public override Rarities Rarity => Rarities.Common;
		public override ItemTiers WeaponTier => ItemTiers.Poor;

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<OldRapier>();
			ArcaneOdysseyMod.Sets.greatsword[Type] = true;
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.height = Item.height = 60;
			Item.DamageType = AOUtils.TrueMelee();
			Item.useStyle = ItemUseStyleID.Swing;
		}

		public override Color Motif => Color.Gray;
	}
}
