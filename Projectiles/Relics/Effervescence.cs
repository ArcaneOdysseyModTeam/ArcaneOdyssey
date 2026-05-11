using ArcaneOdyssey.Projectiles.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Projectiles.Relics
{
	public class Effervescence : SpiritProjectile
	{
		public override float Size => .75f;

		public override bool CanHaveImbueVFX => false;
		public override string Texture => AOUtils.GetTexture<SpiritRaincloud>();

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			Main.projFrames[Type] = 7;
		}

		public Rectangle Proj1 => Utils.CenteredRectangle(Projectile.Center + new Vector2(0, Player.defaultHeight * Projectile.scale * 2f).RotatedBy(RandomRotation), new Vector2(64f * Projectile.scale));
		public Rectangle Proj2 => Utils.CenteredRectangle(Projectile.Center - new Vector2(0, Player.defaultHeight * Projectile.scale * 2f).RotatedBy(RandomRotation), new Vector2(64f * Projectile.scale));
		public Rectangle Proj3 => Utils.CenteredRectangle(Projectile.Center + new Vector2(Player.defaultHeight * Projectile.scale * 2f, 0).RotatedBy(RandomRotation), new Vector2(64f * Projectile.scale));
		public Rectangle Proj4 => Utils.CenteredRectangle(Projectile.Center - new Vector2(Player.defaultHeight * Projectile.scale * 2f, 0).RotatedBy(RandomRotation), new Vector2(64f * Projectile.scale));

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = Projectile.height = Player.defaultHeight * 2;
			Projectile.timeLeft = 60;
			Projectile.tileCollide = false;
			Projectile.Opacity = .95f;
			Target = -1;
		}

		public int Target { get => (int)Projectile.ai[0]; set => Projectile.ai[0] = value; }
		public int OldTarget;

		public ref float RandomRotation => ref Projectile.ai[1];


		private Vector2 defaultTargetPos;

		public override void AI()
		{
			if (Projectile.ai[2] == 0)
			{
				Projectile.ai[2] = 1;
				NetUpdate();
				if (Main.myPlayer == Projectile.owner)
					defaultTargetPos = Main.MouseWorld;
				else
					defaultTargetPos = Projectile.Center + (Projectile.velocity * 250f);
				RandomRotation = Main.rand.NextFloat(MathHelper.TwoPi);
			}
			Projectile.velocity = Vector2.Zero;

			if (Projectile.frameCounter++ > 5)
			{
				Projectile.frameCounter = 0;
				if (++Projectile.frame >= Main.projFrames[Type])
				{
					Projectile.frame = 0;
				}
			}

			Projectile.spriteDirection = (Projectile.rotation.ToRotationVector2().X > 0).ToDirectionInt();


			if (Main.myPlayer == Projectile.owner)
			{
				Target = Projectile.Center.GetMinionTarget(ApplySpeed(12f) * 120, Owner, true, true)?.whoAmI ?? -1;
				if (Target != OldTarget)
				{
					NetUpdate();
					OldTarget = Target;
				}
			}
			if (Target != -1)
			{
				var targetnpc = Main.npc[Target];
				Projectile.rotation = Projectile.SafeDirectionTo(targetnpc.Center).ToRotation();
			}
			else
			{
				Projectile.rotation = Projectile.SafeDirectionTo(defaultTargetPos).ToRotation();
			}

			Imbue?.LingeringEffects(Proj1, Projectile.velocity, Projectile);
			SecondImbue?.LingeringEffects(Proj1, Projectile.velocity, Projectile);

			Imbue?.LingeringEffects(Proj2, Projectile.velocity, Projectile);
			SecondImbue?.LingeringEffects(Proj2, Projectile.velocity, Projectile);

			Imbue?.LingeringEffects(Proj3, Projectile.velocity, Projectile);
			SecondImbue?.LingeringEffects(Proj3, Projectile.velocity, Projectile);

			Imbue?.LingeringEffects(Proj4, Projectile.velocity, Projectile);
			SecondImbue?.LingeringEffects(Proj4, Projectile.velocity, Projectile);
		}

		public override void OnKill(int timeLeft)
		{
			if (Main.myPlayer == Projectile.owner)
			{
				// spawn projectiles
				if (Target == -1)
				{
					Projectile.rotation = Proj1.Center().DirectionTo(defaultTargetPos).ToRotation();
				}
				AOUtils.ShootProjectile(Projectile.GetSource_Death(), Proj1.Center(), Projectile.rotation.ToRotationVector2() * 12f, ModContent.ProjectileType<SpiritBlast>(), Projectile.damage / 4, Projectile.knockBack / 4f, Projectile.owner, Imbue, SecondImbue, true);
				if (Target == -1)
				{
					Projectile.rotation = Proj2.Center().DirectionTo(defaultTargetPos).ToRotation();
				}
				AOUtils.ShootProjectile(Projectile.GetSource_Death(), Proj2.Center(), Projectile.rotation.ToRotationVector2() * 12f, ModContent.ProjectileType<SpiritBlast>(), Projectile.damage / 4, Projectile.knockBack / 4f, Projectile.owner, Imbue, SecondImbue, true);
				if (Target == -1)
				{
					Projectile.rotation = Proj3.Center().DirectionTo(defaultTargetPos).ToRotation();
				}
				AOUtils.ShootProjectile(Projectile.GetSource_Death(), Proj3.Center(), Projectile.rotation.ToRotationVector2() * 12f, ModContent.ProjectileType<SpiritBlast>(), Projectile.damage / 4, Projectile.knockBack / 4f, Projectile.owner, Imbue, SecondImbue, true);
				if (Target == -1)
				{
					Projectile.rotation = Proj4.Center().DirectionTo(defaultTargetPos).ToRotation();
				}
				AOUtils.ShootProjectile(Projectile.GetSource_Death(), Proj4.Center(), Projectile.rotation.ToRotationVector2() * 12f, ModContent.ProjectileType<SpiritBlast>(), Projectile.damage / 4, Projectile.knockBack / 4f, Projectile.owner, Imbue, SecondImbue, true);
			}
		}

		public override bool? CanDamage() => false;

		public override bool PreDraw(ref Color lightColor)
		{
			SpriteEffects mode = Projectile.spriteDirection > 0 ? SpriteEffects.None : FlippedMode;

			lightColor = Imbue?.Colour ?? lightColor;

			Lighting.AddLight(Proj1.Center(), Imbue.Colour.ToVector3() * Projectile.scale / 4f);
			Main.EntitySpriteDraw(Sprite, Proj1.Center() - Main.screenPosition, new(0, Sprite.Height / Main.projFrames[Type] * Projectile.frame, Sprite.Width, Sprite.Height / Main.projFrames[Type]), Projectile.GetAlpha(lightColor), Projectile.rotation, new Vector2(Sprite.Width, Sprite.Height / Main.projFrames[Type]) / 2f, Projectile.scale, mode);

			Lighting.AddLight(Proj2.Center(), Imbue.Colour.ToVector3() * Projectile.scale / 4f);
			Main.EntitySpriteDraw(Sprite, Proj2.Center() - Main.screenPosition, new(0, Sprite.Height / Main.projFrames[Type] * Projectile.frame, Sprite.Width, Sprite.Height / Main.projFrames[Type]), Projectile.GetAlpha(lightColor), Projectile.rotation, new Vector2(Sprite.Width, Sprite.Height / Main.projFrames[Type]) / 2f, Projectile.scale, mode);

			Lighting.AddLight(Proj3.Center(), Imbue.Colour.ToVector3() * Projectile.scale / 4f);
			Main.EntitySpriteDraw(Sprite, Proj3.Center() - Main.screenPosition, new(0, Sprite.Height / Main.projFrames[Type] * Projectile.frame, Sprite.Width, Sprite.Height / Main.projFrames[Type]), Projectile.GetAlpha(lightColor), Projectile.rotation, new Vector2(Sprite.Width, Sprite.Height / Main.projFrames[Type]) / 2f, Projectile.scale, mode);

			Lighting.AddLight(Proj4.Center(), Imbue.Colour.ToVector3() * Projectile.scale / 4f);
			Main.EntitySpriteDraw(Sprite, Proj4.Center() - Main.screenPosition, new(0, Sprite.Height / Main.projFrames[Type] * Projectile.frame, Sprite.Width, Sprite.Height / Main.projFrames[Type]), Projectile.GetAlpha(lightColor), Projectile.rotation, new Vector2(Sprite.Width, Sprite.Height / Main.projFrames[Type]) / 2f, Projectile.scale, mode);

			return false;
		}
	}
}
