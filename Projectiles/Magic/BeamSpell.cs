using ArcaneOdyssey.Projectiles.Base;
using System.Collections.Generic;
using System.IO;

namespace ArcaneOdyssey.Projectiles.Magic
{
	public class BeamSpell : MagicSpell
	{
		public const int TravelTime = 75;
		public const int LingerTime = 100 * 60;

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			Main.projFrames[Type] = 4;
			ProjectileID.Sets.DrawScreenCheckFluff[Type] = 99999999;
		}

		public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
		{
			if (Projectile.hide)
				overWiresUI.Add(index);
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.height = Projectile.width = 10; // hitscan
			Projectile.stopsDealingDamageAfterPenetrateHits = true;
			Projectile.extraUpdates = 100;
			Projectile.timeLeft = TravelTime + LingerTime;
			Projectile.frame = Main.rand.Next(Main.projFrames[Type]);
			Projectile.hide = true;
			Projectile.ArmorPenetration += 5;
		}

		public override string Texture => AOUtils.BlankTexture;
		public Texture2D MidSprite => Imbue?.Ray?.Value ?? Sprite;
		public Texture2D EndSprite => Imbue?.RayEnd?.Value ?? Sprite;
		public Texture2D StartSprite => Imbue?.RayStart?.Value ?? Sprite;


		public override float Size => .75f;
		public override bool CanHaveImbueVFX => !dying;


		internal Vector2 origin = default;
		public bool dying = false;

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(dying);
			writer.WriteVector2(origin);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			dying = reader.ReadBoolean();
			origin = reader.ReadVector2();
		}

		public override void AI()
		{
			if (origin == default)
			{
				origin = Projectile.Center;
				Projectile.spriteDirection = (Projectile.velocity.X > 0).ToDirectionInt();
			}

			if (Projectile.timeLeft <= LingerTime)
			{
				Projectile.hide = AOPlayerOwner.myCircle is not null && Projectile.hide;
				Projectile.velocity = Vector2.Zero;
				dying = true;

				if (Projectile.numUpdates == 0)
					Projectile.Opacity -= Circle.GlobalChargeSpeed * 2f;

				if (Projectile.alpha >= 255 && Main.myPlayer == Projectile.owner)
				{
					Kill();
				}
			}
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			width = height = 0;
			fallThrough = true;
			return true;
		}

		public override bool? CanCutTiles() => !dying;

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Projectile.tileCollide = false;
			Projectile.timeLeft = LingerTime;
			return false;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			GetDrawColour(ref lightColor);
			if (origin != default)
			{
				SpriteEffects mode = Projectile.spriteDirection > 0 ? SpriteEffects.None : FlippedMode;
				var info = AOUtils.DrawChain(origin, Projectile.Center, MidSprite, Projectile.scale, Main.projFrames[Type], Projectile.frame, Projectile.GetAlpha(lightColor), mode);
				var frame = StartSprite.Frame(1, Main.projFrames[Type], 0, Projectile.frame);
				Main.EntitySpriteDraw(StartSprite, origin - Main.screenPosition, frame, Projectile.GetAlpha(lightColor), info.Rotation, frame.Size() / 2f, Projectile.scale, mode);
				var ending = info.Ending + new Vector2(EndSprite.Width * Projectile.scale, 0).RotatedBy(info.Rotation);
				Main.EntitySpriteDraw(EndSprite, ending - Main.screenPosition, EndSprite.Frame(1, Main.projFrames[Type], 0, info.FinalFrame), Projectile.GetAlpha(lightColor), info.Rotation, new Vector2(EndSprite.Width, EndSprite.Height / Main.projFrames[Type]) / 2f, Projectile.scale, mode);
			}
			return false;
		}

		public override bool? CanDamage()
		{
			if (!dying)
				return null;
			return false;
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			Projectile.timeLeft = LingerTime;
		}
	}
}
