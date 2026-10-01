using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Projectiles.Magic.Effects;

namespace ArcaneOdyssey.Gimmicks.Magic
{
	public class FantasiaRifts : ImbueGimmick
	{
		public override void KillEffects(Projectile projectile)
		{
			if (projectile.GetOwner().ownedProjectileCounts[ModContent.ProjectileType<FantasiaRift>()] < 1)
				Projectile.NewProjectile(projectile.GetSource_FromThis(), projectile.Center, Vector2.Zero, ModContent.ProjectileType<FantasiaRift>(), projectile.damage, 0, projectile.owner);
		}
	}
}
