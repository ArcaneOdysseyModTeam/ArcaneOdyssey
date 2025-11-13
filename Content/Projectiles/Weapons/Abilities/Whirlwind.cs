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
		public Color colour = Color.White;
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
			//Projectile.alpha = (255 / AfterimageCount * 2).Round();
		}

        public static readonly float AfterimageCount = 5f;

		public override bool PreDraw(ref Color lightColour)
		{
			Player player = aoPlayerOwner.Player;
            var rotoffset = MathHelper.Pi / AfterimageCount;
            for (float i = AfterimageCount; i > 0; i--)
            {
                var rotoffset1 = rotoffset * i;
                var adjustedrotation1 = player.MountedCenter + ((Projectile.rotation - rotoffset1).ToRotationVector2() * 44f * Projectile.scale);
                var colour1 = Color.Lerp(Color.White, colour, 1f / AfterimageCount) with { A = (byte)(255 / AfterimageCount) };
                var scale = Projectile.scale - (Projectile.scale / 20f * i);
                Main.EntitySpriteDraw(Sprite, adjustedrotation1 - Main.screenPosition, null, colour1, Projectile.rotation + rotoffset1, Projectile.GetDrawOriginCentre(), scale, SpriteEffects.None);
                Lighting.AddLight(adjustedrotation1, colour1.R / 255f * Projectile.scale, colour1.G / 255f * Projectile.scale, colour1.B / 255f * Projectile.scale);
            }
            return AfterimageCount < 1;
        }
	}

	public class WhirlwindCooldown : CooldownSystem
	{
		public override int CooldownLength => 60 + Whirlwind.MaxTime;
		public override string Name => "Whirlwind Cooldown";
	}
}
