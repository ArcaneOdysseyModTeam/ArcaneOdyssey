using ArcaneOdyssey.Content.Items.Base;
using Microsoft.Xna.Framework;
using Terraria;

namespace ArcaneOdyssey.Content.Projectiles.Base
{
	public abstract class BlastSpell : MagicSpell
	{

		// ai 0 is first frame bool


		public virtual void SetDefaultsBlast() {}
		public override void SetDefaultsSpell()
		{
			Projectile.timeLeft = 5 * 60;
			SetDefaultsBlast();
			Projectile.height = Projectile.width = 64;
			BaseScale = Projectile.ai[2] != 2 ? 0.6f : 1.2f;
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			width = Projectile.width / 4;
			height = Projectile.height / 4;
			fallThrough = true;
			return true;
		}


		public override void AI()
		{
			if (Projectile.frameCounter > 5)
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
                if (Projectile.frame + 1 >= Main.projFrames[Projectile.type])
                {
                    Projectile.frame = 0;
                }
            }
			Projectile.frameCounter++;
			if (Projectile.ai[0] == 0f)
			{
				Projectile.ai[0] = 1f;
				Projectile.netUpdate = true;
			}
			aoPlayerOwner ??= Main.player[Projectile.owner].ArcaneOdyssey();
			Projectile.rotation = Projectile.velocity.ToRotation();
			if (Projectile.TryGetImbue(out Imbuable imbue) && !imbue.CanBeWet && Projectile.wet)
			{
				Kill();
				return;
			}
		}
	}
}
