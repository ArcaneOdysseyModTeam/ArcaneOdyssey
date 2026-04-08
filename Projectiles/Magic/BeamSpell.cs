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
		public Texture2D MidSprite => ArcaneOdysseyMod.Sets.raySprites[Imbue?.Type ?? ModContent.ItemType<WindMagic>()]?.Value ?? ArcaneOdysseyMod.Sets.raySprites[ModContent.ItemType<WindMagic>()]?.Value ?? base.Sprite;
		public Texture2D EndSprite => ArcaneOdysseyMod.Sets.rayEndSprites[Imbue?.Type ?? ModContent.ItemType<WindMagic>()]?.Value ?? ArcaneOdysseyMod.Sets.rayEndSprites[ModContent.ItemType<WindMagic>()]?.Value ?? base.Sprite;
		public Texture2D StartSprite => ArcaneOdysseyMod.Sets.rayStartSprites[Imbue?.Type ?? ModContent.ItemType<WindMagic>()]?.Value ?? ArcaneOdysseyMod.Sets.rayStartSprites[ModContent.ItemType<WindMagic>()]?.Value ?? base.Sprite;


		public override float Size => .75f;


		internal Vector2? origin = null;
		internal Vector2? end = null;
		public override bool CanHaveImbueVFX => !dying;

		public bool dying = false;

		public override void AI()
		{
			if (Projectile.timeLeft > LingerTime)
			{
				Projectile.rotation = Projectile.velocity.ToRotation();
				Projectile.spriteDirection = (Projectile.velocity.X > 0).ToDirectionInt();
				origin ??= Projectile.Center;
				//Imbue?.LingeringEffects(Projectile.Hitbox, Projectile.velocity, Projectile);
				//SecondImbue?.LingeringEffects(Projectile.Hitbox, Projectile.velocity, Projectile);
			}
			else
			{
				Projectile.hide = AOPlayerOwner.myCircle is not null && Projectile.hide;
				end ??= Projectile.Center;
				Projectile.Center = origin.GetValueOrDefault(Owner.Center);
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
			Main.EntitySpriteDraw(StartSprite, Projectile.Center - Main.screenPosition, StartSprite.Frame(1, Main.projFrames[Type], 0, Projectile.frame), Projectile.GetAlpha(), Projectile.AngleTo(end.GetValueOrDefault(Owner.Center)), new Vector2(StartSprite.Width, StartSprite.Height / Main.projFrames[Type]) / 2f, Projectile.scale, mode);
			var info = AOUtils.DrawChain(Projectile.Center, end.GetValueOrDefault(origin.GetValueOrDefault(Owner.Center)), MidSprite, Projectile.scale, Main.projFrames[Type], Projectile.frame, Projectile.GetAlpha(), mode);
			var rot = end.GetValueOrDefault(origin.GetValueOrDefault(Projectile.Center)).DirectionFrom(Projectile.Center).ToRotation();
			var ending = info.Ending + new Vector2(EndSprite.Width * Projectile.scale, 0).RotatedBy(rot);
			Main.EntitySpriteDraw(EndSprite, ending - Main.screenPosition, EndSprite.Frame(1, Main.projFrames[Type], 0, info.FinalFrame), Projectile.GetAlpha(), rot, new Vector2(EndSprite.Width, EndSprite.Height / Main.projFrames[Type]) / 2f, Projectile.scale, mode);
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
