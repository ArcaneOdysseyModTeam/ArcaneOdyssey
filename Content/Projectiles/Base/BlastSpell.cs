using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Projectiles.Magic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Projectiles.Base
{
	/// <summary>
	/// Blast is the most versatile projectile ever lmao
	/// </summary>
	public abstract class BlastSpell : MagicSpell
	{
		// ai 0 is the BlastMode
		// ai 2 is first frame bool

		public virtual void SetDefaultsBlast() {}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.height = Projectile.width = 64;
            if (Projectile.ai[0] == (int)BlastMode.Blast)
            {
                Projectile.timeLeft = 5 * 60;
            }
            else if (Projectile.ai[0] == (int)BlastMode.Cannon)
            {
                Projectile.penetrate = -1;
                Projectile.tileCollide = false;
                Projectile.timeLeft = 2 * 60;
                Projectile.velocity /= 4;
            }
            else if (Projectile.ai[0] == (int)BlastMode.Pulsar)
            {
                Projectile.velocity /= 4;
            }
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
			switch ((BlastMode)Projectile.ai[0])
            {
                case BlastMode.Cannon:
                case BlastMode.Blast:
					Projectile.rotation = Projectile.velocity.ToRotation();
					break;
                case BlastMode.Pulsar:
                    Projectile.rotation = Projectile.velocity.ToRotation();
                    if (Main.myPlayer == Projectile.owner && ++Projectile.localAI[0] > 30)
                    {
                        Projectile.localAI[0] = 0;
                        var proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.position, Vector2.Zero, ModContent.ProjectileType<ExplosionSpell>(), 40, 0f, Projectile.owner, 1.5f);
                        proj.Center = Projectile.Center + (Projectile.velocity * 20);
                    }
                    break;
			}
			if (Imbue is null || ((!Imbue.CanBeWet) && Projectile.wet))
			{
				Kill();
				return;
			}
		}
	}

	public enum BlastMode
	{
		Blast,
		Cannon,
		Pulsar,
	}
}
