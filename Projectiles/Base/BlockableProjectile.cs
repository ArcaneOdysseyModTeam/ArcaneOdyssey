namespace ArcaneOdyssey.Projectiles.Base
{
	public class BlockableProjectile : BaseProjectile
	{
		public override void ModifyHitPlayer(Player player, ref Player.HurtModifiers modifiers)
		{
			if (player.ArcaneOdyssey().Blocking)
			{
				if (player.ArcaneOdyssey().Parrying)
				{
					modifiers.FinalDamage /= 10f;
				}
				else
				{
					modifiers.FinalDamage *= 0.45f;
				}
				modifiers.Knockback *= 0f;
			}
		}
	}
}
