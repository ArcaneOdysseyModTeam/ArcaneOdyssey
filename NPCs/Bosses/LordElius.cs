using System;
using System.IO;
using ArcaneOdyssey.Biomes;
using ArcaneOdyssey.Imbues.Relics;
using ArcaneOdyssey.Items.Armour.RavennaNoble;
using ArcaneOdyssey.Items.Weapons.RavennaNoble;
using ArcaneOdysseyMusic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Chat;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.NPCs.Bosses
{
	public class LordElius : AOBaseNPC
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
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.friendly = false;
			NPC.trapImmune = true;
			NPC.lavaImmune = true;
			NPC.boss = true;
			SpawnModBiomes = [AOUtils.BiomeType<EliusArena>()];
		}

		public override AOMusicTrack Theme => AOMusicTrack.Elius;

		public bool sentMessage = false;
		private bool hasSetSpawnLocation = false;
		public Vector2 spawnLocation;
		
		public override void AI()
		{
			Main.raining = true;
			Main.rainTime = 2;
			Main.windSpeedTarget = MathHelper.Lerp(-.8f, -.4f, NPC.life / (float)NPC.lifeMax);
			Main.maxRaining = MathHelper.Lerp(1, .7f, NPC.life / (float)NPC.lifeMax);


			if (sparing)
			{
				if (!sentMessage)
				{
					Main.NewText(Mod.CustomLocalization(LocalizationCategory + "." + Name + ".DoomMessage"), Color.MediumPurple);
					CombatText.NewText(NPC.Hitbox, Color.MediumPurple, Mod.CustomLocalization(LocalizationCategory + "." + Name + ".DoomMessage").Value, true);
					sentMessage = true;
				}
				return;
			}

			if (!sentMessage)
			{
				Main.NewText(Mod.CustomLocalization(LocalizationCategory + "." + Name + ".SpawnMessage"), Color.MediumPurple);
				CombatText.NewText(NPC.Hitbox, Color.MediumPurple, Mod.CustomLocalization(LocalizationCategory + "." + Name + ".SpawnMessage").Value, true);
				sentMessage = true;
			}
			
			if(!hasSetSpawnLocation) //this also is used for setup
			{
				// test specific, elius will be spawned in a location with his spawner later
				NPC.Center = new Vector2((EliusArenaLoader.eliusArena.Center.X + 25) * 16, (EliusArenaLoader.eliusArena.Center.Y + 2) * 16);
				// end test specific
				NPC.Center = NPC.Center - new Vector2(0,32);
				Main.NewText("Test: Elius location set");
				spawnLocation = NPC.position;
				hasSetSpawnLocation = true;
				NPC.ai[0] = 1f;
				NPC.ai[1] = 0f;
			}

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
					NPC.Center += new Vector2(100,0); //lmao hes just leaving fuck you
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
			npcLoot.Add(
				AnyDropHelper.Create(
					ModContent.ItemType<EliusBoots>(),
					ModContent.ItemType<EliusChest>(),
					ModContent.ItemType<EliusHelm>(),
					ModContent.ItemType<NobleThunderspear>(),
					ModContent.ItemType<ScimitarofStorm>(),
					ModContent.ItemType<StormCaller>()
					)
				);
		}

		public override void OnKill()
		{
			Main.windSpeedTarget = -.1f;
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
			Main.windSpeedTarget = -.1f;
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

		public override void OnChatButtonClicked(bool firstButton, ref string shopName)
		{
			if (Main.netMode == NetmodeID.MultiplayerClient)
			{
				foreach (var player in Main.ActivePlayers)
				{
					player.ArcaneOdyssey().evil = firstButton;
				}
				if (firstButton)
				{
					ChatHelper.BroadcastChatMessage(Mod.CustomLocalization(LocalizationCategory + "." + Name + ".Killed").ToNetworkText(), SpiritEnergy.Instance.SpiritColor);
				}
				else
				{
					ChatHelper.BroadcastChatMessage(Mod.CustomLocalization(LocalizationCategory + "." + Name + ".Spared").ToNetworkText(), SpiritEnergy.Instance.SpiritColor);
				}
			}
			else
			{
				Main.LocalPlayer.ArcaneOdyssey().evil = firstButton;
				if (firstButton)
				{
					Main.NewText(Mod.CustomLocalization(LocalizationCategory + "." + Name + ".Killed").Value, SpiritEnergy.Instance.SpiritColor);
				}
				else
				{
					Main.NewText(Mod.CustomLocalization(LocalizationCategory + "." + Name + ".Spared").Value, SpiritEnergy.Instance.SpiritColor);
				}
			}

			NPC.active = false;
			NPC.netUpdate = true;
			NPC.NPCLoot();
		}

		public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
		{
			NPC.lifeMax = (int)(NPC.lifeMax * 0.8f * balance * bossAdjustment);
		}
	}
}
