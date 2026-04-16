using ArcaneOdyssey.Imbues.Magic.Lost;
using ArcaneOdyssey.Projectiles.Base;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Graphics.CameraModifiers;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Projectiles.Magic.Effects
{
	public class AetherLightningAftershock : PlayerProjectile
	{
		public const int SpriteSize = 256;

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
			Projectile.rotation = Main.rand.NextFloat(MathHelper.TwoPi);
			Projectile.light = 2f;
			Projectile.hide = true;
			DrawOriginOffsetX = Sprite.Bounds.Width/-2;
			DrawOriginOffsetY = (Sprite.Bounds.Height/Main.projFrames[Type])/-4;
		}

		public override Debuff? ProjectileDebuff => null;

		public override void OnSpawn(IEntitySource source)
		{
			if (source is EntitySource_Parent { Entity: Projectile projectile })
			{
				Projectile.scale = ApplySize(MathHelper.Max((projectile.width + projectile.height) / 2f / (SpriteSize / 2f), Size));
				Projectile.Hitbox = Utils.CenteredRectangle(Projectile.Center, new(SpriteSize)).Scaled(Projectile.scale);
			}
			else if (source is EntitySource_Parent { Entity: Item item } && item.ModItem is AetherLightningMagic)
			{
				Projectile.scale = Projectile.ai[0];
				Projectile.Hitbox = Utils.CenteredRectangle(Projectile.Center, new(SpriteSize)).Scaled(Projectile.scale);
				Projectile.ai[1] = 44;
			}
			else
			{
				Kill();
			}
			Projectile.scale /= 5f;
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

		private bool playedSound = false;

		public override bool PreAI()
		{
			Projectile.ai[1]++;
			if (Projectile.ai[1] < 45)
			{
				if (!playedSound)
				{
					SoundEngine.PlaySound(SoundID.Item121 with { MaxInstances = 0 }, Projectile.Center);
					playedSound = true;
				}
				return false;
			}
			else if (Projectile.ai[1] == 45)
			{
				Projectile.scale *= 5f;
				Projectile.netUpdate = true;
				Projectile.netSpam = 0;
				if (!Main.dedServ)
				{
					PunchCameraModifier modifier = new(Projectile.Center, (Main.rand.NextFloat() * MathHelper.TwoPi).ToRotationVector2(), ApplyKnockback(10f), ApplyKnockback(4f), 10, ApplyKnockback(500f), FullName);
					Main.instance.CameraModifiers.Add(modifier);
				}
				//SoundEngine.PlaySound(SoundID.Thunder, Projectile.Center); // PORT change to InstantThunder
				return true;
			}	
			else
			{
				return true;
			}
		}

		public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
		{
			modifiers.ScalingArmorPenetration += 1f;
		}
		public override bool PreDraw(ref Color lightColor)
		{
			lightColor = Color.White;
			return true;
		}

		public override bool? CanCutTiles() => false;

		public override bool? CanDamage()
		{
			if (Projectile.ai[1] >= 60)
				return null;
			return false;
		}
	}
}
