using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Projectiles.Berserker;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Scrolls.Weapons.Common
{
	public class ShotScroll : CommonScroll
	{
		public override bool CanHaveFS => true;
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.useTime = Item.useAnimation = 30;
			Item.damage = 50;
			Item.shoot = ModContent.ProjectileType<ShotTechnique>();
			Item.shootSpeed = 5f;
			Item.DamageType = DamageClass.Melee;
		}
	}
}
