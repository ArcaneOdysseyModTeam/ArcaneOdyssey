using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Imbues.Magic.Normal;
using ArcaneOdyssey.Projectiles.Base;
using System;
using System.IO;

namespace ArcaneOdyssey.Projectiles.Enemies.Elius
{
	public class EliusArrowStorm : BaseProjectile
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.timeLeft = 500;
			Projectile.hostile = true;
			Projectile.friendly = false;
			Projectile.penetrate = -1;
			Projectile.height = Projectile.width = 2;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
		}
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailingMode[Type] = 0;
			ProjectileID.Sets.TrailCacheLength[Type] = 3;
		}

		internal bool secondphase = false;

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(secondphase);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			secondphase = reader.ReadBoolean();
		}

		public Imbuable Imbue => secondphase ? ModContent.GetInstance<LightningMagic>() : null;

		public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)
		{
			modifiers = AOUtils.CalculateImbueDamage(Imbue, target, modifiers);
		}

		public override void AI()
		{
			if (Projectile.ai[0] == 0)
			{
				Projectile.velocity = Projectile.Center.DirectionTo(new Vector2(Projectile.ai[1], Projectile.ai[2])).SafeNormalize() * 20f;
				if (Projectile.Center.Distance(new Vector2(Projectile.ai[1], Projectile.ai[2])) < 25f)
				{
					Projectile.Center = new Vector2(Projectile.ai[1], Projectile.ai[2]);
					if (AOUtils.ServerOrSingleplayer)
					{
						for (int i = -5; i <= 5; i++)
						{
							(Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, new Vector2(i * 3f, 0f), ModContent.ProjectileType<EliusArrowStorm>(), Projectile.damage, 0f, Projectile.owner, 1f).ModProjectile as EliusArrowStorm).secondphase = secondphase;
						}
						Projectile.Kill();
					}
				}
			}
			else
			{
				Projectile.velocity.Y += 0.4f;
				Projectile.velocity *= 0.98f;
			}
			Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.PiOver2;
			if (secondphase)
			{
				var updates = (float)Main.GameUpdateCount;
				Rectangle area = new Rectangle((int)Projectile.Center.X, (int)Projectile.Center.Y, 1, 1);
				updates += Projectile.numUpdates;
				float waveVal = 4f * (MathF.Abs(MathF.Abs(((updates + 110)) % 10) - 5f) - 2.5f);
				Vector2 baseVec = new(0f, waveVal);
				Dust spawnedDust = Dust.NewDustPerfect(area.Center() + baseVec.RotatedBy(Projectile.velocity.ToRotation()), DustID.CrystalPulse, Vector2.Zero, Scale: 1.2f);
				spawnedDust.noGravity = true;

				Lighting.AddLight(area.Center(), 2, 1, 2);
				Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.WitherLightning, Scale: 0.4f * area.RelativeScale());
			}
		}
		public override bool PreDraw(ref Color lightColor)
		{
			for (int k = Projectile.oldPos.Length - 1; k > -1; k--)
			{
				Vector2 drawPos = Projectile.oldPos[k] + (Projectile.Size / 2f) + new Vector2(0f, Projectile.gfxOffY);
				var colour2 = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				Main.EntitySpriteDraw(Sprite, drawPos - Main.screenPosition, null, colour2, Projectile.rotation, Sprite.Size() / 2, Projectile.scale - (k * .01f), SpriteEffects.None, 0);
			}
			return false;
		}
	}
}
