using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Projectiles.Berserker.Effects;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Imbues.Gimmicks.FightingStyle
{
	public class PowderBurst : ImbueGimmick
	{
		public override void OnHitNPC(Item item, Player player, NPC target, NPC.HitInfo hit, int damageDone)
		{
			Projectile.NewProjectile(item.GetSource_ItemUse(player), target.Center, Vector2.Zero, ModContent.ProjectileType<PowderExplosion>(), damageDone / 2, 3f, player.whoAmI);
		}

		public override void KillEffects(Projectile projectile)
		{
			Projectile.NewProjectile(projectile.GetSource_FromThis(), projectile.Center, Vector2.Zero, ModContent.ProjectileType<PowderExplosion>(), projectile.damage / 2, 3f, projectile.owner);
		}
	}
}
