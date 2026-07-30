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
			return base.PreDraw(ref lightColor);
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
				float waveVal = 25f*(MathF.Abs(MathF.Abs(((updates+110)/2f)%10)-5f)-2.5f);
				Vector2 baseVec = new(0f, waveVal);
				Dust spawnedDust = Dust.NewDustPerfect(area.Center() + baseVec.RotatedBy(Projectile.velocity.ToRotation()), DustID.CrystalPulse, Vector2.Zero, Scale: 1.2f);
				spawnedDust.noGravity = true;

				Lighting.AddLight(area.Center(), 2, 1, 2);
				Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.WitherLightning, Scale: 0.4f * area.RelativeScale());
			}
		}
	}
}
