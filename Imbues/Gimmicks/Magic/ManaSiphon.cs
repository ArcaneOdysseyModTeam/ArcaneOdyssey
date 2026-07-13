using ArcaneOdyssey.Imbues.Base;
using System;
using Terraria;

namespace ArcaneOdyssey.Imbues.Gimmicks.Magic
{
	public class ManaSiphon : ImbueGimmick
	{
		public override void OnHitNPC(Item item, Player player, NPC target, NPC.HitInfo hit, int damageDone)
		{
			var dam = (8 * MathF.Log(damageDone + 1)).Round();
			player.statMana = Utils.Clamp(player.statMana + dam, 0, player.statManaMax2);
			player.ManaEffect(dam);
		}

		public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
		{
			var dam = (8 * MathF.Log(damageDone + 1)).Round();
			if (projectile.TryGetOwner(out var player))
			{
				player.statMana = Utils.Clamp(player.statMana + dam, 0, player.statManaMax2);
				player.ManaEffect(dam);
			}
		}

		public override void OnHitNPC(Imbuable imbue, Player player, NPC target, NPC.HitInfo hit, int damageDone)
		{
			var dam = (8 * MathF.Log(damageDone + 1)).Round();
			player.statMana = Utils.Clamp(player.statMana + dam, 0, player.statManaMax2);
			player.ManaEffect(dam);
		}
	}
}
