using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Projectiles.Base
{
	public abstract class CannonSpell : MagicSpell, ILocalizedModType
	{
		public override string Texture => GetType().FullName.Replace('.', '/').Replace("Cannon", "Blast");
		public override string LocalizationCategory => "Magic.Spells.Cannons";
		public int TileTimer = 0;

        public override float AOSize => 2f;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.height = Projectile.width = 64;
			Projectile.penetrate = -1;
			Projectile.ownerHitCheck = true;
			Projectile.timeLeft = 3 * 60;
			Projectile.velocity /= 3;
		}

		public override void AI()
		{
			if (TileTimer > 0)
				TileTimer--;
			if (Projectile.frameCounter > 5)
			{
				Projectile.frameCounter = 0;
				if (++Projectile.frame >= Main.projFrames[Projectile.type])
				{
					Projectile.frame = 0;
				}
			}
			Projectile.frameCounter++;
			if (Projectile.ai[2] == 0f)
			{
				Projectile.ai[2] = 1f;
				Projectile.netUpdate = true;
			}
			aoPlayerOwner ??= Main.player[Projectile.owner].ArcaneOdyssey();
			Rotate();
			if (Imbue is null || ((!Imbue.CanBeWet) && Projectile.wet))
			{
				Kill();
				return;
			}
		}

		public virtual void Rotate()
		{
			Projectile.rotation = Projectile.velocity.ToRotation();
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			width /= 4;
			height /= 4;
			fallThrough = true;
			return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (TileTimer <= 0)
			{
				for (var i = 0; i < 10; i++)
					Imbue?.KillEffects(Projectile);
			}
			if (TileTimer < 60 && TileTimer > 0)
			{
				return true;
			}
			Projectile.velocity = Projectile.oldVelocity;
			Projectile.position = Projectile.oldPosition;
			TileTimer = 65;
			return false;
		}
	}
}
