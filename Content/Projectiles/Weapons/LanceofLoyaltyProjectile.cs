using ArcaneOdyssey.Content.Items.Weapons.RavennaLion;
using ArcaneOdyssey.Content.Projectiles.Base;
using ArcaneOdyssey.PlayerClasses;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace ArcaneOdyssey.Content.Projectiles.Weapons
{
	public class LanceofLoyaltyProjectile : BaseLanceProjectile
	{
		public override float AOSize => 1.25f;
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			if (Imbue is not null && Owner.velocity.Length() > AOPlayerOwner.MaxRunSpeed && !AOPlayerOwner.OnCooldown<RagingImpact>())
			{
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
		public override int CooldownLength => 90;
		public override string ExtraIconTexture => AOUtils.GetTexture<LanceofLoyaltyProjectile>();
	}
}
