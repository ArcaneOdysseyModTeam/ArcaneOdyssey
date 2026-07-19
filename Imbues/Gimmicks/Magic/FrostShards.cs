using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Projectiles.Magic.Effects;

namespace ArcaneOdyssey.Imbues.Gimmicks.Magic
{
	public class FrostShards : ImbueGimmick
	{
		public override void KillEffects(Projectile projectile)
		{
			for (int i = 0; i < 3; i++)
			{
				var angle = Main.rand.NextFloat(MathHelper.TwoPi).ToRotationVector2() * 7f;
				angle.Y *= 0.35f;
				if (Main.LocalPlayer.ownedProjectileCounts[ModContent.ProjectileType<FrostmetalShard>()] < 3)
				{
					var proj = Projectile.NewProjectileDirect(projectile.GetSource_FromThis(), projectile.Center, angle, ModContent.ProjectileType<FrostmetalShard>(), projectile.damage / 6, projectile.knockBack / 6, projectile.owner);
					proj.frame = i;
				}
			}
		}
	}
}
