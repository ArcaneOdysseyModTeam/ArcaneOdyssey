using ArcaneOdyssey.AOPlayers;
using ArcaneOdyssey.Items.Weapons.Bronze;
using ArcaneOdyssey.Projectiles.Base;


namespace ArcaneOdyssey.Projectiles.Abilities
{
	public class BronzeRapierProjectile : PlayerProjectile
	{
		public override string Texture => AOUtils.GetTexture<BronzeRapier>();
		public override float Speed => 1.05f;
		public override float Size => .9f;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.height = Projectile.width = 46;
			Projectile.friendly = true;
			Projectile.DamageType = AOUtils.TrueMeleeNoSpeed();
			Projectile.penetrate = -1;
			Projectile.timeLeft = 35;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.ownerHitCheck = true;
		}

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ProjectileID.Sets.HeldProjDoesNotUsePlayerGfxOffY[Type] = true;
		}

		public override void AI()
		{
			if (Projectile.ai[0] == 0)
			{
				Projectile.ai[0] = 1;
				if (Projectile.owner == Main.myPlayer)
				{
					Projectile.netUpdate = true;
					Projectile.netSpam = 0; ;
				}
				Projectile.velocity.Normalize();
			}
			Projectile.Center = Owner.HandPosition.GetValueOrDefault(Owner.RotatedRelativePoint(Owner.MountedCenter)) + (Projectile.velocity * 18);
			//Projectile.Center = Projectile.Center with { Y = Projectile.Center.Y - 8f };
			Projectile.rotation = Projectile.velocity.ToRotation() + (MathHelper.PiOver2 * Projectile.spriteDirection) - MathHelper.PiOver4;
			Owner.heldProj = Projectile.whoAmI;
		}
	}

	public class PiercingStrikes(Entity source) : ModDash(source)
	{
		public override int DashMax => 20;
		public override float DashSpeed => 12;
		public override bool Immune => true;
		public override bool OnHit(Player player, NPC target) => false;
		public override bool LocksPlayer => true;
		public override int Cooldown => 180;

		public override void OnStart(Player player)
		{
			Source.velocity = player.ArcaneOdyssey().DashVelocity;
			player.PlayerItem().useStyle = ItemUseStyleID.Rapier;
		}
		public override void DashEffect(Player player)
		{
			player.itemAnimation = player.itemTime = 10;
		}

		public override void OnEnd(Player player)
		{
			Source.Kill();
			player.velocity *= .65f;
		}

		public override int DisplayedCooldownID => ModContent.BuffType<PiercingStrikesCooldown>();
	}

	public class PiercingStrikesCooldown : DisplayedCooldown
	{
		public override string Texture => AOUtils.GetTexture<BronzeRapier>();
	}
}
