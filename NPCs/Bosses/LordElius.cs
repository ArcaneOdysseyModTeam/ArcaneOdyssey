using ArcaneOdyssey.Biomes;
using ArcaneOdyssey.Imbues.Relics;
using ArcaneOdyssey.Items.Armour.RavennaNoble;
using ArcaneOdyssey.Items.BossBags;
using ArcaneOdyssey.Items.BossRelics;
using ArcaneOdyssey.Items.BossTrophies;
using ArcaneOdyssey.Items.Equipment.Pets;
using ArcaneOdyssey.Items.Weapons.RavennaNoble;
using ArcaneOdysseyMusic;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using ArcaneOdyssey.Projectiles.Enemies;
using ArcaneOdyssey.Gores;
using Terraria.Audio;

namespace ArcaneOdyssey.NPCs.Bosses
{
	[AutoloadBossHead]
	public class LordElius : BaseNPC
	{
		private int hptoheal;
		private float tempPodiumID;
		private Vector2 previousPodiumLocation,nextPodiumLocation;
		private float[] dashSelectArray = {0f,4f};
		private float[] moveSelectArrayOne = {1f,4f,1f,1f};
		private float[] moveSelectArrayTwo = {2f,2f,6f};
		private Vector2[] podiumPos = [new(-665f,16f),new(-320f,0f),new(0f,0f),new(366f,0f),new(686f,16f)];
		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[NPC.type] = 1;
			NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new() { Direction = 1 };
			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
			NPCID.Sets.NoTownNPCHappiness[Type] = true;
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange([
				new FlavorTextBestiaryInfoElement($"Mods.{Mod.Name}.Bestiary.{Name}")
			]);
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			NPC.lifeMax = 3000;
			NPC.noGravity = true;
			NPC.noTileCollide = true;
			NPC.damage = 0;
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

		public override MusicTrack Theme => MusicTrack.Elius;

		public bool sentMessage = false;
		private bool hasSetSpawnLocation = false;
		public Vector2 spawnLocation;

		private int spareTimer = 60 * 60; // 1 minute

