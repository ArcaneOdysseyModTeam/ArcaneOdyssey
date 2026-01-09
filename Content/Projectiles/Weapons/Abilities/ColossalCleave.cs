using ArcaneOdyssey.Content.Projectiles.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey.Content.Projectiles.Weapons.Abilities
{
	public class ColossalCleave : AOPlayerProjectile
	{
		public override float AOSpeed => .65f;
		public override float AOSize => 1.2f;
		public override SoundStyle? DebuffApplySound => SoundID.NPCHit42;

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			Main.projFrames[Type] = 3;
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.penetrate = -1;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.timeLeft = 60 * 3;
			Projectile.friendly = true;
			Projectile.height = Projectile.width = 234;
			Projectile.knockBack = 4.5f;
		}

		public override void AI()
		{
			if (Projectile.ai[0] == 0)
			{
				Projectile.ai[0] = 1;
				Projectile.netUpdate = true;
			}

			if (++Projectile.frameCounter > 6)
			{
				if (++Projectile.frame >= Main.projFrames[Projectile.type])
				{
					Projectile.frame = 0;
				}
			}

			if (++Projectile.localAI[0] >= 30 && !Main.dedServ)
			{
				Projectile.localAI[0] = 0;
				for (int i = 0; i < 10; i++)
				{
					Imbue?.ExplosionEffects(Projectile);
					SecondImbue?.ExplosionEffects(Projectile);
				}
			}

			if (Projectile.timeLeft <= 30)
			{
				Projectile.ai[1] = 1;
			}

			if (Projectile.ai[1] != 0)
			{
				Projectile.alpha += 255 / 30;
			}
			else
			{
				Projectile.rotation = Projectile.velocity.ToRotation();
			}
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			height = width = 1;
			fallThrough = true;
			return true;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Projectile.velocity = Vector2.Zero;
			Projectile.timeLeft = 30;
			Projectile.ai[1] = 1;
			Projectile.ai[2] = 1;
			return false;
		}

		public override bool? CanDamage()
		{
			return Projectile.ai[2] == 0;
		}
	}
}
