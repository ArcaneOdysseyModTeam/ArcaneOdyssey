using ArcaneOdyssey.Content.Projectiles.Base;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;

namespace ArcaneOdyssey.Content.Projectiles.Magic
{
	public class ExplosionTracker : AOPlayerProjectile
	{
		public const int defaultMax = 3 * 60;
        public const int defaultMin = 10;
        public int charge = defaultMin;
        public override void SetDefaults()
        {
			Projectile.tileCollide = false;
        }
		public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			if (Main.mouseRight)
				player.direction = ((Projectile.position - player.position).X > 0).ToDirectionInt();
			AOPlayer playah = player.AOPlayer();
			if (charge < defaultMax && playah.myCircle is not null && playah.myCircle.ai[0] < 1)
			{
				Projectile.position = playah.myCircle.Center;
				charge++;
			}
			else
			{
				player.channel = false;
				if (playah.myCircle is not null)
				{
					playah.myCircle.ai[0] += 1;
                    playah.myCircle = null;
                }
				player.itemAnimation = 0;
				player.itemTime = 0;
				float dmgmult = charge / 60f;
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position, Vector2.Zero, ModContent.ProjectileType<ExplosionSpell>(), (int)Math.Round(25 * dmgmult), Projectile.knockBack, Projectile.owner);
				SoundEngine.PlaySound(thisMagic.MagicSound, Projectile.position, null);
				Kill();
			}
		}
	}
}
