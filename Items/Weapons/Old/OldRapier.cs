using ArcaneOdyssey.Items.Base;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace ArcaneOdyssey.Items.Weapons.Old
{
	public class OldRapier : Weapon
	{
		public override int Value => 20;
		public override float Size => .9f;
		public override float Speed => 1.025f;
		public override float AODamage => 1.025f;
		public override Rarities Rarity => Rarities.Common;
		public override ItemTiers WeaponTier => ItemTiers.Poor;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.DamageType = AOUtils.TrueMelee();
			Item.height = Item.height = 46;
			Item.useStyle = ItemUseStyleID.Rapier;
			Item.DamageType = AOUtils.TrueMelee();
			Item.useTurn = true;
		}

		private bool canSwing = true;
		public override bool CanUseItem(Player player)
		{
			canSwing = !canSwing;
			if (!canSwing)
			{
				if (Item.useStyle == ItemUseStyleID.Thrust)
					Item.useStyle = ItemUseStyleID.Swing;
				else
					Item.useStyle = ItemUseStyleID.Thrust;
			}
			return canSwing;
		}

		public override Color Motif => Color.Gray;

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<OldSword>();
			ArcaneOdysseyMod.Sets.rapier[Type] = true;
		}
	}
}
