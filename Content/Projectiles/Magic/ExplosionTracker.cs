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
using ArcaneOdyssey.Content.Projectiles.Base;
using ArcaneOdyssey.Content.Items.Base;

namespace ArcaneOdyssey.Content.Projectiles.Magic
{
	public class ExplosionTracker : AOPlayerProjectile
	{
		public const int defaultMax = 3 * 60;
        public const int defaultMin = 10;
        public int charge = defaultMin;
		public bool isPlacedExplosion = Main.mouseRight;
		public Vector2 ensuredPosition = Main.MouseWorld;
        public override void SetDefaults()
        {
			Projectile.tileCollide = false;
        }
		public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			player.direction = ((Main.MouseWorld - player.position).X > 0).ToDirectionInt();
			AOPlayer playah = player.ArcaneOdyssey();
			if (charge < defaultMax && playah.myCircle is not null && playah.myCircle.ai[0] < 1)
			{
				Projectile.position = playah.myCircle.Center;
				ensuredPosition = Projectile.position;
				charge++;
				if (!isPlacedExplosion)
				{
					ensuredPosition = player.Center;
				}
			}
			else
			{
				if (Vector2.Distance(player.Center, Main.MouseWorld) > 400)
				{
					Projectile.Center = player.Center + player.Center.DirectionTo(Main.MouseWorld) * 400;
					ensuredPosition = Projectile.Center;
				}
				player.channel = false;
				if (playah.myCircle is not null)
				{
					playah.myCircle.ai[0] += 1;
					playah.myCircle = null;
				}
				player.itemAnimation = 0;
				player.itemTime = 0;
				float dmgmult = charge / 60f;
				if (!isPlacedExplosion)
				{
					ensuredPosition = player.Center;
				}
				Projectile explosionProjectile = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), ensuredPosition + ((1 - Imbue.AOScrollSize) * new Vector2(100, 100)), Vector2.Zero, ModContent.ProjectileType<ExplosionSpell>(), (int)Math.Round(25 * dmgmult), Projectile.knockBack, Projectile.owner);
				explosionProjectile.localAI[0] = 1f;
				if (!isPlacedExplosion)
				{
					explosionProjectile.localAI[0] = 1.2f; //size mult
					explosionProjectile.damage = (int)((float)explosionProjectile.damage*1.2f); //Damage mult
					explosionProjectile.AI();
					explosionProjectile.Center = ensuredPosition;
				}
				SoundEngine.PlaySound(Imbue.ImbueSound, Projectile.position, null);
				Kill();
			}
			// Outline vfx
			float extraScale = 1f;
			if (!isPlacedExplosion)
			{
				extraScale = 1.2f;//size mult
			}
			Projectile.TryGetImbue(out Imbuable imbue);
			for (int n = 0; n < 360; n+=4)
			{
				Vector2 currentDustPos = (new Vector2((float)Math.Cos((float)n * (MathHelper.Pi / 180f)), (float)Math.Sin((float)n * (MathHelper.Pi / 180f)))) * ((imbue.AOScrollSize * 109)*extraScale);
				currentDustPos.X = Utils.Clamp<float>(currentDustPos.X, -1 * (imbue.AOScrollSize * 100 * extraScale), (imbue.AOScrollSize * 100 * extraScale));
				currentDustPos.Y = Utils.Clamp<float>(currentDustPos.Y, -1 * (imbue.AOScrollSize * 100 * extraScale), (imbue.AOScrollSize * 100 * extraScale));
				Dust dust = Dust.NewDustPerfect(ensuredPosition + currentDustPos, DustID.ShimmerSpark, Vector2.Zero, 0, default, 1f);
			}
		}
	}
}
