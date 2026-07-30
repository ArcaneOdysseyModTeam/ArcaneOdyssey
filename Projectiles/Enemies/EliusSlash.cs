using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Imbues.Relics;
using ArcaneOdyssey.Projectiles.Base;
using ArcaneOdyssey.Projectiles.Relics;
using Terraria.Audio;
using System;

namespace ArcaneOdyssey.Projectiles.Enemies
{
	public class EliusSlash : BaseProjectile
	{
		public override string Texture => AOUtils.SlashTexture;
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailingMode[Type] = 0;
			ProjectileID.Sets.TrailCacheLength[Type] = 5;
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.DamageType = DamageClass.Melee;
			Projectile.timeLeft = 500;
			Projectile.hostile = true;
			Projectile.friendly = false;
			Projectile.penetrate = -1;
			Projectile.height = 200;
			Projectile.width = 70;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.scale = 0.6f;
		}
		public override bool PreDraw(ref Color lightColor)
		{
			lightColor = Color.Gold;
			if(Projectile.localAI[0] > 0f)
			{
				lightColor = Color.Plum;
			}
			for (int k = Projectile.oldPos.Length - 1; k > -1; k--)
			{
				Vector2 drawPos = Projectile.oldPos[k] + (Projectile.Size / 2f) + new Vector2(0f, Projectile.gfxOffY);
				var colour2 = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				Main.EntitySpriteDraw(Sprite, drawPos - Main.screenPosition, null, colour2, Projectile.rotation, Sprite.Size() / 2, Projectile.scale - (k * .01f), SpriteEffects.None, 0);
			}
			return false;
		}
		public override void AI()
		{
			base.AI();
			Projectile.rotation = Projectile.velocity.ToRotation();
			if(Projectile.localAI[0] > 0f)
			{
				var updates = (float)Main.GameUpdateCount;
				Rectangle area = new Rectangle((int)Projectile.Center.X,(int)Projectile.Center.Y,1,1);
				updates += Projectile.numUpdates;
				float waveVal = 15f*(MathF.Abs(MathF.Abs((updates+110)%10)-5f)-2.5f);
				Vector2 baseVec = new(0f, waveVal);
				Dust spawnedDust = Dust.NewDustPerfect(area.Center() + baseVec.RotatedBy(Projectile.velocity.ToRotation()), DustID.CrystalPulse, Vector2.Zero, Scale: 1.2f);
				spawnedDust.noGravity = true;

				Lighting.AddLight(area.Center(), 2, 1, 2);
				Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.WitherLightning, Scale: 0.4f * area.RelativeScale());
			}
		}
	}
}
