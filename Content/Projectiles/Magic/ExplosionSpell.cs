using ArcaneOdyssey.Content.Projectiles.Base;
using ArcaneOdyssey.Content.Projectiles.Circles;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace ArcaneOdyssey.Content.Projectiles.Magic
{
	public class ExplosionSpell : MagicSpell
	{
		public override bool CanHaveImbueVFX => false;
		public override string Texture => AOUtils.BlankTexture;
		public float charge = 1f;
		public bool isPlacedExplosion = Main.mouseRight;
		public Vector2 ensuredPosition = Main.MouseWorld;
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			charge = 1f;
		}

		public override bool? CanDamage() => false;

		public override void AI()
		{
			var size = isPlacedExplosion ? 1f : 1.2f;
			if (Projectile.position != Projectile.oldPosition)
			{
				if (Projectile.owner == Main.myPlayer)
				{
					Projectile.netUpdate = true;
					Projectile.netSpam = 0;
				}
			}
			Owner.direction = ((Main.MouseWorld - Owner.position).X > 0).ToDirectionInt();
			if (charge < BaseMagicCircle.GlobalMaxCharge && AOPlayerOwner.myCircle is not null && AOPlayerOwner.myCircle.ModProjectile is RotatingMagicCircle circle1 && !circle1.MarkedForDeath)
			{
				Projectile.Center = AOPlayerOwner.myCircle.Center;
				ensuredPosition = AOPlayerOwner.myCircle.Center;
				charge += BaseMagicCircle.GlobalChargeSpeed;
			}
			else
			{
				Owner.channel = false;
				if (AOPlayerOwner.myCircle is not null && AOPlayerOwner.myCircle.Imbue().Name == Imbue.Name)
				{
					if (AOPlayerOwner.myCircle.ModProjectile is RotatingMagicCircle circle)
					{
						circle.MarkedForDeath = true;
					}
					AOPlayerOwner.myCircle = null;
				}
				if (Main.myPlayer == Projectile.owner)
				{
					var damage = 50 * charge * size;
					AOUtils.SimulateAOE(size * 100f * charge, damage, ensuredPosition, Projectile.knockBack, Projectile, Projectile.DamageType);
					if (ArcaneOdysseyClientConfig.Instance.AbilityText && Owner is not null && Owner.active && !Owner.DeadOrGhost)
					{
						var name = (Imbue.PrettySpellPrefix + " " + DisplayName).Trim();
						if (SecondImbue is not null)
						{
							name = SecondImbue.PrettyAttackPrefix + " " + name;
						}
						CombatText.NewText(Owner.Hitbox, Imbue.GetColour(), (name + "!").Trim(), true);
					}
				}
				for (int i = 0; i < 30; i++)
				{
					Imbue?.ExplosionEffects(Projectile.Center, size * charge);
					SecondImbue?.ExplosionEffects(Projectile.Center, size * charge);
				}
				SoundEngine.PlaySound(Imbue?.ImbueSound, ensuredPosition, null);
				Kill();
				return;
			}
			// Outline vfx
			if (Main.myPlayer == Projectile.owner && Imbue is not null)
			{
				for (int n = 0; n < 360; n += 4)
				{
					Vector2 currentDustPos = new Vector2((float)Math.Cos(n * (MathHelper.Pi / 180f)), (float)Math.Sin(n * (MathHelper.Pi / 180f))) * (Imbue.AOScrollSize * 109 * size * charge);
					Dust.NewDustPerfect(ensuredPosition + currentDustPos, DustID.ShimmerSpark, Vector2.Zero, 0, Imbue.GetColour(), 1f);
				}
			}
		}
	}
}
