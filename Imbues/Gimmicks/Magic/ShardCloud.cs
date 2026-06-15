using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Projectiles.Magic.Effects;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Imbues.Gimmicks.Magic
{
	public class ShardCloud : ImbueGimmick
	{
		public override void KillEffects(Projectile projectile)
		{
			if (projectile.GetOwner().ownedProjectileCounts[ModContent.ProjectileType<PrismLinger>()] < 3)
				Projectile.NewProjectile(projectile.GetSource_FromThis(), projectile.Center, Vector2.Zero, ModContent.ProjectileType<PrismLinger>(), projectile.damage / 6, 0, projectile.owner);
		}
	}
}
