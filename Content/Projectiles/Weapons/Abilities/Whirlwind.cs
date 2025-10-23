using ArcaneOdyssey.Content.Projectiles.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey.Content.Projectiles.Weapons.Abilities
{
	public class Whirlwind : AOPlayerProjectile
	{
		public Color color = Color.White;
		public const int MaxTime = 20;
		public Texture2D Sprite => ModContent.Request<Texture2D>(Texture).Value;

		public override void SetDefaults()
		{
			Projectile.width = Projectile.height = 144;
			Projectile.friendly = true;
			Projectile.timeLeft = MaxTime;
			Projectile.DamageType = TrueMeleeNoSpeed();
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
			Projectile.ownerHitCheck = true;
			Projectile.localNPCHitCooldown = -1;
			Projectile.usesLocalNPCImmunity = true;
		}
		public override float AOSize => 1;
		public override float AOSpeed => .925f;
		public override float AODamage => 1.05f;

		public override void AI()
		{
			aoPlayerOwner ??= Main.player[Projectile.owner].ArcaneOdyssey();
			Player player = aoPlayerOwner.Player;
			Projectile.rotation += MathHelper.Pi / (MaxTime / 2) * 1.1f;
			Projectile.Center = player.MountedCenter + (Projectile.rotation.ToRotationVector2() * 44f * Projectile.scale);
		}

		public override void PostDraw(Color lightColor)
		{
			Main.EntitySpriteDraw(Sprite, Projectile.Center - Main.screenPosition, new Rectangle(0, Projectile.height * Projectile.frame, Projectile.width, Projectile.height), Color.Lerp(lightColor, color, .5f), Projectile.rotation, Projectile.GetDrawOriginCentre(), Projectile.scale * .95f, SpriteEffects.None);
			Main.EntitySpriteDraw(Sprite, Projectile.Center - Main.screenPosition, new Rectangle(0, Projectile.height * Projectile.frame, Projectile.width, Projectile.height), color, Projectile.rotation, Projectile.GetDrawOriginCentre(), Projectile.scale * .90f, SpriteEffects.None);
		}
	}
}
