using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Projectiles;

namespace ArcaneOdyssey.Items.Accessories
{
	[AutoloadEquip(EquipType.Back)]
	public class ThunderingCape : BaseItem
	{
		public override ItemRarities Rarity => ItemRarities.Uncommon;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.expert = true;
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			if (player.whoAmI == Main.myPlayer)
			{
				if (Main.rand.NextBool(5 * 60))
				{
					Projectile.NewProjectile(player.GetSource_Accessory(Item), new Vector2(Main.screenPosition.X + Main.rand.NextFloat(Main.screenWidth), Main.screenPosition.Y - 16), Vector2.UnitY, ModContent.ProjectileType<ThunderingEffect>(), Main.rand.Next(20, 50), 0f, player.whoAmI);
				}
			}
		}
	}
}
