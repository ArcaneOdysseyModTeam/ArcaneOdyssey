using ArcaneOdyssey.AOPlayers;
using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Projectiles.Base;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace ArcaneOdyssey.Projectiles.Weapons
{
	public class LanceofLoyaltyProjectile : BaseLanceProjectile
	{
		public override float Size => 1.25f;
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			if (Imbue is not null && Math.Abs(Owner.velocity.X) > (AOPlayerOwner.MaxRunSpeed * 1.1f) && !AOPlayerOwner.OnCooldown<RagingImpact>())
			{
				if (Owner.PlayerItem()?.ModItem is Weapon weap)
				{
					weap.ActivateAbility(Owner, false);
				}
				SoundEngine.PlaySound(SoundID.DD2_ExplosiveTrapExplode, target.Center);
				AOPlayerOwner.SetCooldown<RagingImpact>();
				AOUtils.SimulateAOE(Projectile.width * 3, damageDone / 3, target.Center, hit.Knockback, Projectile, Projectile.DamageType, false, target.whoAmI);
				for (int i = 0; i < 10; i++)
				{
					Imbue?.ExplosionEffects(target.Center);
					SecondImbue?.ExplosionEffects(target.Center);
				}
			}
		}
	}

	public class RagingImpact : DisplayedCooldown
	{
		public override int CooldownLength => 180;
		public override string ExtraIconTexture => AOUtils.GetTexture<LanceofLoyaltyProjectile>();
	}
}
