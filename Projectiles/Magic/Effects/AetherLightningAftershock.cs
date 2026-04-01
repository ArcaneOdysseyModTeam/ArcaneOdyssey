using ArcaneOdyssey.Imbues.Magic.Lost;
using ArcaneOdyssey.Projectiles.Base;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Projectiles.Magic.Effects
{
	public class AetherLightningAftershock : PlayerProjectile
	{
		private static int _count = 0;

		internal static int Count 
		{ 
			get
			{
				return _count;
			}
			set
			{
				_count = Utils.Clamp(value, 0, 10);
			} 
		}

		public override float AOSize => .4f;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.height = Projectile.width = 128;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = -1;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
		}

		public override Debuff? ProjectileDebuff => null;

		public override void OnSpawn(IEntitySource source)
		{
			if (source is EntitySource_Parent { Entity: Projectile projectile })
			{
				Count++;
				Projectile.scale = MathHelper.Clamp((projectile.width + projectile.height) * projectile.scale * 1.2f / ((projectile.width + projectile.height) / 2f), .4f, 1.75f);
				Projectile.Hitbox = Projectile.Hitbox.Scaled(Projectile.scale);
			}
			else if (source is EntitySource_Parent { Entity: Item item } && item.ModItem is AetherLightningMagic)
			{
				Projectile.scale = Projectile.ai[0];
				Projectile.Hitbox = Projectile.Hitbox.Scaled(Projectile.scale);
			}
			else
			{
				Kill();
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

		public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
		{
			modifiers.ScalingArmorPenetration += .25f;
		}

		public override void OnKill(int timeLeft)
		{
			Count--;
		}
	}
}
