using ArcaneOdyssey.Content.Projectiles.Base;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOConversion;

namespace ArcaneOdyssey.Content.Projectiles
{
	public class FuryoftheSea : AOPlayerProjectile
	{
		public new const float AOSpeed = .9f;
		public new const float AOSize = 1.25f;
		public new const float AODamage = 1f;
		public override AODebuff? Debuff => new(BuffID.Wet, 600);
        public override SoundStyle? DebuffApplySound => SoundID.Splash;
		public const int AOWeaponTier = AOWeaponTiers.Excellent;
		

		public override void SetDefaults()
		{
			Projectile.height = Projectile.width = 64;
			Projectile.alpha = (int)(225 * .75f);
			Projectile.DamageType = DamageClass.Melee;
			Projectile.damage = (int)WeaponDamage(AOWeaponTier);
			Projectile.knockBack = 4.5f;
			Projectile.timeLeft = 60;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.penetrate = -1;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 5;
		}

		public override void AI()
		{
			aoPlayerOwner ??= Main.player[Projectile.owner].GetModPlayer<AOPlayer>();
			if (Projectile.ai[2] == 0)
            {
				Projectile.ai[2] = 1;
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            }
			Projectile.rotation += Projectile.ai[1];
            Projectile.spriteDirection = (Projectile.velocity.X < 0f).ToDirectionInt();
            Projectile.scale += .1f * (aoPlayerOwner.imbue is not null ? aoPlayerOwner.imbue.AOImbueSize : 1f) * AOSize;
			Projectile.ai[0] = Projectile.scale;
			if (Main.netMode != NetmodeID.Server)
			{
				Dust dust = Main.dust[Dust.NewDust(Projectile.TopLeft, Projectile.width, Projectile.height, DustID.Water, 0, 0, 100, default, Projectile.ai[0])];
				dust.noGravity = true;
				//dust.velocity = Projectile.velocity * -1;
			}
		}

        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
			hitbox.Height = (int)(hitbox.Height * Projectile.ai[0]);
			hitbox.Width = (int)(hitbox.Width * Projectile.ai[0]);
        }
	}
}
