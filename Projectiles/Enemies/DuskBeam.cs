using ArcaneOdyssey.Buffs.MagicMarks;
using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Imbues.Enemies;
using ArcaneOdyssey.Imbues.Relics;
using ArcaneOdyssey.Projectiles.Base;
using System.Linq;
using Terraria.Audio;

namespace ArcaneOdyssey.Projectiles.Enemies
{
	public class DuskBeam : BaseProjectile
	{
		public override string Texture => AOUtils.BlankTexture;
		public Texture2D MidSprite => ArcaneOdysseyMod.Sets.Assets.raySprites[ModContent.ItemType<SpiritEnergy>()]?.Value ?? base.Sprite;
		public Texture2D EndSprite => ArcaneOdysseyMod.Sets.Assets.rayEndSprites[ModContent.ItemType<SpiritEnergy>()]?.Value ?? base.Sprite;
		public Texture2D StartSprite => ArcaneOdysseyMod.Sets.Assets.rayStartSprites[ModContent.ItemType<SpiritEnergy>()]?.Value ?? base.Sprite;

		public Imbuable Imbue => ModContent.GetInstance<DuskStaff>();

		public const int TravelTime = 90;
		public const int LingerTime = 2 * 60;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.DamageType = DamageClass.Summon;
			Projectile.height = Projectile.width = 20; // hitscan
			Projectile.extraUpdates = 4;
			Projectile.timeLeft = TravelTime + LingerTime;
			Projectile.hostile = true;
			Projectile.penetrate = -1;
		}

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ProjectileID.Sets.TrailingMode[Type] = 0;
			Main.projFrames[Type] = 4;
		}

		private Vector2 origin = default;
		private Vector2? end = null;

		public bool dying = false;

		public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)
		{
			target.AddBuff(ModContent.BuffType<DrainedEffect>(), 60 * 5);
			modifiers = AOUtils.CalculateImbueDamage(Imbue, target, modifiers);
		}

		public bool sentMessage = false;
		public override void AI()
		{
			if (ArcaneOdysseyClientConfig.Instance.AbilityText && !Main.dedServ && !sentMessage)
			{
				sentMessage = true;
				CombatText.NewText(Projectile.Hitbox, Imbue?.Colour ?? Color.White, (DisplayName + "!").Trim(), true);
			}

			if (Projectile.ai[0] == 0)
			{
				Projectile.ai[0] = 1;
				SoundEngine.PlaySound(Imbue?.ImbueSound, Projectile.Center);
				if (Projectile.owner == Main.myPlayer)
				{
					Projectile.netUpdate = true;
					Projectile.netSpam = 0; ;
				}
			}

			if (Projectile.velocity.Length() < 3f)
			{
				foreach (var player in Main.ActivePlayers)
				{
					if (player.Hitbox.Distance(Projectile.Center) < 500)
					{
						Projectile.velocity = Projectile.SafeDirectionTo(player.Center, Vector2.UnitX) * 7f;
					}
				}
			}

			if (origin == default)
			{
				Projectile.spriteDirection = (Projectile.velocity.X > 0).ToDirectionInt();
				origin = Projectile.Center + (Projectile.velocity.SafeNormalize(Projectile.velocity) * 60f);
			}

			if (Projectile.timeLeft <= LingerTime)
			{
				end ??= Projectile.Center;
				Projectile.Center = origin;
				Projectile.velocity = Vector2.Zero;
				dying = true;

				if (Projectile.numUpdates == 0)
					Projectile.Opacity -= Circle.GlobalChargeSpeed * 4f;

				if (Projectile.alpha >= 255 && Main.myPlayer == Projectile.owner)
				{
					Kill();
				}
			}
		}

		public override void OnHitPlayer(Player target, Player.HurtInfo info)
		{
			Projectile.timeLeft = LingerTime;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			if (origin != default)
			{
				Projectile.scale *= 1.25f;
				SpriteEffects mode = Projectile.spriteDirection > 0 ? SpriteEffects.None : FlippedMode;
				var col = Imbue.Colour;
				var info = AOUtils.DrawChain(Projectile.Center, end.GetValueOrDefault(origin), MidSprite, Projectile.scale, Main.projFrames[Type], Projectile.frame, Projectile.GetAlpha(col), mode);
				var frame = StartSprite.Frame(1, Main.projFrames[Type], 0, Projectile.frame);
				Main.EntitySpriteDraw(StartSprite, Projectile.Center - Main.screenPosition, frame, Projectile.GetAlpha(col), info.Rotation, frame.Size() / 2f, Projectile.scale, mode);
				var ending = info.Ending + new Vector2(EndSprite.Width * Projectile.scale, 0).RotatedBy(info.Rotation);
				Main.EntitySpriteDraw(EndSprite, ending - Main.screenPosition, EndSprite.Frame(1, Main.projFrames[Type], 0, info.FinalFrame), Projectile.GetAlpha(col), info.Rotation, new Vector2(EndSprite.Width, EndSprite.Height / Main.projFrames[Type]) / 2f, Projectile.scale, mode);
				Projectile.scale /= 1.25f;
			}
			return false;
		}

		public override bool? CanDamage()
		{
			if (!dying)
				return null;
			return false;
		}

		public override bool? CanCutTiles() => !dying;

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			width = height = 1;
			fallThrough = true;
			return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Projectile.tileCollide = false;
			Projectile.timeLeft = LingerTime;
			return false;
		}
	}
}
