using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Imbues.Relics;
using ArcaneOdyssey.Projectiles.Base;
using ArcaneOdyssey.Projectiles.Relics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Projectiles.Enemies
{
	public class DuskHound : BaseProjectile
	{
		public override string Texture => AOUtils.GetTexture<SpiritHound>();

		public int TileTimer = 0;

		public int Penetrations { get => (int)Projectile.ai[0]; set => Projectile.ai[0] = value; }

		public Imbuable Imbue => ModContent.GetInstance<StaffofNight>();

		public const int TimeLeftMax = 60 * 5;
		public override void SetDefaults()
		{
			Projectile.DamageType = DamageClass.Summon;
			Projectile.hostile = true;
			Projectile.height = 84;
			Projectile.width = 104;
			Projectile.penetrate = -1;
			Projectile.AverageDimensions();
			Projectile.timeLeft = TimeLeftMax;
			Projectile.ignoreWater = true;
			Projectile.Opacity = .75f;
		}

		public bool sentMessage = false;
		public override void AI()
		{
			if (ArcaneOdysseyClientConfig.Instance.AbilityText && !Main.dedServ && !sentMessage)
			{
				sentMessage = true;
				CombatText.NewText(Projectile.Hitbox, Imbue?.Colour ?? Color.White, (DisplayName + "!").Trim(), true);
			}
			Imbue?.LingeringEffects(Projectile.Hitbox, Projectile.velocity, Projectile);

			Projectile.spriteDirection = Projectile.direction;

			if (TileTimer > 0)
				TileTimer--;

			if (Projectile.timeLeft == (TimeLeftMax / 2))
			{
				Penetrations++;
				Imbue?.KillEffects(Projectile.Hitbox);
			}

			if (Projectile.ai[2] == 0)
			{
				Projectile.ai[2] = 1;
				Imbue?.SpawningEffects(Projectile.Hitbox, Projectile.velocity);
				SoundEngine.PlaySound(Imbue?.ImbueSound, Projectile.Center);
				if (Projectile.owner == Main.myPlayer)
				{
					Projectile.netUpdate = true;
					Projectile.netSpam = 0; ;
				}
			}

			if (Penetrations == 2 && Main.myPlayer == Projectile.owner)
			{
				Projectile.Kill();
				if (Main.netMode != NetmodeID.SinglePlayer)
					NetMessage.SendData(MessageID.KillProjectile, -1, -1, null, Projectile.identity, Projectile.owner);
				return;
			}

			Projectile.rotation = Projectile.velocity.ToRotation();
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			width /= 4;
			height /= 4;
			fallThrough = true;
			return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
		}

		public override void OnHitPlayer(Player target, Player.HurtInfo info)
		{
			if (Penetrations++ == 0)
			{
				Imbue?.KillEffects(target.Hitbox.Scaled(4f));
				Projectile.timeLeft -= TimeLeftMax / 2;
			}
		}

		public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)
		{
			modifiers = AOUtils.CalculateImbueDamage(Imbue, target, modifiers);
		}

		public override void OnKill(int timeLeft)
		{
			Imbue?.KillEffects(Projectile.Hitbox, Projectile);
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (TileTimer <= 0)
			{
				Penetrations++;
				Imbue?.KillEffects(Projectile.Hitbox);
			}
			if (TileTimer < 60 && TileTimer > 0)
			{
				return true;
			}
			Projectile.velocity = oldVelocity;
			Projectile.position = Projectile.oldPosition;
			TileTimer = 65;
			return false;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			lightColor = Imbue?.Colour ?? Color.White;
			if (ModContent.RequestIfExists<Texture2D>(Texture, out var tex))
			{
				SpriteEffects mode = Projectile.spriteDirection > 0 ? SpriteEffects.None : SpriteEffects.FlipVertically;
				Main.EntitySpriteDraw(tex.Value, Projectile.Center - Main.screenPosition, new(0, tex.Height() / Main.projFrames[Type] * Projectile.frame, tex.Width(), tex.Height() / Main.projFrames[Type]), Projectile.GetAlpha(lightColor), Projectile.rotation, new Vector2(tex.Width(), tex.Height() / Main.projFrames[Type]) / 2f, Projectile.scale, mode);
				return false;
			}
			return true;
		}

		public override void PostDraw(Color lightColor)
		{
			if (ModContent.RequestIfExists<Texture2D>(GlowTexture, out var tex))
			{
				SpriteEffects mode = Projectile.spriteDirection > 0 ? SpriteEffects.None : SpriteEffects.FlipVertically;
				Main.EntitySpriteDraw(tex.Value, Projectile.Center - Main.screenPosition, new(0, tex.Height() / Main.projFrames[Type] * Projectile.frame, tex.Width(), tex.Height() / Main.projFrames[Type]), Projectile.GetAlpha(lightColor), Projectile.rotation, new Vector2(tex.Width(), tex.Height() / Main.projFrames[Type]) / 2f, Projectile.scale, mode);
			}
		}
	}
}
