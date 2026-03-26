using ArcaneOdyssey.AOPlayers;
using ArcaneOdyssey.Items.Weapons.Bronze;
using ArcaneOdyssey.Projectiles.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;


namespace ArcaneOdyssey.Projectiles.Abilities
{
	public class Whirlwind : PlayerProjectile
	{
		public Color Colour => Imbue?.Colour ?? Color.Orange;
		public static int MaxTime => 20;
		public static int TrueMaxTime => MaxTime * 2;

		public override string Texture => $"Terraria/Images/Projectile_{ProjectileID.TerraBlade2}";

		public override float AOSize => 3f;

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ProjectileID.Sets.TrailingMode[Type] = 2;
			ProjectileID.Sets.TrailCacheLength[Type] = 15;
			Main.projFrames[Type] = 4;
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = Projectile.height = 150;
			Projectile.friendly = true;
			Projectile.timeLeft = TrueMaxTime;
			Projectile.DamageType = AOUtils.TrueMeleeNoSpeed();
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
			Projectile.ownerHitCheck = true;
			Projectile.localNPCHitCooldown = MaxTime;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.frame = 1;
		}

		private int OriginalDir;

		public override void AI()
		{
			if (Projectile.ai[0] == 0)
			{
				Projectile.ai[0] = 1;
				if (Main.myPlayer == Projectile.owner)
				{
					Projectile.netUpdate = true;
					Projectile.netSpam = 0;
				}
				Projectile.velocity = Vector2.Zero;
				OriginalDir = Owner.direction;
			}
			Projectile.rotation = MathHelper.Pi / (MaxTime / 2) * ApplySpeed(1f) * OriginalDir * (MaxTime - (Projectile.timeLeft - MaxTime));
			if (Projectile.timeLeft > (TrueMaxTime - MaxTime))
			{
				Owner.itemTime = Owner.itemAnimation = 2;
				Owner.itemRotation = Projectile.rotation + MathHelper.PiOver4 + (Owner.direction == 1 ? 0f : -MathHelper.PiOver2);
				Owner.PlayerItem().noMelee = true;
			}
			else
			{
				Projectile.Opacity = (Projectile.timeLeft - 1f) / MaxTime;
				Owner.PlayerItem().noMelee = false;
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			for (int k = Projectile.oldPos.Length - 1; k > -1; k--)
			{
				Vector2 drawPos = Projectile.oldPos[k] + (Projectile.Size / 2f) + new Vector2(0f, Projectile.gfxOffY);
				var colour2 = Projectile.GetAlpha(Colour) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				var rotaitoneoffset = SpriteEffects.None;
				if (OriginalDir == -1)
				{
					rotaitoneoffset = SpriteEffects.FlipHorizontally;
				}
				Main.EntitySpriteDraw(Sprite, drawPos - Main.screenPosition, Sprite.Frame(1, Main.projFrames[Type], 0, Projectile.frame), colour2, Projectile.oldRot[k], (Sprite.Size() with { Y = Sprite.Height / Main.projFrames[Type] }) / 2f, Projectile.scale, rotaitoneoffset, 0);
			}
			return false;
		}
	}

	public class WhirlwindCooldown : DisplayedCooldown
	{
		public override int CooldownLength => 60 + Whirlwind.MaxTime;
		public override string ExtraIconTexture => AOUtils.GetTexture<RavennaSword>();
	}
}
