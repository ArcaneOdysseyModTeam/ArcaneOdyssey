using ArcaneOdyssey.Content.Projectiles.Base;

namespace ArcaneOdyssey.Content.Projectiles.Weapons
{
	public class WoodenStaffProjectile : BaseStaffProjectile
	{
		public override float AOSpeed => 1.05f;
		public override float AOSize => .9f;
		public override float AODamage => 1f;
		public override AODebuffRequirement? Debuff => null;
	}
}
