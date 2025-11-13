using ArcaneOdyssey.Content.Buffs.MagicMarks;
using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Imbues.Magic.Lost;
using ArcaneOdyssey.Content.Projectiles.Base;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey.Content.Projectiles.Magic.MagicEffects
{
	public class AetherExplosion : AOPlayerProjectile
	{
        internal static int Count = 0;
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.height = Projectile.width = 128;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = -1;
			Projectile.Center = Projectile.position;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.alpha = 40;
		}
		public override AODebuffRequirement? Debuff => null;

		public override void OnSpawn(IEntitySource source)
        {
            if (source is EntitySource_Parent { Entity: Projectile projectile })
			{
                Count++;
				BaseScale = MathHelper.Clamp((projectile.width + projectile.height) * projectile.scale / 2f / Projectile.width, .2f, 2f);
			}
            else
            {
                Projectile.active = false;
            }    
		}

		public override void SetStaticDefaults()
		{
			Main.projFrames[Type] = 13;
		}

		public override void AI()
        {
            if (++Projectile.frameCounter >= 3)
			{
				Projectile.frameCounter = 0;
				if (++Projectile.frame >= Main.projFrames[Type])
				{
					Kill();
				}
			}
		}
        public override bool PreKill(int timeLeft)
        {
            Count--;
            return base.PreKill(timeLeft);
        }
	}
}
