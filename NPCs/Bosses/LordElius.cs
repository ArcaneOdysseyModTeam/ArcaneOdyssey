using ArcaneOdyssey.Biomes;
using ArcaneOdyssey.Imbues.Relics;
using ArcaneOdyssey.Items.Armour.RavennaNoble;
using ArcaneOdyssey.Items.BossBags;
using ArcaneOdyssey.Items.BossRelics;
using ArcaneOdyssey.Items.BossTrophies;
using ArcaneOdyssey.Items.Equipment.Pets;
using ArcaneOdyssey.Items.Weapons.RavennaNoble;
using ArcaneOdysseyMusic;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;

namespace ArcaneOdyssey.NPCs.Bosses
{
	[AutoloadBossHead]
	public class LordElius : BaseNPC
	{
		private int hptoheal;
		private Vector2 previousLocation;
		private Vector2[] podiumPos = [new(-660f,16f),new(-330f,0f),new(0*16f,0f),new(376f,0f),new(696f,16f)];
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
				spawnLocation = NPC.Center;
				hasSetSpawnLocation = true;
				NPC.ai[0] = 1f;
				NPC.ai[1] = 0f;
				NPC.ai[2] = 0f;
			}
			else if (!NPC.Hitbox.Intersects(EliusArenaLoader.eliusArena.ToWorldRect()))
			{
				NPC.position = spawnLocation;
			}

			NPC.spriteDirection = (NPC.SafeDirectionTo(Main.player[Player.FindClosest(NPC.position, NPC.width, NPC.height)].Center).X > 0).ToDirectionInt();
			NPC.TargetClosest();

			
			if(NPC.life > NPC.lifeMax / 2) //prevents healing right when he gets to half
			{
				NPC.ai[2] = 0f;
			}
			if((NPC.life < NPC.lifeMax/2)&&NPC.ai[2]>=5000f) //healing
			{
				NPC.ai[2] = 0f;
				hptoheal = (int)(Main.rand.Next(150)+50);
				NPC.life += hptoheal;
				CombatText.NewText(new Rectangle((int)NPC.position.X,(int)NPC.position.Y,0,0),CombatText.HealLife,hptoheal,false,false);
			}
			// State Machine
			// ai[1] is the state frame, ai[0] is the state ID
			NPC.ai[2] += 1f;
			if (NPC.ai[0] == 1) //storm of arrows
			{
				if (NPC.ai[1] < 2f)
				{
					//Main.NewText("Storm of arrows or something idk");
					NPC.ai[1] = 2f;
				}
				if (NPC.ai[1] > 60f)
				{
					NPC.ai[1] = 0f;
					NPC.ai[0] = Main.rand.Next(4) + 1f;
					//Main.NewText(NPC.ai[0]);
				}
			}
			else if (NPC.ai[0] == 2) //podium jump
			{
				if (NPC.ai[1] < 2f)
				{
					//NPC.Center += new Vector2(Main.rand.NextFloat(-10f, 10f), Main.rand.NextFloat(-10f, 10f)); //lmao hes just leaving fuck you
					NPC.Center = spawnLocation + podiumPos[Main.rand.Next(5)];
					NPC.ai[1] = 2f;
				}
				if (NPC.ai[1] > 60f)
				{
					NPC.ai[1] = 0f;
					NPC.ai[0] = Main.rand.Next(4) + 1f;
					//Main.NewText(NPC.ai[0]);
				}
			} else if (NPC.ai[0] == 3) //spear throw
			{
				if(NPC.ai[1] < 2f)
				{
					previousLocation = NPC.position;
					NPC.ai[1] = 2f;
				}
				if (NPC.ai[1] < 20f && NPC.ai[1] > 2f)
				{
					//movement
					
				}
				if (NPC.ai[1] > 20f && NPC.ai[1] < 22f)
				{
					
					//throw
					NPC.ai[1] = 30f;
				}
				if (NPC.ai[1] > 60f)
				{
					NPC.position = previousLocation;
					NPC.ai[1] = 0f;
					NPC.ai[0] = Main.rand.Next(4) + 1f;
					if(NPC.ai[0] == 3f) {NPC.ai[0] = 2f;}
					//Main.NewText(NPC.ai[0]);
				}
			} else if (NPC.ai[0] == 4) //twin crecents
			{
				if (NPC.ai[1] < 2f)
				{
					//NPC.Center += new Vector2(Main.rand.NextFloat(-10f, 10f), Main.rand.NextFloat(-10f, 10f)); //lmao hes just leaving fuck you
					
					NPC.ai[1] = 2f;
				}
				if (NPC.ai[1] > 60f)
				{
					NPC.ai[1] = 0f;
					NPC.ai[0] = Main.rand.Next(4) + 1f;
					//Main.NewText(NPC.ai[0]);
				}
			}
			NPC.ai[1] += 1f;
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
