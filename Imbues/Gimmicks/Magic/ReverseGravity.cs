using ArcaneOdyssey.Imbues.Base;

namespace ArcaneOdyssey.Imbues.Gimmicks.Magic
{
	public class ReverseGravity : ImbueGimmick
	{
		public override void ModifyHitNPC(Item item, Player player, NPC target, ref NPC.HitModifiers modifiers)
		{
			modifiers.HitDirectionOverride = -modifiers.HitDirection;
		}

		public override void ModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers)
		{
			modifiers.HitDirectionOverride = -modifiers.HitDirection;
		}
	}
}
