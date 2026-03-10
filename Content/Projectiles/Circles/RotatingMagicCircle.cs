using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Projectiles.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;

namespace ArcaneOdyssey.Content.Projectiles.Circles
{
	public class RotatingMagicCircle : BaseMagicCircle
	{
		public override float AOSize => 100 / 2000f;
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.height = Projectile.width = 2000;
			Projectile.tileCollide = false;
			playedsound = false;
			Projectile.hide = true;
		}

		public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
		{
			behindNPCs.Add(index);
		}

		public override void AI()
		{
			if (Projectile.position != Projectile.oldPosition && Main.myPlayer == Projectile.owner)
			{
				Projectile.netUpdate = true;
				Projectile.netSpam = 0;
			}

			MarkedForDeath |= !(Owner.channel || Main.mouseRight) || Owner.dead;
			if (!MarkedForDeath)
			{
				AOPlayerOwner.HeavySkillActive = true;
				AOPlayerOwner.myCircle = Projectile;
				if (Projectile.ai[1] != 2)
				{
					Projectile.Center = Owner.RotatedRelativePoint(Owner.MountedCenter);
				}
				else
				{
					Owner.itemAnimation = Owner.itemAnimationMax;
					Owner.itemTime = Owner.itemTimeMax;
					if (Main.myPlayer == Projectile.owner)
					{
						Owner.itemRotation = Owner.RotatedRelativePoint(Owner.MountedCenter).DirectionTo(Vector2.Lerp(Projectile.Center, Main.MouseWorld, .5f)).ToRotation();
						if (Owner.direction != 1)
						{
							Owner.itemRotation += MathHelper.Pi;
						}
						if (Vector2.Distance(Main.MouseWorld, Owner.position) < 400)
						{
							Projectile.Center = Projectile.Center.MoveTowards(Main.MouseWorld, ApplySpeed(10f));
						}
						else
							Projectile.Center = Projectile.Center.MoveTowards(Owner.Center + Owner.Center.DirectionTo(Main.MouseWorld) * 400, ApplySpeed(10f));
					}
				}
			}

			Projectile.rotation += ApplySpeed(MathHelper.Pi / 120f);
		}

		public override void OnKill(int timeLeft)
		{
			if (AOPlayerOwner.myCircle is not null)
			{
				AOPlayerOwner.myCircle = null;
				Owner.channel = false;
			}
		}

		public override string Texture => $"{Mod.Name}/Effects/MagicCircles/{ArcaneOdysseyClientConfig.Instance.MagicCircleType}";

		public override bool PreDraw(ref Color lightColor)
		{
			if (Imbue is null or AOMagic)
			{
				lightColor = Imbue?.GetColour(Color.White) ?? Color.White;
				Lighting.AddLight(Projectile.Center, lightColor.ToVector3());
			}
			else
				lightColor = Color.Transparent;

			Main.EntitySpriteDraw(Sprite, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(lightColor), Projectile.rotation, Sprite.Size() / 2f, Projectile.scale, SpriteEffects.None);
			return false;
		}
	}
}
