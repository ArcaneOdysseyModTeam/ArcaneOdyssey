using ArcaneOdyssey.Content.Projectiles.Base;
using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Projectiles.Weapons.Abilities
{
	public class PiercingGale : AOPlayerProjectile
	{
		public override string Texture => Mod.Name + "/Backgrounds/Blank";
		public override AODebuffRequirement? Debuff => null;
		public const int DustCount = 30;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = Projectile.height = 64;
			Projectile.friendly = true;
			Projectile.extraUpdates = 2;
			Projectile.timeLeft = 60 * (Projectile.extraUpdates + 1);
			Projectile.DamageType = DamageClass.Melee;
		}

		public override void AI()
		{
			if (Projectile.ai[0] == 0)
			{
				Projectile.ai[0] = 1;
				Projectile.netUpdate = true;
				Projectile.velocity /= Projectile.extraUpdates + 1;
			}
			Projectile.rotation += (MathHelper.Pi / 60) / Projectile.extraUpdates + 1;

			var dust = DustID.BubbleBurst_White;
			if (!Main.dedServ)
			{
				for (float i = 0; i < DustCount; i++)
				{
					var centre1 = ((MathHelper.PiOver4 / DustCount * i) + Projectile.rotation).ToRotationVector2() * (Projectile.width / 3);
					var dust1 = Dust.NewDustPerfect(centre1 + Projectile.Center, dust, -(centre1 / 15), 0, Imbue is null ? default : Imbue.GetColour(), .75f);
					dust1.noLight = true;
					dust1.noGravity = true;
					var centre2 = (MathHelper.TwoPi / DustCount * i).ToRotationVector2() * (Projectile.width / 2);
					var dust2 = Dust.NewDustPerfect(centre2 + Projectile.Center, dust, Vector2.Zero, 0, Imbue is null ? default : Imbue.GetColour(), .5f);
					dust2.noLight = true;
					dust2.noGravity = true;
				}
				var dust3 = Dust.NewDustPerfect(Projectile.Center, dust, Vector2.Zero, 0, Imbue is null ? default : Imbue.GetColour(), 1.5f);
				dust3.noLight = true;
				dust3.noGravity = true;
			}
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			AOUtils.SimulateAOE(Projectile.width * 2, Projectile.damage, Projectile.Center, Projectile.knockBack, Projectile, Projectile.DamageType, false, target.whoAmI);
		}

		public override bool PreKill(int timeLeft)
		{
			if (!Main.dedServ)
			{
				for (float i = 0; i < DustCount; i++)
				{
					var centre2 = (MathHelper.TwoPi / DustCount * i).ToRotationVector2() * (Projectile.width * 2);
					var dust2 = Dust.NewDustPerfect(centre2 + Projectile.Center, DustID.BubbleBurst_White, (-centre2) / 5, 0, Imbue is null ? default : Imbue.GetColour(), 1.5f);
					dust2.noLight = true;
					dust2.noGravity = true;
					Imbue?.ExplosionEffects(Projectile.Center);
				}
			}
			return base.PreKill(timeLeft);
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			width /= 4;
			height /= 4;
			fallThrough = true;
			return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
		}

		public override bool PreDraw(ref Color lightColor) => false;
	}
}
