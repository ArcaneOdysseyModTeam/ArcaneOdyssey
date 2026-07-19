using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Imbues.Relics;
using ArcaneOdyssey.NPCs.Minibosses;
using ArcaneOdyssey.Projectiles.Base;
using ArcaneOdyssey.Projectiles.Relics;
using Terraria.Audio;

namespace ArcaneOdyssey.Projectiles.Enemies
{
	public class DuskRaincloud : BaseProjectile
	{
		public override string Texture => AOUtils.GetTexture<SpiritRaincloud>();


		public const int MaxTimeLeft = SpiritRaincloud.MaxTimeLeft;

		public Imbuable Imbue => ModContent.GetInstance<StaffofNight>();
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.height = Projectile.width = 64;
			Projectile.DamageType = DamageClass.Summon;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.timeLeft = MaxTimeLeft;
			Projectile.scale = 1.5f;
			Projectile.penetrate = -1;
		}

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			Main.projFrames[Type] = 7;
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			var fakebox = new Rectangle(Projectile.Hitbox.Center.X - 190, Projectile.Hitbox.Center.Y, 190 * 2, 700).Scaled(Imbue.ScrollSize, 1, 2);
			return targetHitbox.Intersects(fakebox);
		}


		public bool sentMessage = false;
		public override void AI()
		{
			if (AOUtils.NPCAlive<Dusk>(out var dusk))
			{
				if (ArcaneOdysseyClientConfig.Instance.AbilityText && !Main.dedServ && !sentMessage)
				{
					sentMessage = true;
					CombatText.NewText(Projectile.Hitbox, Imbue?.Colour ?? Color.White, (DisplayName + "!").Trim(), true);
				}
				Imbue.LingeringEffects(Projectile.Hitbox, Projectile.velocity, Projectile);
				if (Projectile.ai[0] == 0)
				{
					Projectile.ai[0] = 1;
					SoundEngine.PlaySound(Imbue?.ImbueSound, Projectile.Center);
					if (Projectile.owner == Main.myPlayer)
					{
						Projectile.netUpdate = true;
						Projectile.netSpam = 0; ;
					}
					Projectile.velocity = -Vector2.UnitY * 5;
				}

				Projectile.Center = Projectile.Center with { X = dusk.Center.X };

				if (Projectile.timeLeft <= (MaxTimeLeft - 60))
				{
					Projectile.hostile = true;
					Projectile.velocity = Vector2.Zero;
					AOUtils.ShootProjectile(Projectile.GetSource_FromThis(), Projectile.Center, new Vector2(Main.rand.NextFloat(-10f, 10f), Main.rand.NextFloat(2f)), ModContent.ProjectileType<DuskRaindrop>(), Projectile.damage / 2, 0f, Projectile.owner, Imbue, null, true);
				}


				if (Projectile.frameCounter++ > 5)
				{
					Projectile.frameCounter = 0;
					SoundEngine.PlaySound(Imbue?.ImbueSound, Projectile.Center);
					if (++Projectile.frame >= Main.projFrames[Type])
					{
						Projectile.frame = 0;
					}
				}
			}
			else
			{
				Kill();
			}
		}

		public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)
		{
			modifiers = AOUtils.CalculateImbueDamage(Imbue, target, modifiers);
		}

		public override bool PreDraw(ref Color lightColor)
		{
			if (ModContent.RequestIfExists<Texture2D>(Texture, out var tex))
			{
				SpriteEffects mode = Projectile.spriteDirection > 0 ? SpriteEffects.None : SpriteEffects.FlipVertically;
				Main.EntitySpriteDraw(tex.Value, Projectile.Center - Main.screenPosition, new(0, tex.Height() / Main.projFrames[Type] * Projectile.frame, tex.Width(), tex.Height() / Main.projFrames[Type]), Projectile.GetAlpha(Imbue.ImbueColour), Projectile.rotation, new Vector2(tex.Width(), tex.Height() / Main.projFrames[Type]) / 2f, Projectile.scale, mode);
				return false;
			}
			return true;
		}
	}
}
