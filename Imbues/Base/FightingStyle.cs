using ArcaneOdyssey.Projectiles.Berserker;
using Terraria;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Imbues.Base
{
	public abstract class FightingStyle : Imbuable
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.DamageType = AOUtils.TrueMeleeNoSpeed();
			Item.shoot = ModContent.ProjectileType<BasicStrike>();
			Item.autoReuse = true;
			Item.damage = 15 + (100 * (int)ImbuableTier);
			Item.shootSpeed = 2f;
			Item.knockBack = 10f;
		}

		public override bool CanShoot(Player player) => !player.AltUse();
	}
}
