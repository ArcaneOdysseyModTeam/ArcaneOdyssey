using ArcaneOdyssey.Items.SealedChests;
using System.Collections.Generic;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.Personalities;

namespace ArcaneOdyssey.NPCs.Town
{
	[AutoloadHead]
	public class ShipMaster : BaseNPC
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			NPC.townNPC = true;
			NPC.friendly = true;
			NPC.height = Player.defaultHeight;
			NPC.width = Player.defaultWidth;
			NPC.lifeMax = 250;
			NPC.aiStyle = NPCAIStyleID.Passive;
			NPC.defense = 15;
			NPC.HitSound = SoundID.NPCHit52;
			NPC.DeathSound = SoundID.NPCDeath52;
			NPC.knockBackResist = 0.5f;
			AnimationType = NPCID.Guide;
		}

		public override bool IsLoadingEnabled(Mod mod) => ArcaneOdysseyMod.DevMode; // dev mode only right now

		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[Type] = 25;
			NPCID.Sets.ExtraFramesCount[Type] = 9;
			NPCID.Sets.NPCBestiaryDrawOffset[Type] = NPCID.Sets.NPCBestiaryDrawOffset[NPCID.Guide];
			NPC.Happiness.
				SetBiomeAffection<DungeonBiome>(AffectionLevel.Hate).
				SetBiomeAffection<AOUtils.SkyBiome>(AffectionLevel.Dislike).
				SetBiomeAffection<ForestBiome>(AffectionLevel.Like).
				SetBiomeAffection<OceanBiome>(AffectionLevel.Love).
				SetNPCAffection(NPCID.Steampunker, AffectionLevel.Hate).
				SetNPCAffection(NPCID.Pirate, AffectionLevel.Dislike).
				SetNPCAffection(NPCID.Mechanic, AffectionLevel.Like).
				SetNPCAffection(NPCID.Clothier, AffectionLevel.Love);
			NPCID.Sets.AttackFrameCount[Type] = 4;
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange([
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Ocean,
				new FlavorTextBestiaryInfoElement($"Mods.{Mod.Name}.Bestiary.{Name}")
			]);
		}

		public override List<string> SetNPCNameList()
		{
			return [
				"Gamma",
				"Taz Antares",
				"Kiliov",
				"Nico Mako",
				"Tobi",
				"Acsazf",
				"Nuss",
				"Ryan",
				"Axel Ronin",
				"Homer Creed",
				"Shayna Stillwater",
				"Kindra",
				"Minty"
			];
		}

		public override bool CanTownNPCSpawn(int numTownNPCs) => DownedBosses.DownedElius;

		public override bool CanGoToStatue(bool toKingStatue) => toKingStatue;

		public override void SetChatButtons(ref string button, ref string button2) // PORT change to new method
		{
			button = Mod.CustomLocalization("RandomWords.Travel").Value;
			button2 = Mod.CustomLocalization("RandomWords.Unseal").Value;
		}

		public override void OnChatButtonClicked(bool firstButton, ref string shopName) // PORT change to new method
		{
			if (firstButton)
			{
				// open odyssey system and ship shop
				Main.npcChatText = "Odyssey System! (maybe also ship shop later)";
			}
			else
			{
				// change to cycle first button later?
				// unbox sealed crates
				Main.npcChatText = $"Unsealed {/*Main.LocalPlayer.ArcaneOdyssey().BronzeSealed + Main.LocalPlayer.ArcaneOdyssey().NimbusSealed + */Main.LocalPlayer.ArcaneOdyssey().DarkSealed} chests";
				if (Main.LocalPlayer.ArcaneOdyssey().DarkSealed > 0)
				{
					Main.LocalPlayer.QuickSpawnItem(NPC.GetSource_FromThis(), ModContent.ItemType<DarkSealedChest>(), Main.LocalPlayer.ArcaneOdyssey().DarkSealed);
				}
				Main.LocalPlayer.ArcaneOdyssey().DarkSealed = 0;
			}
		}

		private static string LastDialogue = "";

		public override string GetChat()
		{
			return $"Greetings, {Main.LocalPlayer.name}.";

			List<string> options = [];

			void AddOption(string value)
			{
				options.Add(this.GetLocalizedValue($"Chat.{value}"));
			}



			options.RemoveAll(e => e == LastDialogue);

			if (options.Count == 0)
				options = [this.GetLocalizedValue("Chat.Hello")];

			string chosen = Main.rand.Next(options);
			LastDialogue = chosen;
			return chosen;
		}
	}
}
