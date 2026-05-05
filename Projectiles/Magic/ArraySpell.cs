using ArcaneOdyssey.Imbues.Magic.Lost;
using ArcaneOdyssey.Imbues.Magic.Normal;
using ArcaneOdyssey.Projectiles.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Projectiles.Magic
{
	public class ArraySpell : MagicSpell
	{
		// ai 2 is first frame bool
		public override string Texture => (Mod.Name + "/" + ArcaneOdysseyMod.Sets.Assets.blasts[Imbue?.Type ?? ModContent.ItemType<WindMagic>()]?.Name ?? typeof(WindMagic).FullName.Replace('.', '/').Replace(nameof(WindMagic), ModContent.GetInstance<WindMagic>().AttackPrefix + "Blast")).Replace("\\", "/");

		public override Texture2D Sprite => ArcaneOdysseyMod.Sets.Assets.blasts[Imbue?.Type ?? ModContent.ItemType<WindMagic>()]?.Value ?? base.Sprite;

		public override float Size => .75f;

		public override bool CanHaveImbueVFX => false;

		public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
		{
			overWiresUI.Add(index);
		}

		public Rectangle Proj1 => new(Projectile.Center.X.Round(), Projectile.position.Y.Round() - (20 * Projectile.scale).Round(), (64 * Projectile.scale).Round(), (64 * Projectile.scale).Round());
		public Rectangle Proj2 => new(Proj1.X - (64 * Projectile.scale).Round(), Projectile.position.Y.Round() - (20 * Projectile.scale).Round(), (64 * Projectile.scale).Round(), (64 * Projectile.scale).Round());
		public Rectangle Proj3 => new(Proj1.X + (64 * Projectile.scale).Round(), Projectile.position.Y.Round() + (20 * Projectile.scale).Round(), (64 * Projectile.scale).Round(), (64 * Projectile.scale).Round());
		public Rectangle Proj4 => new(Proj2.X - (64 * Projectile.scale).Round(), Projectile.position.Y.Round() + (20 * Projectile.scale).Round(), (64 * Projectile.scale).Round(), (64 * Projectile.scale).Round());


		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.height = Proj1.Height + 40;
			Projectile.width = Proj1.Width + Proj2.Width + Proj3.Width + Proj4.Width;
			Projectile.timeLeft = 60;
			Projectile.hide = true;
			Projectile.tileCollide = false;
			Target = -1;
		}

		public int Target { get => (int)Projectile.ai[0]; set => Projectile.ai[0] = value; }
		public int OldTarget;

		public override void AI()
		{
			Projectile.velocity = Vector2.Zero;
			if (Projectile.ai[2] == 0)
			{
				Projectile.ai[2] = 1;
				if (Projectile.owner == Main.myPlayer)
				{
					Projectile.netUpdate = true;
					Projectile.netSpam = 0;
				}
			}

			if (Projectile.frameCounter++ > 5)
			{
				Projectile.frameCounter = 0;
				if (++Projectile.frame >= ArcaneOdysseyMod.Sets.BlastMaxFrames[Imbue?.Type ?? ModContent.ItemType<WindMagic>()])
				{
					Projectile.frame = 0;
				}
			}

			Owner.direction = (Projectile.rotation.ToRotationVector2().X > 0).ToDirectionInt();
			Projectile.spriteDirection = Owner.direction;

			
			Projectile.Center = Owner.RotatedRelativePoint(Owner.MountedCenter - new Vector2(0, Player.defaultHeight * Size * Projectile.scale));


			if (Main.myPlayer == Projectile.owner)
			{
				Target = AOUtils.ClosestNPCAt(Projectile.Center, ApplySpeed(12f) * 120, false, true)?.whoAmI ?? -1;
				if (Target != OldTarget)
				{
					Projectile.netUpdate = true;
					Projectile.netSpam = 0;
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
				Projectile.rotation = Projectile.SafeDirectionTo(Main.MouseWorld).ToRotation();
			}
		}

		public override void OnKill(int timeLeft)
		{
			if (Main.myPlayer == Projectile.owner)
			{
				// spawn projectiles
				if (Target == -1)
				{
					Projectile.rotation = Proj1.Center().DirectionTo(Main.MouseWorld).ToRotation();
				}
				AOUtils.ShootProjectile(Projectile.GetSource_Death(), Proj1.Center(), Projectile.rotation.ToRotationVector2() * 12f, ModContent.ProjectileType<HomingArray>(), Projectile.damage / 4, Projectile.knockBack / 4f, Projectile.owner, Imbue, SecondImbue, true, Target);
				if (Target == -1)
				{
					Projectile.rotation = Proj2.Center().DirectionTo(Main.MouseWorld).ToRotation();
				}
				AOUtils.ShootProjectile(Projectile.GetSource_Death(), Proj2.Center(), Projectile.rotation.ToRotationVector2() * 12f, ModContent.ProjectileType<HomingArray>(), Projectile.damage / 4, Projectile.knockBack / 4f, Projectile.owner, Imbue, SecondImbue, true, Target);
				if (Target == -1)
				{
					Projectile.rotation = Proj3.Center().DirectionTo(Main.MouseWorld).ToRotation();
				}
				AOUtils.ShootProjectile(Projectile.GetSource_Death(), Proj3.Center(), Projectile.rotation.ToRotationVector2() * 12f, ModContent.ProjectileType<HomingArray>(), Projectile.damage / 4, Projectile.knockBack / 4f, Projectile.owner, Imbue, SecondImbue, true, Target);
				if (Target == -1)
				{
					Projectile.rotation = Proj4.Center().DirectionTo(Main.MouseWorld).ToRotation();
				}
				AOUtils.ShootProjectile(Projectile.GetSource_Death(), Proj4.Center(), Projectile.rotation.ToRotationVector2() * 12f, ModContent.ProjectileType<HomingArray>(), Projectile.damage / 4, Projectile.knockBack / 4f, Projectile.owner, Imbue, SecondImbue, true, Target);
			}
		}

		public override void PostDraw(Color lightColor)
		{
			if (ModContent.RequestIfExists<Texture2D>(GlowTexture, out var tex))
			{
				SpriteEffects mode = Projectile.spriteDirection > 0 ? SpriteEffects.None : SpriteEffects.FlipVertically;
				Main.EntitySpriteDraw(tex.Value, Proj1.Center() - Main.screenPosition, new(0, tex.Height() / ArcaneOdysseyMod.Sets.BlastMaxFrames[Imbue?.Type ?? ModContent.ItemType<WindMagic>()] * Projectile.frame, tex.Width(), tex.Height() / ArcaneOdysseyMod.Sets.BlastMaxFrames[Imbue?.Type ?? ModContent.ItemType<WindMagic>()]), Projectile.GetAlpha(Color.White), Projectile.rotation, new Vector2(tex.Width(), tex.Height() / ArcaneOdysseyMod.Sets.BlastMaxFrames[Imbue?.Type ?? ModContent.ItemType<WindMagic>()]) / 2f, Projectile.scale, mode);
				
				Main.EntitySpriteDraw(tex.Value, Proj2.Center() - Main.screenPosition, new(0, tex.Height() / ArcaneOdysseyMod.Sets.BlastMaxFrames[Imbue?.Type ?? ModContent.ItemType<WindMagic>()] * Projectile.frame, tex.Width(), tex.Height() / ArcaneOdysseyMod.Sets.BlastMaxFrames[Imbue?.Type ?? ModContent.ItemType<WindMagic>()]), Projectile.GetAlpha(Color.White), Projectile.rotation, new Vector2(tex.Width(), tex.Height() / ArcaneOdysseyMod.Sets.BlastMaxFrames[Imbue?.Type ?? ModContent.ItemType<WindMagic>()]) / 2f, Projectile.scale, mode);
				
				Main.EntitySpriteDraw(tex.Value, Proj3.Center() - Main.screenPosition, new(0, tex.Height() / ArcaneOdysseyMod.Sets.BlastMaxFrames[Imbue?.Type ?? ModContent.ItemType<WindMagic>()] * Projectile.frame, tex.Width(), tex.Height() / ArcaneOdysseyMod.Sets.BlastMaxFrames[Imbue?.Type ?? ModContent.ItemType<WindMagic>()]), Projectile.GetAlpha(Color.White), Projectile.rotation, new Vector2(tex.Width(), tex.Height() / ArcaneOdysseyMod.Sets.BlastMaxFrames[Imbue?.Type ?? ModContent.ItemType<WindMagic>()]) / 2f, Projectile.scale, mode);
				
				Main.EntitySpriteDraw(tex.Value, Proj4.Center() - Main.screenPosition, new(0, tex.Height() / ArcaneOdysseyMod.Sets.BlastMaxFrames[Imbue?.Type ?? ModContent.ItemType<WindMagic>()] * Projectile.frame, tex.Width(), tex.Height() / ArcaneOdysseyMod.Sets.BlastMaxFrames[Imbue?.Type ?? ModContent.ItemType<WindMagic>()]), Projectile.GetAlpha(Color.White), Projectile.rotation, new Vector2(tex.Width(), tex.Height() / ArcaneOdysseyMod.Sets.BlastMaxFrames[Imbue?.Type ?? ModContent.ItemType<WindMagic>()]) / 2f, Projectile.scale, mode);
			}
		}

		public override bool? CanDamage() => false;

		public override bool PreDraw(ref Color lightColor)
		{
			SpriteEffects mode = Projectile.spriteDirection > 0 ? SpriteEffects.None : FlippedMode;

			Lighting.AddLight(Proj1.Center(), Imbue.Colour.ToVector3() * Projectile.scale / 4f);
			Main.EntitySpriteDraw(Sprite, Proj1.Center() - Main.screenPosition, new(0, Sprite.Height / ArcaneOdysseyMod.Sets.BlastMaxFrames[Imbue?.Type ?? ModContent.ItemType<WindMagic>()] * Projectile.frame, Sprite.Width, Sprite.Height / ArcaneOdysseyMod.Sets.BlastMaxFrames[Imbue?.Type ?? ModContent.ItemType<WindMagic>()]), Projectile.GetAlpha(lightColor), Projectile.rotation, new Vector2(Sprite.Width, Sprite.Height / ArcaneOdysseyMod.Sets.BlastMaxFrames[Imbue?.Type ?? ModContent.ItemType<WindMagic>()]) / 2f, Projectile.scale, mode);

			Lighting.AddLight(Proj2.Center(), Imbue.Colour.ToVector3() * Projectile.scale / 4f);
			Main.EntitySpriteDraw(Sprite, Proj2.Center() - Main.screenPosition, new(0, Sprite.Height / ArcaneOdysseyMod.Sets.BlastMaxFrames[Imbue?.Type ?? ModContent.ItemType<WindMagic>()] * Projectile.frame, Sprite.Width, Sprite.Height / ArcaneOdysseyMod.Sets.BlastMaxFrames[Imbue?.Type ?? ModContent.ItemType<WindMagic>()]), Projectile.GetAlpha(lightColor), Projectile.rotation, new Vector2(Sprite.Width, Sprite.Height / ArcaneOdysseyMod.Sets.BlastMaxFrames[Imbue?.Type ?? ModContent.ItemType<WindMagic>()]) / 2f, Projectile.scale, mode);

			Lighting.AddLight(Proj3.Center(), Imbue.Colour.ToVector3() * Projectile.scale / 4f);
			Main.EntitySpriteDraw(Sprite, Proj3.Center() - Main.screenPosition, new(0, Sprite.Height / ArcaneOdysseyMod.Sets.BlastMaxFrames[Imbue?.Type ?? ModContent.ItemType<WindMagic>()] * Projectile.frame, Sprite.Width, Sprite.Height / ArcaneOdysseyMod.Sets.BlastMaxFrames[Imbue?.Type ?? ModContent.ItemType<WindMagic>()]), Projectile.GetAlpha(lightColor), Projectile.rotation, new Vector2(Sprite.Width, Sprite.Height / ArcaneOdysseyMod.Sets.BlastMaxFrames[Imbue?.Type ?? ModContent.ItemType<WindMagic>()]) / 2f, Projectile.scale, mode);

			Lighting.AddLight(Proj4.Center(), Imbue.Colour.ToVector3() * Projectile.scale / 4f);
			Main.EntitySpriteDraw(Sprite, Proj4.Center() - Main.screenPosition, new(0, Sprite.Height / ArcaneOdysseyMod.Sets.BlastMaxFrames[Imbue?.Type ?? ModContent.ItemType<WindMagic>()] * Projectile.frame, Sprite.Width, Sprite.Height / ArcaneOdysseyMod.Sets.BlastMaxFrames[Imbue?.Type ?? ModContent.ItemType<WindMagic>()]), Projectile.GetAlpha(lightColor), Projectile.rotation, new Vector2(Sprite.Width, Sprite.Height / ArcaneOdysseyMod.Sets.BlastMaxFrames[Imbue?.Type ?? ModContent.ItemType<WindMagic>()]) / 2f, Projectile.scale, mode);
			
			return false;
		}
	}
}
