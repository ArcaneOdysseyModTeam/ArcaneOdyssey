using ArcaneOdyssey.Imbues.Base;
using Terraria;

namespace ArcaneOdyssey.Imbues.Gimmicks
{
	public class ManaSiphon : ImbueGimmick
	{
		public override void OnHitNPC(Item item, Player player, NPC target, NPC.HitInfo hit, int damageDone)
		{
			var dam = damageDone / 4;
			player.statMana = Utils.Clamp(player.statMana + dam, 0, player.statManaMax2);
			player.ManaEffect(dam);
		}

		public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
		{
			var dam = damageDone / 4;
			if (projectile.TryGetOwner(out var player))
			{
				player.statMana = Utils.Clamp(player.statMana + dam, 0, player.statManaMax2);
				player.ManaEffect(dam);
			}
		}
	}
}
