using ArcaneOdyssey.Imbues.Magic.Lost;
using ArcaneOdyssey.Projectiles.Base;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Projectiles.Magic.Effects
{
	public class AetherLightningAftershock : PlayerProjectile
	{
		public const int SpriteSize = 256;

		public override float Size => .4f;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.height = Projectile.width = SpriteSize;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = -1;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.Opacity = 0f;
		}

		public override Debuff? ProjectileDebuff => null;

		public override void OnSpawn(IEntitySource source)
		{
			if (source is EntitySource_Parent { Entity: Projectile projectile })
			{
				Projectile.scale = MathHelper.Clamp((projectile.width + projectile.height) * projectile.scale * 1.2f / (SpriteSize / 2f), Size, 1.75f);
				Projectile.Hitbox = Utils.CenteredRectangle(Projectile.Center, new(SpriteSize)).Scaled(Projectile.scale);
			}
			else if (source is EntitySource_Parent { Entity: Item item } && item.ModItem is AetherLightningMagic)
			{
				Projectile.scale = Projectile.ai[0];
				Projectile.Hitbox = Utils.CenteredRectangle(Projectile.Center, new(SpriteSize)).Scaled(Projectile.scale);
			}
			else
			{
				Kill();
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			lightColor = Color.White;
			return base.PreDraw(ref lightColor);
		}

		public override void SetStaticDefaults()
		{
			Main.projFrames[Type] = 14;
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

		public override bool PreAI()
		{
			Projectile.ai[0]++;
			if (Projectile.ai[0] < 60)
			{
				return false;
			}
			else if (Projectile.ai[0] == 60)
			{
				Projectile.netUpdate = true;
				Projectile.netSpam = 0;
				Projectile.alpha = 0;
				SoundEngine.PlaySound(SoundID.Thunder, Projectile.Center);
				return true;
			}	
			else
			{
				return true;
			}
		}

		public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
		{
			modifiers.ScalingArmorPenetration += .25f;
		}

		public override bool? CanCutTiles() => false;

		public override bool? CanDamage()
		{
			if (Projectile.ai[0] >= 60)
				return null;
			return false;
		}
	}
}
