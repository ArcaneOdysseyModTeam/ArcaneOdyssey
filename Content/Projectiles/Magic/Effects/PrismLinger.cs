using ArcaneOdyssey.Content.Items.Imbues.Magic.Lost;
using ArcaneOdyssey.Content.Projectiles.Base;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Projectiles.Magic.Effects
{
	public class PrismLinger : AOPlayerProjectile
	{
		public override bool PreDraw(ref Color lightColor) => false;
		public override string Texture => AOUtils.BlankTexture;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = Projectile.height = 200;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.timeLeft = 120;
			Projectile.localNPCHitCooldown = Projectile.timeLeft / 3;
			Projectile.Center = Projectile.position;
			Projectile.DamageType = DamageClass.Magic;
		}

		public override void AI()
		{
			if (Main.dedServ)
				return;
			var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.AncientLight, newColor: PrismMagic.rainbowColors[Main.GameUpdateCount % 3], Scale: 1.25f);
			dust.noGravity = true;
		}
	}
}
