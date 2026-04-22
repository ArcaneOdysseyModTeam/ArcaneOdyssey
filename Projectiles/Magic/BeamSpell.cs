using ArcaneOdyssey.Imbues.Magic.Normal;
using ArcaneOdyssey.Projectiles.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

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
		}


		public override string Texture => typeof(WindMagic).FullName.Replace('.', '/').Replace(nameof(WindMagic), ModContent.GetInstance<WindMagic>().AttackPrefix + "RayEnd");
		public Texture2D MidSprite => ArcaneOdysseyMod.Sets.Assets.raySprites[Imbue?.Type ?? ModContent.ItemType<WindMagic>()]?.Value ?? ArcaneOdysseyMod.Sets.Assets.raySprites[ModContent.ItemType<WindMagic>()]?.Value ?? base.Sprite;
		public Texture2D EndSprite => ArcaneOdysseyMod.Sets.Assets.rayEndSprites[Imbue?.Type ?? ModContent.ItemType<WindMagic>()]?.Value ?? ArcaneOdysseyMod.Sets.Assets.rayEndSprites[ModContent.ItemType<WindMagic>()]?.Value ?? base.Sprite;
		public Texture2D StartSprite => ArcaneOdysseyMod.Sets.Assets.rayStartSprites[Imbue?.Type ?? ModContent.ItemType<WindMagic>()]?.Value ?? ArcaneOdysseyMod.Sets.Assets.rayStartSprites[ModContent.ItemType<WindMagic>()]?.Value ?? base.Sprite;


		public override float Size => .75f;


		internal Vector2 origin = default;
		internal Vector2? end = null;
		public override bool CanHaveImbueVFX => !dying;

		public bool dying = false;

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
				end ??= Projectile.Center;
				Projectile.Center = origin;
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
			SpriteEffects mode = Projectile.spriteDirection > 0 ? SpriteEffects.None : FlippedMode;
			var info = AOUtils.DrawChain(Projectile.Center, end.GetValueOrDefault(origin), MidSprite, Projectile.scale, Main.projFrames[Type], Projectile.frame, Projectile.GetAlpha(), mode);
			var frame = StartSprite.Frame(1, Main.projFrames[Type], 0, Projectile.frame);
			Main.EntitySpriteDraw(StartSprite, Projectile.Center - Main.screenPosition, frame, Projectile.GetAlpha(), info.Rotation, frame.Size() / 2f, Projectile.scale, mode);
			var ending = info.Ending + new Vector2(EndSprite.Width * Projectile.scale, 0).RotatedBy(info.Rotation);
			Main.EntitySpriteDraw(EndSprite, ending - Main.screenPosition, EndSprite.Frame(1, Main.projFrames[Type], 0, info.FinalFrame), Projectile.GetAlpha(), info.Rotation, new Vector2(EndSprite.Width, EndSprite.Height / Main.projFrames[Type]) / 2f, Projectile.scale, mode);
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
