using ArcaneOdyssey.Imbues.Magic.Lost;
using ArcaneOdyssey.Projectiles.Base;
using Microsoft.Xna.Framework;
using ReLogic.Utilities;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Projectiles.Magic
{
	public class MeteorSpell : MagicSpell
	{
		// ai 2 is first frame bool

		public override float AOSize => 3f;
		public override float AOSpeed => .5f;

		public int ExplodingTime => ApplySpeed(60 * 8, true).Round();

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.height = Projectile.width = 64;
			Projectile.timeLeft = 600;
			Projectile.usesIDStaticNPCImmunity = true;
			Projectile.localNPCHitCooldown = -1;
			Projectile.rotation = Main.rand.NextFloat(MathHelper.TwoPi);
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			width = Projectile.width / 4;
			height = Projectile.height / 4;
			fallThrough = true;
			return true;
		}

		public SlotId? sound = null;

		public override void AI()
		{
			if (Projectile.ai[2] == 0)
			{
				Projectile.ai[2] = 1;
				if (Projectile.owner == Main.myPlayer)
				{
					Projectile.netUpdate = true;
					Projectile.netSpam = 0;
				}
			}

			if (!Main.dedServ)
			{
				if (!sound.HasValue || !SoundEngine.TryGetActiveSound(sound.Value, out var activeSound))
				{
					sound = SoundEngine.PlaySound(SoundID.DD2_BookStaffTwisterLoop with { Pitch = .25f }, Projectile.Center);
				}
				else
				{
					activeSound.Position = Projectile.Center;
				}
			}

			//if (Imbue is SoundMagic) // manually do sound magic
			//{
			//	var DustCount = 30;
			//	for (float i = 0; i < DustCount; i++)
			//	{
			//		var centre = (MathHelper.TwoPi / DustCount * (i + Main.rand.NextFloat())).ToRotationVector2() * (64 * Projectile.scale);
			//		var dust = Dust.NewDustPerfect(Projectile.Center, DustID.MushroomTorch, centre / (DustCount * .75f), Scale: Projectile.scale);
			//		dust.noGravity = true;
			//	}
			//}
			//if (Imbue is SlashMagic)
			//	Imbue.LingeringEffects(AOUtils.ScaleRectangleNotRef(Projectile.Hitbox, 2f));
		}

		public override bool PreKill(int timeLeft)
		{
			if (!Main.dedServ)
			{
				if (sound.HasValue)
				{
					if (SoundEngine.TryGetActiveSound(sound.Value, out var activeSound))
					{
						activeSound.Stop();
					}
				}
			}

			return base.PreKill(timeLeft);
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
}
