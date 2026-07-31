using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Imbues.Magic.Normal;
using ArcaneOdyssey.Projectiles.Base;
using System.Collections.Generic;
using Terraria.Audio;

namespace ArcaneOdyssey.Projectiles.Enemies.Elius
{
	public class EliusPlacedExplosion : BaseProjectile
	{
		public override string Texture => AOUtils.BlankTexture;

		public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
		{
			behindNPCs.Add(index);
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.timeLeft = 400;
			Projectile.hide = true;
			Projectile.hostile = true;
			Projectile.friendly = false;
			Projectile.penetrate = -1;
			Projectile.height = Projectile.width = 1;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.alpha = 255;
			Projectile.scale = 100 / 2000f;
		}

		public LightningMagic Imbue => ModContent.GetInstance<LightningMagic>();
		public override bool CanHitPlayer(Player target) => false;
		public override void AI()
		{
			Projectile.rotation += Imbue.ApplySpeed(MathHelper.Pi / 120f);

			if (Projectile.timeLeft <= 200)
			{
				if (Projectile.ai[0] == 0)
				{
					Projectile.ai[0] = 1;
					Projectile.Opacity = 1f;
					if (Projectile.owner == Main.myPlayer)
					{
						Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center + new Vector2(0, -1000), Vector2.Zero, ModContent.ProjectileType<EliusTrail>(), 0, 0f, -1, Projectile.Center.X, Projectile.Center.Y);
						Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<EliusExplosion>(), Projectile.damage, 8f, -1);
					}
					SoundEngine.PlaySound(SoundID.Thunder, Projectile.Center); // PORT change to instant
				}
				else
				{
					Projectile.Opacity -= Circle.GlobalChargeSpeed * 2f;
				}
			}
			else
			{
				Dust.NewDustDirect(Projectile.Center - new Vector2(25, 25), 50, 50, DustID.WitherLightning, 0f, -0.1f, 0, default, (200f - (Projectile.timeLeft - 200f)) * 0.005f).noGravity = true;
				Projectile.Opacity += Circle.GlobalChargeSpeed / Circle.GlobalMaxCharge;
			}
		}

		public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)
		{
			modifiers = AOUtils.CalculateImbueDamage(Imbue, target, modifiers);
		}

		public override bool PreDraw(ref Color lightColor)
		{
			lightColor = Imbue.Colour;
			return base.PreDraw(ref lightColor);
		}

		public override Texture2D Sprite => Imbue.Circle.Texture.Value;
	}
}
