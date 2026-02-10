using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Projectiles.Magic.Effects
{
	public class ProminenceProjectile : ModProjectile
	{
		private Vector2 originPos;
		private int timeAlive;
		public override void SetStaticDefaults()
		{
			Main.projFrames[Type] = 3;
		}
		public override void SetDefaults()
		{
			Projectile.tileCollide = false;
			Projectile.width = Projectile.height = 20;
			Projectile.ignoreWater = true;
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 30;
			Projectile.timeLeft = 6 * 60;

		}
		public override void OnSpawn(IEntitySource source)
		{
			originPos = Projectile.position;
			timeAlive = 0;
		}
		public override void AI()
		{
			timeAlive++;
			if (timeAlive == 29)
			{
				Projectile.velocity += new Vector2(Main.rand.NextFloat() * 0.5f, Main.rand.NextFloat() * 0.5f) * 10;
			}
			if (timeAlive > 30)
			{
				Projectile.velocity += (originPos - Projectile.position).SafeNormalize(Vector2.Zero) * 0.4f;
			}
			Animate();
			Lighting.AddLight(Projectile.position, 2, 1, 0);
			Dust.NewDust(Projectile.position, 1, 1, DustID.Torch, 0, 0, 0, default, 1);
		}
		private void Animate()
		{
			if (Projectile.frameCounter++ > 5)
			{
				Projectile.frameCounter = 0;
				if (++Projectile.frame >= Main.projFrames[Type])
				{
					Projectile.frame = 0;
				}
			}
		}
		public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
		{
			target.AddBuff(BuffID.OnFire3, 120);
		}
	}
}