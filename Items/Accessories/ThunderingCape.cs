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
					var proj = Projectile.NewProjectileDirect(player.GetSource_Accessory(Item), new Vector2(Main.screenPosition.X + Main.rand.NextFloat(Main.screenWidth), Main.screenPosition.Y - 16), Vector2.UnitY * 7f, ModContent.ProjectileType<ThunderingEffect>(), Main.rand.Next(20, 50), 0f, player.whoAmI);
					var target = proj.Center.ClosestNPCAt(proj.timeLeft * 7f, false, true);
					if (target is not null)
					{
						proj.position.X = target.Center.X;
						proj.damage = (int)MathHelper.Clamp(target.lifeMax * 0.005f, proj.damage, 1000f);
						proj.netUpdate = true;
						proj.netSpam = 0;
					}
				}
			}
		}
	}
}
