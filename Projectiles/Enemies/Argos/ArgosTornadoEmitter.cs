using ArcaneOdyssey.AOPlayers;
using ArcaneOdyssey.AOPlayers.DrawLayers;
using ArcaneOdyssey.NPCs.Bosses;
using ArcaneOdyssey.Projectiles.Abilities;
using ArcaneOdyssey.Projectiles.Base;
using System;
using System.Collections.Generic;
using Terraria.GameContent;

namespace ArcaneOdyssey.Projectiles.Enemies.Argos
{
	public class ArgosTornadoEmitter : BaseProjectile
	{
		public override string Texture => AOUtils.GetTexture<Tempest>();
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
							//Kill();
							return;
						}
					}
				}
			}
			else
			{
				drawIndicator = true;
			}
		}

		public override float Size => 5f;
		public const int MaxTime = 60 * 2;

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			Main.projFrames[Type] = 10;
		}

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

		public static bool[] randomVariance = new bool[10];

		public sealed class TornadoBlade()
		{
			public int lifevalue;
			public float position
			{
				get
				{
					return lifevalue * ModContent.GetInstance<ArgosTornadoEmitter>().Size;
				}
				set
				{
					lifevalue = (value / ModContent.GetInstance<ArgosTornadoEmitter>().Size).Round();
				}
			}

			public static Asset<Texture2D> Sprite => TextureAssets.Projectile[Type];
			public static int Type => ModContent.ProjectileType<ArgosTornadoEmitter>();
			public bool dir = Main.rand.NextBool();
			public int frameNum => lifevalue / 5;

			public void Update()
			{
				lifevalue++;
			}

			public void Draw(Projectile Projectile)
			{
				if (frameNum < Main.projFrames[Type])
				{
					var pos = Projectile.Center - (Vector2.UnitY * position) - Main.screenPosition;
					var frame = Sprite.Frame(1, Main.projFrames[Type], 0, frameNum);
					var opacity = MathF.Min(.5f + (frameNum / (float)Main.projFrames[Type]), 1f) * (Projectile.timeLeft / (float)MaxTime);
					Main.EntitySpriteDraw(Sprite.Value, pos, frame, Projectile.GetAlpha(Color.Orange * opacity), Projectile.rotation, frame.Size() / 2f, Projectile.scale, dir ? SpriteEffects.None : SpriteEffects.FlipHorizontally);
				}
			}
		}
		public List<TornadoBlade> TornadoBlades = [];
		private int oldTime;

		public override bool PreDraw(ref Color lightColor)
		{
			if (Projectile.timeLeft <= MaxTime)
			{
				if (oldTime != Projectile.timeLeft)
				{
					oldTime = Projectile.timeLeft;
					if (Projectile.timeLeft % 10 == 0 && TornadoBlades.Count <= 20)
					{
						TornadoBlades.Add(new());
					}

					foreach (TornadoBlade blade in TornadoBlades)
					{
						blade.Update();
					}

					TornadoBlades.RemoveAll(e => e.frameNum >= Main.projFrames[Type]);
				}

				foreach (TornadoBlade blade in TornadoBlades)
				{
					blade.Draw(Projectile);
				}
			}

			return false;
		}

		public override void PostDraw(Color lightColor)
		{
			BlockingIndicator.Show |= drawIndicator;
			base.PostDraw(lightColor);
		}
	}
}
