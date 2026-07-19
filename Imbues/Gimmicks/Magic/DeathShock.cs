using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Projectiles.Magic.Effects;

namespace ArcaneOdyssey.Imbues.Gimmicks.Magic
{
	public class DeathShock : ImbueGimmick
	{
		public override void KillEffects(Projectile projectile)
		{
			Projectile.NewProjectile(projectile.GetSource_FromThis(), projectile.Center, Vector2.Zero, ModContent.ProjectileType<DeathShockProjectile>(), projectile.damage * 10, 0, projectile.owner);
		}
	}
}
