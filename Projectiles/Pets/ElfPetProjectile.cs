using ArcaneOdyssey.Items.Equipment.Pets;
using ArcaneOdyssey.Projectiles.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace ArcaneOdyssey.Projectiles.Pets
{
	public class ElfPetProjectile : PlayerProjectile
	{
		private Vector2 targetPosition;
		private bool wasThereABoss = false;
		private bool isThereABoss = false;
		private bool haveICelebrated = false;
		public static readonly SoundStyle ElfYippeeSound = new(ArcaneOdysseyMod.InternalName + "/Sounds/ElfPetYippee");
		public override void SetStaticDefaults()
		{
			Main.projFrames[Type] = 13;
			Main.projPet[Type] = true;
		}
		public override void SetDefaults()
		{
			Projectile.width = 132;
			Projectile.height = 109;
			Projectile.tileCollide = false;
			Projectile.frame = 12;
			Projectile.netImportant = true;
		}
		public override bool PreAI()
		{
			wasThereABoss = isThereABoss;
			isThereABoss = false;
			foreach (NPC npc in Main.ActiveNPCs)
			{
				if (npc.boss)
				{
					isThereABoss = true;
				}
			}
			if (!isThereABoss && wasThereABoss)
			{
				wasThereABoss = false;
				isThereABoss = false;
				Projectile.ai[0] = 300;
				Projectile.frame = 0;
				haveICelebrated = false;
			}
			return true;
		}

		public override void AI()
		{
			var modPlayer = Owner.GetModPlayer<ThyPlayer>();
			if (modPlayer.elfPet)
			{
				Projectile.timeLeft = 2;
			}
			if (Projectile.ai[0] > 0)
			{
				wasThereABoss = false;
				targetPosition = Owner.Center + new Vector2(Owner.direction * -38f, -30f);
				if (Projectile.Center.X > Owner.Center.X)
				{
					Projectile.spriteDirection = -1;
				}
				else
				{
					Projectile.spriteDirection = 1;
				}
				//Get frame
				if (Projectile.frame > 3)
				{
					Projectile.frame = 0;
				}
				if (Projectile.frameCounter > 10)
				{
					Projectile.frameCounter = 0;
					Projectile.frame++;
					if (Projectile.frame > 3)
					{
						Projectile.frame = 0;
					}
					if (Projectile.frame == 3)
					{
						if (!haveICelebrated && !Main.dedServ)
						{
							// Confetti
							for (int n = 0; n < 20; n++)
							{
								int[] confettis = [DustID.Confetti_Blue, DustID.Confetti_Green, DustID.Confetti_Pink, DustID.Confetti_Yellow];
								Dust.NewDust(Projectile.Center + new Vector2(0f, -25f), 1, 1, confettis[(int)Math.Round(Main.rand.NextFloat() * 3f)], 0, 0);
							}
							//Audio here
							SoundEngine.PlaySound(ElfYippeeSound, Projectile.Center);
							haveICelebrated = true;
						}
					}
				}
			}
			else
			{
				targetPosition = Owner.Center + new Vector2(Owner.direction * 60f, 7f);
				Projectile.spriteDirection = Owner.direction;
				//Get frame
				if (Projectile.frame < 4)
				{
					Projectile.frame = 4;
				}
				if (Projectile.frameCounter > 10)
				{
					Projectile.frameCounter = 0;
					Projectile.frame++;
					if (Projectile.frame < 4)
					{
						Projectile.frame = 4;
					}
					else if (Projectile.frame > 11)
					{
						Projectile.frame = 4;
					}
				}
			}
			float targetAngle = Projectile.Center.AngleTo(targetPosition);
			if (Vector2.Distance(Projectile.Center, targetPosition) >= 5)
			{
				Projectile.Center += new Vector2(MathF.Cos(targetAngle), MathF.Sin(targetAngle)) * 5;
			}
			if (Vector2.Distance(Projectile.Center, targetPosition) > 2500)
			{
				Projectile.Center = targetPosition;
				//idk add like teleport dust here
			}
			else if (Vector2.Distance(Projectile.Center, targetPosition) > 500)
			{
				Projectile.Center += new Vector2(MathF.Cos(targetAngle), MathF.Sin(targetAngle)) * 15;
			}
			if (Projectile.ai[0] > -1f)
			{
				Projectile.ai[0] -= 1f;
			}
			Projectile.frameCounter++;
		}

		public override bool? CanDamage() => false;

		public override SpriteEffects FlippedMode => SpriteEffects.FlipHorizontally;
	}
}