using ArcaneOdyssey.Buffs.MagicMarks;
using ArcaneOdyssey.Projectiles.Base;
using System.Collections.Generic;
using Terraria.DataStructures;

namespace ArcaneOdyssey.Projectiles.Magic.Effects
{
	public class AetherExplosion : PlayerProjectile
	{
		public const int SpriteSize = 128;

		public override float Size => .4f;

		public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
		{
			overPlayers.Add(index);
		}

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
			Projectile.light = 1f;
			Projectile.hide = true;
		}

		public override Debuff? ProjectileDebuff => Debuff.Create<CharredEffect>(60 * 5);

		public override void OnSpawn(IEntitySource source)
		{
			if (Projectile.ai[0] != 0)
			{
				Projectile.scale = Projectile.ai[0];
				Projectile.Hitbox = Utils.CenteredRectangle(Projectile.Center, new(SpriteSize)).Scaled(Projectile.scale);
			}
			else if (source is EntitySource_Parent { Entity: Projectile projectile })
			{
				Projectile.scale = ApplySize(MathHelper.Max((projectile.width + projectile.height) / 2f / SpriteSize, Size));
				Projectile.Hitbox = Utils.CenteredRectangle(Projectile.Center, new(SpriteSize)).Scaled(Projectile.scale);
			}
			else
			{
				Kill();
			}
		}

		public override void SetStaticDefaults()
		{
			Main.projFrames[Type] = 13;
			ArcaneOdysseyMod.Sets.imbueEffect[Type] = true;
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

		public override bool PreDraw(ref Color lightColor)
		{
			lightColor = Color.White;
			if (Projectile.ai[0] != 0 && Projectile.owner != Main.myPlayer)
			{
				return false;
			}
			return base.PreDraw(ref lightColor);
		}

		public override bool? CanCutTiles() => false;
	}
}
