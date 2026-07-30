using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Imbues.Relics;
using ArcaneOdyssey.Projectiles.Base;
using ArcaneOdyssey.Projectiles.Relics;
using Terraria.Audio;
using System;

namespace ArcaneOdyssey.Projectiles.Enemies
{
	public class EliusTrail : BaseProjectile
	{
		public override string Texture => AOUtils.BlankTexture;
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.DamageType = DamageClass.Melee;
			Projectile.timeLeft = 10000;
			Projectile.hostile = true;
			Projectile.friendly = false;
			Projectile.penetrate = -1;
			Projectile.height = 1;
			Projectile.width = 1;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.extraUpdates = 80;
		}
		public override void AI()
		{
			Projectile.velocity = new Vector2(Projectile.ai[0],Projectile.ai[1]).DirectionFrom(Projectile.position).SafeNormalize();
			var updates = (float)Main.GameUpdateCount;
			Rectangle area = new Rectangle((int)Projectile.position.X,(int)Projectile.position.Y,1,1);
			updates += Projectile.numUpdates;
			float waveVal = 4f*(MathF.Abs(MathF.Abs(((updates+110)/MathHelper.TwoPi)%10)-5f)-2.5f);
			Vector2 baseVec = new(0f, waveVal);
			Dust spawnedDust = Dust.NewDustPerfect(area.Center() + baseVec.RotatedBy(Projectile.velocity.ToRotation()), DustID.CrystalPulse, Vector2.Zero, Scale: 1.2f);
			spawnedDust.noGravity = true;

			Lighting.AddLight(area.Center(), 2, 1, 2);
			Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.WitherLightning, Scale: 0.4f * area.RelativeScale());
			if (Projectile.position.Distance(new Vector2(Projectile.ai[0],Projectile.ai[1])) < 8f)
			{
				Projectile.Kill();
			}
		}
	}
}
