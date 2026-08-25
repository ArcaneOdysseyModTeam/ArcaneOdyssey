using ArcaneOdyssey.Biomes;
using ArcaneOdyssey.Gores;
using ArcaneOdyssey.Imbues.Relics;
using ArcaneOdyssey.Items.Armour.RavennaNoble;
using ArcaneOdyssey.Items.BossBags;
using ArcaneOdyssey.Items.BossRelics;
using ArcaneOdyssey.Items.BossTrophies;
using ArcaneOdyssey.Items.Equipment.Pets;
using ArcaneOdyssey.Items.Weapons.RavennaNoble;
using ArcaneOdyssey.Projectiles.Enemies.Elius;
using ArcaneOdysseyMusic;
using AssGen;
using System;
using System.IO;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;

namespace ArcaneOdyssey.NPCs.Bosses
{
	[AutoloadBossHead]
	public class LordElius : BaseNPC
	{
		private Texture2D frontBowArmTexture = ModContent.Request<Texture2D>("ArcaneOdyssey/NPCs/Bosses/EliusBowFrontArm").Value;
		private Texture2D backBowArmTexture = ModContent.Request<Texture2D>("ArcaneOdyssey/NPCs/Bosses/EliusBowBackArm").Value;
		private Texture2D bowTexture = ModContent.Request<Texture2D>("ArcaneOdyssey/NPCs/Bosses/EliusBow").Value;
		//for elius position tests
		private Texture2D eliusTestTexture = ModContent.Request<Texture2D>("ArcaneOdyssey/NPCs/Bosses/LordElius").Value;
		//end for elius position tests
		private Texture2D spearArmTexture = ModContent.Request<Texture2D>("ArcaneOdyssey/NPCs/Bosses/EliusSpearThrowArm").Value;
		private Texture2D spearTexture = ModContent.Request<Texture2D>(AOUtils.GetTexture<NobleThunderspear>()).Value;
		private int hptoheal;
		private float tempPodiumID;
		private Vector2 previousPodiumLocation, nextPodiumLocation;
		private readonly float[] dashSelectArray = { 0f, 4f };
		private readonly float[] moveSelectArrayOne = { 1f, 4f, 1f, 1f };
		private readonly float[] moveSelectArrayTwo = { 2f, 2f, 6f };
		private readonly Vector2[] podiumPos = [new(-665f, 16f), new(-320f, 0f), new(0f, 0f), new(366f, 0f), new(686f, 16f)];
		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[NPC.type] = 65;
			NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new() { Direction = 1 };
			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
			NPCID.Sets.NoTownNPCHappiness[Type] = true;
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.Add(new FlavorTextBestiaryInfoElement($"Mods.{Mod.Name}.Bestiary.{Name}"));
		}

