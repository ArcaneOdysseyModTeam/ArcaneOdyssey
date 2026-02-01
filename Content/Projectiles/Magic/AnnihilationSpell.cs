using ArcaneOdyssey.Content.Projectiles.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Projectiles.Magic
{
	public class AnnihilationSpell : MagicSpell, ILocalizedModType
	{
		// ai 2 is first frame bool

		public override float AOSize => 2f;
		public override float AOSpeed => .3f;

		public const int FlightTime = 60 * 10;
		public const int ChargeTime = 60;
		public const int ExplodingTime = 60 * 6;

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
			fallThrough = true;
			return true;
		}

		public AnnihilationState State
		{
			get
			{
				return (AnnihilationState)Projectile.ai[0];
			}

			set
			{
				Projectile.ai[0] = (int)value;
			}
		}


		internal Vector2 originalVelocity;

		public override void AI()
		{
			if (Projectile.ai[2] == 0)
			{
				Projectile.ai[2] = 1;
				Projectile.netUpdate = true;
				originalVelocity = Projectile.velocity;
				Projectile.velocity = Vector2.Zero;
			}
			if (Imbue is null || ((!Imbue.CanBeWet) && Projectile.wet))
			{
				Kill();
				return;
			}

			if (Projectile.wet && State == AnnihilationState.Moving)
			{
				Projectile.timeLeft = ExplodingTime;
				Projectile.velocity = Vector2.Zero;
				State = AnnihilationState.Exploding;
			}

			switch (State)
			{
				case AnnihilationState.Hovering:
					Projectile.Opacity = 1f - ((Projectile.timeLeft - FlightTime) / (float)ChargeTime);
					Projectile.scale = Imbue.AOScrollSize;
					if (SecondImbue is not null)
						Projectile.scale *= SecondImbue.AOScrollSize;
					Projectile.Center = Owner.Center - new Vector2(0, ((Player.defaultHeight / 2f) + Projectile.width) * Projectile.scale);
					Projectile.scale *= BaseScale;
					AOPlayerOwner.HeavySkillActive = true;

					if (++Projectile.ai[1] > ChargeTime)
					{
						Projectile.ai[1] = 0;
						State = AnnihilationState.Moving;
						Projectile.velocity = originalVelocity.Length() * Projectile.SafeDirectionTo(Main.MouseWorld, Projectile.Center + originalVelocity);
					}
					return;


				case AnnihilationState.Exploding:
					Projectile.Opacity = Projectile.timeLeft / (float)ExplodingTime;
					if (++Projectile.ai[1] >= (ExplodingTime / 7))
					{
						Projectile.ai[1] = 0;
						AOUtils.SimulateAOE(Projectile.width * 8, Projectile.damage, Projectile.Center, Projectile.knockBack, Projectile, Projectile.DamageType);
						for (int i = 0; i < 30; i++)
						{
							Imbue?.ExplosionEffects(Projectile.Center, 3f);
							SecondImbue?.ExplosionEffects(Projectile.Center, 2f);
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
				Projectile.timeLeft = ExplodingTime;
				Projectile.velocity = Vector2.Zero;
				State = AnnihilationState.Exploding;
			}
			return false;
		}

		public override string Texture
		{
			get
			{
				if (Imbue is not null)
				{
					if (ModContent.RequestIfExists<Texture2D>(AOUtils.GetTexture<AnnihilationSpell>().Replace(Name, $"Annihilations/{Imbue.ImbuableTier}/{Imbue.AttackPrefix}Annihilation"), out _))
					{
						return AOUtils.GetTexture<AnnihilationSpell>().Replace(Name, $"Annihilations/{Imbue.ImbuableTier}/{Imbue.AttackPrefix}Annihilation");
					}
					else
					{
						Main.NewText(Imbue.DisplayName.Value + " is missing " + DisplayName.Value + " sprite.", Color.Red);
					}
				}
				return Mod.Name + "/Backgrounds/Blank";
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
