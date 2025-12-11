using ArcaneOdyssey.Content.Projectiles.Magic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Projectiles.Base
{
	public abstract class PulsarSpell : MagicSpell, ILocalizedModType
	{
		public override string Texture => GetType().FullName.Replace('.', '/').Replace("Pulsar", "Blast");
		public override string LocalizationCategory => "Magic.Spells.Pulsars";
		public override float AOSize => .5f;
        public override float AOSpeed => .25f;
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.height = Projectile.width = 64;
		}
		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			width /= 4;
			height /= 4;
			fallThrough = true;
			return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
		}

		public override void AI()
		{

			if (Main.myPlayer == Projectile.owner && Imbue is not null)
			{
				if (Projectile.localAI[0] > 30)
				{
					Projectile.localAI[0] = 0;
					var proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.position, Projectile.velocity/4, ModContent.ProjectileType<ExplosionSpell>(), 40, 0f, Projectile.owner, 1.3f);
					proj.Center = Projectile.Center;
				}
				else
				{
					Projectile.localAI[0] += Imbue.AOScrollSpeed;
				}
			}
			if (Projectile.ai[2] == 0f)
			{
				Projectile.ai[2] = 1f;
				Projectile.netUpdate = true;
			}
			aoPlayerOwner ??= Main.player[Projectile.owner].ArcaneOdyssey();
			Animate();
			Rotate();
			if (Imbue is null || ((!Imbue.CanBeWet) && Projectile.wet))
			{
				Kill();
				return;
			}
		}

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
			Projectile.rotation = Projectile.velocity.ToRotation();
		}
	}
}
