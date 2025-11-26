using ArcaneOdyssey.Content.Projectiles.Base;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey.Content.Projectiles.Weapons.Abilities
{
	public class PiercingStrikesProjectile : AOPlayerProjectile
	{
		public override float AOSpeed => 1.05f;
		public override float AOSize => .9f;
		public override float AODamage => 1.05f;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.height = Projectile.width = 46;
			Projectile.friendly = true;
			Projectile.DamageType = TrueMeleeNoSpeed();
			Projectile.penetrate = -1;
			Projectile.timeLeft = 35;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.ownerHitCheck = true;
		}

		public override void AI()
		{
			aoPlayerOwner ??= Main.player[Projectile.owner].ArcaneOdyssey();
			if (Projectile.ai[0] == 0)
			{
				Projectile.ai[0] = 1;
				Projectile.netUpdate = true;
				Projectile.velocity.Normalize();
			}
			Projectile.Center = aoPlayerOwner.Player.HandPosition.GetValueOrDefault(aoPlayerOwner.Player.MountedCenter) + (Projectile.velocity * 15);
			Projectile.rotation = Projectile.velocity.ToRotation() + (MathHelper.PiOver2 * Projectile.spriteDirection) - MathHelper.PiOver4;
			aoPlayerOwner.Player.heldProj = Projectile.whoAmI;
		}
	}

	public class PiercingStrikes : DashSystem
	{
		public override int DashMax => 20;
		public override float DashSpeed => 12;
		public override bool Immune => true;
		public override bool OnHit(Player player, Entity target)
		{
			return false;
		}
		public override bool AnyDirection => true;
		public override int Cooldown => 180;
		public ModProjectile projectile;

		public override void OnStart(Player player)
		{ 
			projectile.Projectile.velocity = player.ArcaneOdyssey().DashVelocity;
			player.PlayerItem().useStyle = ItemUseStyleID.Rapier;
		}
		public override void DashEffect(Player player)
		{
			player.itemAnimation = player.itemTime = 10;
		}

		public override void OnEnd(Player player)
		{
			projectile.Projectile.Kill();
		}

        public override int DisplayedCooldownID => ModContent.BuffType<PiercingStrikesCooldown>();
    }

    public class PiercingStrikesCooldown : DisplayedCooldown { }
}
