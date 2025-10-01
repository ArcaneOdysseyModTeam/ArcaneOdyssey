using ArcaneOdyssey.Content.Projectiles.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Projectiles.Weapons.Abilities
{
	public class Whirlwind : AOPlayerProjectile
	{
		public Color color = Color.White;
		public const int MaxTime = 20;
		public static Texture2D Sprite => ModContent.Request<Texture2D>("ArcaneOdyssey/Content/Projectiles/Weapons/Abilities/Whirlwind").Value;

		public override void SetStaticDefaults()
		{
			//Main.projFrames[Type] = 4;
		}

		public override void SetDefaults()
		{
			Projectile.width = //60;
			Projectile.height = 144;
			Projectile.friendly = true;
			Projectile.timeLeft = MaxTime;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
			Projectile.ownerHitCheck = true;
			Projectile.localNPCHitCooldown = -1;
			Projectile.usesLocalNPCImmunity = true;
		}

		public override void AI()
		{
			aoPlayerOwner ??= Main.player[Projectile.owner].ArcaneOdyssey();
			Player player = aoPlayerOwner.Player;
			//ManageFrame();
			Projectile.rotation += (MathHelper.Pi / (MaxTime / 2)) * 1.1f;
			//Projectile.rotation = (MathHelper.Pi * 4 / Projectile.timeLeft) - MathHelper.PiOver2;
			Projectile.Center = player.MountedCenter + (Projectile.rotation.ToRotationVector2() * 40f);
			//Projectile.rotation = (Projectile.Center - player.Center).ToRotation() + MathHelper.PiOver4;
		}

		public override void PostDraw(Color lightColor)
		{
			Main.EntitySpriteDraw(Sprite, Projectile.Center - Main.screenPosition, new Rectangle(0, Projectile.height * Projectile.frame, Projectile.width, Projectile.height), color, Projectile.rotation, Projectile.GetDrawOriginCentre(), Projectile.scale * .95f, SpriteEffects.None);
		}

		public void ManageFrame()
		{

			//var stagething = Projectile.timeLeft < MaxTime / 2f ? MaxTime / 2f : MaxTime * 2f;
			//if (Projectile.timeLeft < MaxTime / 2f)
			//{
			//	if (Projectile.timeLeft < stagething / 4f)
			//	{
			//		Projectile.frame = 3;
			//	}
			//	else if (Projectile.timeLeft < stagething / 3f)
			//	{
			//		Projectile.frame = 2;
			//	}
			//	else if (Projectile.timeLeft < stagething / 2f)
			//	{
			//		Projectile.frame = 1;
			//	}
			//	else
			//	{
			//		Projectile.frame = 0;
			//	}
			//}
			//else
			//{
			//	if (Projectile.timeLeft < stagething / 4f)
			//	{
			//		Projectile.frame = 0;
			//	}
			//	else if (Projectile.timeLeft < stagething / 3f)
			//	{
			//		Projectile.frame = 1;
			//	}
			//	else if (Projectile.timeLeft < stagething / 2f)
			//	{
			//		Projectile.frame = 2;
			//	}
			//	else
			//	{
			//		Projectile.frame = 3;
			//	}
			//}
		}
	}
}
