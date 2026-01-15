using ArcaneOdyssey.Content.Items.Weapons.Bronze;
using ArcaneOdyssey.Content.Projectiles.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey.Content.Projectiles.Weapons.Abilities
{
	public class Whirlwind : AOPlayerProjectile
	{
		public Color colour = Color.White;
		public static int MaxTime => 20;
		public static int TrueMaxTime => MaxTime * 2;
		public Texture2D Sprite => ModContent.Request<Texture2D>(Texture).Value;

		public override float AOSize => 1.5f;

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ProjectileID.Sets.TrailingMode[Type] = 2;
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = Projectile.height = 144;
			Projectile.friendly = true;
			Projectile.timeLeft = TrueMaxTime;
			Projectile.DamageType = TrueMeleeNoSpeed();
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
			Projectile.ownerHitCheck = true;
			Projectile.localNPCHitCooldown = -1;
			Projectile.usesLocalNPCImmunity = true;
		}

		public override float AOSpeed => .925f;

		private Vector2 RotationOrigin;
		private int OriginalDir;

		public override void AI()
		{
			if (Projectile.ai[0] == 0)
			{
				Projectile.ai[0] = 1;
				Projectile.netUpdate = true;
				Projectile.velocity = Vector2.Zero;
				RotationOrigin = Owner.MountedCenter;
				OriginalDir = Owner.direction;
			}
			Projectile.rotation = MathHelper.Pi / (MaxTime / 2) * 1.25f * (Imbue?.AOImbueSpeed ?? 1f) * OriginalDir * (MaxTime - (Projectile.timeLeft - MaxTime));
			Projectile.Center = RotationOrigin + (Projectile.rotation.ToRotationVector2() * 44f * Projectile.scale * OriginalDir);
			if (Projectile.timeLeft > MaxTime)
			{
				Owner.itemTime = Owner.itemAnimation = 2;
				Owner.itemRotation = RotationOrigin.DirectionTo(Projectile.Center).ToRotation() + (Owner.direction == 1 ? 0f : MathHelper.PiOver2);
				AOPlayerOwner.WhirlwindActive = true;
			}
			else
			{
				Projectile.Opacity = (Projectile.timeLeft - 1) / (float)MaxTime;
				AOPlayerOwner.WhirlwindActive = false;
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			for (int k = Projectile.oldPos.Length - 1; k > -1; k--)
			{
				Vector2 drawPos = Projectile.oldPos[k] + (Projectile.Size / 2f) + new Vector2(0f, Projectile.gfxOffY);
				var colour2 = Projectile.GetAlpha(Imbue?.GetColour(colour) ?? colour) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				var rotaitoneoffset = SpriteEffects.None;
				if (OriginalDir == -1)
				{
					rotaitoneoffset = SpriteEffects.FlipHorizontally;
				}	
				Main.EntitySpriteDraw(Sprite, drawPos - Main.screenPosition, null, colour2, Projectile.oldRot[k], Sprite.Size() / 2, Projectile.scale, rotaitoneoffset, 0);
			}
			return false;
		}
	}

	public class WhirlwindCooldown : DisplayedCooldown
	{
		public override int CooldownLength => 60 + Whirlwind.MaxTime;
		public override string ExtraIconTexture => typeof(RavennaSword).FullName.Replace('.', '/');
	}
}
