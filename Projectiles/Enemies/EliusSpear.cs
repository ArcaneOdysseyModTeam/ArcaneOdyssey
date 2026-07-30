using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Imbues.Relics;
using ArcaneOdyssey.Projectiles.Base;
using ArcaneOdyssey.Projectiles.Relics;
using Terraria.Audio;
using ArcaneOdyssey.Items.Weapons.RavennaNoble;
using System;

namespace ArcaneOdyssey.Projectiles.Enemies
{
	public class EliusSpear : BaseProjectile
	{
		public override string Texture => AOUtils.GetTexture<NobleThunderspear>();
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.timeLeft = 500;
			Projectile.hostile = true;
			Projectile.friendly = false;
			Projectile.penetrate = -1;
			Projectile.height = Projectile.width = 70;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
		}
		public override void AI()
		{
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
			if(Projectile.localAI[0] > 0f)
			{
				var updates = (float)Main.GameUpdateCount;
				Rectangle area = new Rectangle((int)Projectile.Center.X,(int)Projectile.Center.Y,1,1);
				updates += Projectile.numUpdates;
				float waveVal = 4f*(MathF.Abs(MathF.Abs(((updates+110))%10)-5f)-2.5f);
				Vector2 baseVec = new(0f, waveVal);
				Dust spawnedDust = Dust.NewDustPerfect(area.Center() + baseVec.RotatedBy(Projectile.velocity.ToRotation()), DustID.CrystalPulse, Vector2.Zero, Scale: 1.2f);
				spawnedDust.noGravity = true;

				Lighting.AddLight(area.Center(), 2, 1, 2);
				Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.WitherLightning, Scale: 0.4f * area.RelativeScale());
			}
		}
	}
}
