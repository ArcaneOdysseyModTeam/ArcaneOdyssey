using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Projectiles.Enemies
{
	public class EvanderMelee : ModProjectile
	{
		//public override float AOSpeed => .65f;
		//public override float AOSize => 1.2f;
		//public override float AODamage => 1.15f;
		//public override SoundStyle? DebuffApplySound => SoundID.NPCHit42;

		//public AOWeaponTiers AOWeaponTier = AOWeaponTiers.Good;

		public override void SetDefaults()
		{
			Projectile.penetrate = -1;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.damage = 25;
			Projectile.timeLeft = 25;
			Projectile.hostile = true;
			Projectile.height = Projectile.width = 110;
			Projectile.knockBack = 4.5f;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = -1;
			Projectile.tileCollide = false;
		}
		public override bool PreDraw(ref Color lightColor) => false;
	}
}
