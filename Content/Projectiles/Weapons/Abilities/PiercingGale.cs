using ArcaneOdyssey.Content.Projectiles.Base;
using ArcaneOdyssey.Content.Projectiles.Berserker;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Projectiles.Weapons.Abilities
{
	public class PiercingGale : AOPlayerProjectile
	{
		public static int AfterimagesType => ModContent.ProjectileType<Crescendo>();
		public static Texture2D Afterimages => TextureAssets.Projectile[AfterimagesType].Value;
		public Color Colour => Imbue?.GetColour(Color.Orange) ?? Color.Orange;

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			Main.projFrames[Type] = 4;
		}

		public List<Vector2> cache = [];

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = Projectile.height = 24;
			Projectile.friendly = true;
			Projectile.timeLeft = 60 * (Projectile.extraUpdates + 1);
			Projectile.DamageType = DamageClass.Melee;
		}

		public override bool PreAI()
		{
			cache.Insert(0, Projectile.Center);
			if (cache.Count > 10)
			{
				cache.RemoveAt(10);
			}
			Projectile.localAI[0]++;
			return base.PreAI();
		}

		public override void AI()
		{
			if (Projectile.ai[0] == 0)
			{
				Projectile.ai[0] = 1;
				SoundEngine.PlaySound(SoundID.Item66, Projectile.Center);
				if (Main.myPlayer == Projectile.owner)
				{
					Projectile.netUpdate = true;
					Projectile.netSpam = 0;
				}
			}

			Projectile.rotation = Projectile.velocity.ToRotation();

			if (++Projectile.frameCounter > 3)
			{
				Projectile.frameCounter = 0;
				if (++Projectile.frame >= Main.projFrames[Type])
				{
					Projectile.frame = 0;
				}
			}
		}

		public override bool PreKill(int timeLeft)
		{
			if (!Main.dedServ)
			{
				for (float i = 0; i < 60; i++)
				{
					var centre = Main.rand.NextFloat(MathHelper.TwoPi).ToRotationVector2();
					var dust = AOUtils.NewDustImperfect(centre + Projectile.Center, DustID.BubbleBurst_White, centre * (Projectile.width / 5f), 0, Colour, 2f);
					dust.noLight = true;
					dust.noGravity = true;
				}
			}
			return base.PreKill(timeLeft);
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			width /= 4;
			height /= 4;
			fallThrough = true;
			return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
		}

		public override bool PreDraw(ref Color lightColor)
		{
			var oldPos = cache.ToArray();
			lightColor = Colour;
			if (Projectile.localAI[0] > 2) Projectile.localAI[0] = 0;
			SpriteEffects mode = Projectile.spriteDirection > 0 ? SpriteEffects.None : SpriteEffects.FlipVertically;
			for (int k = oldPos.Length - 1; k > -1; k--)
			{
				if (k % 3 == Projectile.localAI[0])
				{
					Vector2 drawPos = oldPos[k] + new Vector2(0f, Projectile.gfxOffY);
					var colour2 = Projectile.GetAlpha(lightColor) * ((oldPos.Length - k) / (float)oldPos.Length);
					Main.EntitySpriteDraw(Afterimages, drawPos - Main.screenPosition, new(0, 0, Afterimages.Width, Afterimages.Height / Main.projFrames[AfterimagesType]), colour2, Projectile.rotation, new Vector2(Afterimages.Width, Afterimages.Height / Main.projFrames[AfterimagesType]) / 2f, Projectile.scale * .15f + (k * .05f), mode);
				}
			}
			return base.PreDraw(ref lightColor);
		}
	}
}
