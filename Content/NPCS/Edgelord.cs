using System.Collections.Generic;
using System.Linq;
using System;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.Personalities;
using Terraria.ID;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOUtils;
using Microsoft.Xna.Framework;
using ArcaneOdyssey.Content.Projectiles;
using Terraria.Chat;
using Terraria.Audio;
using Terraria.GameInput;
using ArcaneOdyssey.Content.Items.Base;
using Terraria.Localization;
using ArcaneOdyssey.Content.Items.Weapons.Sunken;
using ArcaneOdyssey.Content.Items.Imbues.Relics;

namespace ArcaneOdyssey.Content.NPCS
{
	[AutoloadHead]
	public class Edgelord : ModNPC
	{
		public override void SetDefaults()
		{
			NPC.townNPC = true;
			NPC.friendly = true;
			NPC.height = 44;
			NPC.width = 20;
			NPC.lifeMax = 1000;
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
			if (item.Imbue() is not AOMagic or SpiritEnergy)
			{
				modifiers.FinalDamage *= 0;
				NPC.life += 5;
			}
		}

		public override void ModifyHitByProjectile(Projectile projectile, ref NPC.HitModifiers modifiers)
		{
			if (!(projectile.Imbue() is AOMagic or SpiritEnergy || ((projectile.DamageType.CountsAsClass(DamageClass.Magic) || projectile.DamageType.CountsAsClass(DamageClass.Summon)) && projectile.hostile)))
			{
				modifiers.FinalDamage *= 0;
				NPC.life = Utils.Clamp(NPC.life + 5, 0, NPC.lifeMax + 1);
			}
		}

