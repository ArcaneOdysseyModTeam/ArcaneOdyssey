using ArcaneOdyssey.Items.Equipment.Pets;
using ArcaneOdyssey.Projectiles.Base;
using Terraria.Audio;

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
			base.SetStaticDefaults();
			Main.projFrames[Type] = 13;
			Main.projPet[Type] = true;
		}

		public override float Size => .5f;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = 132 * 2;
			Projectile.height = 109 * 2;
			Projectile.tileCollide = false;
			Projectile.frame = 12;
			Projectile.netImportant = true;
		}
		public override bool PreAI()
		{
			wasThereABoss = isThereABoss; // set if there was a boss
			isThereABoss = false; // there is not a boss by default
			foreach (NPC npc in Main.ActiveNPCs) // check every npc
			{
				if (npc.boss) // if the npc is a boss
				{
					isThereABoss = true; // set that a boss is alive
				}
			}
			if (!isThereABoss && wasThereABoss) // if there was a boss and now there isnt
			{
				wasThereABoss = false; // there was not a boss
				isThereABoss = false; // there is not a boss
				Projectile.ai[0] = 300; // sets ai 0 to 300
				Projectile.frame = 0; // sets frame to 1
				haveICelebrated = false; // has not celebrated
			}
			return true;
		}

		public override void AI()
		{
			var modPlayer = Owner.GetModPlayer<ThyPlayer>(); // get player
			if (modPlayer.elfPet) // if the player is supposed to have this pet
			{
				Projectile.timeLeft = 2; // keep pet alive
			}
			if (Projectile.ai[0] > 0) // if ai 0 is over 0
			{
				Projectile.ai[0] -= 1f;
				wasThereABoss = false; // there was not a boss
				targetPosition = Owner.Center + new Vector2(Owner.direction * -38f, -30f); // sets target position to beside the player
				if (Projectile.Center.X > Owner.Center.X) // changes direction to face player
				{
					Projectile.spriteDirection = -1;
				}
				else
				{
					Projectile.spriteDirection = 1;
				}
				//Get frame
				if (Projectile.frame > 3) // if the frame is over 4
				{
					Projectile.frame = 0; // set frame to 1
				}
				if (Projectile.frameCounter > 10) // every 10 ticks
				{
					Projectile.frameCounter = 0; // reset frame counter
					if (++Projectile.frame > 3) // if the frame is over 4
					{
						Projectile.frame = 0; // set frame to 1
					}
					if (Projectile.frame == 3) // if the frame is 4
					{
						if (!haveICelebrated && !Main.dedServ) // if not celebrated
						{
							// Confetti
							for (int n = 0; n < 20; n++)
							{
								// celebrate
								int[] confettis = [DustID.Confetti_Blue, DustID.Confetti_Green, DustID.Confetti_Pink, DustID.Confetti_Yellow];
								Dust.NewDust(Projectile.Center + new Vector2(0f, -25f), 1, 1, Main.rand.Next(confettis), 0, 0);
							}
							//Audio here
							SoundEngine.PlaySound(ElfYippeeSound, Projectile.Center); // play sound
							haveICelebrated = true; // pet has celebrated
						}
					}
				}
			}
			else
			{
				if (Owner.afkCounter > 60 * 10) // if player afk for 10 seconds
				{
					targetPosition = Owner.Center + new Vector2(Owner.direction * 100f, 7.5f); // move to beside player
					Projectile.spriteDirection = Owner.direction * -1; // face towards player
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
							Projectile.frame = 6;
						}
					}
				}
				else
				{
					// Get frame
					targetPosition = Owner.Center + new Vector2(Owner.direction * -38f, -30f); // sets target position to beside the player
					if (Projectile.Center.X > Owner.Center.X) // changes direction to face player
					{
						Projectile.spriteDirection = -1;
					}
					else
					{
						Projectile.spriteDirection = 1;
					}
					if (Projectile.frame > 3) // if the frame is over 4
					{
						Projectile.frame = 0; // set frame to 1
					}
					if (Projectile.frameCounter > 10) // every 10 ticks
					{
						Projectile.frameCounter = 0; // reset frame counter
						if (++Projectile.frame > 3) // if the frame is over 4
						{
							Projectile.frame = 0; // set frame to 1
						}
					}
				}
			}
			if (Vector2.Distance(Projectile.Center, targetPosition) > 2500)
			{
				Projectile.Center = targetPosition;
			}
			if (Vector2.Distance(Projectile.Center, targetPosition) > 500)
			{
				Projectile.Center = Projectile.Center.MoveTowards(targetPosition, 10f);
			}
			else
			{
				Projectile.Center = Projectile.Center.MoveTowards(targetPosition, 5f);
			}
			Projectile.frameCounter++;
		}

		public override bool? CanDamage() => false;

		public override SpriteEffects FlippedMode => SpriteEffects.FlipHorizontally;
	}
}