using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Projectiles.Magic.Effects;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Imbues.Gimmicks
{
	public class AetherShocks : ImbueGimmick
	{
		public override void KillEffects(Projectile projectile)
		{
			if (projectile.GetOwner().ownedProjectileCounts[ModContent.ProjectileType<AetherExplosion>()] < 3)
			{
				Projectile.NewProjectile(projectile.GetSource_FromThis(), projectile.Center, Vector2.Zero, ModContent.ProjectileType<AetherExplosion>(), projectile.damage / 4, 0, projectile.owner);
			}
		}
	}
}
