using ArcaneOdyssey.Buffs.DOT;
using ArcaneOdyssey.Projectiles.Base;
using System.Collections.Generic;

namespace ArcaneOdyssey.Projectiles.Magic.Effects
{
	public class FantasiaRift : PlayerProjectile
	{
		public override Debuff? ProjectileDebuff => Debuff.Create<HeavyBleed>();

		public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
		{
			behindNPCs.Add(index);
		}

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			Main.projFrames[Type] = 5;
			ArcaneOdysseyMod.Sets.imbueEffect[Type] = true;
		}
		public override bool CanHaveImbueVFX => false;
		public override float Size => 2f;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.hide = true;
			Projectile.width = Projectile.height = 64;
			Projectile.friendly = true;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 60 * 5;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.noEnchantmentVisuals = true;
			Projectile.light = 1f;
		}

		public override void AI()
		{
			if (++Projectile.frameCounter >= 5)
			{
				Projectile.frameCounter = 0;
				if (++Projectile.frame >= Main.projFrames[Type])
				{
					Projectile.frame = 0;
				}
			}

			foreach (var npc in Main.ActiveNPCs)
			{
				if ((!npc.friendly) && npc.WithinRange(Projectile.Center, Projectile.Size.Length() * 3f))
				{
					npc.velocity = npc.SafeDirectionTo(Projectile.Center) * 2f;
				}
			}

			foreach (var item in Main.ActiveItems)
			{
				if ((!item.shimmered) && item.WithinRange(Projectile.Center, Projectile.Size.Length() * 3f))
				{
					item.velocity = item.SafeDirectionTo(Projectile.Center) * 2f;
				}
			}

			if (Projectile.timeLeft == 2)
			{
				if (!Main.dedServ)
				{
					for (int i = 0; i < 20; i++)
					{
						Imbue?.ExplosionEffects(Projectile.Center, 2f);
						SecondImbue?.ExplosionEffects(Projectile.Center, 1f);
					}
				}

				if (Main.myPlayer == Projectile.owner)
				{
					AOUtils.SimulateAOE(Projectile.Size.Length() * 2f, Projectile.damage, Projectile.Center, Projectile.knockBack, Projectile, Projectile.DamageType, false);
				}
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			lightColor = Color.White;
			return base.PreDraw(ref lightColor);
		}

		public override bool? CanDamage() => false;
		public override bool? CanCutTiles() => false;
	}
}
