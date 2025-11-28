using ArcaneOdyssey.Content.Projectiles.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey.Content.Projectiles.Weapons.Abilities
{
	public class ColossalCleave : AOPlayerProjectile
	{
		public override float AOSpeed => .65f;
		public override float AOSize => 1.2f;
		public override float AODamage => 1.15f;
		public override SoundStyle? DebuffApplySound => SoundID.NPCHit42;

		public AOItemTiers AOWeaponTier = AOItemTiers.Good;

		public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.penetrate = -1;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.damage = (int)WeaponDamage(AOWeaponTier);
			Projectile.timeLeft = 60*3;
			Projectile.friendly = true;
			Projectile.height = 234;
			Projectile.width = 74;
			Projectile.knockBack = 4.5f;
		}

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ProjectileID.Sets.TrailingMode[Type] = 0;
		}

		public Texture2D Sprite => ModContent.Request<Texture2D>(Texture).Value;

        public override bool PreDraw(ref Color lightColor)
        {
            for (int k = Projectile.oldPos.Length - 1; k > -1; k--)
            {
                Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + Projectile.GetDrawOriginCentre();// + new Vector2(0f, Projectile.gfxOffY);
                Color colour = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                Main.EntitySpriteDraw(Sprite, drawPos, null, Imbue is not null ? Color.Lerp(Imbue.GetColor(colour), colour, .5f) : colour, Projectile.rotation, Projectile.GetDrawOriginCentre(), Projectile.scale, SpriteEffects.None, 0);
            }
            return false;
        }

        public override void AI()
		{
			if (Projectile.ai[0] == 0)
			{
				Projectile.ai[0] = 1;
				Projectile.netUpdate = true;
			}

			if (Projectile.localAI[0] >= 30 && !Main.dedServ)
			{
				Projectile.localAI[0] = 0;
				for (int i = 1; i < 20; i++)
				{
					Imbue?.ExplosionEffects(Projectile);
				}
			}
			Projectile.localAI[0]++;

			if (Projectile.timeLeft <= 30)
			{
				Projectile.ai[1]++;
			}

			if (Projectile.ai[1] != 0)
			{
				Projectile.alpha += 255 / 30;
			}
			else
			{
				Projectile.rotation = Projectile.velocity.ToRotation();
			}
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			height = width = 1;
			fallThrough = true;
			return true;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Projectile.velocity = Vector2.Zero;
			Projectile.timeLeft = 30;
			Projectile.ai[1]++;
			return false;
		}
	}
}
