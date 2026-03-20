using ArcaneOdyssey.Projectiles.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Projectiles.Magic
{
	public class MagicRay : MagicSpell
	{
		public override bool CanHaveImbueVFX => false;

		public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
		{
			overWiresUI.Add(index);
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.height = Projectile.width = 30; // thicker
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.hide = true;
		}

		public static Asset<Texture2D> EndTexture;

		public override void AutoStaticDefaults()
		{
			base.AutoStaticDefaults();
			if (!Main.dedServ)
				EndTexture = ModContent.Request<Texture2D>(Texture + "_End");
		}

		public Vector2 End
		{
			get
			{
				Vector2 proj = Projectile.Center;
				for (float i = 0; i < 85f; i++)
				{
					proj += Projectile.velocity;
					if (!Collision.CanHitLine(Projectile.Center, Projectile.width / 10, Projectile.height / 10, proj, Projectile.width / 10, Projectile.height / 10))
					{
						break;
					}
				}
				return proj;
			}
		}

		public override void AI()
		{
			if (AOPlayerOwner.myCircle is not null)
			{
				Projectile.Opacity = AOPlayerOwner.myCircle.Projectile.Opacity;
				Projectile.velocity = AOPlayerOwner.myCircle.Projectile.rotation.ToRotationVector2() * Projectile.velocity.Length();
				Projectile.Center = AOPlayerOwner.myCircle.Projectile.Center - Projectile.velocity;
				if (Main.GameUpdateCount % 5 == 0)
				{
					for (Vector2 i = Vector2.Zero; i.Length() < Projectile.Center.Distance(End); i += Projectile.velocity)
					{
						Imbue?.LingeringEffects(Projectile.Hitbox with { Location = (Projectile.position + i).ToPoint() }, Projectile.velocity, Projectile);
						SecondImbue?.LingeringEffects(Projectile.Hitbox with { Location = (Projectile.position + i).ToPoint() }, Projectile.velocity, Projectile);
					}
				}
			}
			else
			{
				Kill();
			}
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			float _ = 0f;
			if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, End, projHitbox.Width, ref _))
			{
				return true;
			}

			return false;
		}

		public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
		{
			if (AOPlayerOwner?.myCircle is not null)
				modifiers.SourceDamage *= AOPlayerOwner.myCircle.Projectile.Opacity;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			AOUtils.DrawChain(Projectile.Center, End, Sprite, Projectile.scale, colour: Projectile.GetAlpha(Imbue?.Colour ?? lightColor));
			return false;
		}

		public override void PostDraw(Color lightColor)
		{
			var end = End;
			Main.EntitySpriteDraw(EndTexture.Value, end - Main.screenPosition, null, Projectile.GetAlpha(Imbue?.Colour ?? lightColor), Projectile.AngleTo(end), EndTexture.Size() / 2f, Projectile.scale, SpriteEffects.None);
		}
	}
}
