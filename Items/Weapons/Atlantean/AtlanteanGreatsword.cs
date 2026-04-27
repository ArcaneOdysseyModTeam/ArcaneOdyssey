using ArcaneOdyssey.Buffs.DOT;
using ArcaneOdyssey.Items.Base;
using Microsoft.Xna.Framework;
using Terraria.ID;

namespace ArcaneOdyssey.Items.Weapons.Atlantean
{
	public class AtlanteanGreatsword : Weapon
	{
		public override ItemTiers WeaponTier => ItemTiers.Great;

		public override Color Motif => Color.PaleVioletRed;

		public override ItemRarities Rarity => ItemRarities.Rare;

		public override Debuff? WeaponDebuff => Debuff.Create<HeavyBleed>();

		public override float Size => 1.15f;

		public override float Speed => .9f;

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ArcaneOdysseyMod.Sets.atlanteanItem[Type] = true;
			ArcaneOdysseyMod.Sets.greatsword[Type] = true;
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.height = Item.height = 64;
			Item.useStyle = ItemUseStyleID.Swing;
		}
	}
}
