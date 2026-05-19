using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Imbues.Magic.Ancient;
using ArcaneOdyssey.Imbues.Relics;
using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Projectiles;
using ArcaneOdyssey.UI;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.Chat;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.Personalities;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;


namespace ArcaneOdyssey.NPCs.Town
{
	[AutoloadHead]
	public class Edgelord : BaseNPC
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			NPC.townNPC = true;
			NPC.friendly = true;
			NPC.height = Player.defaultHeight;
			NPC.width = Player.defaultWidth;
			NPC.lifeMax = 6000;
			NPC.aiStyle = NPCAIStyleID.Passive;
			NPC.defense = 15;
			NPC.HitSound = SoundID.NPCHit52;
			NPC.DeathSound = SoundID.NPCDeath52;
			NPC.knockBackResist = 0;
			AnimationType = NPCID.Guide;
		}

		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[Type] = 25;
			NPCID.Sets.ExtraFramesCount[Type] = 9;
			NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new() { Direction = 1 };
			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
			NPC.Happiness.
				SetBiomeAffection<DungeonBiome>(AffectionLevel.Hate).
				SetBiomeAffection<SnowBiome>(AffectionLevel.Dislike).
				SetBiomeAffection<DesertBiome>(AffectionLevel.Like).
				SetBiomeAffection<OceanBiome>(AffectionLevel.Love).
				SetNPCAffection(NPCID.WitchDoctor, AffectionLevel.Hate).
				SetNPCAffection(NPCID.Pirate, AffectionLevel.Dislike).
				SetNPCAffection(NPCID.Wizard, AffectionLevel.Like).
				SetNPCAffection(NPCID.Clothier, AffectionLevel.Love);
			NPCID.Sets.AttackFrameCount[Type] = 4; // morden doesnt attack but im keeping this
			NPCID.Sets.ImmuneToRegularBuffs[Type] = true;
		}

		public override List<string> SetNPCNameList() => ["Morden"];

		public override bool CanBeHitByNPC(NPC attacker) => !attacker.IsDamageDodgeable();

		public override void ModifyHitByItem(Player player, Item item, ref NPC.HitModifiers modifiers)
		{
			if (item.Imbue() is not MagicType or SpiritEnergy)
			{
				modifiers.FinalDamage *= 0;
				NPC.life += 5;
			}
		}

		public override void ModifyHitByProjectile(Projectile projectile, ref NPC.HitModifiers modifiers)
		{
			if (!(projectile.Imbue() is MagicType or SpiritEnergy || ((projectile.DamageType.CountsAsClass(DamageClass.Magic) || projectile.DamageType.CountsAsClass(DamageClass.Summon)) && projectile.hostile)))
			{
				modifiers.FinalDamage *= 0;
				NPC.life = Utils.Clamp(NPC.life + 5, 0, NPC.lifeMax + 1);
			}
		}

		public override void UpdateLifeRegen(ref int damage)
		{
			if (NPC.wet && !NPC.honeyWet && !NPC.lavaWet && !NPC.shimmerWet)
			{
				NPC.lifeRegen = 120 * -5;
				HitEffect(NPC.CalculateHitInfo(5, 0));
			}
		}

		public override void HitEffect(NPC.HitInfo hit)
		{
			if (!Main.dedServ)
			{
				for (int n = 0; n < 10; n++)
				{
					Dust spawnedDust = Main.dust[Dust.NewDust(NPC.Center, 1, 1, DustID.Wraith, (Main.rand.NextFloat() - 0.5f) * 3f, (Main.rand.NextFloat() - 0.5f) * 8f, Scale: 1f)];
					spawnedDust.noGravity = true;
					Dust spawnedDust2 = Main.dust[Dust.NewDust(NPC.Center, 1, 1, DustID.Vortex, (Main.rand.NextFloat() - 0.5f) * 3f, (Main.rand.NextFloat() - 0.5f) * 8f, Scale: 1.6f)];
					spawnedDust2.noGravity = true;
				}
			}
		}

		public override void OnKill()
		{
			// Have death curse shoot out
			if (!Main.dedServ)
			{
				for (int n = 0; n < 20; n++)
				{
					Dust spawnedDust = Main.dust[Dust.NewDust(NPC.Center, 1, 1, DustID.Wraith, (Main.rand.NextFloat() - 0.5f) * 3f, (Main.rand.NextFloat() - 0.5f) * 3f, Scale: 2f)];
					spawnedDust.noGravity = true;
					Dust spawnedDust2 = Main.dust[Dust.NewDust(NPC.Center, 1, 1, DustID.Vortex, (Main.rand.NextFloat() - 0.5f) * 3f, (Main.rand.NextFloat() - 0.5f) * 3f, Scale: 2.6f)];
					spawnedDust2.noGravity = true;
				}
				Main.NewText(Mod.CustomLocalization($"{LocalizationCategory}.{Name}.DeathCurse").Value, Color.DarkCyan);
			}
			else
			{
				ChatHelper.BroadcastChatMessage(Mod.CustomLocalization($"{LocalizationCategory}.{Name}.DeathCurse").ToNetworkText(), Color.DarkCyan);
			}
			if (AOUtils.ServerOrSingleplayer)
				Projectile.NewProjectile(NPC.GetSource_Death(), NPC.Center, new(0, 10), ModContent.ProjectileType<DeathCurse>(), 700, 0f);
			if (NPC.wet && !NPC.honeyWet && !NPC.lavaWet && !NPC.shimmerWet)
			{
				ExplodeMorden();
			}
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange([
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Ocean,
				new FlavorTextBestiaryInfoElement($"Mods.{Mod.Name}.Bestiary.{Name}")
			]);
		}

		public override void SetChatButtons(ref string button, ref string button2)
		{
			button = Mod.CustomLocalization("RandomWords.Guide").Value;
			button2 = Mod.CustomLocalization("ImbueStuff.SynergiesButton").Value;
		}

		public override void OnChatButtonClicked(bool firstButton, ref string shopName)
		{
			if (firstButton)
			{
				Main.npcChatText = Main.rand.Next(Player.ArcaneOdyssey().AvailablePages()).Description.Value; // placeholder, open up guidebook ui instead

				//AOPlayer modPlayer = Player.ArcaneOdyssey();
				//List<GuidebookPage> list = modPlayer.AvailablePages();
				//Main.NewText($"Hmm {Player.name}, modPlayer: {modPlayer.Name}\n");
				//foreach (var l in list)
				//{
				//	Main.NewText($"Name: {l.DisplayName} \n" +
				//		$"\t{l.Description}\n");
				//}

				Main.CloseNPCChatOrSign();
				ModContent.GetInstance<ModUISystem>().ShowReadingSimulator();
			}
			else
			{
				if (Main.LocalPlayer.PlayerItem() is not null && !Main.LocalPlayer.PlayerItem().IsAir && Main.LocalPlayer.PlayerItem().active && Main.LocalPlayer.PlayerItem().ModItem is Imbuable imbue)
				{
					Main.npcChatText = imbue.SynergiesText();
				}
				else
				{
					Main.npcChatText = Mod.CustomLocalization("ImbueStuff.HoldAnImbue").Value;
				}
			}
		}

		public static Player Player => Main.LocalPlayer;


		private static string LastDialogue = "";

		public override string GetChat()
		{
			if (NPC.wet && !NPC.honeyWet && !NPC.lavaWet && !NPC.shimmerWet)
			{
				return this.GetLocalizedValue("DyingText");
			}

			if (Main.rand.NextBool(100))
			{
				return this.GetLocalizedValue("Chat.EasterEgg");
			}

			List<string> options = [];

			void AddOption(string value)
			{
				options.Add(this.GetLocalizedValue($"Chat.{value}"));
			}

			if (NPC.downedBoss1 && !DownedBosses.DownedElius)
			{
				AddOption("CloudsShift");
			}

			AddOption("Water");
			if (AOUtils.BossesKilled == 0)
			{
				options.Add(Language.GetTextValue(this.GetLocalizationKey("Chat.Intro"), Player.name));
				AddOption("Grave");
			}
			else
				AddOption("Hello");
			AddOption("AskHelp");
			if (AOUtils.BossesKilled > 0 && !NPC.downedBoss3)
			{
				AddOption("OldManTalk");
			}

			if (Player.HasItemInInventory(e => ArcaneOdysseyMod.Sets.weaponType[e.type] == WeaponType.Strength))
			{
				AddOption("StrongWarrior");
			}

			if (!Player.HasTypeInInventory<Scroll>())
			{
				AddOption("Pots");
			}

			if (Player.HasTypeInInventory<DeathMagic>())
			{
				AddOption("DeathMagic");
			}

			options.RemoveAll(e => e == LastDialogue);

			if (options.Count == 0)
				return this.GetLocalizedValue("Chat.Hello");

			string chosen = Main.rand.Next(options);
			LastDialogue = chosen;
			return chosen;
		}

		public void ExplodeMorden()
		{
			if (!Main.dedServ)
			{
				for (int n = 0; n < 50; n++)
				{
					Dust spawnedDust = Dust.NewDustDirect(new Vector2(NPC.position.X + (NPC.width / 2f), NPC.position.Y + (NPC.height / 2f)), 1, 1, DustID.Wraith, (Main.rand.NextFloat() - 0.5f) * 50f, (Main.rand.NextFloat() - 0.5f) * 50f, Scale: 2f);
					spawnedDust.noGravity = true;
					Dust spawnedDust2 = Dust.NewDustDirect(new Vector2(NPC.position.X + (NPC.width / 2f), NPC.position.Y + (NPC.height / 2f)), 1, 1, DustID.Vortex, (Main.rand.NextFloat() - 0.5f) * 50f, (Main.rand.NextFloat() - 0.5f) * 50f, Scale: 2.6f);
					spawnedDust2.noGravity = true;
				}
				SoundEngine.PlaySound(SoundID.Item74, NPC.position, null);
			}
		}

		public override bool CanTownNPCSpawn(int numTownNPCs) => true;

		public override bool CanGoToStatue(bool toKingStatue) => toKingStatue;
	}
}
