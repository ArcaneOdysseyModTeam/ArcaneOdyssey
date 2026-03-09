using ArcaneOdyssey.Content.Imbues.Magic.Lost;
using ArcaneOdyssey.Content.Projectiles.Base;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Projectiles.Magic
{
	public class AnnihilationSpell : MagicSpell
	{
		// ai 2 is first frame bool

		public override float AOSize => 2f;
		public override float AOSpeed => .3f;

		public const int FlightTime = 60 * 10;
		public const int ChargeTime = 90;
		public int ExplodingTime => ApplySpeed(60 * 8, true).Round();

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.height = Projectile.width = 64;
			Projectile.timeLeft = FlightTime + ChargeTime; // exploding time is added after
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			width = Projectile.width / 4;
			height = Projectile.height / 4;
			fallThrough = false;
			return true;
		}

		public AnnihilationState State
		{
			get => (AnnihilationState)Projectile.ai[0];
			set => Projectile.ai[0] = (int)value;
		}


		internal Vector2 originalVelocity;

		public override void PostAI()
		{
			base.PostAI();
			if (Imbue is SoundMagic) // manually do sound magic
			{
				var DustCount = 30;
				for (float i = 0; i < DustCount; i++)
				{
					var centre = (MathHelper.TwoPi / DustCount * (i + Main.rand.NextFloat())).ToRotationVector2() * (64 * Projectile.scale);
					var dust = Dust.NewDustPerfect(Projectile.Center, DustID.MushroomTorch, centre / (DustCount * .75f), Scale: Projectile.scale);
					dust.noGravity = true;
				}
			}
			if (Imbue is SlashMagic)
				Imbue.LingeringEffects(AOUtils.ScaleRectangleNotRef(Projectile.Hitbox, 2f));
		}

		public void StartExploding()
		{
			Projectile.timeLeft = ExplodingTime;
			Projectile.velocity = Vector2.Zero;
			State = AnnihilationState.Exploding;
		}

		public override void AI()
		{
			if (Projectile.ai[2] == 0)
			{
				Projectile.ai[2] = 1;
				if (Projectile.owner == Main.myPlayer)
				{
					Projectile.netUpdate = true;
					Projectile.netSpam = 0;
					originalVelocity = Projectile.velocity;
				}
				Projectile.velocity = Vector2.Zero;
			}

			if (Projectile.wet && State == AnnihilationState.Moving)
			{
				StartExploding();
			}

			switch (State)
			{
				case AnnihilationState.Hovering:
					Projectile.Opacity = 1f - ((Projectile.timeLeft - FlightTime) / (float)ChargeTime);
					Owner.ChangeDir(Math.Sign(Main.MouseWorld.X - Owner.position.X));
					Projectile.Bottom = Owner.Top;
					AOPlayerOwner.HeavySkillActive = true;

					if (++Projectile.ai[1] > ChargeTime)
					{
						Projectile.ai[1] = 0;
						State = AnnihilationState.Moving;
						SoundEngine.PlaySound(Imbue?.ImbueSound, Projectile.Center);
						if (Projectile.owner == Main.myPlayer)
						{
							Projectile.velocity = originalVelocity.Length() * Projectile.SafeDirectionTo(Main.MouseWorld, Projectile.Center + originalVelocity);
							Projectile.netUpdate = true;
							Projectile.netSpam = 0;
						}
					}
					return;

				case AnnihilationState.Moving:
					Projectile.rotation += ApplySpeed(MathHelper.Pi / 60f) * Math.Sign(Projectile.velocity.X);
					return;

				case AnnihilationState.Exploding:
					Projectile.Opacity = Projectile.timeLeft / (float)ExplodingTime;
					if (++Projectile.ai[1] >= (ExplodingTime / 7))
					{
						Projectile.ai[1] = 0;
						AOUtils.SimulateAOE(Projectile.width * 6, Projectile.damage, Projectile.Center, Projectile.knockBack, Projectile, Projectile.DamageType);
						for (int i = 0; i < 30; i++)
						{
							Imbue?.ExplosionEffects(Projectile.Center, 2.5f);
							SecondImbue?.ExplosionEffects(Projectile.Center, 1.25f);
							SoundEngine.PlaySound(Imbue?.ImbueSound, Projectile.Center);
						}
					}
					return;
			}
		}

		public override bool? CanDamage() => false;

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (State == AnnihilationState.Moving)
			{
				StartExploding();
			}
			return false;
		}

		public string BackupTexture = AOUtils.GetTexture<AnnihilationSpell>().Replace(nameof(AnnihilationSpell), $"Annihilations/Normal/WindAnnihilation");

		public override string Texture
		{
			get
			{
				if (Imbue is not (null or SoundMagic or SlashMagic))
				{
					var asset = AOUtils.GetTexture<AnnihilationSpell>().Replace(nameof(AnnihilationSpell), $"Annihilations/{Imbue.ImbuableTier}/{Imbue.AttackPrefix}Annihilation");
					if (ModContent.HasAsset(asset))
					{
						return asset;
					}
				}
				return BackupTexture;
			}
		}
	}

	public enum AnnihilationState
	{
		Hovering,
		Moving,
		Exploding
	}
}