		private bool secondphase = false;

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(previousPodiumLocation);
			writer.Write(nextPodiumLocation);
			writer.Write(secondphase);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			previousPodiumLocation = reader.ReadVector2();
			nextPodiumLocation = reader.ReadVector2();
			secondphase = reader.ReadBoolean();
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			NPC.lifeMax = 3000;
			NPC.noGravity = true;
			NPC.noTileCollide = true;
			NPC.damage = 30;
			NPC.knockBackResist = 0f;
			NPC.defense = 0;
			NPC.width = Player.defaultWidth;
			NPC.height = Player.defaultHeight;
			NPC.value = Item.buyPrice(gold: 3);
			NPC.SpawnWithHigherTime(10);
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.friendly = false;
			NPC.trapImmune = true;
			NPC.lavaImmune = true;
			NPC.boss = true;
			SpawnModBiomes = [AOUtils.BiomeType<EliusArena>()];
			NPC.buffImmune[BuffID.Confused] = true;
		}
		public override bool CanHitPlayer(Player target, ref int cooldownSlot) => false;

		public override MusicTrack Theme => MusicTrack.Elius;

		public bool sentMessage = false;
		private bool hasSetSpawnLocation = false;
		public Vector2 spawnLocation;

		private int spareTimer = 60 * 60; // 1 minute

		public override void AI()
		{
			if (NPC.life < NPC.lifeMax / 2)
			{
				if (!secondphase)
				{
					NPC.NPCDialogue(this.GetLocalizedValue("SecondPhaseMessage"), Color.MediumPurple,true);
				}
				secondphase = true;
			}
			if (!sparing)
			{
				Main.raining = true;
				Main.rainTime = 2;
				Main.windSpeedTarget = MathHelper.Lerp(-.8f, -.4f, NPC.life / (float)NPC.lifeMax);
				Main.maxRaining = MathHelper.Lerp(1, .7f, NPC.life / (float)NPC.lifeMax);
			}
			else
			{
				Main.windSpeedTarget = -.1f;
				if (!sentMessage)
				{
					NPC.NPCDialogue(this.GetLocalizedValue("DoomMessage"), Color.MediumPurple);
					sentMessage = true;
				}
				if (spareTimer-- <= 0)
				{
					Main.NewText(this.GetLocalizedValue("Spared"), new Color(0, 183, 255));
					NPC.active = false;
					if (!DownedBosses.DownedElius)
					{
						EliusSpareSystem.spared = true;
					}
					if (AOUtils.ServerOrSingleplayer)
					{
						NPC.netUpdate = true;
						NPC.NPCLoot();
					}
				}
				return;
			}

			if (!sentMessage)
			{
				if (DownedBosses.DownedElius)
				{
					if (EliusSpareSystem.spared)
					{
						NPC.NPCDialogue(this.GetLocalizedValue("Refight"), Color.MediumPurple);
					}
					else
					{
						NPC.Opacity = .5f;
					}
				}
				else
				{
					NPC.NPCDialogue(this.GetLocalizedValue("SpawnMessage"), Color.MediumPurple);
				}
				sentMessage = true;
			}

			if (!hasSetSpawnLocation) //this also is used for setup
			{
				spawnLocation = NPC.position;
				hasSetSpawnLocation = true;
				NPC.position = spawnLocation + podiumPos[2];
				tempPodiumID = 2;
				NPC.ai[0] = -1f;
				NPC.ai[1] = 0f;
				NPC.ai[2] = 0f;
				NPC.ai[3] = 2f;
				secondphase = false;
			}


			NPC.spriteDirection = (NPC.SafeDirectionTo(Main.player[Player.FindClosest(NPC.position, NPC.width, NPC.height)].Center).X > 0).ToDirectionInt();
			NPC.TargetClosest();
			if (secondphase && AOUtils.ServerOrSingleplayer && Main.player[NPC.target].Distance(NPC.position) < 7000 && (Main.GameUpdateCount % 300 == 0 || (Main.GameUpdateCount % 150 == 0 && (Main.expertMode || Main.masterMode))))
			{
				Projectile.NewProjectile(NPC.GetSource_FromThis(), Main.player[NPC.target].Center, Vector2.Zero, ModContent.ProjectileType<EliusPlacedExplosion>(), (int)(NPC.damage * 1.5), 0f, -1);
			}

			// State Machine
			// ai[1] is the state frame, ai[0] is the state ID, ai[2] is the healing timer, and should not bee touched, ai[3] is extra numerical data
			if (NPC.ai[0] == -1) //Spawn In
			{
				if (NPC.ai[1] > 120f)
				{
					NPC.ai[1] = -1f;
					NPC.ai[0] = 1;
					NPC.ai[3] = 2;
				}
			}
			else if (NPC.ai[0] == 0)
			{
				if(NPC.ai[1] == 4f)
				{
					Gore.NewGore(NPC.GetSource_FromThis(), NPC.Center+new Vector2(NPC.spriteDirection * 20f, -3f), new Vector2(NPC.spriteDirection * 5f, -1f), ModContent.GoreType<EmptyHealthPotion>(), 0.8f);
				}
				if (NPC.ai[1] > 120f)
				{
					NPC.ai[1] = -1f;
					NPC.ai[0] = 1;
				}
			}
			else if (NPC.ai[0] == 1) //Hop move
			{
				if (NPC.ai[1] < 2f) //Choose the podium
				{
					if (AOUtils.ServerOrSingleplayer)
					{
						NPC.netUpdate = true;
						tempPodiumID = NPC.ai[3];
						while (tempPodiumID == NPC.ai[3])
						{
							NPC.ai[3] = (float)Main.rand.Next(5);
						}
						NPC.ai[1] = 2f;
						previousPodiumLocation = NPC.position;
						nextPodiumLocation = spawnLocation + podiumPos[(int)NPC.ai[3]];
					}
				}
				else //prevent skipping to the next parts
				{
					if (!secondphase)
					{
						NPC.spriteDirection = nextPodiumLocation.X > previousPodiumLocation.X ? 1 : -1;
						
						// newer dash code
						if(NPC.ai[1] < 70f && NPC.ai[1] > 30f)
						{
							NPC.position.X += (nextPodiumLocation.X - previousPodiumLocation.X) / 39f;
							NPC.position = FindPointInCurve(previousPodiumLocation,nextPodiumLocation,new Vector2((nextPodiumLocation.X+previousPodiumLocation.X)/2f,(nextPodiumLocation.Y < previousPodiumLocation.Y ? nextPodiumLocation.Y: previousPodiumLocation.Y) - 30),NPC.position.X);
						}
						
						
						if (NPC.ai[1] >= 100f) //Break out of this ai cycle
						{
							if (AOUtils.ServerOrSingleplayer)
							{
								NPC.netUpdate = true;
								NPC.position = nextPodiumLocation;
								NPC.ai[1] = -1f;
								NPC.ai[1] = -1f;
								NPC.ai[0] = moveSelectArrayTwo[Main.rand.Next(3)];
								NPC.ai[2] += 1f; //increment heal cooldown
								if (NPC.ai[2] >= 6f && NPC.life < NPC.lifeMax - 100) //override to heal if cooldown is expended and hp is low enough
								{
									NPC.ai[0] = 3f;
								}
							}
						}
					}
					else //phase 2
					{
						if (NPC.ai[1] < 30f)
						{
							NPC.spriteDirection = nextPodiumLocation.X > previousPodiumLocation.X ? 1 : -1;
							Dust.NewDustDirect(nextPodiumLocation + new Vector2(-10f, 35f), 50, 3, DustID.WitherLightning, 0f, -0.1f).noGravity = true;
						}
						if (NPC.ai[1] >= 30f) //Break out of this ai cycle
						{
							if (NPC.ai[1] == 30f)
							{
								if (AOUtils.ServerOrSingleplayer)
								{
									NPC.netUpdate = true;
									Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<EliusTrail>(), 0, 0f, -1, nextPodiumLocation.X + 10f, nextPodiumLocation.Y + 21f);
								}
								SoundEngine.PlaySound(SoundID.DD2_LightningBugZap with { Volume = 2.25f }, NPC.Center);
							}
							if (NPC.ai[1] > 35f)
							{
								NPC.position = nextPodiumLocation;
								if (AOUtils.ServerOrSingleplayer)
								{
									NPC.netUpdate = true;
									NPC.ai[1] = -1f;
									NPC.ai[1] = -1f;
									NPC.ai[0] = moveSelectArrayTwo[Main.rand.Next(3)];
									NPC.ai[2] += 1f; //increment heal cooldown
									if (NPC.ai[2] >= 6f && NPC.life < NPC.lifeMax - 100) //override to heal if cooldown is expended and hp is low enough
									{
										NPC.ai[0] = 3f;
									}
								}
							}
						}
					}
				}
			}
			else if (NPC.ai[0] == 2) //spear throw
			{
				if (NPC.ai[1] >= 40f && NPC.ai[1] < 42f)
				{
					NPC.ai[1] = 42f;
					SoundEngine.PlaySound(SoundID.Item1, NPC.Center);
					if (AOUtils.ServerOrSingleplayer)
					{
						NPC.netUpdate = true;
						Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, (Main.player[NPC.target].Center - NPC.Center).SafeNormalize() * 15f, ModContent.ProjectileType<EliusSpear>(), (int)(NPC.damage * 0.5), 1f, -1, secondphase.ToInt());
					}
				}
				else if (NPC.ai[1] > 70f)
				{
					if (AOUtils.ServerOrSingleplayer)
					{
						NPC.netUpdate = true;
						NPC.ai[1] = -1f;
						NPC.ai[0] = moveSelectArrayOne[Main.rand.Next(4)];
					}
				}
			}
			else if (NPC.ai[0] == 3) //healing
			{
				if (NPC.ai[1] == 0 &&(!(NPC.ai[2] > 7f || Main.player[NPC.target].Center.Distance(NPC.Center) > 300f || secondphase)))
				{
					NPC.ai[1] = -1f;
					NPC.ai[0] = 1;
				}
				if (NPC.ai[1] > 30f)
				{
						NPC.ai[2] = 0f;
						hptoheal = Main.rand.Next(150) + 50;
						if (AOUtils.ServerOrSingleplayer)
						{
							NPC.netUpdate = true;
							NPC.life += hptoheal;
						}
						if (secondphase)
						{
							if (AOUtils.ServerOrSingleplayer)
							{
								Projectile.NewProjectileDirect(NPC.GetSource_FromThis(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<EliusPlacedExplosion>(), (int)(NPC.damage * 1.5), 1f, -1).timeLeft = 200;
							}
							SoundEngine.PlaySound(SoundID.Thunder, NPC.Center);
						}
						CombatText.NewText(new Rectangle((int)NPC.position.X, (int)NPC.position.Y, 0, 0), CombatText.HealLife, hptoheal, false, false);
						SoundEngine.PlaySound(SoundID.Item3, NPC.Center);
						NPC.ai[1] = -1f;
						NPC.ai[0] = 0;
						if (NPC.life > NPC.lifeMax)
						{
							NPC.life = NPC.lifeMax;
						}
				}
			}
			else if (NPC.ai[0] == 4) //Hop move into sword move
			{
				if (NPC.ai[1] < 2f) //Choose the podium
				{
					if (AOUtils.ServerOrSingleplayer)
					{
						NPC.netUpdate = true;
						tempPodiumID = NPC.ai[3];
						while (tempPodiumID == NPC.ai[3])
						{
							NPC.ai[3] = dashSelectArray[Main.rand.Next(2)];
						}
						NPC.ai[1] = 2f;
						previousPodiumLocation = NPC.position;
						nextPodiumLocation = spawnLocation + podiumPos[(int)NPC.ai[3]];
					}
				}
				else //prevent skipping to the next parts
				{
					if (!secondphase)
					{
						NPC.spriteDirection = nextPodiumLocation.X > previousPodiumLocation.X ? 1 : -1;
						
						// newer dash code
						if(NPC.ai[1] < 70f && NPC.ai[1] > 30f)
						{
							NPC.position.X += (nextPodiumLocation.X - previousPodiumLocation.X) / 39f;
							NPC.position = FindPointInCurve(previousPodiumLocation,nextPodiumLocation,new Vector2((nextPodiumLocation.X+previousPodiumLocation.X)/2f,(nextPodiumLocation.Y < previousPodiumLocation.Y ? nextPodiumLocation.Y: previousPodiumLocation.Y) - 30),NPC.position.X);
						}
						
						if (NPC.ai[1] >= 100f) //Break out of this ai cycle
						{
							NPC.position = nextPodiumLocation;
							NPC.ai[1] = -1f;
							NPC.ai[0] = 5;
						}
					}
					else // Second Phase
					{
						if (NPC.ai[1] < 30f)
						{
							NPC.spriteDirection = nextPodiumLocation.X > previousPodiumLocation.X ? 1 : -1;
							Dust.NewDustDirect(nextPodiumLocation + new Vector2(-10f, 35f), 50, 3, DustID.WitherLightning, 0f, -0.1f).noGravity = true;
						}
						if (NPC.ai[1] >= 30f) //Break out of this ai cycle
						{
							if (NPC.ai[1] == 30f)
							{
								if (AOUtils.ServerOrSingleplayer)
								{
									NPC.netUpdate = true;
									Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<EliusTrail>(), 0, 0f, -1, nextPodiumLocation.X + 10f, nextPodiumLocation.Y + 21f);
								}
								SoundEngine.PlaySound(SoundID.DD2_LightningBugZap with { Volume = 2.25f }, NPC.Center);
							}
							if (NPC.ai[1] > 35f)
							{
								NPC.position = nextPodiumLocation;
							}
							if (NPC.ai[1] > 75f)
							{
								NPC.ai[1] = -1f;
								NPC.ai[0] = 5;
							}
						}
					}
				}
			}
			else if (NPC.ai[0] == 5) //sword move
			{
				if (NPC.ai[1] < 61f && NPC.ai[1] >= 45f)
				{
					NPC.position.Y += 132f / 16f;
				}
				else if (NPC.ai[1] < 446f && NPC.ai[1] >= 122f)
				{
					int swordTiming = !secondphase ? 40 : 24;
					if ((int)NPC.ai[1] % swordTiming == 4 && NPC.ai[1] < 444f)
					{
						NPC.NPCDialogue(!secondphase ? this.GetLocalizedValue("FlyingSlashMessage") : this.GetLocalizedValue("MoveElementName") + this.GetLocalizedValue("FlyingSlashMessage"), !secondphase ? Color.Gold : Color.MediumPurple, false);
						SoundEngine.PlaySound(SoundID.Item1 with { Volume = 2.25f }, NPC.Center);
						if (AOUtils.ServerOrSingleplayer)
						{
							NPC.netUpdate = true;
							Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, new Vector2(NPC.spriteDirection * 20f, 0f), ModContent.ProjectileType<EliusSlash>(), (int)(NPC.damage * 0.5), 1f, -1, secondphase.ToInt());
						}
					}
				}
				else if (NPC.ai[1] < 532f && NPC.ai[1] >= 516f)
				{
					NPC.position.Y -= 132f / 16f;
				}
				else if (NPC.ai[1] > 593f)
				{
					NPC.ai[1] = -1f;
					NPC.ai[0] = 1;
				}
			}
			else if (NPC.ai[0] == 6)  //storm of arrows
			{
				if (NPC.ai[1] > 20f && NPC.ai[1] < 22f)
				{
					NPC.NPCDialogue(!secondphase ? this.GetLocalizedValue("StormOfArrowsMessage") : this.GetLocalizedValue("MoveElementName") + this.GetLocalizedValue("StormOfArrowsMessage"), Color.MediumPurple, false);
					SoundEngine.PlaySound(SoundID.Item5, NPC.Center);
					if (AOUtils.ServerOrSingleplayer)
					{
						NPC.netUpdate = true;
						(Projectile.NewProjectileDirect(NPC.GetSource_FromThis(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<EliusArrowStorm>(), (int)(NPC.damage * 0.6), 0f, -1, 0, Main.player[NPC.target].Center.X, Main.player[NPC.target].Center.Y - 600f).ModProjectile as EliusArrowStorm).secondphase = secondphase;
						NPC.ai[1] = 22f;
					}
				}
				else if (NPC.ai[1] > 50f)
				{
					if (AOUtils.ServerOrSingleplayer)
					{
						NPC.netUpdate = true;
						NPC.ai[1] = -1f;
						NPC.ai[0] = moveSelectArrayOne[Main.rand.Next(4)];
					}
				}
			}
			NPC.ai[1] += 1f; //increment frame
		}

		public override void FindFrame(int frameHeight)
		{
			if(NPC.HasValidTarget)
			{
				if(NPC.ai[0] == 3) //healing
				{
					if(NPC.ai[1] == 0)
					{
						NPC.frame.Y = 0;
					}
					if(NPC.ai[1] == 1)
					{
						NPC.frame.Y = frameHeight * 1;
						NPC.frameCounter = 0;
					}
					if(NPC.frameCounter >= 6)
					{
						NPC.frame.Y += frameHeight;	
						NPC.frameCounter = 0;
					}
				} else if(NPC.ai[0] == 0) //healing end
				{
					if(NPC.ai[1] == 0)
					{
						NPC.frame.Y = frameHeight * 7;
						NPC.frameCounter = 0;
					}
					if(NPC.frameCounter >= 6)
					{
						NPC.frame.Y += frameHeight;	
						NPC.frameCounter = 0;
					}
					if(NPC.ai[1] > 15)
					{
						NPC.frame.Y = 0;
					}
				} else if(NPC.ai[0] == 1 || NPC.ai[0] == 4) // dashes
				{
					if(secondphase)
					{
						NPC.frame.Y = 0;
					} else
					{
						//first phase dash stuff
						if(NPC.ai[1] == 0)
						{
							NPC.frame.Y = frameHeight * 10;
							NPC.frameCounter = 0;
						}
						if(NPC.ai[1] < 30f && NPC.ai[1] > 15 && NPC.frameCounter >= 5f)
						{
							NPC.frame.Y += frameHeight;
							NPC.frameCounter = 0;
						}
						if (NPC.ai[1] < 40 && NPC.ai[1] > 30)
						{
							NPC.frame.Y = frameHeight * 14;
							NPC.frameCounter = 0;
						} 
						if (NPC.ai[1] < 60 && NPC.ai[1] > 40)
						{
							NPC.frame.Y = frameHeight * 15;
							NPC.frameCounter = 0;
						} 
						if (NPC.ai[1] < 70 && NPC.ai[1] > 60)
						{
							NPC.frame.Y = frameHeight * 16;
							NPC.frameCounter = 0;
						}
						if(NPC.ai[1] == 70)
						{
							NPC.frame.Y = frameHeight * 17;
							NPC.frameCounter = 0;
						}
						if (NPC.ai[1] < 85 && NPC.ai[1] > 70 && NPC.frameCounter >= 5f)
						{
							NPC.frame.Y += frameHeight;
							NPC.frameCounter = 0;
						}
						if(NPC.ai[1] > 85)
						{
							NPC.frame.Y = 0;
							NPC.frameCounter = 0;
						}
					}
				} else if(NPC.ai[0] == 5) // flying slashes
				{
					if(NPC.ai[1] == 0)
					{
						NPC.frame.Y = 0;
					}
					if(NPC.ai[1] == 44)
					{
						NPC.frame.Y = 20 * frameHeight;
						NPC.frameCounter = 0;
					}
					if (NPC.ai[1] >= 45 && NPC.ai[1] < 61 && NPC.frameCounter >= 8)
					{
						NPC.frame.Y += frameHeight;
						NPC.frameCounter = 0;
					}
					if (NPC.ai[1] == 61)
					{
						NPC.frame.Y = frameHeight * 22;
						NPC.frameCounter = 0;
					}
					if (NPC.ai[1] < 92 && NPC.ai[1] > 61 && NPC.frameCounter >= 5)
					{
						NPC.frame.Y += frameHeight;
						NPC.frameCounter = 0;
					}
					if (NPC.ai[1] == 92)
					{
						NPC.frame.Y = frameHeight * 28;
						NPC.frameCounter = 0;
					}
					if(NPC.ai[1] > 92 && NPC.ai[1] < 122 && NPC.frameCounter >= 5)
					{
						NPC.frame.Y += frameHeight;
						NPC.frameCounter = 0;
					}
					if(NPC.ai[1] > 122 && NPC.ai[1] < 446)
					{
						if(secondphase)
						{
							if(NPC.frameCounter >= 6)
							{
								NPC.frame.Y += frameHeight;
								NPC.frameCounter = 0;
							}
						} else
						{
							if(NPC.frameCounter >= 10)
							{
								NPC.frame.Y += frameHeight;
								NPC.frameCounter = 0;
							}
						}
						if(NPC.frame.Y/frameHeight > 41)
						{
							NPC.frame.Y = frameHeight * 34;
						}
					}
					if(NPC.ai[1] == 446) //putting weapons away
					{
						NPC.frame.Y = frameHeight * 33;
						NPC.frameCounter = 0;
					}
					if(NPC.ai[1] > 448 && NPC.ai[1] < 466 && NPC.frameCounter >= 3)
					{
						NPC.frame.Y -= frameHeight;
						NPC.frameCounter = 0;
					}
					if(NPC.ai[1] == 495) // starting to rise up prep
					{
						NPC.frame.Y = frameHeight * 42;
						NPC.frameCounter = 0;
					}
					if(NPC.ai[1] > 495 && NPC.ai[1] < 516 && NPC.frameCounter >= 5) //rising up prep
					{
						NPC.frame.Y += frameHeight;
						NPC.frameCounter = 0;
					}
					if(NPC.ai[1] == 516) //starting rise up
					{
						NPC.frame.Y = frameHeight * 46;
						NPC.frameCounter = 0;
					}
					if(NPC.ai[1] > 516 && NPC.ai[1] < 532) //rising up
					{
						if(NPC.ai[1] < 520)
						{
							NPC.frame.Y = frameHeight * 46;
							NPC.frameCounter = 0;
						}
						else if(NPC.ai[1] < 528)
						{
							NPC.frame.Y = frameHeight * 47;
							NPC.frameCounter = 0;
						}
						else
						{
							NPC.frame.Y = frameHeight * 48;
							NPC.frameCounter = 0;
						}
					}
					if(NPC.ai[1] == 532)
					{
						NPC.frame.Y = frameHeight * 49;
						NPC.frameCounter = 0;
					}
					if(NPC.ai[1] > 532 && NPC.ai[1] < 552 && NPC.frameCounter >= 5)
					{
						NPC.frame.Y += frameHeight;
						NPC.frameCounter = 0;	
					}
					if(NPC.ai[1] > 557) //completing
					{
						NPC.frame.Y = 0;
						NPC.frameCounter = 0;
					}
				} else if(NPC.ai[0] == 6) //storm of arrows
				{
					NPC.frame.Y = 64 * frameHeight;
				}  else if(NPC.ai[0] == 2) //spear throw
				{
					NPC.frame.Y = 59 * frameHeight;
				}
				else
				{
					// IF NOT IN THE SELECTED BEHAVIORS
					NPC.frame.Y = 0;
					NPC.frameCounter = 0;
				}
			} else
			{
				NPC.frame.Y = 0;
			}
			NPC.frameCounter++;
			if(sparing)
			{
				NPC.frame.Y = frameHeight * 0;
			}
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			//Elius position tests
			Main.EntitySpriteDraw(eliusTestTexture,(((spawnLocation+podiumPos[0])+new Vector2(NPC.width/2,(NPC.height/2)-4))-screenPos),new Rectangle(0,0,eliusTestTexture.Width,eliusTestTexture.Height/65),new Color(255,255,255,170),0f,new Vector2(eliusTestTexture.Width/2,(eliusTestTexture.Height/65)/2),1f,SpriteEffects.None);
			Main.EntitySpriteDraw(eliusTestTexture,(((spawnLocation+podiumPos[1])+new Vector2(NPC.width/2,(NPC.height/2)-4))-screenPos),new Rectangle(0,0,eliusTestTexture.Width,eliusTestTexture.Height/65),new Color(255,255,255,170),0f,new Vector2(eliusTestTexture.Width/2,(eliusTestTexture.Height/65)/2),1f,SpriteEffects.None);
			Main.EntitySpriteDraw(eliusTestTexture,(((spawnLocation+podiumPos[2])+new Vector2(NPC.width/2,(NPC.height/2)-4))-screenPos),new Rectangle(0,0,eliusTestTexture.Width,eliusTestTexture.Height/65),new Color(255,255,255,170),0f,new Vector2(eliusTestTexture.Width/2,(eliusTestTexture.Height/65)/2),1f,SpriteEffects.None);
			Main.EntitySpriteDraw(eliusTestTexture,(((spawnLocation+podiumPos[3])+new Vector2(NPC.width/2,(NPC.height/2)-4))-screenPos),new Rectangle(0,0,eliusTestTexture.Width,eliusTestTexture.Height/65),new Color(255,255,255,170),0f,new Vector2(eliusTestTexture.Width/2,(eliusTestTexture.Height/65)/2),1f,SpriteEffects.None);
			Main.EntitySpriteDraw(eliusTestTexture,(((spawnLocation+podiumPos[4])+new Vector2(NPC.width/2,(NPC.height/2)-4))-screenPos),new Rectangle(0,0,eliusTestTexture.Width,eliusTestTexture.Height/65),new Color(255,255,255,170),0f,new Vector2(eliusTestTexture.Width/2,(eliusTestTexture.Height/65)/2),1f,SpriteEffects.None);
			Main.EntitySpriteDraw(eliusTestTexture,(((spawnLocation+podiumPos[0]+new Vector2(0,132))+new Vector2(NPC.width/2,(NPC.height/2)-4))-screenPos),new Rectangle(0,0,eliusTestTexture.Width,eliusTestTexture.Height/65),new Color(255,255,255,170),0f,new Vector2(eliusTestTexture.Width/2,(eliusTestTexture.Height/65)/2),1f,SpriteEffects.None);
			Main.EntitySpriteDraw(eliusTestTexture,(((spawnLocation+podiumPos[4]+new Vector2(0,132))+new Vector2(NPC.width/2,(NPC.height/2)-4))-screenPos),new Rectangle(0,0,eliusTestTexture.Width,eliusTestTexture.Height/65),new Color(255,255,255,170),0f,new Vector2(eliusTestTexture.Width/2,(eliusTestTexture.Height/65)/2),1f,SpriteEffects.None);
			//End elius position tests
			if(NPC.ai[0] == 6) //bow rendering
			{
				
			}
			return true;
		}
		public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			if(NPC.ai[0] == 2 && NPC.HasValidTarget) //spear rendering and spear arm rendering
			{
				//Main.EntitySpriteDraw(spearTexture,(NPC.Center-screenPos)+new Vector2(0,0),new Rectangle(0,0,spearTexture.Width,spearTexture.Height),drawColor,(Main.player[NPC.target].Center - NPC.Center).SafeNormalize().ToRotation()+MathHelper.PiOver4,new Vector2(spearTexture.Width/2,spearTexture.Height/2),1f,SpriteEffects.None);
				Main.EntitySpriteDraw(spearArmTexture,(NPC.Center-screenPos)+new Vector2(0,0),new Rectangle(0,0,spearArmTexture.Width,spearArmTexture.Height/7),drawColor,(Main.player[NPC.target].Center - NPC.Center).SafeNormalize().ToRotation(),new Vector2(spearArmTexture.Width/2,spearArmTexture.Height/14),1f,NPC.Center.X<Main.player[NPC.target].Center.X?SpriteEffects.None:SpriteEffects.FlipHorizontally);
				
			}
			if(NPC.ai[0] == 6) //bow rendering
			{
				
			}
		}
		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(AOUtils.Common<EliusTrophy>(10));
			LeadingConditionRule leadingConditionRule1 = new(new Conditions.NotExpert());
			leadingConditionRule1.OnSuccess(new AnyDropHelper([
					ModContent.ItemType<EliusBoots>(),
					ModContent.ItemType<EliusChest>(),
					ModContent.ItemType<EliusHelm>(),
					ModContent.ItemType<NobleThunderspear>(),
					ModContent.ItemType<ScimitarsofStorm>(),
					ModContent.ItemType<StormCaller>()
					], rolls: 2));
			npcLoot.Add(leadingConditionRule1);
			npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<EliusBossBag>()));
			leadingConditionRule1 = new(new Conditions.IsMasterMode());
			leadingConditionRule1.OnSuccess(AOUtils.Common<EliusBossRelic>());
			leadingConditionRule1.OnSuccess(AOUtils.Common<VermillionBracelet>(4));
			npcLoot.Add(leadingConditionRule1);
		}

		public override void OnKill()
		{
			Main.windSpeedTarget = -.1f;
			DownedBosses.DownedElius = true;
			if (Main.dedServ)
			{
				var packet = Mod.GetPacket();
				packet.Write(ArcaneOdysseyMod.PacketID.MarkGlobalDowned);
				packet.Write(Type);
				packet.Send();
			}
			else
			{
				GlobalData.MarkDefeated(this);
			}
			var hitbox = NPC.Hitbox;
			if (!EliusSpareSystem.spared) // kill in singeplayer
			{
				// gore goes here
				for (int n = 0; n < 17; n++)
				{
					Dust.NewDust(hitbox.Center(), 0, 0, DustID.Blood, (Main.rand.NextFloat() - 0.5f) * 3f, (Main.rand.NextFloat() - 0.5f) * 8f);
				}
			}
			else
			{
				for (int n = 0; n < 17; n++)
				{
					Dust.NewDust(hitbox.Center(), 0, 0, DustID.Smoke, (Main.rand.NextFloat() - 0.5f) * 3f, (Main.rand.NextFloat() - 0.5f) * 8f, 255 / 2);
				}
			}
		}


		public bool sparing = false;

		public override bool CheckDead()
		{
			if (DownedBosses.DownedElius)
			{
				return true;
			}
			sparing = true;
			NPC.life = 1;
			NPC.active = true;
			NPC.dontTakeDamage = true;
			NPC.chaseable = false;
			NPC.netUpdate = true;
			sentMessage = false;
			NPC.noGravity = false;
			NPC.noTileCollide = false;
			NPC.velocity = Vector2.Zero;
			return false;
		}

		public override bool CanChat() => sparing;

		public override void SetChatButtons(ref string button, ref string button2)
		{
			button = Mod.CustomLocalization("RandomWords.Kill").Value;
			button2 = Mod.CustomLocalization("RandomWords.Spare").Value;
		}

		public override string GetChat() => this.GetLocalizedValue("DoomMessage");

		public override bool CanGoToStatue(bool toKingStatue) => false;

		public override bool CheckActive() => !sparing;

		public override void OnChatButtonClicked(bool firstButton, ref string shopName)
		{
			NPC.active = false;
			NPC.netUpdate = true;

			if (Main.netMode == NetmodeID.MultiplayerClient)
			{
				var packet = Mod.GetPacket();
				packet.Write(ArcaneOdysseyMod.PacketID.EliusSpare);
				packet.Write(!firstButton);
				packet.Write(NPC.whoAmI);
				packet.Send();
			}
			else
			{
				EliusSpareSystem.spared = !firstButton;
				if (firstButton) // kill
				{
					Main.NewText(this.GetLocalizedValue("Killed"), Color.Purple);
				}
				else
				{
					Main.NewText(this.GetLocalizedValue("Spared"), new Color(0, 183, 255));
				}
				NPC.NPCLoot();
			}
		}

		public override void HitEffect(NPC.HitInfo hit)
		{
			if (!Main.dedServ)
			{
				for (int n = 0; n < 3; n++)
				{
					Dust.NewDust(NPC.Center, 0, 0, DustID.Blood, (Main.rand.NextFloat() - 0.5f) * 3f, (Main.rand.NextFloat() - 0.5f) * 8f);
				}
				if (NPC.life <= 0)
				{
					if (DownedBosses.DownedElius)
					{
						if (EliusSpareSystem.spared)
						{
							Main.NewText(this.GetLocalizedValue("Spared"), SpiritEnergy.Instance.Colour);
						}
					}
				}
			}
		}

		public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
		{
			NPC.lifeMax = (int)(NPC.lifeMax * 0.8f * balance * bossAdjustment);
		}

		private static Vector2 FindPointInCurve(Vector2 pointOne, Vector2 pointTwo,Vector2 pointThree, float xPos)
		{
			float alphaOne = ((-1*MathF.Pow(pointOne.X,2))+MathF.Pow(pointTwo.X,2));
			float betaOne = ((-1*pointOne.X)+pointTwo.X);
			float deltaOne = ((-1*pointOne.Y)+pointTwo.Y);
			float alphaTwo = ((-1*MathF.Pow(pointTwo.X,2))+MathF.Pow(pointThree.X,2));
			float betaTwo = ((-1*pointTwo.X)+pointThree.X);
			float deltaTwo = ((-1*pointTwo.Y)+pointThree.Y);
			float betaMult = (-1*(betaTwo/betaOne));
			float alphaThree = ((betaMult*alphaOne)+alphaTwo);
			float deltaThree = ((betaMult*deltaOne)+deltaTwo);
			float alphaZero = (deltaThree/alphaThree);
			float betaZero = ((deltaOne-(alphaOne*alphaZero))/betaOne);
			float charlieZero = ((pointOne.Y-(alphaZero*MathF.Pow(pointOne.X,2)))-(betaZero*pointOne.X));
			float yPos = (alphaZero*MathF.Pow(xPos,2))+(xPos*betaZero)+charlieZero;
			return new Vector2(xPos,yPos);
		}
	}
}
