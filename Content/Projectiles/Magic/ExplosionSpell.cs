using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using ArcaneOdyssey.Content.Projectiles.Base;

namespace ArcaneOdyssey.Content.Projectiles.Magic
{
	public class ExplosionSpell : MagicSpell
	{
		public override string Texture => Mod.Name + "/Backgrounds/Blank";
		private bool wascharging;
		public const float defaultMax = 3f;
		public const float defaultMin = 0.6f;
		public float charge = 1f;
		public bool isPlacedExplosion = Main.mouseRight;
		public Vector2 ensuredPosition = Main.MouseWorld;
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
		}

		public override bool? CanDamage() => false;

		public override void AI()
		{
			if (Projectile.position != Projectile.oldPosition)
				Projectile.netUpdate = true;
			Owner.direction = ((Main.MouseWorld - Owner.position).X > 0).ToDirectionInt();
			var size = isPlacedExplosion ? 1f : 1.2f;
			if (charge < defaultMax && AOPlayerOwner.myCircle is not null && AOPlayerOwner.myCircle.ai[0] < 1)
			{
				if (Projectile.ai[1] == 0)
				{
					charge = defaultMin;
					Projectile.ai[1]++;
				}
				Projectile.position = AOPlayerOwner.myCircle.Center;
				ensuredPosition = AOPlayerOwner.myCircle.Center;
				charge += 1 / 60f;
				if (!isPlacedExplosion)
				{
					ensuredPosition = Owner.Center;
				} else
				{
					ensuredPosition = AOPlayerOwner.myCircle.Center;
				}
			}
			else
			{
				if (wascharging)
					AOPlayerOwner.chargingSpell = false;
				if (Projectile.ai[1] == 0)
				{
					charge = 1f;
					Projectile.ai[1]++;
				}
				if (Vector2.Distance(Owner.Center, Projectile.Center) > 400)
				{
					Projectile.Center = Owner.Center + Owner.Center.DirectionTo(Main.MouseWorld) * 400;
					ensuredPosition = Projectile.Center;
				}
				Owner.channel = false;
				if (AOPlayerOwner.myCircle is not null && AOPlayerOwner.myCircle.Imbue().Name == Imbue.Name)
				{
					AOPlayerOwner.myCircle.ai[0]++;
					AOPlayerOwner.myCircle = null;
				}
				Owner.itemAnimation = 0;
				Owner.itemTime = 0;
				Owner.reuseDelay = 60;
				if (!isPlacedExplosion)
				{
					ensuredPosition = Owner.Center;
				}
				if (Main.myPlayer == Projectile.owner)
				{
					var damage = 25 * (charge * (charge / 2)) * (isPlacedExplosion ? 1f : 1.2f);
					AOUtils.SimulateAOE(size * 100, damage, ensuredPosition, Projectile.knockBack, Projectile, DamageClass.Magic);
				}
				for (int i = 0; i < 10 * charge * size; i++)
				{
					Imbue?.ExplosionEffects(Projectile);
				}
				for (int i = 0; i < 5 * charge * size; i++)
				{
					SecondImbue?.ExplosionEffects(Projectile);
				}
				SoundEngine.PlaySound(Imbue?.ImbueSound, ensuredPosition, null);
				Kill();
			}
			// Outline vfx
			if (Main.myPlayer == Projectile.owner && Imbue is not null)
			{
				for (int n = 0; n < 360; n += 4)
				{
					Vector2 currentDustPos = (new Vector2((float)Math.Cos(n * (MathHelper.Pi / 180f)), (float)Math.Sin(n * (MathHelper.Pi / 180f)))) * ((Imbue.AOScrollSize * 109) * size);
					currentDustPos.X = Utils.Clamp(currentDustPos.X, -1 * (Imbue.AOScrollSize * 100 * size), (Imbue.AOScrollSize * 100 * size));
					currentDustPos.Y = Utils.Clamp(currentDustPos.Y, -1 * (Imbue.AOScrollSize * 100 * size), (Imbue.AOScrollSize * 100 * size));
					Dust.NewDustPerfect(ensuredPosition + currentDustPos, DustID.ShimmerSpark, Vector2.Zero, 0, Imbue.GetColor(), 1f);
				}
			}
		}
	}
}
