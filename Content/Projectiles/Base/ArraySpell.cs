using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Projectiles.Base
{
	public abstract class ArraySpell : MagicSpell, ILocalizedModType
	{
		// ai 2 is first frame bool
		public override string Texture => GetType().FullName.Replace('.', '/').Replace("Array", "Blast");

		public override string LocalizationCategory => base.LocalizationCategory + ".Arrays." + Tier;

		public override float AOSize => .6f;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.height = Projectile.width = 64;
			Projectile.timeLeft = 40 + (60 * 3);
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			width = Projectile.width / 4;
			height = Projectile.height / 4;
			fallThrough = true;
			return true;
		}

		public bool Hovering
		{
			get => Projectile.ai[0] == 0;
			set
			{
				if (value)
				{
					Projectile.ai[0] = 0;
				}
				else
				{
					Projectile.ai[0] = 1;
				}
			}
		}

		public int target = -1;
		internal Vector2 originalVelocity;

		public override void AI()
		{
			if (Projectile.ai[2] == 0)
			{
				Projectile.ai[2] = 1;
				Projectile.netUpdate = true;
				originalVelocity = Projectile.velocity;
				Projectile.velocity.Normalize();
			}
			Animate();
			Rotate();
			if (Imbue is null || ((!Imbue.CanBeWet) && Projectile.wet))
			{
				Kill();
				return;
			}

			if (Hovering)
			{
				//Projectile.scale = Imbue.AOScrollSize;
				//if (SecondImbue is not null)
				//	Projectile.scale *= SecondImbue.AOScrollSize;
				Projectile.Center = Owner.Center - new Vector2(0, Player.defaultHeight * Projectile.scale);
				//Projectile.scale *= BaseScale;
				target = Projectile.FindTargetWithLineOfSight(originalVelocity.Length() * 40);
				if (target != -1)
				{
					var targetnpc = Main.npc[target];
					bool canhit = Collision.CanHit(Projectile.position, Projectile.width, Projectile.height, targetnpc.position, targetnpc.width, targetnpc.height);
					float idkwahtimedoijngbutitsworking = canhit ? 40f : 20f;
					Projectile.rotation = Projectile.SafeDirectionTo(targetnpc.Center + targetnpc.velocity * idkwahtimedoijngbutitsworking).ToRotation();
				}
				else
				{
					Projectile.rotation = Projectile.Center.AngleTo(Main.MouseWorld);
				}

				if (++Projectile.ai[1] > (60 * 3))
				{
					Hovering = false;
					Projectile.velocity = Projectile.rotation.ToRotationVector2() * originalVelocity.Length();
				}
			}
		}

		public override bool? CanDamage() => !Hovering;

		public override bool OnTileCollide(Vector2 oldVelocity) => !Hovering;

		public virtual void Animate()
		{
			if (Projectile.frameCounter++ > 5)
			{
				Projectile.frameCounter = 0;
				if (++Projectile.frame >= Main.projFrames[Projectile.type])
				{
					Projectile.frame = 0;
				}
			}
		}

		public virtual void Rotate()
		{
			Projectile.spriteDirection = Projectile.direction;
			Projectile.rotation = Projectile.velocity.ToRotation();
		}
	}
}