		public override void UpdateLifeRegen(ref int damage)
		{
			if ((NPC.wet && !NPC.honeyWet && !NPC.lavaWet && !NPC.shimmerWet) || !ArcaneOdysseyConfig.Instance.EnableMorden)
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
					Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(NPC.position.X + (NPC.width / 2f), NPC.position.Y + (NPC.height / 2f)), 1, 1, DustID.Wraith, (Main.rand.NextFloat() - 0.5f) * 3f, (Main.rand.NextFloat() - 0.5f) * 8f, Scale: 1f)];
					spawnedDust.noGravity = true;
					Dust spawnedDust2 = Main.dust[Dust.NewDust(new Vector2(NPC.position.X + (NPC.width / 2f), NPC.position.Y + (NPC.height / 2f)), 1, 1, DustID.Vortex, (Main.rand.NextFloat() - 0.5f) * 3f, (Main.rand.NextFloat() - 0.5f) * 8f, Scale: 1.6f)];
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
					Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(NPC.position.X + (NPC.width / 2f), NPC.position.Y + (NPC.height / 2f)), 1, 1, DustID.Wraith, (Main.rand.NextFloat() - 0.5f) * 3f, (Main.rand.NextFloat() - 0.5f) * 3f, Scale: 2f)];
					spawnedDust.noGravity = true;
					Dust spawnedDust2 = Main.dust[Dust.NewDust(new Vector2(NPC.position.X + (NPC.width / 2f), NPC.position.Y + (NPC.height / 2f)), 1, 1, DustID.Vortex, (Main.rand.NextFloat() - 0.5f) * 3f, (Main.rand.NextFloat() - 0.5f) * 3f, Scale: 2.6f)];
					spawnedDust2.noGravity = true;
				}
				Main.NewText(Mod.CustomLocalization($"NPCs.{Name}.DeathCurse").Value, Color.DarkCyan);
			}
			else
			{
				ChatHelper.BroadcastChatMessage(Mod.CustomLocalization($"NPCs.{Name}.DeathCurse").ToNetworkText(), Color.DarkCyan);
			}
			if (ServerOrSingleplayer)
				Projectile.NewProjectile(NPC.GetSource_Death(), NPC.position + (NPC.Size / 2f), new(0, 10), ModContent.ProjectileType<DeathCurse>(), 700, 0f);
			if (NPC.wet && !NPC.honeyWet && !NPC.lavaWet && !NPC.shimmerWet)
			{
				ExplodeMorden();
			}
		}

		public override void ModifyTypeName(ref string typeName) => typeName = Mod.CustomLocalization($"NPCs.{Name}.DisplayNam{(!Main.zenithWorld ? "e" : "e1")}").Value;

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange([
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Ocean,
				new FlavorTextBestiaryInfoElement($"Mods.{Mod.Name}.Bestiary.{Name}")
			]);
		}

		public override void SetChatButtons(ref string button, ref string button2)
		{
			button = Mod.CustomLocalization("RandomWords.Help").Value;
		}

		public override void OnChatButtonClicked(bool firstButton, ref string shopName)
		{
			if (firstButton)
			{
				Main.npcChatText = GetChatHelpButton();
			}
		}

		internal static Dictionary<string, Func<bool>> helpOptions = [];
		public static void AddHelpOption(string value, Func<bool> condition)
		{
			helpOptions[value] = condition;
		}

		public static Player Player => Main.LocalPlayer;

		public string GetChatHelpButton()
		{
			if ((NPC.wet && !NPC.honeyWet && !NPC.lavaWet && !NPC.shimmerWet) || !ArcaneOdysseyConfig.Instance.EnableMorden)
			{
				return this.GetLocalizedValue("DyingText");
			}

			List<string> options = [];

			void AddOption(string value)
			{
				options.Add(this.GetLocalizedValue($"Help.{value}"));
			}

			if (false) // add conditions later
			{
				AddOption("DarkSeaWarning");
			}

			foreach (string key in helpOptions.Keys)
			{
				if (helpOptions[key]())
				{
					options.Add(key);
				}
			}

			if (BossesKilled < 3)
			{
				AddOption("Relics");
				AddOption("Early1");
				AddOption("WorldofMagic");
				AddOption("WeaponSkills");
				if (Player.HasTypeInInventory<AOMagic>())
				{
					AddOption("EarlyMagic1");
					AddOption("EarlyMagic2");
					AddOption("EarlyMagic3");
				}
				if (Player.HasTypeInInventory<FightingStyle>())
				{
					if (Main.hardMode)
						AddOption("VanishingStyle");
					AddOption("SailorStyle");
					AddOption("EarlyFighting1");
					AddOption("EarlyMagic3");
					string doubletapdash = Mod.CustomLocalization("KeybindStuff.DashHelp").Value;
					if (ModLoader.HasMod("CalamityMod"))
					{
						doubletapdash = Mod.CustomLocalization("RandomWords.Press", ExternalModSupport.DashBind()?.GetAssignedKeys().FirstOrDefault(Mod.CustomLocalization("RandomWords.Unbound").Value)).Value;
					}
					else if (ModLoader.TryGetMod("Fargowiltas", out Mod fargos))
					{
						if ((bool)fargos.Call("DoubleTapDashDisabled"))
						{
							doubletapdash = Mod.CustomLocalization("RandomWords.Press", ExternalModSupport.DashBind()?.GetAssignedKeys().FirstOrDefault(Mod.CustomLocalization("RandomWords.Unbound").Value)).Value;
						}
					}
					string dashbind = AOKeybinds.DashBind.GetAssignedKeys(InputMode.Keyboard).FirstOrDefault(Mod.CustomLocalization("RandomWords.Unbound").Value);
					options.Add(Language.GetTextValue(this.GetLocalizationKey("Help.EarlyFighting2"), doubletapdash, Mod.CustomLocalization("RandomWords.Press", dashbind).Value));
				}
			}

			if (NPC.downedBoss2 && !Main.hardMode)
			{
				AddOption("BronzeTip");
			}

			if (Main.hardMode && !NPC.downedMechBossAny)
			{
				AddOption("EarlyHard1");
				AddOption("EarlyHard2");
			}

			if (Main.hardMode && !DownedBosses.downedEvander)
			{
				AddOption("EvanderWarning");
			}

			if (Player.PlayerItem()?.ArcaneOdyssey()?.WeaponsType == WeaponType.Strength)
			{
				AddOption("HasStrengthWeapon");
			}

			if (Player.PlayerItem()?.ArcaneOdyssey()?.WeaponsType == WeaponType.Artisinal)
			{
				AddOption("ArtisinalWeapon");
			}

			if (Main.hardMode && NPC.downedPirates)
			{
				AddOption("CannonFist");
			}

			if (!Main.hardMode)
			{
				AddOption("PreHard1");
				AddOption("PreHard2");
			}

			if (Player.GetAllImbues().Count > 1)
			{
				AddOption("StackImbues");
			}

			if (Player.PlayerItem()?.ModItem is SunkenSword || Player.PlayerItem()?.ModItem is SunkenStaff)
			{
				AddOption("SunkenWeapon");
			}

			if (!NPC.downedAncientCultist && NPC.downedGolemBoss)
			{
				AddOption("CultistTip");
			}

			options.RemoveAll(e => e == LastHelp);

			if (options.Count == 0)
				return this.GetLocalizedValue("Help.NothingToSay");

			string chosen = Main.rand.Next(options);
			LastHelp = chosen;
			return chosen;
		}

		private static string LastDialogue = "";
		private static string LastHelp = "";

		public override string GetChat()
		{
			if ((NPC.wet && !NPC.honeyWet && !NPC.lavaWet && !NPC.shimmerWet) || !ArcaneOdysseyConfig.Instance.EnableMorden)
			{
				return this.GetLocalizedValue("DyingText");
			}
			List<string> options = [];

			void AddOption(string value)
			{
				options.Add(this.GetLocalizedValue($"Chat.{value}"));
			}

			AddOption("Water");
			if (BossesKilled == 0)
			{
				options.Add(Language.GetTextValue(this.GetLocalizationKey("Chat.Intro"), Player.name));
				AddOption("Grave");

			}
			else
				AddOption("Hello");
			AddOption("AskHelp");
			if (BossesKilled > 0 && !NPC.downedBoss3)
			{
				AddOption("OldManTalk");
			}

			if (Player.PlayerItem()?.ArcaneOdyssey()?.WeaponsType == WeaponType.Strength)
			{
				AddOption("StrongWarrior");
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

		public override bool CanTownNPCSpawn(int numTownNPCs) => ArcaneOdysseyConfig.Instance.EnableMorden;

		public override bool CanGoToStatue(bool toKingStatue) => toKingStatue;
	}
}
