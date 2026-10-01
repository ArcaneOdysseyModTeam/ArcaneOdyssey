using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Imbues.Magic.Ancient;
using ArcaneOdyssey.Imbues.Relics;
using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.NPCs.Base;
using ArcaneOdyssey.Projectiles;
using ArcaneOdyssey.Projectiles.TownNPC;
using ArcaneOdyssey.UI;
using System.Collections.Generic;
using Terraria.Audio;
using Terraria.Chat;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.Personalities;


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
			NPCID.Sets.NPCBestiaryDrawOffset[Type] = NPCID.Sets.NPCBestiaryDrawOffset[NPCID.Guide];
			NPC.Happiness.
				SetBiomeAffection<DungeonBiome>(AffectionLevel.Hate).
				SetBiomeAffection<SnowBiome>(AffectionLevel.Dislike).
				SetBiomeAffection<DesertBiome>(AffectionLevel.Like).
				SetBiomeAffection<OceanBiome>(AffectionLevel.Love).
				SetNPCAffection(NPCID.WitchDoctor, AffectionLevel.Hate).
				SetNPCAffection(NPCID.Pirate, AffectionLevel.Dislike).
				SetNPCAffection(NPCID.Wizard, AffectionLevel.Like).
				SetNPCAffection(NPCID.Clothier, AffectionLevel.Love);
			NPCID.Sets.AttackFrameCount[Type] = 4;
			NPCID.Sets.DangerDetectRange[Type] = 700;
			NPCID.Sets.AttackTime[Type] = 30;
			NPCID.Sets.AttackAverageChance[Type] = 30;
			NPCID.Sets.ShimmerTownTransform[Type] = false;
			NPCID.Sets.ImmuneToRegularBuffs[Type] = true;
			NPCID.Sets.AttackType[Type] = 2;
			NPCID.Sets.MagicAuraColor[Type] = Imbue.ImbueColour2;
		}

		public static DeathMagic Imbue => ModContent.GetInstance<DeathMagic>();

		public override void TownNPCAttackProj(ref int projType, ref int attackDelay)
		{
			projType = ModContent.ProjectileType<DeathBlast>();
			attackDelay = 1;
		}

		public override void TownNPCAttackMagic(ref float auraLightMultiplier)
		{
			base.TownNPCAttackMagic(ref auraLightMultiplier);
		}

		public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
		{
			multiplier = 7f * Imbue.ScrollSpeed;
		}

		public override void TownNPCAttackStrength(ref int damage, ref float knockback)
		{
			damage = 20;
			knockback = 5f;
		}

		public override List<string> SetNPCNameList() => ["Morden"];

		public override bool CanBeHitByNPC(NPC attacker) => attacker.Distance(NPC.Center) < 20f || !attacker.IsDamageDodgeable();

		public override void ModifyHitByItem(Player player, Item item, ref NPC.HitModifiers modifiers)
		{
			if (item.Imbue() is not (MagicType or SpiritEnergy))
			{
				modifiers.FinalDamage *= 0;
				NPC.life = Utils.Clamp(NPC.life + 5, 0, NPC.lifeMax + 1);
				modifiers.HideCombatText();
				modifiers.DisableCrit();
			}
		}

		public override void ModifyHitByProjectile(Projectile projectile, ref NPC.HitModifiers modifiers)
		{
			if (!(projectile.Imbue() is MagicType or SpiritEnergy || ((projectile.DamageType.CountsAsClass(DamageClass.Magic) || projectile.DamageType.CountsAsClass(DamageClass.Summon)) && projectile.hostile)))
			{
				modifiers.FinalDamage *= 0;
				NPC.life = Utils.Clamp(NPC.life + 5, 0, NPC.lifeMax + 1);
				modifiers.HideCombatText();
				modifiers.DisableCrit();
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
					Imbue?.LingeringEffects(NPC.Hitbox, source: NPC);
				}
			}
		}

		public override void OnKill()
		{
			// Have death curse shoot out
			if (!Main.dedServ)
			{
				for (int n = 0; n < 7; n++)
				{
					Imbue.ExplosionEffects(NPC.Center);
				}
				Main.NewText(Mod.CustomLocalization($"{LocalizationCategory}.{Name}.DeathCurse").Value, Color.DarkCyan);
				if (NPC.wet && !NPC.honeyWet && !NPC.lavaWet && !NPC.shimmerWet)
				{
					ExplodeMorden();
				}
			}
			else
			{
				ChatHelper.BroadcastChatMessage(Mod.CustomLocalization($"{LocalizationCategory}.{Name}.DeathCurse").ToNetworkText(), Color.DarkCyan);
			}
			Projectile.NewProjectile(NPC.GetSource_Death(), NPC.Center, new(0, 10), ModContent.ProjectileType<DeathCurse>(), 700, 0f);
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange([
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
				new FlavorTextBestiaryInfoElement($"Mods.{Mod.Name}.Bestiary.{Name}")
			]);
		}

		public override void SetChatButtons(ref string button, ref string button2) // PORT change to new method
		{
			button = Mod.CustomLocalization("RandomWords.Guide").Value;
			button2 = Mod.CustomLocalization("ImbueStuff.SynergiesButton").Value;
		}

		public override void OnChatButtonClicked(bool firstButton, ref string shopName) // PORT change to new method
		{
			if (firstButton)
			{
				Main.CloseNPCChatOrSign();
				ModContent.GetInstance<ModUISystem>().ShowReadingSimulator();
			}
			else
			{
				if (Player.PlayerItem() is not null && !Player.PlayerItem().IsAir && Player.PlayerItem().active && Player.PlayerItem().ModItem is Imbuable imbue)
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
				options.Add(Mod.CustomLocalization(this.GetLocalizationKey("Chat.Intro"), Player.name).Value);
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
				options = [this.GetLocalizedValue("Chat.Hello")];

			string chosen = Main.rand.Next(options);
			LastDialogue = chosen;
			return chosen;
		}

		public void ExplodeMorden()
		{
			if (!Main.dedServ)
			{
				for (int n = 0; n < 16; n++)
				{
					Imbue.ExplosionEffects(NPC.Center, 2f);
				}
				SoundEngine.PlaySound(SoundID.Item74, NPC.position, null);
			}
		}

		public override bool CanTownNPCSpawn(int numTownNPCs) => true;

		public override bool CanGoToStatue(bool toKingStatue) => toKingStatue;
	}
}
