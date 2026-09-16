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
			Projectile.DamageType = DamageClass.Generic;
		}

		public override void OnSpawn(IEntitySource source)
		{
			Imbue = ModContent.GetInstance<LightningMagic>();
			var target = Projectile.Center.ClosestNPCAt(Projectile.timeLeft * 7f, false, true);
			if (target is not null)
			{
				Projectile.position.X = target.Center.X;
				Projectile.damage = (int)MathHelper.Clamp(target.lifeMax * 0.005f, Projectile.damage, 1000f);
			}
		}

		public override void AI()
		{
			Imbue ??= ModContent.GetInstance<LightningMagic>();
			var updates = (float)Main.GameUpdateCount;
			updates += Projectile.numUpdates;
			float waveVal = 5f * (MathF.Abs(MathF.Abs((((updates + 110) * 2.3f) / MathHelper.TwoPi) % 10) - 5f) - 2.5f);
			Vector2 baseVec = new(0f, waveVal);
			Dust spawnedDust = Dust.NewDustPerfect(Projectile.Center + baseVec.RotatedBy(Projectile.velocity.ToRotation()), DustID.CrystalPulse, Vector2.Zero, Scale: 1.5f);
			spawnedDust.noGravity = true;

			Lighting.AddLight(Projectile.Center, 2, 1, 2);
			Dust.NewDust(Projectile.Center, 0, 0, DustID.WitherLightning, Scale: 0.4f * Projectile.Hitbox.RelativeScale());

			if (Projectile.wet)
			{
				Kill();
			}
		}

		public override void OnKill(int timeLeft)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.WitherLightning, (Main.rand.NextFloat() - 0.5f) * 15f, (Main.rand.NextFloat() - 0.5f) * 15f, Scale: 1.2f).noGravity = true;
			}
			if (Main.myPlayer == Projectile.owner)
			{
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<LightningBurst>(), 0, 0, Projectile.owner, ai0: Projectile.Hitbox.RelativeScale(AetherExplosion.SpriteSize) * 1.5f);
			}
			SoundEngine.PlaySound(SoundID.DD2_LightningBugZap with { Volume = 2.25f }, Projectile.Center);
			base.OnKill(timeLeft);
		}

		public override bool? CanCutTiles() => false;
	}
}
