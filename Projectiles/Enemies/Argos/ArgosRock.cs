using ArcaneOdyssey.NPCs.Bosses;
using ArcaneOdyssey.Projectiles.Base;
using System.Collections.Generic;
using System.Linq;

namespace ArcaneOdyssey.Projectiles.Enemies.Argos
{
	public class ArgosRock : BaseProjectile
	{
		public enum RockMode
		{
			None,
			Falling,
			Hover,
			Homing
		}
		public RockMode Mode
		{
			get => (RockMode)Projectile.ai[0];
			set => Projectile.ai[0] = (int)value;
		}

		public override float Size => 2f;

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			Main.projFrames[Type] = 3;
			ProjectileID.Sets.DontAttachHideToAlpha[Type] = true;
		}

		public const int TimeLeft = 60;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.frame = Main.rand.Next(Main.projFrames[Type]);
			Projectile.timeLeft = TimeLeft;
			Projectile.width = Projectile.height = 16;
			Projectile.hostile = true;
			Projectile.tileCollide = false;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.rotation = Main.rand.NextFloat(MathHelper.TwoPi);
			Projectile.hide = true;
		}

		public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
		{
			behindNPCsAndTiles.Add(index);
		}

		private float baseRot;
		private Vector2 baseVelo;
		public override void AI()
		{
			if (Mode == RockMode.Falling)
			{
				if (Projectile.ai[1] == 0)
				{
					Projectile.ai[1]++;
					NetUpdate();
					baseRot = Projectile.rotation;
				}
				Projectile.rotation = baseRot + (Projectile.velocity.ToRotation() + (MathHelper.PiOver4 * Projectile.spriteDirection));
				Projectile.velocity.Y += 0.13f;
				if (Projectile.velocity.Y > 16f)
				{
					Projectile.velocity.Y = 16f;
				}
				if (Projectile.timeLeft < (TimeLeft - 10))
				{
					Projectile.tileCollide = true;
				}
			}
			else if (Mode == RockMode.Hover)
			{
				if (Projectile.ai[1] == 0)
				{
					Projectile.ai[1]++;
					NetUpdate();
					baseVelo = Projectile.velocity;
					Projectile.timeLeft += GeneralArgos.RockStabTime;
					Projectile.velocity = -Vector2.UnitY * Projectile.velocity.Length();
				}
				Projectile.velocity *= .9f;
				if (Projectile.timeLeft <= TimeLeft)
				{
					Mode = RockMode.Homing; // would just use none and set velocity here but we might want more visuals
					Projectile.ai[1] = 0;
					Projectile.tileCollide = true;
				}
			}
			else if (Mode is RockMode.None or RockMode.Homing)
			{
				if (Mode == RockMode.Homing)
				{
					if (Projectile.ai[1] == 0)
					{
						Projectile.ai[1]++;
						NetUpdate();
						baseRot = Projectile.rotation;
						if (baseVelo == default)
						{
							baseVelo = Projectile.velocity;
						}
						var target = AOUtils.Singleplayer ? Main.LocalPlayer.Center : Main.player.Where(e => e.active).OrderBy(e => e.Distance(Projectile.Center)).Select(e => e.Center).FirstOrDefault();
						Projectile.velocity = Projectile.DirectionTo(target) * baseVelo.Length();
					}
					// maybe add visuals here
				}
				Projectile.rotation += MathHelper.TwoPi / 40f * Projectile.direction;
			}
		}
	}
}
