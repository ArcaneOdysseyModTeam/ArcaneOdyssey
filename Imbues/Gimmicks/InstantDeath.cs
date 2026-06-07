using ArcaneOdyssey.Imbues.Base;
using Terraria;

namespace ArcaneOdyssey.Imbues.Gimmicks
{
	public class InstantDeath : ImbueGimmick
	{
		public override void OnHitNPC(Item item, Player player, NPC target, NPC.HitInfo hit, int damageDone)
		{
			if (target.lifeMax < (player.statLifeMax2 * 2))
			{
				target.StrikeInstantKill();
			}
		}

		public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
		{
			if (projectile.TryGetOwner(out var owner))
			{
				if (target.lifeMax < (owner.statLifeMax2 * 2))
				{
					target.StrikeInstantKill();
				}
			}
		}
	}
}
