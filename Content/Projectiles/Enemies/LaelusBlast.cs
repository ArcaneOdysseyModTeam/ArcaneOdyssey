using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Imbues.Relics;
using ArcaneOdyssey.Content.Projectiles.Base;
using ArcaneOdyssey.Content.Projectiles.Relics;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Projectiles.Enemies
{
	public class LaelusBlast : AOBaseProjectile
	{
		public Imbuable Imbue = ModContent.GetInstance<NyxStaff>();

		public override string Texture => AOUtils.GetTexture<SpiritBlast>();

		public override void SetDefaults()
		{
			Projectile.DamageType = DamageClass.Summon;
			Projectile.hostile = true;
			Projectile.height = Projectile.width = 64;
			Projectile.timeLeft = 2 * 60;
			Projectile.Opacity = .95f;
		}

		public override void SetStaticDefaults()
		{
			Main.projFrames[Type] = 4;
		}

		public override void AI()
		{
			Imbue?.LingeringEffects(Projectile.Hitbox, Projectile.velocity, Projectile);
			if (Projectile.ai[0] == 0)
			{
				Projectile.ai[0] = 1;
				SoundEngine.PlaySound(Imbue?.ImbueSound, Projectile.Center);
				Imbue?.SpawningEffects(Projectile.Hitbox, Projectile.velocity);
				if (Main.myPlayer == Projectile.owner)
				{
					Projectile.netUpdate = true;
					Projectile.netSpam = 0;
				}
			}

			Projectile.rotation = Projectile.velocity.ToRotation();
			if (Projectile.frameCounter++ > 5)
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
			Imbue?.KillEffects(Projectile.Hitbox, Projectile);
			return base.PreKill(timeLeft);
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			width = Projectile.width / 4;
			height = Projectile.height / 4;
			fallThrough = true;
			return true;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			lightColor = Imbue?.GetColour() ?? Color.White;
			return base.PreDraw(ref lightColor);
		}
	}
}
