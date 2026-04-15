using ArcaneOdyssey.Projectiles.Base;
using Microsoft.Xna.Framework;
using System.IO;
using Terraria;

namespace ArcaneOdyssey.Projectiles.Magic
{
	public class Surge : MagicSpell
	{
		public override bool CanHaveImbueVFX => false;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.height = 10;
			Projectile.width = 10;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
		}

		internal float length;
		internal float spread;

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			length = reader.ReadSingle();
			spread = reader.ReadSingle();
			Projectile.rotation = reader.ReadSingle();
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(length);
			writer.Write(spread);
			writer.Write(Projectile.rotation);
		}

		public override void AI()
		{
			Projectile.velocity = Vector2.Zero;
			if (AOPlayerOwner.myCircle is not null)
			{
				if (AOPlayerOwner.myCircle.MarkedForDeath && Main.myPlayer == Projectile.owner)
				{
					Kill();
					return;
				}
				Projectile.Center = AOPlayerOwner.myCircle.Projectile.Center;
				Projectile.rotation = AOPlayerOwner.myCircle.Projectile.rotation;
				spread = AOPlayerOwner.myCircle.ProjectileSpread;
				length = ApplySize(400f);
			}

			if (AOPlayerOwner.myCircle.Projectile.Opacity == 1f)
			{
				if (!Main.dedServ)
				{
					Imbue?.ConeEffects(Projectile.Center, length, Projectile.rotation, spread);
					SecondImbue?.ConeEffects(Projectile.Center, length, Projectile.rotation, spread);
				}
			}

			if (Projectile.position != Projectile.oldPosition)
			{
				NetUpdate();
			}
		}

		public override bool TouchingWater()
		{
			Owner.channel = false;
			return base.TouchingWater();
		}

		public override string Texture => AOUtils.BlankTexture;

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			if (AOPlayerOwner.myCircle.Projectile.Opacity == 1f)
			{
				if (AOUtils.BossAlive)
				{
					if (targetHitbox.IntersectsConeSlowMoreAccurate(projHitbox.Center(), length, Projectile.rotation, spread))
					{
						return true;
					}
				}
				else
				{
					if (targetHitbox.IntersectsConeFastInaccurate(projHitbox.Center(), length, Projectile.rotation, spread))
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}
