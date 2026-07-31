using ArcaneOdyssey.Imbues.Magic.Normal;
using ArcaneOdyssey.Projectiles.Base;
using Terraria.DataStructures;
using System;
using Terraria.Audio;
using ArcaneOdyssey.Projectiles.Magic.Effects;

namespace ArcaneOdyssey.Projectiles
{
	public class ThunderingEffect : PlayerProjectile
	{
		public override string Texture => AOUtils.BlankTexture;
		public override bool CanHaveImbueVFX => false;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.friendly = true;
			Projectile.extraUpdates = 100;
			Projectile.height = Projectile.width = 2;
			Projectile.ignoreWater = true;
			Projectile.DamageType = DamageClass.Generic;
		}

		public override void OnSpawn(IEntitySource source)
		{
			Imbue = ModContent.GetInstance<LightningMagic>();
		}
		public override void AI()
		{
			var updates = (float)Main.GameUpdateCount;
			Rectangle area = new Rectangle((int)Projectile.position.X,(int)Projectile.position.Y,1,1);
			updates += Projectile.numUpdates;
			float waveVal = 5f*(MathF.Abs(MathF.Abs((((updates+110)*2.3f)/MathHelper.TwoPi)%10)-5f)-2.5f);
			Vector2 baseVec = new(0f, waveVal);
			Dust spawnedDust = Dust.NewDustPerfect(area.Center() + baseVec.RotatedBy(Projectile.velocity.ToRotation()), DustID.CrystalPulse, Vector2.Zero, Scale: 1.5f);
			spawnedDust.noGravity = true;

			Lighting.AddLight(area.Center(), 2, 1, 2);
			Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.WitherLightning, Scale: 0.4f * area.RelativeScale());
			base.AI();
		}
		public override void OnKill(int timeLeft)
		{
			Rectangle area = new Rectangle((int)Projectile.position.X,(int)Projectile.position.Y,1,1);
			for (int n = 0; n < 3; n++)
			{
				Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.WitherLightning, (Main.rand.NextFloat() - 0.5f) * 15f, (Main.rand.NextFloat() - 0.5f) * 15f, Scale: 1.2f).noGravity = true;
			}
			Projectile.NewProjectile(Projectile.GetSource_FromThis(), area.Center(), Vector2.Zero, ModContent.ProjectileType<LightningBurst>(), 0, 0, Projectile.owner, ai0: area.RelativeScale(AetherExplosion.SpriteSize) * 1.5f);
			SoundEngine.PlaySound(SoundID.DD2_LightningBugZap with { Volume = 2.25f }, area.Center());
			base.OnKill(timeLeft);
		}

		public override bool? CanCutTiles() => false;
	}
}
