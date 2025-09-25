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
		private bool wascharging;
		public const float defaultMax = 3f;
        public const float defaultMin = 0.6f;
        public float charge = 1f;
		public bool isPlacedExplosion = Main.mouseRight;
		public Vector2 ensuredPosition = Main.MouseWorld;
        public override void SetDefaults()
        {
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
        }
		public override void AI()
		{
			if (Projectile.position != Projectile.oldPosition)
				Projectile.netUpdate = true;
			Player player = Main.player[Projectile.owner];
			player.direction = ((Main.MouseWorld - player.position).X > 0).ToDirectionInt();
			AOPlayer playah = player.ArcaneOdyssey();
			if (charge < defaultMax && playah.myCircle is not null && playah.myCircle.ai[0] < 1)
			{
				if (Projectile.ai[1] == 0)
				{
					charge = defaultMin;
					Projectile.ai[1]++;
				}
				playah.chargingSpell = wascharging = true;
				Projectile.position = playah.myCircle.Center;
				ensuredPosition = Projectile.position;
				charge += 1/60f;
				if (!isPlacedExplosion)
				{
					ensuredPosition = player.Center;
				}
			}
			else
			{
				if (wascharging)
					playah.chargingSpell = false;
				if (Projectile.ai[1] == 0)
				{
					charge = 1f;
					Projectile.ai[1]++;
				}
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
				player.reuseDelay = 60;
				if (!isPlacedExplosion)
				{
					ensuredPosition = player.Center;
				}
				if (Main.myPlayer == Projectile.owner)
				{
					var explosionProjectile = Main.projectile[Projectile.NewProjectile(Projectile.GetSource_FromThis(), ensuredPosition + ((1 - Imbue.AOScrollSize) * new Vector2(100, 100)), Vector2.Zero, ModContent.ProjectileType<ExplosionSpell>(), (int)Math.Round(25 * (charge * (charge/2))), Projectile.knockBack, Projectile.owner)];
					explosionProjectile.localAI[0] = 1f;
					if (!isPlacedExplosion)
					{
						explosionProjectile.localAI[0] = 1.2f; //size mult
						explosionProjectile.damage = (explosionProjectile.damage * 1.2f).Round(); //Damage mult
						explosionProjectile.AI();
						explosionProjectile.Center = ensuredPosition;
					}
				}
				SoundEngine.PlaySound(Imbue.ImbueSound, Projectile.position, null);
				Kill();
			}
			// Outline vfx
			if (Main.myPlayer == Projectile.owner)
			{
				float extraScale = 1f;
				if (!isPlacedExplosion)
				{
					extraScale = 1.2f;//size mult
				}
				Projectile.TryGetImbue(out Imbuable imbue);
				for (int n = 0; n < 360; n += 4)
				{
					Vector2 currentDustPos = (new Vector2((float)Math.Cos(n * (MathHelper.Pi / 180f)), (float)Math.Sin(n * (MathHelper.Pi / 180f)))) * ((imbue.AOScrollSize * 109) * extraScale);
					currentDustPos.X = Utils.Clamp(currentDustPos.X, -1 * (imbue.AOScrollSize * 100 * extraScale), (imbue.AOScrollSize * 100 * extraScale));
					currentDustPos.Y = Utils.Clamp(currentDustPos.Y, -1 * (imbue.AOScrollSize * 100 * extraScale), (imbue.AOScrollSize * 100 * extraScale));
					Dust.NewDustPerfect(ensuredPosition + currentDustPos, DustID.ShimmerSpark, Vector2.Zero, 0, imbue.ImbueColour, 1f);
				}
			}
		}
	}
}
