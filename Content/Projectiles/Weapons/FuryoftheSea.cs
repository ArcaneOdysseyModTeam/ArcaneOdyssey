using ArcaneOdyssey.Content.Projectiles.Base;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey.Content.Projectiles.Weapons
{
	public class FuryoftheSea : AOPlayerProjectile
	{
		public override float AOSpeed => .9f;
		public override float AOSize => 1.25f;
		public override float AODamage => 1f;
		public override AODebuff Debuff => new(BuffID.Wet, 600);
        public override SoundStyle? DebuffApplySound => SoundID.Splash;
		public int AOWeaponTier = AOWeaponTiers.Excellent;
		

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
		}
		public override void AI()
        {
            // projectile.ai[0] is the scale
            // projectile.ai[1] is the spin speed
            aoPlayerOwner ??= Main.player[Projectile.owner].GetModPlayer<AOPlayer>();
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2 + (Projectile.ai[1] * FramesAlive);
            Projectile.spriteDirection = (Projectile.velocity.X < 0f).ToDirectionInt();
            BaseScale += .05f;
			Projectile.ai[0] = Projectile.scale;
			if (Main.netMode != NetmodeID.Server)
			{
				Random rnd = new();
				Dust dust = Main.dust[Dust.NewDust(Projectile.TopLeft, Projectile.width, Projectile.height, DustID.Water, 0, 0, 100, default, Projectile.ai[0])];
				dust.noGravity = true;
				//dust.velocity = Projectile.velocity * -1;

				//Random Fling Dust
				for (int dustCountInt = 0;dustCountInt < 10;dustCountInt++) 
				{
					Dust.NewDust(Projectile.position + new Vector2(Projectile.width / 2f,Projectile.height / 2f), 1, 1, DustID.Water, 50f *(0.5f - rnd.NextSingle()), 50f * (0.7f - rnd.NextSingle()), 1, default, 1.3f);
				}
				//Spiral Dust
				Dust.NewDustPerfect(Vector2.Normalize(new Vector2(-1f, -1f / (Projectile.velocity.Y / Projectile.velocity.X) - (-2f / (Projectile.velocity.X/Projectile.velocity.Y)))) * ((float)Math.Sin(FramesAlive * 150) * 100f) + (Projectile.position + new Vector2(Projectile.width / 2f, Projectile.height / 2f)), DustID.Water_Jungle, new Vector2(0f, 0f), 1, default, 4f);
				Dust.NewDustPerfect(Vector2.Normalize(new Vector2(-1f, -1f / (Projectile.velocity.Y / Projectile.velocity.X) - (-2f / (Projectile.velocity.X/Projectile.velocity.Y)))) * ((float)Math.Cos(FramesAlive * 150) * -100f) + (Projectile.position + new Vector2(Projectile.width / 2f, Projectile.height / 2f)), DustID.Water_Jungle, new Vector2(0f, 0f), 1, default, 4f);
			}
		}

        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
			hitbox.Height = (int)(hitbox.Height * Projectile.ai[0]);
			hitbox.Width = (int)(hitbox.Width * Projectile.ai[0]);
        }
	}
}
