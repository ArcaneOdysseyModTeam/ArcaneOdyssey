using ArcaneOdyssey.Imbues;
using ArcaneOdyssey.Imbues.Magic.Normal;
using ArcaneOdyssey.Projectiles.Base;
using System.Collections.Generic;

namespace ArcaneOdyssey.Projectiles.Magic
{
	public class ArraySpell : MagicSpell
	{
		// ai 2 is first frame bool
		public override string Texture => AOUtils.BlankTexture;

		public override Asset<Texture2D> Sprite => Imbue?.Blast ?? base.Sprite;

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
				if (++Projectile.frame >= ImbueID.Sets.BlastFrames[Imbue?.ID ?? WindMagic.StaticID])
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
				else
				{
					var targetnpc = Main.npc[Target];
					Projectile.rotation = Proj1.Center().DirectionTo(targetnpc.Center).ToRotation();
				}
				AOUtils.ShootProjectile(Projectile.GetSource_Death(), Proj1.Center(), Projectile.rotation.ToRotationVector2() * 12f, ModContent.ProjectileType<HomingArray>(), Projectile.damage / 4, Projectile.knockBack / 4f, Projectile.owner, Imbue, SecondImbue, true, Target);
				if (Target == -1)
				{
					Projectile.rotation = Proj2.Center().DirectionTo(Main.MouseWorld).ToRotation();
				}
				else
				{
					var targetnpc = Main.npc[Target];
					Projectile.rotation = Proj2.Center().DirectionTo(targetnpc.Center).ToRotation();
				}
				AOUtils.ShootProjectile(Projectile.GetSource_Death(), Proj2.Center(), Projectile.rotation.ToRotationVector2() * 12f, ModContent.ProjectileType<HomingArray>(), Projectile.damage / 4, Projectile.knockBack / 4f, Projectile.owner, Imbue, SecondImbue, true, Target);
				if (Target == -1)
				{
					Projectile.rotation = Proj3.Center().DirectionTo(Main.MouseWorld).ToRotation();
				}
				else
				{
					var targetnpc = Main.npc[Target];
					Projectile.rotation = Proj3.Center().DirectionTo(targetnpc.Center).ToRotation();
				}
				AOUtils.ShootProjectile(Projectile.GetSource_Death(), Proj3.Center(), Projectile.rotation.ToRotationVector2() * 12f, ModContent.ProjectileType<HomingArray>(), Projectile.damage / 4, Projectile.knockBack / 4f, Projectile.owner, Imbue, SecondImbue, true, Target);
				if (Target == -1)
				{
					Projectile.rotation = Proj4.Center().DirectionTo(Main.MouseWorld).ToRotation();
				}
				else
				{
					var targetnpc = Main.npc[Target];
					Projectile.rotation = Proj4.Center().DirectionTo(targetnpc.Center).ToRotation();
				}
				AOUtils.ShootProjectile(Projectile.GetSource_Death(), Proj4.Center(), Projectile.rotation.ToRotationVector2() * 12f, ModContent.ProjectileType<HomingArray>(), Projectile.damage / 4, Projectile.knockBack / 4f, Projectile.owner, Imbue, SecondImbue, true, Target);
			}
		}

		public override bool? CanDamage() => false;

		public override bool PreDraw(ref Color lightColor)
		{
			GetDrawColour(ref lightColor);

			SpriteEffects mode = Projectile.spriteDirection > 0 ? SpriteEffects.None : FlippedMode;

			var frame = Sprite.Frame(1, ImbueID.Sets.BlastFrames[Imbue?.ID ?? WindMagic.StaticID], 0, Projectile.frame);

			Main.EntitySpriteDraw(Sprite.Value, Proj1.Center() - Main.screenPosition, frame, Projectile.GetAlpha(lightColor), Projectile.rotation, frame.Size() / 2f, Projectile.scale, mode);

			Main.EntitySpriteDraw(Sprite.Value, Proj2.Center() - Main.screenPosition, frame, Projectile.GetAlpha(lightColor), Projectile.rotation, frame.Size() / 2f, Projectile.scale, mode);

			Main.EntitySpriteDraw(Sprite.Value, Proj3.Center() - Main.screenPosition, frame, Projectile.GetAlpha(lightColor), Projectile.rotation, frame.Size() / 2f, Projectile.scale, mode);

			Main.EntitySpriteDraw(Sprite.Value, Proj4.Center() - Main.screenPosition, frame, Projectile.GetAlpha(lightColor), Projectile.rotation, frame.Size() / 2f, Projectile.scale, mode);

			return false;
		}
	}
}
