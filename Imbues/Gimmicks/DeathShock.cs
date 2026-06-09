using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Projectiles.Magic.Effects;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Imbues.Gimmicks
{
	public class DeathShock : ImbueGimmick
	{
		public override void KillEffects(Projectile projectile)
		{
			Projectile.NewProjectile(projectile.GetSource_FromThis(), projectile.Center, Vector2.Zero, ModContent.ProjectileType<AetherLightningAftershock>(), projectile.damage * 10, 0, projectile.owner);
		}
	}
}
