using ArcaneOdyssey.Biomes;
using ArcaneOdyssey.Gores;
using ArcaneOdyssey.Gores.Elius;
using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Imbues.Magic.Lost;
using ArcaneOdyssey.Imbues.Magic.Normal;
using ArcaneOdyssey.Imbues.Relics;
using ArcaneOdyssey.Items.Armour.RavennaNoble;
using ArcaneOdyssey.Items.BossBags;
using ArcaneOdyssey.Items.BossRelics;
using ArcaneOdyssey.Items.BossTrophies;
using ArcaneOdyssey.Items.Equipment.Pets;
using ArcaneOdyssey.Items.Weapons.RavennaNoble;
using ArcaneOdyssey.NPCs.Base;
using ArcaneOdyssey.Projectiles.Enemies.Elius;
using ArcaneOdysseyMusic;
using System;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;

namespace ArcaneOdyssey.NPCs.Bosses
{
	[AutoloadBossHead]
	public class LordElius : BaseNPC
	{
		private int hptoheal, despawnTimer;
		private float tempPodiumID;
		private Vector2 previousPodiumLocation, nextPodiumLocation;
		private readonly int[] dashSelectArray = { 0, 4 };
		private readonly int[] moveSelectArrayOne = { 1, 4, 1, 1 };
		private readonly int[] moveSelectArrayTwo = { 2, 2, 6 };
		private readonly Vector2[] podiumPos = [new(-655f, 12f), new(-320f, -4f), new(0f, -4f), new(370f, -4f), new(690f, 12f)];
		public override void SetStaticDefaults()
		{
			NPCID.Sets.NoMultiplayerSmoothingByType[Type] = true;
			Main.npcFrameCount[Type] = 89;
			NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new() { Direction = 1 };
			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
			NPCID.Sets.NoTownNPCHappiness[Type] = true;
			NPCID.Sets.TakesDamageFromHostilesWithoutBeingFriendly[Type] = true;
			NPCID.Sets.TeleportationImmune[Type] = true;
			NPCID.Sets.ReflectStarShotsInForTheWorthy[Type] = true;
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.Add(new FlavorTextBestiaryInfoElement($"Mods.{Mod.Name}.Bestiary.{Name}"));
		}

		private bool secondphase = false;

		public static MagicType Imbue => Main.getGoodWorld ? ModContent.GetInstance<AncientLightningMagic>() : ModContent.GetInstance<LightningMagic>();

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

		public int FrameTimer
		{
			get
			{
				return (int)NPC.ai[1];
			}
			set
			{
				NPC.ai[1] = value;
			}
		}

		public int CurrentState
		{
			get
			{
				return (int)NPC.ai[0];
			}
			set
			{
				NPC.ai[0] = value;
			}
		}

