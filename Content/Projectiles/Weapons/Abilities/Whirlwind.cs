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
		public static readonly int MaxTime = 20;
        public static readonly float AfterimageCount = 5f;
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
			Projectile.rotation += (MathHelper.Pi / (MaxTime / 2) * 1.1f) * (Imbue?.AOImbueSpeed ?? 1f);
			Projectile.Center = player.MountedCenter + (Projectile.rotation.ToRotationVector2() * 44f * Projectile.scale);
            player.itemRotation = player.itemAnimation = 5;
            player.itemRotation = player.MountedCenter.DirectionTo(Projectile.Center).ToRotation() - MathHelper.Pi + (MathHelper.Pi / (AfterimageCount / .5f));
            player.direction = ((Projectile.Center.X > 0).ToDirectionInt());
            //Projectile.alpha = (255 / AfterimageCount * 2).Round();
        }

		public override bool PreDraw(ref Color lightColour)
		{
			Player player = aoPlayerOwner.Player;
            var rotoffset = MathHelper.Pi / (AfterimageCount * 2);
            for (float i = 1; i < AfterimageCount + 1; i++)
            {
                var rotoffset1 = rotoffset * i;
                var adjustedrotation1 = player.MountedCenter + ((Projectile.rotation + rotoffset1).ToRotationVector2() * 44f * Projectile.scale);
                var colour1 = Color.Lerp(Color.Transparent with { A = lightColour.A }, colour, 1f / AfterimageCount * i);// with { A = (byte)(255 / AfterimageCount * i) };
                var scale = Projectile.scale - (Projectile.scale / 18f * AfterimageCount) + (Projectile.scale / 18f * i);
                Main.EntitySpriteDraw(Sprite, adjustedrotation1 - Main.screenPosition, null, colour1, Projectile.rotation + rotoffset1, Projectile.GetDrawOriginCentre(), scale, SpriteEffects.None);
                Lighting.AddLight(adjustedrotation1, colour1.R / 255f * Projectile.scale, colour1.G / 255f * Projectile.scale, colour1.B / 255f * Projectile.scale);
            }
            return AfterimageCount < 1;
        }

        public override void OnKill(int timeLeft)
        {
            aoPlayerOwner.Player.itemAnimation = aoPlayerOwner.Player.itemTime = 0;
        }
	}

	public class WhirlwindCooldown : CooldownSystem
	{
		public override int CooldownLength => 60 + Whirlwind.MaxTime;
		public override string Name => "Whirlwind Cooldown";
	}
}