		public override void AI()
		{
			if(NPC.life < NPC.lifeMax/2)
			{
				if(NPC.localAI[0] < 1f)
				{
					NPC.NPCDialogue(this.GetLocalizedValue("SecondPhaseMessage"), Color.MediumPurple);
				}
				NPC.localAI[0] = 1f;
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
					Main.NewText(this.GetLocalizedValue("Spared"), SpiritEnergy.Instance.Colour);
					NPC.active = false;
					NPC.netUpdate = true;
					NPC.NPCLoot();
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
				NPC.localAI[0] = 0f;
			}
			else if (!NPC.Hitbox.Intersects(EliusArenaLoader.eliusArena.ToWorldRect()))
			{
				NPC.position = spawnLocation;
			}

			
			NPC.spriteDirection = (NPC.SafeDirectionTo(Main.player[Player.FindClosest(NPC.position, NPC.width, NPC.height)].Center).X > 0).ToDirectionInt();
			NPC.TargetClosest();
			if(NPC.localAI[0] > 0f && Main.GameUpdateCount % 300 == 0)
			{
				Projectile.NewProjectile(NPC.GetSource_FromThis(),Main.player[NPC.target].Center,Vector2.Zero,ModContent.ProjectileType<EliusPlacedExplosion>(),0,0f,-1);
			}

			// State Machine
			// ai[1] is the state frame, ai[0] is the state ID, ai[2] is the healing timer, and should not bee touched, ai[3] is extra numerical data
			if (NPC.ai[0] == -1) //Spawn In
			{
				if(NPC.ai[1] > 120f)
				{
					NPC.ai[1] = -1f;
					NPC.ai[0] = 1;
					NPC.ai[3] = 2;
				}
			}
			else if (NPC.ai[0] == 0)
			{
				if(NPC.ai[1] > 120f)
				{

					NPC.ai[1] = -1f;
					NPC.ai[0] = 1;
				}
			}
			else if(NPC.ai[0] == 1) //Hop move
			{
				if(NPC.ai[1] < 2f) //Choose the podium
				{
					tempPodiumID = NPC.ai[3];
					while(tempPodiumID == NPC.ai[3]) 
					{
						NPC.ai[3] = (float)Main.rand.Next(5);
					}
					NPC.ai[1] = 2f;
					previousPodiumLocation = NPC.position;
					nextPodiumLocation = spawnLocation+podiumPos[(int)NPC.ai[3]];
				} else //prevent skipping to the next parts
				{
					if(NPC.localAI[0] < 1f)
					{
						if(NPC.ai[1] < 20f) //Rise
						{
							NPC.spriteDirection = nextPodiumLocation.X > previousPodiumLocation.X? 1 : -1;
							NPC.position.Y-= 3f;
						} else if(NPC.ai[1] <52f) //Dash
						{
							NPC.spriteDirection = nextPodiumLocation.X > previousPodiumLocation.X? 1 : -1;
							NPC.position.X += (nextPodiumLocation.X - previousPodiumLocation.X) / 32f;
						}
						if (NPC.ai[1] > 53f && NPC.position.Y < nextPodiumLocation.Y) //Fall
						{
							NPC.position.Y += 4f;
						}
						if (NPC.ai[1] >= 80f) //Break out of this ai cycle
						{
							NPC.position = nextPodiumLocation;
							NPC.ai[1] = -1f;
							NPC.ai[1] = -1f;
							NPC.ai[0] = moveSelectArrayTwo[Main.rand.Next(3)];
							NPC.ai[2]+=1f; //increment heal cooldown
							if(NPC.ai[2] >= 6f && NPC.life < NPC.lifeMax-100) //override to heal if cooldown is expended and hp is low enough
							{
								NPC.ai[0] = 3f;
							}
						}
					} else //phase 2
					{
						if(NPC.ai[1] < 50f)
						{
							NPC.spriteDirection = nextPodiumLocation.X > previousPodiumLocation.X? 1 : -1;
							Dust.NewDustDirect(nextPodiumLocation+new Vector2(-10f,35f),50,3,DustID.WitherLightning,0f,-0.1f).noGravity = true;
						}
						if (NPC.ai[1] >= 50f) //Break out of this ai cycle
						{
							if(NPC.ai[1] == 50f)
							{
								Projectile.NewProjectile(NPC.GetSource_FromThis(),NPC.Center,Vector2.Zero,ModContent.ProjectileType<EliusTrail>(),0,0f,-1,nextPodiumLocation.X+10f,nextPodiumLocation.Y+21f);
								SoundEngine.PlaySound(SoundID.DD2_LightningBugZap with { Volume = 2.25f },NPC.Center);
							}
							if(NPC.ai[1] > 55f)
							{
								NPC.position = nextPodiumLocation;
								NPC.ai[1] = -1f;
								NPC.ai[1] = -1f;
								NPC.ai[0] = moveSelectArrayTwo[Main.rand.Next(3)];
								NPC.ai[2]+=1f; //increment heal cooldown
								if(NPC.ai[2] >= 6f && NPC.life < NPC.lifeMax-100) //override to heal if cooldown is expended and hp is low enough
								{
									NPC.ai[0] = 3f;
								}
							}
						}
					}
				}
			} 
			else if(NPC.ai[0] == 2) //spear throw
			{
				if(NPC.ai[1] < 40f)
				{
					//charge vfx goes here
				} else if (NPC.ai[1] < 42f)
				{
					NPC.ai[1] = 42f;
					SoundEngine.PlaySound(SoundID.Item1,NPC.Center);
					Projectile attackProj = Projectile.NewProjectileDirect(NPC.GetSource_FromThis(),NPC.Center,(Main.player[NPC.target].Center - NPC.Center).SafeNormalize()*15f,ModContent.ProjectileType<EliusSpear>(),15,1f,-1);
					if (NPC.localAI[0] > 0f)
					{
						attackProj.localAI[0] = 1f;
					}
				} else if (NPC.ai[1] > 70f)
				{
					NPC.ai[1] = -1f;
					NPC.ai[0] = moveSelectArrayOne[Main.rand.Next(4)];
				}
			} 
			else if (NPC.ai[0] == 3) //healing
			{
				if(NPC.ai[1] > 30f)
				{
					if (NPC.ai[2] > 7f || Main.player[NPC.target].Center.Distance(NPC.Center) > 300f || NPC.localAI[0] > 0f) {
						NPC.ai[2] = 0f;
						hptoheal = (int)(Main.rand.Next(150)+50);
						NPC.life += hptoheal;
						if(NPC.localAI[0] > 0f)
						{
							Projectile.NewProjectile(NPC.GetSource_FromThis(),NPC.Center+new Vector2(0,-1000),Vector2.Zero,ModContent.ProjectileType<EliusTrail>(),0,0f,-1,NPC.Center.X,NPC.Center.Y);
							Projectile.NewProjectile(NPC.GetSource_FromThis(),NPC.Center,Vector2.Zero,ModContent.ProjectileType<EliusExplosion>(),30,1f,-1);
							SoundEngine.PlaySound(SoundID.Thunder,NPC.Center);
						}
						CombatText.NewText(new Rectangle((int)NPC.position.X,(int)NPC.position.Y,0,0),CombatText.HealLife,hptoheal,false,false);
						Gore.NewGorePerfect(NPC.GetSource_FromThis(),NPC.Center,new Vector2(NPC.spriteDirection*5f,-1f),ModContent.GoreType<EmptyHealthPotion>(),1f);
						SoundEngine.PlaySound(SoundID.Item3,NPC.Center);
						NPC.ai[1] = -1f;
						NPC.ai[0] = 0;
						if(NPC.life > NPC.lifeMax)
						{
							NPC.life = NPC.lifeMax;
						}
						} else
						{
							NPC.ai[1] = -1f;
							NPC.ai[0] = 1;
						}
				}
			} 
			else if(NPC.ai[0] == 4) //Hop move into sword move
			{
				if(NPC.ai[1] < 2f) //Choose the podium
				{
					tempPodiumID = NPC.ai[3];
					while(tempPodiumID == NPC.ai[3]) 
					{
						NPC.ai[3] = dashSelectArray[Main.rand.Next(2)];
					}
					NPC.ai[1] = 2f;
					previousPodiumLocation = NPC.position;
					nextPodiumLocation = spawnLocation+podiumPos[(int)NPC.ai[3]];
				} else //prevent skipping to the next parts
				{
					if(NPC.localAI[0] < 1f)
					{
						if(NPC.ai[1] < 20f) //Rise
						{
							NPC.spriteDirection = nextPodiumLocation.X > previousPodiumLocation.X? 1 : -1;
							NPC.position.Y-= 3f;
						} else if(NPC.ai[1] <52f) //Dash
						{
							NPC.spriteDirection = nextPodiumLocation.X > previousPodiumLocation.X? 1 : -1;
							NPC.position.X += (nextPodiumLocation.X - previousPodiumLocation.X) / 32f;
						}
						if (NPC.ai[1] > 53f && NPC.position.Y < nextPodiumLocation.Y) //Fall
						{
							NPC.position.Y += 4f;
						}
						if (NPC.ai[1] >= 80f) //Break out of this ai cycle
						{
							NPC.position = nextPodiumLocation;
							NPC.ai[1] = -1f;
							NPC.ai[0] = 5;
						}
					} else // Second Phase
					{
						if(NPC.ai[1] < 50f)
						{
							NPC.spriteDirection = nextPodiumLocation.X > previousPodiumLocation.X? 1 : -1;
							Dust.NewDustDirect(nextPodiumLocation+new Vector2(-10f,35f),50,3,DustID.WitherLightning,0f,-0.1f).noGravity = true;
						}
						if (NPC.ai[1] >= 50f) //Break out of this ai cycle
						{
							if(NPC.ai[1] == 50f)
							{
								Projectile.NewProjectile(NPC.GetSource_FromThis(),NPC.Center,Vector2.Zero,ModContent.ProjectileType<EliusTrail>(),0,0f,-1,nextPodiumLocation.X+10f,nextPodiumLocation.Y+21f);
								SoundEngine.PlaySound(SoundID.DD2_LightningBugZap with { Volume = 2.25f },NPC.Center);
							}
							if(NPC.ai[1] > 55f) 
							{
								NPC.position = nextPodiumLocation;
							}
							if(NPC.ai[1] > 85f)
							{
								NPC.ai[1] = -1f;
								NPC.ai[0] = 5;
							}
						}
					}
				}
			} 
			else if(NPC.ai[0] == 5) //sword move
			{
				if(NPC.ai[1] < 16f)
				{
					NPC.position.Y += 128f/15f;
				} else if(NPC.ai[1] < 300f)
				{
					int swordTiming = NPC.localAI[0]<1f ? 60 : 40;
					if((int)NPC.ai[1]%swordTiming == 0)
					{
						NPC.NPCDialogue(NPC.localAI[0]<1f ? "" : this.GetLocalizedValue("MoveElementName") + this.GetLocalizedValue("FlyingSlashMessage"), NPC.localAI[0]<1f ? Color.Gold : Color.MediumPurple);
						SoundEngine.PlaySound(SoundID.Item1 with { Volume = 2.25f },NPC.Center);
						Projectile attackProj = Projectile.NewProjectileDirect(NPC.GetSource_FromThis(),NPC.Center,new Vector2(NPC.spriteDirection*20f,0f),ModContent.ProjectileType<EliusSlash>(),16,1f,-1);
						if (NPC.localAI[0] > 0f)
						{
							attackProj.localAI[0] = 1f;
						}
					}
				} else if(NPC.ai[1] < 316f)
				{
					NPC.position.Y -= 128f/15f;
				} else if(NPC.ai[1] > 360f)
				{
					NPC.ai[1] = -1f;
					NPC.ai[0] = 1;
				}
			} 
			else if(NPC.ai[0] == 6) //storm of arrows
			{
				if(NPC.ai[1] > 20f && NPC.ai[1] < 22f)
				{
					NPC.NPCDialogue(NPC.localAI[0]<1f ? "" : this.GetLocalizedValue("MoveElementName") + this.GetLocalizedValue("StormOfArrowsMessage"), Color.MediumPurple);
					SoundEngine.PlaySound(SoundID.Item5,NPC.Center);
					Projectile attackProj = Projectile.NewProjectileDirect(NPC.GetSource_FromThis(),NPC.Center,Vector2.Zero,ModContent.ProjectileType<EliusArrowStorm>(),20,0f,-1,0f,Main.player[NPC.target].Center.X,Main.player[NPC.target].Center.Y - 600f);
					if (NPC.localAI[0] > 0f)
					{
						attackProj.localAI[0] = 1f;
					}
					NPC.ai[1] = 22f;
				} else if(NPC.ai[1] > 50f)
				{
					NPC.ai[1] = -1f;
					NPC.ai[0] = moveSelectArrayOne[Main.rand.Next(4)];
				}
			}
			NPC.ai[1]+= 1f; //increment frame
			
			
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
					ModContent.ItemType<ScimitarofStorm>(),
					ModContent.ItemType<StormCaller>()
					], rolls: 2));
			npcLoot.Add(leadingConditionRule1);
			npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<EliusBossBag>()));
			leadingConditionRule1 = new(new Mastvengence());
			leadingConditionRule1.OnSuccess(AOUtils.Common<EliusBossRelic>());
			leadingConditionRule1.OnSuccess(AOUtils.Common<VermillionBracelet>(4));
			npcLoot.Add(leadingConditionRule1);
		}

		public override void OnKill()
		{
			Main.windSpeedTarget = -.1f;
			DownedBosses.DownedElius = true;
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

		public override void OnGoToStatue(bool toKingStatue)
		{
			NPC.position = NPC.oldPosition;
		}

		public override bool CanGoToStatue(bool toKingStatue) => true;

		public override bool CheckActive() => !sparing;

		public override bool UsesPartyHat() => false;

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
	}
}
