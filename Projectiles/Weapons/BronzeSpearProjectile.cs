using ArcaneOdyssey.Items.Weapons.Bronze;
using ArcaneOdyssey.Projectiles.Base;

namespace ArcaneOdyssey.Projectiles.Weapons
{
	public class BronzeSpearProjectile : BaseSpearProjectile
	{
		public override string Texture => AOUtils.GetTexture<BronzeSpear>();
	}
}