		public override void AI()
		{
			if (AOUtils.ServerOrSingleplayer)
			{
				if (despawnTimer > 160)
				{
					Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<EliusTrail>(), 0, 0f, -1, NPC.Center.X, NPC.Center.Y - 800f);
					NPC.active = false;
					NPC.netUpdate = true;
				}
				if (!NPC.HasValidTarget)
				{
					despawnTimer++;
				}
				else if (despawnTimer != 0)
				{
					despawnTimer = 0;
				}

			}
			if (NPC.life < NPC.lifeMax / 2)
			{
				if (!secondphase)
				{
					NPC.NPCDialogue(this.GetLocalizedValue("SecondPhaseMessage"), Imbue.Colour, true);
				}
				secondphase = true;
			}
			if (!Sparing)
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
					NPC.NPCDialogue(this.GetLocalizedValue("DoomMessage"), Imbue.Colour);
					sentMessage = true;
				}
				if (spareTimer-- <= 0)
				{
					if (AOUtils.ServerOrSingleplayer)
					{
						Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<EliusTrail>(), 0, 0f, -1, NPC.Center.X, NPC.Center.Y - 1000f);
					}
					Main.NewText(this.GetLocalizedValue("Spared"), new Color(0, 183, 255));
					EliusSpareSystem.spared = true;
					NPC.active = false;
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
						NPC.NPCDialogue(this.GetLocalizedValue("Refight"), Imbue.Colour);
					}
					else
					{
						NPC.Opacity = .5f;
					}
				}
				else
				{
					NPC.NPCDialogue(this.GetLocalizedValue("SpawnMessage"), Imbue.Colour);
				}
				sentMessage = true;
			}

			if (!hasSetSpawnLocation) //this also is used for setup
			{
				spawnLocation = NPC.position;
				hasSetSpawnLocation = true;
				NPC.position = spawnLocation + podiumPos[2];
				tempPodiumID = 2;
				CurrentState = -1;
				FrameTimer = 0;
				NPC.ai[2] = 0f;
				NPC.ai[3] = 2f;
				secondphase = false;
			}


			NPC.spriteDirection = (NPC.SafeDirectionTo(Main.player[Player.FindClosest(NPC.position, NPC.width, NPC.height)].Center).X > 0).ToDirectionInt();
			NPC.TargetClosest();
			if (secondphase && AOUtils.ServerOrSingleplayer && Main.player[NPC.target].Distance(NPC.position) < 7000 && (Main.GameUpdateCount % 300 == 0 || (Main.expertMode && Main.GameUpdateCount % 150 == 0)))
			{
				Projectile.NewProjectile(NPC.GetSource_FromThis(), Main.player[NPC.target].Center, Vector2.Zero, ModContent.ProjectileType<EliusPlacedExplosion>(), (int)(NPC.damage * 1.5), 0f, -1);
			}

			// State Machine
			// ai[1] is the state frame, ai[0] is the state ID, ai[2] is the healing timer, and should not bee touched, ai[3] is extra numerical data
			if (NPC.HasValidTarget)
			{
				if (CurrentState == -1) //Spawn In
				{
					if (FrameTimer > 120f)
					{
						FrameTimer = -1;
						CurrentState = 1;
						NPC.ai[3] = 2;
					}
				}
				else if (CurrentState == 0)
				{
					if (FrameTimer == 4f)
					{
						Gore.NewGore(NPC.GetSource_FromThis(), NPC.Center + new Vector2(NPC.spriteDirection * 20f, -3f), new Vector2(NPC.spriteDirection * 5f, -1f), ModContent.GoreType<EmptyHealthPotion>(), 0.8f);
					}
					if (FrameTimer > 120f)
					{
						FrameTimer = -1;
						CurrentState = 1;
					}
				}
				else if (CurrentState == 1) //Hop move
				{
					if (FrameTimer < 2f) //Choose the podium
					{
						if (AOUtils.ServerOrSingleplayer)
						{
							NPC.netUpdate = true;
							tempPodiumID = NPC.ai[3];
							while (tempPodiumID == NPC.ai[3])
							{
								NPC.ai[3] = Main.rand.Next(5);
							}
							FrameTimer = 2;
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
							if (FrameTimer < 70f && FrameTimer > 30f)
							{
								NPC.position.X += (nextPodiumLocation.X - previousPodiumLocation.X) / 39f;
								NPC.position = FindPointInCurve(previousPodiumLocation, nextPodiumLocation, new Vector2((nextPodiumLocation.X + previousPodiumLocation.X) / 2f, (nextPodiumLocation.Y < previousPodiumLocation.Y ? nextPodiumLocation.Y : previousPodiumLocation.Y) - 30), NPC.position.X);
							}


							if (FrameTimer >= 100f) //Break out of this ai cycle
							{
								if (AOUtils.ServerOrSingleplayer)
								{
									NPC.netUpdate = true;
									NPC.position = nextPodiumLocation;
									FrameTimer = -1;
									CurrentState = moveSelectArrayTwo[Main.rand.Next(3)];
									NPC.ai[2] += 1f; //increment heal cooldown
									if (NPC.ai[2] >= 6f && NPC.life < NPC.lifeMax - 100) //override to heal if cooldown is expended and hp is low enough
									{
										CurrentState = 3;
									}
								}
							}
						}
						else //phase 2
						{
							if (FrameTimer < 30f)
							{
								NPC.spriteDirection = nextPodiumLocation.X > previousPodiumLocation.X ? 1 : -1;
								if (Main.getGoodWorld)
									Dust.NewDustDirect(nextPodiumLocation + new Vector2(-10f, 35f), 50, 3, DustID.Firework_Red, 0f, -0.1f).noGravity = true;
								else
									Dust.NewDustDirect(nextPodiumLocation + new Vector2(-10f, 35f), 50, 3, DustID.WitherLightning, 0f, -0.1f).noGravity = true;
							}
							if (FrameTimer >= 30f) //Break out of this ai cycle
							{
								if (FrameTimer == 30f)
								{
									if (AOUtils.ServerOrSingleplayer)
									{
										NPC.netUpdate = true;
										Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<EliusTrail>(), 0, 0f, -1, nextPodiumLocation.X + 10f, nextPodiumLocation.Y + 21f);
									}
									SoundEngine.PlaySound(SoundID.DD2_LightningBugZap with { Volume = 2.25f }, NPC.Center);
								}
								if (FrameTimer > 35)
								{
									NPC.position = nextPodiumLocation;
								}
								if (FrameTimer > 105f)
								{
									if (AOUtils.ServerOrSingleplayer)
									{
										NPC.netUpdate = true;
										FrameTimer = -1;
										CurrentState = moveSelectArrayTwo[Main.rand.Next(3)];
										NPC.ai[2] += 1f; //increment heal cooldown
										if (NPC.ai[2] >= 6f && NPC.life < NPC.lifeMax - 100) //override to heal if cooldown is expended and hp is low enough
										{
											CurrentState = 3;
										}
									}
								}
							}
						}
					}
				}
				else if (CurrentState == 2) //spear throw
				{
					if (FrameTimer >= 40f && FrameTimer < 42f)
					{
						FrameTimer = 42;
						SoundEngine.PlaySound(SoundID.Item1, NPC.Center);
						if (AOUtils.ServerOrSingleplayer)
						{
							NPC.netUpdate = true;
							Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center - (new Vector2(0, 12) * NPC.scale), NPC.SafeDirectionTo(Main.player[NPC.target].Center) * 15f, ModContent.ProjectileType<EliusSpear>(), (int)(NPC.damage * 0.5), 1f, -1, secondphase.ToInt());
						}
					}
					else if (FrameTimer > 70f)
					{
						if (AOUtils.ServerOrSingleplayer)
						{
							NPC.netUpdate = true;
							FrameTimer = -1;
							CurrentState = moveSelectArrayOne[Main.rand.Next(4)];
						}
					}
				}
				else if (CurrentState == 3) //healing
				{
					if (FrameTimer == 0 && (!(NPC.ai[2] > 7f || Main.player[NPC.target].Center.Distance(NPC.Center) > 300f || secondphase)))
					{
						FrameTimer = -1;
						CurrentState = 1;
					}
					if (FrameTimer > 30f)
					{
						NPC.ai[2] = 0f;
						if (AOUtils.ServerOrSingleplayer)
						{
							hptoheal = Main.rand.Next(150) + 50;
							NPC.netUpdate = true;
							NPC.life = Math.Min(NPC.life + hptoheal, NPC.lifeMax);
							NPC.HealEffect(hptoheal);
						}
						if (secondphase)
						{
							if (AOUtils.ServerOrSingleplayer)
							{
								Projectile.NewProjectileDirect(NPC.GetSource_FromThis(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<EliusPlacedExplosion>(), (int)(NPC.damage * 1.5), 1f, -1).timeLeft = 200;
							}
							SoundEngine.PlaySound(SoundID.Thunder, NPC.Center);
						}
						SoundEngine.PlaySound(SoundID.Item3, NPC.Center);
						FrameTimer = -1;
						CurrentState = 0;
						if (NPC.life > NPC.lifeMax)
						{
							NPC.life = NPC.lifeMax;
						}
					}
				}
				else if (CurrentState == 4) //Hop move into sword move
				{
					if (FrameTimer < 2f) //Choose the podium
					{
						if (AOUtils.ServerOrSingleplayer)
						{
							NPC.netUpdate = true;
							tempPodiumID = NPC.ai[3];
							while (tempPodiumID == NPC.ai[3])
							{
								NPC.ai[3] = dashSelectArray[Main.rand.Next(2)];
							}
							FrameTimer = 2;
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
							if (FrameTimer < 70f && FrameTimer > 30f)
							{
								NPC.position.X += (nextPodiumLocation.X - previousPodiumLocation.X) / 39f;
								NPC.position = FindPointInCurve(previousPodiumLocation, nextPodiumLocation, new Vector2((nextPodiumLocation.X + previousPodiumLocation.X) / 2f, (nextPodiumLocation.Y < previousPodiumLocation.Y ? nextPodiumLocation.Y : previousPodiumLocation.Y) - 30), NPC.position.X);
							}

							if (FrameTimer >= 100f) //Break out of this ai cycle
							{
								NPC.position = nextPodiumLocation;
								FrameTimer = -1;
								CurrentState = 5;
							}
						}
						else // Second Phase
						{
							if (FrameTimer < 30f)
							{
								NPC.spriteDirection = nextPodiumLocation.X > previousPodiumLocation.X ? 1 : -1;
								Dust.NewDustDirect(nextPodiumLocation + new Vector2(-10f, 35f), 50, 3, !Main.getGoodWorld ? DustID.WitherLightning : DustID.Firework_Red, 0f, -0.1f).noGravity = true;
							}
							if (FrameTimer >= 30f) //Break out of this ai cycle
							{
								if (FrameTimer == 30f)
								{
									if (AOUtils.ServerOrSingleplayer)
									{
										NPC.netUpdate = true;
										Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<EliusTrail>(), 0, 0f, -1, nextPodiumLocation.X + 10f, nextPodiumLocation.Y + 21f);
									}
									SoundEngine.PlaySound(SoundID.DD2_LightningBugZap with { Volume = 2.25f }, NPC.Center);
								}
								if (FrameTimer > 35f)
								{
									NPC.position = nextPodiumLocation;
								}
								if (FrameTimer > 105f)
								{
									FrameTimer = -1;
									CurrentState = 5;
								}
							}
						}
					}
				}
				else if (CurrentState == 5) //sword move
				{
					if (FrameTimer < 61f && FrameTimer >= 45f)
					{
						NPC.position.Y += 145f / 16f;
					}
					else if (FrameTimer < 446f && FrameTimer >= 122f)
					{
						int swordTiming = !secondphase ? 40 : 24;
						if ((int)FrameTimer % swordTiming == 4 && FrameTimer < 444f)
						{
							NPC.NPCDialogue(Mod.CustomLocalization("ImbueStuff.Exclaim", !secondphase ? this.GetLocalizedValue("FlyingSlashMessage") : Mod.CustomLocalization("ImbueStuff.Space", Imbue.PrettyAttackPrefix.Value, this.GetLocalizedValue("FlyingSlashMessage")).Value).Value, !secondphase ? Color.Gold : Imbue.Colour, false);
							SoundEngine.PlaySound(SoundID.Item1 with { Volume = 2.25f }, NPC.Center);
							if (AOUtils.ServerOrSingleplayer)
							{
								NPC.netUpdate = true;
								Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, new Vector2(NPC.spriteDirection * 20f, 0f), ModContent.ProjectileType<EliusSlash>(), (int)(NPC.damage * 0.5), 1f, -1, secondphase.ToInt());
							}
						}
					}
					else if (FrameTimer < 532f && FrameTimer >= 516f)
					{
						NPC.position.Y -= 145f / 16f;
					}
					else if (FrameTimer > 593f)
					{
						FrameTimer = -1;
						CurrentState = 1;
					}
				}
				else if (CurrentState == 6)  //storm of arrows
				{
					if (FrameTimer > 20f && FrameTimer < 22f)
					{
						NPC.NPCDialogue(Mod.CustomLocalization("ImbueStuff.Exclaim", !secondphase ? this.GetLocalizedValue("StormOfArrowsMessage") : Mod.CustomLocalization("ImbueStuff.Space", Imbue.PrettyAttackPrefix.Value, this.GetLocalizedValue("StormOfArrowsMessage")).Value).Value, !secondphase ? Color.MediumPurple : Imbue.Colour, false);
						SoundEngine.PlaySound(SoundID.Item5, NPC.Center);
						if (AOUtils.ServerOrSingleplayer)
						{
							NPC.netUpdate = true;
							(Projectile.NewProjectileDirect(NPC.GetSource_FromThis(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<EliusArrowStorm>(), (int)(NPC.damage * 0.6), 0f, -1, 0, bowAimPosition.X, bowAimPosition.Y).ModProjectile as EliusArrowStorm).secondphase = secondphase;
						}
						FrameTimer = 22;
					}
					else if (FrameTimer > 50f)
					{
						FrameTimer = -1;
						if (AOUtils.ServerOrSingleplayer)
						{
							NPC.netUpdate = true;
							CurrentState = moveSelectArrayOne[Main.rand.Next(4)];
						}
					}
				}
				FrameTimer++; //increment frame
			}
		}

		public override void FindFrame(int frameHeight)
		{
			if (NPC.HasValidTarget)
			{
				if (CurrentState == 3) //healing
				{
					if (FrameTimer == 0)
					{
						NPC.frame.Y = 0;
					}
					if (FrameTimer == 1)
					{
						NPC.frame.Y = frameHeight * 1;
						NPC.frameCounter = 0;
					}
					if (NPC.frameCounter >= 6)
					{
						NPC.frame.Y += frameHeight;
						NPC.frameCounter = 0;
					}
				}
				else if (CurrentState == 0) //healing end
				{
					if (FrameTimer == 0)
					{
						NPC.frame.Y = frameHeight * 7;
						NPC.frameCounter = 0;
					}
					if (NPC.frameCounter >= 6)
					{
						NPC.frame.Y += frameHeight;
						NPC.frameCounter = 0;
					}
					if (FrameTimer > 15)
					{
						NPC.frame.Y = 0;
					}
				}
				else if (CurrentState == 1 || CurrentState == 4) // dashes
				{
					if (secondphase)
					{
						if (FrameTimer < 16)
						{
							NPC.frame.Y = frameHeight * 10;
							NPC.frameCounter = 0;
						}
						if (FrameTimer < 35 && NPC.frameCounter > 5)
						{
							NPC.frame.Y += frameHeight;
							NPC.frameCounter = 0;
						}
						if (FrameTimer == 35)
						{
							NPC.frame.Y = frameHeight * 17;
							NPC.frameCounter = 0;
						}
						if (FrameTimer > 35 && NPC.frameCounter >= 13)
						{
							NPC.frame.Y += frameHeight;
							NPC.frameCounter = 0;
							if (NPC.frame.Y / frameHeight > 19 || NPC.frame.Y / frameHeight < 15)
							{
								NPC.frame.Y = frameHeight * 0;
							}
						}
					}
					else
					{
						//first phase dash stuff
						if (FrameTimer == 0)
						{
							NPC.frame.Y = frameHeight * 10;
							NPC.frameCounter = 0;
						}
						if (FrameTimer < 30f && FrameTimer > 15 && NPC.frameCounter >= 5f)
						{
							NPC.frame.Y += frameHeight;
							NPC.frameCounter = 0;
						}
						if (FrameTimer < 40 && FrameTimer > 30)
						{
							NPC.frame.Y = frameHeight * 14;
							NPC.frameCounter = 0;
						}
						if (FrameTimer < 60 && FrameTimer > 40)
						{
							NPC.frame.Y = frameHeight * 15;
							NPC.frameCounter = 0;
						}
						if (FrameTimer < 70 && FrameTimer > 60)
						{
							NPC.frame.Y = frameHeight * 16;
							NPC.frameCounter = 0;
						}
						if (FrameTimer == 70)
						{
							NPC.frame.Y = frameHeight * 17;
							NPC.frameCounter = 0;
						}
						if (FrameTimer < 85 && FrameTimer > 70 && NPC.frameCounter >= 5f)
						{
							NPC.frame.Y += frameHeight;
							NPC.frameCounter = 0;
						}
						if (FrameTimer > 85)
						{
							NPC.frame.Y = 0;
							NPC.frameCounter = 0;
						}
					}
				}
				else if (CurrentState == 5) // flying slashes
				{
					if (FrameTimer == 0)
					{
						NPC.frame.Y = 0;
					}
					if (FrameTimer == 44)
					{
						NPC.frame.Y = 20 * frameHeight;
						NPC.frameCounter = 0;
					}
					if (FrameTimer >= 45 && FrameTimer < 61 && NPC.frameCounter >= 8)
					{
						NPC.frame.Y += frameHeight;
						NPC.frameCounter = 0;
					}
					if (FrameTimer == 61)
					{
						NPC.frame.Y = frameHeight * 22;
						NPC.frameCounter = 0;
					}
					if (FrameTimer < 92 && FrameTimer > 61 && NPC.frameCounter >= 5)
					{
						NPC.frame.Y += frameHeight;
						NPC.frameCounter = 0;
					}
					if (FrameTimer == 92)
					{
						NPC.frame.Y = frameHeight * 28;
						NPC.frameCounter = 0;
					}
					if (FrameTimer > 92 && FrameTimer < 122 && NPC.frameCounter >= 5)
					{
						NPC.frame.Y += frameHeight;
						NPC.frameCounter = 0;
					}
					if (FrameTimer > 122 && FrameTimer < 446)
					{
						if (secondphase)
						{
							if (NPC.frameCounter >= 6)
							{
								NPC.frame.Y += frameHeight;
								NPC.frameCounter = 0;
							}
						}
						else
						{
							if (NPC.frameCounter >= 10)
							{
								NPC.frame.Y += frameHeight;
								NPC.frameCounter = 0;
							}
						}
						if (NPC.frame.Y / frameHeight > 41)
						{
							NPC.frame.Y = frameHeight * 34;
						}
					}
					if (FrameTimer == 446) //putting weapons away
					{
						NPC.frame.Y = frameHeight * 33;
						NPC.frameCounter = 0;
					}
					if (FrameTimer > 448 && FrameTimer < 466 && NPC.frameCounter >= 3)
					{
						NPC.frame.Y -= frameHeight;
						NPC.frameCounter = 0;
					}
					if (FrameTimer == 495) // starting to rise up prep
					{
						NPC.frame.Y = frameHeight * 42;
						NPC.frameCounter = 0;
					}
					if (FrameTimer > 495 && FrameTimer < 516 && NPC.frameCounter >= 5) //rising up prep
					{
						NPC.frame.Y += frameHeight;
						NPC.frameCounter = 0;
					}
					if (FrameTimer == 516) //starting rise up
					{
						NPC.frame.Y = frameHeight * 46;
						NPC.frameCounter = 0;
					}
					if (FrameTimer > 516 && FrameTimer < 532) //rising up
					{
						if (FrameTimer < 520)
						{
							NPC.frame.Y = frameHeight * 46;
							NPC.frameCounter = 0;
						}
						else if (FrameTimer < 528)
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
					if (FrameTimer == 532)
					{
						NPC.frame.Y = frameHeight * 49;
						NPC.frameCounter = 0;
					}
					if (FrameTimer > 532 && FrameTimer < 552 && NPC.frameCounter >= 5)
					{
						NPC.frame.Y += frameHeight;
						NPC.frameCounter = 0;
					}
					if (FrameTimer > 557) //completing
					{
						NPC.frame.Y = 0;
						NPC.frameCounter = 0;
					}
				}
				else if (CurrentState == 6) //storm of arrows
				{
					NPC.frame.Y = 64 * frameHeight;
					if (FrameTimer < 22f)
					{
						if (NPC.frameCounter >= 5)
						{
							NPC.frameCounter = 0;
							if (bowArmFrame < 2)
							{
								bowArmFrame++;
							}
							if (bowFrame < 3)
							{
								bowFrame++;
							}
						}
					}
					else
					{
						bowFrame = 2;
					}
				}
				else if (CurrentState == 2) //spear throw
				{
					NPC.frame.Y = 59 * frameHeight;

					if (NPC.frameCounter >= 5)
					{
						NPC.frameCounter = 0;
						if (FrameTimer < 40)
						{
							if (spearArmFrame < 3)
							{
								spearArmFrame++;
							}
						}
						else
						{
							if (spearArmFrame < 6)
							{
								spearArmFrame++;
							}
						}
					}
				}
				else if (!Sparing)
				{
					// IF NOT IN THE SELECTED BEHAVIORS
					NPC.frame.Y = 0;
					NPC.frameCounter = 0;
					spearArmFrame = 0;
					bowArmFrame = 0;
					bowFrame = 0;
				}
			}
			else if (!Sparing)
			{
				NPC.frame.Y = 0;
			}
			NPC.frameCounter++;
			if (CurrentState != 2 && CurrentState != 6)
			{
				spearArmFrame = 0;
				bowArmFrame = 0;
				bowFrame = 0;
			}
			if (Sparing)
			{
				if (NPC.velocity.Y == 0)
				{
					if (ChildSafety.Disabled)
					{
						if (NPC.frame.Y < frameHeight * 65)
						{
							NPC.frame.Y = frameHeight * 65;
						}
						if (NPC.frameCounter >= 5)
						{
							NPC.frameCounter = 0;
							if (CurrentState != -3)
							{
								if (NPC.frame.Y < frameHeight * 74)
								{
									NPC.frame.Y += frameHeight;
								}
							}
							else
							{
								if (NPC.frame.Y < frameHeight * 76)
								{
									spareTimer = 60;
									NPC.frame.Y += frameHeight;
								}
							}
						}
					}
					else
					{
						if (NPC.frame.Y < frameHeight * 77)
						{
							NPC.frame.Y = frameHeight * 77;
						}
						if (NPC.frameCounter >= 5)
						{
							NPC.frameCounter = 0;
							if (CurrentState != -3)
							{
								if (NPC.frame.Y < frameHeight * 86)
								{
									NPC.frame.Y += frameHeight;
								}
							}
							else
							{
								if (NPC.frame.Y < frameHeight * 88)
								{
									spareTimer = 60;
									NPC.frame.Y += frameHeight;
								}
							}
						}
					}
				}
				else
				{
					if (ChildSafety.Disabled)
						NPC.frame.Y = frameHeight * 65;
					else
						NPC.frame.Y = frameHeight * 77;
				}
			}
		}

		private Vector2 bowAimPosition => new(Main.player[NPC.target].Center.X, Main.player[NPC.target].Center.Y - 600f);
		private static Asset<Texture2D> backBowArmTexture, spearArmTexture, frontBowArmTexture, bowTexture;
		private static Texture2D SpearTexture => ModContent.GetInstance<EliusSpear>().Sprite;

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			var dir = NPC.spriteDirection != 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			if (CurrentState == 6 && NPC.HasValidTarget) //bow rendering
			{
				var frame = backBowArmTexture.Frame(1, 3, 0, bowArmFrame);
				spriteBatch.Draw(backBowArmTexture.Value, NPC.Center - screenPos, frame, NPC.GetAlpha(drawColor), NPC.Center.AngleTo(bowAimPosition), frame.Size() / 2f, NPC.scale, dir, 0f);
			}
			return true;
		}

		public override void Load()
		{
			backBowArmTexture = ModContent.Request<Texture2D>(Texture + "BowBackArm");
			frontBowArmTexture = ModContent.Request<Texture2D>(Texture + "BowFrontArm");
			bowTexture = ModContent.Request<Texture2D>(Texture + "Bow");
			spearArmTexture = ModContent.Request<Texture2D>(Texture + "SpearThrowArm");
		}

		private int spearArmFrame, bowArmFrame, bowFrame;

		public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			var dir = NPC.spriteDirection != 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			if (CurrentState == 2 && NPC.HasValidTarget) //spear rendering and spear arm rendering
			{
				if (FrameTimer < 40)
				{
					spriteBatch.Draw(SpearTexture, NPC.Center - screenPos - (new Vector2(0, 12) * NPC.scale), null, drawColor, NPC.Center.AngleTo(Main.player[NPC.target].Center) + (MathHelper.PiOver4 * -NPC.spriteDirection) + (NPC.spriteDirection == 1 ? MathF.PI : 0), SpearTexture.Size() / 2f, NPC.scale, dir, 0f);
				}
				var spearframe = spearArmTexture.Frame(1, 7, 0, spearArmFrame);
				spriteBatch.Draw(spearArmTexture.Value, NPC.Center - screenPos, spearframe, NPC.GetAlpha(drawColor), NPC.Center.AngleTo(Main.player[NPC.target].Center) + (NPC.spriteDirection == -1 ? MathF.PI : 0), spearframe.Size() / 2f, NPC.scale, dir, 1f);

			}
			if (CurrentState == 6 && NPC.HasValidTarget) //bow rendering
			{
				var bowframe = bowTexture.Frame(1, 4, 0, bowFrame);
				spriteBatch.Draw(bowTexture.Value, NPC.Center - screenPos, bowframe, NPC.GetAlpha(drawColor), NPC.Center.AngleTo(bowAimPosition) + (NPC.spriteDirection == -1 ? MathF.PI : 0), bowframe.Size() / 2f, NPC.scale, dir, 0f);
				var armframe = frontBowArmTexture.Frame(1, 3, 0, bowArmFrame);
				spriteBatch.Draw(frontBowArmTexture.Value, NPC.Center - screenPos - (new Vector2(5, 0) * NPC.spriteDirection * NPC.scale), armframe, NPC.GetAlpha(drawColor), NPC.Center.AngleTo(bowAimPosition) + (NPC.spriteDirection == -1 ? MathF.PI : 0), armframe.Size() / 2f, NPC.scale, dir, 0f);
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
			if (EliusSpareSystem.spared)
			{
				Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<EliusTrail>(), 0, 0f, -1, NPC.Center.X, NPC.Center.Y - 1000f);
			}
		}

		public static void SpawnGore(NPC npc)
		{
			Gore.NewGore(npc.GetSource_Death(), npc.Top, npc.velocity, ModContent.GoreType<EliusHead>());
			Gore.NewGore(npc.GetSource_Death(), npc.Left, npc.velocity, ModContent.GoreType<EliusLeftArm>());
			Gore.NewGore(npc.GetSource_Death(), npc.Right, npc.velocity, ModContent.GoreType<EliusRightArm>());
			Gore.NewGore(npc.GetSource_Death(), npc.Center, npc.velocity, ModContent.GoreType<EliusTorso>());
			Gore.NewGore(npc.GetSource_Death(), npc.BottomLeft, npc.velocity, ModContent.GoreType<EliusLeg>());
			Gore.NewGore(npc.GetSource_Death(), npc.BottomRight, npc.velocity, ModContent.GoreType<EliusLeg>());
			for (int n = 0; n < 15; n++)
			{
				Dust.NewDust(npc.Center, 0, 0, DustID.Blood, (Main.rand.NextFloat() - 0.5f) * 3f, (Main.rand.NextFloat() - 0.5f) * 8f);
			}
		}

		public bool Sparing
		{
			get
			{
				return CurrentState == -2 || CurrentState == -3;
			}
			set
			{
				if (value)
					CurrentState = -2;
				else
					CurrentState = -1;
			}
		}

		public override bool CheckDead()
		{
			if (DownedBosses.DownedElius)
			{
				return true;
			}
			Sparing = true;
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

		public override bool CanChat() => Sparing && FrameTimer != -3;

		public override void SetChatButtons(ref string button, ref string button2)
		{
			button = Mod.CustomLocalization("RandomWords.Kill").Value;
			button2 = Mod.CustomLocalization("RandomWords.Spare").Value;
		}

		public override string GetChat() => this.GetLocalizedValue("DoomMessage");

		public override bool CanGoToStatue(bool toKingStatue) => false;

		public override bool CheckActive() => false;

		public override void OnChatButtonClicked(bool firstButton, ref string shopName)
		{
			Main.CloseNPCChatOrSign();
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
					NPC.active = false;
					NPC.NPCLoot();
				}
				else
				{
					CurrentState = -3;
				}
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
							SoundEngine.PlaySound(SoundID.DD2_LightningBugZap with { Volume = 2.25f }, NPC.Center);
						}
						else
						{
							SpawnGore(NPC);
						}
					}
				}
			}
		}

		public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
		{
			NPC.lifeMax = (int)(NPC.lifeMax * balance * bossAdjustment);
		}

		private static Vector2 FindPointInCurve(Vector2 pointOne, Vector2 pointTwo, Vector2 pointThree, float xPos)
		{
			//via https://en.wikipedia.org/wiki/Newton_polynomial
			float coEfOne = (pointTwo.Y - pointOne.Y) / (pointTwo.X - pointOne.X);
			float coEfTwo = (((pointThree.Y - pointTwo.Y) / (pointThree.X - pointTwo.X)) - coEfOne) / (pointThree.X - pointOne.X);
			float yPos = pointOne.Y + coEfOne * (xPos - pointOne.X) + coEfTwo * (xPos - pointOne.X) * (xPos - pointTwo.X);
			return new Vector2(xPos, yPos);
		}
	}
}
