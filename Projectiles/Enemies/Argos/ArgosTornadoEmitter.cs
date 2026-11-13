using ArcaneOdyssey.AOPlayers;
using ArcaneOdyssey.AOPlayers.DrawLayers;
using ArcaneOdyssey.NPCs.Bosses;
using ArcaneOdyssey.Projectiles.Base;

namespace ArcaneOdyssey.Projectiles.Enemies.Argos
{
	public class ArgosTornadoEmitter : BaseProjectile
	{
		public override string Texture => AOUtils.BlankTexture;
		public override bool? CanDamage() => false;
		private bool drawIndicator;
		public override void AI()
		{
			drawIndicator = false;
			if (Projectile.timeLeft <= MaxTime)
			{
				if (Main.myPlayer == Projectile.owner)
				{
					if (Projectile.timeLeft == MaxTime)
					{
						if (AOUtils.NPCAlive<GeneralArgos>(out var argos))
						{
							AOUtils.SimulateAOE(ArgosSpawningSystem.argosArena.Length() * .75f, Projectile.damage + 100, Projectile.Center, Projectile.knockBack, Projectile);
						}
						else
						{
							Kill();
							return;
						}
					}
					if (Projectile.timeLeft % 10 == 0)
					{
						Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<ArgosTornado>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
					}
				}
			}
			else
			{
				drawIndicator = true;
			}
		}

		public override float Size => 2.5f;
		public const int MaxTime = 60 * 2;
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = Projectile.height = 2;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.hostile = true;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.timeLeft = MaxTime + AOPlayer.IndicatorTime;
			Projectile.noEnchantmentVisuals = true;
		}

		public override void PostDraw(Color lightColor)
		{
			BlockingIndicator.Show |= drawIndicator;
			base.PostDraw(lightColor);
		}
	}
}
