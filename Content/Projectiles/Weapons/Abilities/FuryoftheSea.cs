using ArcaneOdyssey.Content.Projectiles.Base;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Projectiles.Weapons.Abilities
{
	public class FuryoftheSea : AOPlayerProjectile
	{
		public override float AOSpeed => .9f;
		public override float AOSize => 1.25f;
		public override bool? Cold => true;
		public override AODebuffRequirement? Debuff => new(BuffID.Wet, 600);
		public override SoundStyle? DebuffApplySound => SoundID.Splash;
		public AOItemTiers AOWeaponTier = AOItemTiers.Good;


		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = 114;
			Projectile.height = 96;
			Projectile.AverageDimensions();
			Projectile.alpha = (int)(225 * .75f);
			Projectile.DamageType = DamageClass.Melee;
			Projectile.timeLeft = 60;
			Projectile.friendly = true;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.penetrate = -1;
		}

		public override void SetStaticDefaults()
		{
			Main.projFrames[Type] = 6;
		}

		public override void AI()
		{
			if (Projectile.ai[0] == 0)
			{
				Projectile.ai[0] = 1;
				Projectile.netUpdate = true;
			}
			if (Projectile.timeLeft % 6 == 0)
			{
				SoundEngine.PlaySound(SoundID.Splash with { Pitch = -.25f }, Projectile.Center);
				if (++Projectile.frame >= Main.projFrames[Type])
				{
					Projectile.frame = 0;
				}
			}
			Projectile.direction = (Projectile.velocity.X > 0).ToDirectionInt();
			Projectile.rotation = Projectile.velocity.ToRotation();
			if (!Main.dedServ)
			{
				Random rnd = new();
				Dust dust = Main.dust[Dust.NewDust(Projectile.TopLeft, Projectile.width, Projectile.height, DustID.Water, 0, 0, 100, default)];
				dust.noGravity = true;
				//dust.velocity = Projectile.velocity * -1;

				//Random Fling Dust
				for (int dustCountInt = 0; dustCountInt < 10; dustCountInt++)
				{
					Dust.NewDust(Projectile.position + (Projectile.Size / 2f), 1, 1, DustID.Water, 50f * (0.5f - rnd.NextSingle()), 50f * (0.7f - rnd.NextSingle()), 1, default, 1.3f);
				}
				//Spiral Dust
				float waveVal = (float)Math.Sin(Main.GameUpdateCount) * 50f;
				Vector2 baseVec = new(0f, waveVal);
				Dust spawnedDust = Dust.NewDustPerfect(Projectile.position + (baseVec.RotatedBy(Projectile.velocity.ToRotation())) + (Projectile.Size / 2f), DustID.Water_Jungle, new Vector2(0f, 0f), 255, default, 3f);
				spawnedDust.noGravity = true;
				float waveVal2 = (float)Math.Cos(Main.GameUpdateCount) * 50f;
				Vector2 baseVec2 = new(0f, waveVal2);
				Dust spawnedDust2 = Dust.NewDustPerfect(Projectile.position + (baseVec2.RotatedBy(Projectile.velocity.ToRotation())) + (Projectile.Size / 2f), DustID.Water_Jungle, new Vector2(0f, 0f), 255, default, 3f);
				spawnedDust2.noGravity = true;
			}
		}
	}
}
