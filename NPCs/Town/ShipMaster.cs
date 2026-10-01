using ArcaneOdyssey.Items.Weapons.Bronze;
using ArcaneOdyssey.NPCs.Base;
using ArcaneOdyssey.Subworlds.Base;
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
				SetNPCAffection(NPCID.PartyGirl, AffectionLevel.Hate).
				SetNPCAffection(NPCID.Angler, AffectionLevel.Dislike).
				SetNPCAffection(NPCID.Painter, AffectionLevel.Like).
				SetNPCAffection(NPCID.Clothier, AffectionLevel.Love);
			NPCID.Sets.AttackFrameCount[Type] = 4;
			NPCID.Sets.DangerDetectRange[Type] = 700;
			NPCID.Sets.AttackType[Type] = 1;
			NPCID.Sets.AttackTime[Type] = 30;
			NPCID.Sets.AttackAverageChance[Type] = 5;
			NPCID.Sets.ShimmerTownTransform[Type] = false;
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
				"Minty",
				"Lemon",
				"Hisaka Kagemori",
				"Green Gaster",
			];
		}

		public override bool CanTownNPCSpawn(int numTownNPCs) => DownedBosses.DownedElius && AOSubworld.UnlockedSubworlds.Length > 0;

		public override bool CanGoToStatue(bool toKingStatue) => true; // gender neutral

		public override void SetChatButtons(ref string button, ref string button2) // PORT change to new method
		{
			button = Mod.CustomLocalization("RandomWords.Travel").Value;
			//button2 = Mod.CustomLocalization("RandomWords.Unseal").Value;
		}

		public override void OnChatButtonClicked(bool firstButton, ref string shopName) // PORT change to new method
		{
			if (firstButton)
			{
				Main.CloseNPCChatOrSign();
				ArcaneOdysseyMod.NoticeQueue.Add("System not complete!");
				// open odyssey system and ship shop
			}
			//else
			//{
			//	// change to cycle first button later?
			//	// unbox sealed crates
			//	Main.npcChatText = $"Unsealed {/*Main.LocalPlayer.ArcaneOdyssey().BronzeSealed + Main.LocalPlayer.ArcaneOdyssey().NimbusSealed + */Main.LocalPlayer.ArcaneOdyssey().DarkSealed} chests";
			//	if (Main.LocalPlayer.ArcaneOdyssey().DarkSealed > 0)
			//	{
			//		Main.LocalPlayer.QuickSpawnItem(NPC.GetSource_FromThis(), ModContent.ItemType<DarkSealedChest>(), Main.LocalPlayer.ArcaneOdyssey().DarkSealed);
			//	}
			//	Main.LocalPlayer.ArcaneOdyssey().DarkSealed = 0;
			//}
		}

		private static string LastDialogue = "";

		public override string GetChat()
		{
			List<string> options = [];

			void AddOption(string value)
			{
				options.Add(this.GetLocalizedValue($"Chat.{value}"));
			}

			if (AOUtils.NPCAlive(NPCID.TravellingMerchant, out var travel))
			{
				options.Add(Mod.CustomLocalization(this.GetLocalizationKey("Chat.TravelingMerchant"), travel.GivenOrTypeName).Value);
			}
			if (AOUtils.NPCAlive(NPCID.Pirate, out var pirate))
			{
				options.Add(Mod.CustomLocalization(this.GetLocalizationKey("Chat.Pirate"), pirate.GivenOrTypeName).Value);
			}
			if (AOUtils.NPCAlive(NPCID.Nurse, out var nurse))
			{
				options.Add(Mod.CustomLocalization(this.GetLocalizationKey("Chat.Nurse"), nurse.GivenOrTypeName).Value);
			}

			options.RemoveAll(e => e == LastDialogue);

			if (options.Count == 0)
				options = [this.GetLocalizedValue("Chat.Hello")];

			string chosen = Main.rand.Next(options);
			LastDialogue = chosen;
			return chosen;
		}

		public override void TownNPCAttackProj(ref int projType, ref int attackDelay)
		{
			projType = ProjectileID.BulletDeadeye;
			attackDelay = 1;
		}

		public override void TownNPCAttackStrength(ref int damage, ref float knockback)
		{
			damage = 15;
			knockback = 2f;
		}

		public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)
		{
			cooldown = 30;
			randExtraCooldown = 30;
		}

		public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
		{
			multiplier = 12f;
		}

		public override void DrawTownAttackGun(ref Texture2D item, ref Rectangle itemFrame, ref float scale, ref int horizontalHoldoutOffset)
		{
			var itemType = ModContent.ItemType<BronzeFlintlock>();
			Main.GetItemDrawFrame(itemType, out item, out itemFrame);
			horizontalHoldoutOffset = (int)Main.DrawPlayerItemPos(1f, itemType).X;
		}

		public override void TownNPCAttackShoot(ref bool inBetweenShots)
		{
			//inBetweenShots = true;
		}
	}
}
