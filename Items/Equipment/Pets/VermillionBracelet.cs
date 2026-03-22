using ArcaneOdyssey.Buffs.Pets;
using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Projectiles.Pets;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Items.Equipment.Pets
{
	public class VermillionBracelet : AOBaseItem
	{
		public override AORarities AORarity => AORarities.Uncommon;

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
			Item.value = AOUtils.GalleonToCopper(60);
			Item.shopCustomPrice = Item.buyPrice(gold: 1);
		}

		public override void UseStyle(Player player, Rectangle heldItemFrame)
		{
			if (player.whoAmI == Main.myPlayer && player.ItemTimeIsZero)
			{
				player.AddBuff(Item.buffType, 3600, true);
			}
		}
	}
}
