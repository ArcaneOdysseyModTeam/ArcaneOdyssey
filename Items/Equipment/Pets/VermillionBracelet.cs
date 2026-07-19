using ArcaneOdyssey.Buffs.Pets;
using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Projectiles.Pets;

namespace ArcaneOdyssey.Items.Equipment.Pets
{
	public class VermillionBracelet : BaseItem
	{
		public override ItemRarities Rarity => ItemRarities.Uncommon;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.UseSound = SoundID.Meowmere;
			Item.noMelee = true;
			Item.useAnimation = 20;
			Item.useTime = 20;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.buffType = ModContent.BuffType<IrisBuff>();
			Item.shoot = ModContent.ProjectileType<Iris>();
			Item.master = true;
		}

		public override int Value => 60;

		public override void UseStyle(Player player, Rectangle heldItemFrame)
		{
			if (player.whoAmI == Main.myPlayer && player.ItemTimeIsZero)
			{
				player.AddBuff(Item.buffType, 3600, true);
			}
		}
	}
}
