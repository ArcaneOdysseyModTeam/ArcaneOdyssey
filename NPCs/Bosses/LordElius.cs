using ArcaneOdyssey.Biomes;
using ArcaneOdyssey.Imbues.Relics;
using ArcaneOdyssey.Items.Armour.RavennaNoble;
using ArcaneOdyssey.Items.BossBags;
using ArcaneOdyssey.Items.BossRelics;
using ArcaneOdyssey.Items.BossTrophies;
using ArcaneOdyssey.Items.Equipment.Pets;
using ArcaneOdyssey.Items.Weapons.RavennaNoble;
using ArcaneOdysseyMusic;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Chat;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.NPCs.Bosses
{
	[AutoloadBossHead]
	public class LordElius : BaseNPC
	{
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
		}

		public override MusicTrack Theme => MusicTrack.Elius;

		public bool sentMessage = false;
		private bool hasSetSpawnLocation = false;
		public Vector2 spawnLocation;
		
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
					NPC.NPCDialogue(Mod.CustomLocalization(LocalizationCategory + "." + Name + ".DoomMessage").Value, Color.MediumPurple);
					sentMessage = true;
				}
				return;
			}

			if (!sentMessage)
			{
				if (!Main.dedServ)
				{
					if (DownedBosses.downedElius)
					{
						if (!Main.LocalPlayer.ArcaneOdyssey().evil)
						{
							NPC.NPCDialogue(Mod.CustomLocalization(LocalizationCategory + "." + Name + ".Refight").Value, Color.MediumPurple);
						}
						else
						{
							NPC.Opacity = .5f;
						}
					}
					else
					{
						NPC.NPCDialogue(Mod.CustomLocalization(LocalizationCategory + "." + Name + ".SpawnMessage").Value, Color.MediumPurple);
					}
				}
				sentMessage = true;
			}
			
			if (!hasSetSpawnLocation) //this also is used for setup
			{
				NPC.Center = new Vector2((EliusArenaLoader.eliusArena.Center.X + 25) * 16, (EliusArenaLoader.eliusArena.Center.Y + 2) * 16);
				// end test specific
				NPC.Center = NPC.Center - new Vector2(0,32);
				Main.NewText("Test: Elius location set");
				spawnLocation = NPC.position;
				hasSetSpawnLocation = true;
				NPC.ai[0] = 1f;
				NPC.ai[1] = 0f;
			}

			NPC.spriteDirection = (NPC.SafeDirectionTo(Main.player[Player.FindClosest(NPC.position, NPC.width, NPC.height)].Center).X > 0).ToDirectionInt();

			// ai here, red
			if (NPC.ai[0] == 1)
			{
				if (NPC.ai[1] < 2f)
				{
					Main.NewText("Storm of arrows or something idk");
					NPC.ai[1] = 2f;
				}
				if (NPC.ai[1] > 60f)
				{
					NPC.ai[1] = 0f;
					NPC.ai[0] = MathF.Round(Main.rand.NextFloat()*1)+1;
					Main.NewText(NPC.ai[0]);
				}
			} else if (NPC.ai[0] == 2)
			{
				if (NPC.ai[1] < 2f)
				{
					NPC.Center += new Vector2(Main.rand.Next(-100, 100)); //lmao hes just leaving fuck you
					NPC.ai[1] = 2f;
				}
				if (NPC.ai[1] > 60f)
				{
					NPC.ai[1] = 0f;
					NPC.ai[0] = MathF.Round(Main.rand.NextFloat()*1)+1;
					Main.NewText(NPC.ai[0]);
				}
			}
			NPC.ai[1]+=1f;
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(AOUtils.Common<EliusTrophy>(10));
			LeadingConditionRule leadingConditionRule1 = new(new Conditions.NotExpert());
			leadingConditionRule1.OnSuccess(AnyDropHelper.Create(
					ModContent.ItemType<EliusBoots>(),
					ModContent.ItemType<EliusChest>(),
					ModContent.ItemType<EliusHelm>(),
					ModContent.ItemType<NobleThunderspear>(),
					ModContent.ItemType<ScimitarofStorm>(),
					ModContent.ItemType<StormCaller>()
					));
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
			if (!DownedBosses.downedElius)
			{
				if (justKilled) // kill
				{
					// gore goes here
					for (int n = 0; n < 17; n++)
					{
						Dust.NewDust(new Vector2(NPC.position.X + (NPC.width / 2f), NPC.position.Y + (NPC.height / 2f)), 1, 1, DustID.Blood, (Main.rand.NextFloat() - 0.5f) * 3f, (Main.rand.NextFloat() - 0.5f) * 8f);
					}
					ChatHelper.BroadcastChatMessage(Mod.CustomLocalization(LocalizationCategory + "." + Name + ".Killed").ToNetworkText(), Color.Purple);
				}
				else
				{
					for (int n = 0; n < 17; n++)
					{
						Dust.NewDust(new Vector2(NPC.position.X + (NPC.width / 2f), NPC.position.Y + (NPC.height / 2f)), 1, 1, DustID.Smoke, (Main.rand.NextFloat() - 0.5f) * 3f, (Main.rand.NextFloat() - 0.5f) * 8f, 255 / 2);
					}
					ChatHelper.BroadcastChatMessage(Mod.CustomLocalization(LocalizationCategory + "." + Name + ".Spared").ToNetworkText(), new(0, 183, 255));
				}
			}
			DownedBosses.downedElius = true;
			if (Main.dedServ)
			{
				NetMessage.SendData(MessageID.WorldData);
			}
		}

		// probably not needed
		//public override void SendExtraAI(BinaryWriter writer)
		//{
		//	writer.Write(sparing);
		//}

		//public override void ReceiveExtraAI(BinaryReader reader)
		//{
		//	sparing = reader.ReadBoolean();
		//}


		public bool sparing = false;

		public override bool CheckDead()
		{
			if (DownedBosses.downedElius)
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

		public override string GetChat()
		{
			return Mod.CustomLocalization(LocalizationCategory + "." + Name + ".DoomMessage").Value;
		}

		public override void OnGoToStatue(bool toKingStatue)
		{
			NPC.Center = new((EliusArenaLoader.eliusArena.Center.X + 25) * 16f, (EliusArenaLoader.eliusArena.Center.Y + 2) * 16f);
		}

		public override bool CanGoToStatue(bool toKingStatue) => true;

		public override bool CheckActive() => !sparing;

		public override bool UsesPartyHat() => false;

		public bool justKilled;

		public override void OnChatButtonClicked(bool firstButton, ref string shopName)
		{
			foreach (var player in Main.ActivePlayers)
			{
				player.ArcaneOdyssey().evil = firstButton;
			}
			justKilled = firstButton;
			NPC.active = false;
			NPC.netUpdate = true;
			NPC.NPCLoot();
		}

		public override void HitEffect(NPC.HitInfo hit)
		{
			if (!Main.dedServ)
			{
				for (int n = 0; n < 3; n++)
				{
					Dust.NewDust(new Vector2(NPC.position.X + (NPC.width / 2f), NPC.position.Y + (NPC.height / 2f)), 1, 1, DustID.Blood, (Main.rand.NextFloat() - 0.5f) * 3f, (Main.rand.NextFloat() - 0.5f) * 8f, Scale: 1f);
				}
				if (NPC.life <= 0)
				{
					if (DownedBosses.downedElius)
					{
						if (!Main.dedServ)
						{
							if (!Main.LocalPlayer.ArcaneOdyssey().evil)
							{
								Main.NewText(Mod.CustomLocalization(LocalizationCategory + "." + Name + ".Spared").Value, SpiritEnergy.Instance.Colour);
							}
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
