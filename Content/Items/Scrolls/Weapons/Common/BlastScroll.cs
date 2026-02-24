using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Projectiles.Relics;
using Terraria;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Scrolls.Weapons.Common
{
	public class BlastScroll : CommonScroll
	{
		public override bool CanHaveRelic => true;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.useTime = Item.useAnimation = 67;
			Item.damage = 20;
			Item.DamageType = DamageClass.Summon;
			Item.shoot = ModContent.ProjectileType<SpiritBlast>();
			Item.shootSpeed = 7f;
		}
	}
}
