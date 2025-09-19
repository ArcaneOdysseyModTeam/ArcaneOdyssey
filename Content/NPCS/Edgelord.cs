using ArcaneOdyssey.Content.Items.Magic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.Personalities;
using Terraria.ID;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOUtils;
using Microsoft.Xna.Framework;
using ArcaneOdyssey.Content.Projectiles.Magic.Blasts;
using Terraria.Localization;
using ArcaneOdyssey.Content.Projectiles;
using Terraria.Chat;

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

		public override void PostAI()
		{
			if (NPC.wet && !NPC.lavaWet && !NPC.honeyWet)
			{
				NPC.life -= 5;
				NPC.localAI[0]++;
				if (NPC.localAI[0] % 15 == 0)
					HitEffect(NPC.CalculateHitInfo(5, 1));
				if (NPC.life <= 0)
				{
					OnKill();
				}
			}
			else
				NPC.localAI[0] = 0;
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

		}
		public override List<string> SetNPCNameList() => ["Morden"];

		public override bool CanBeHitByNPC(NPC attacker) => !attacker.IsDamageDodgeable();

		public override bool? CanBeHitByProjectile(Projectile projectile)
		{
			if (!projectile.hostile && projectile.type != ProjectileID.RottenEgg)
				return false;
			return projectile.TryGetImbue(out _) || ((projectile.DamageType == DamageClass.Magic || projectile.DamageType == DamageClass.MagicSummonHybrid) && projectile.hostile) ? true : null;
		}

		public override void ModifyHitByItem(Player player, Item item, ref NPC.HitModifiers modifiers)
		{
			if (!item.TryGetImbue(out _))
				modifiers.FinalDamage *= 0;
		}

		public override void HitEffect(NPC.HitInfo hit)
		{
			if (!Main.dedServ)
				for (int n = 0; n < 10; n++)
				{
					Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(NPC.position.X + (NPC.width / 2f), NPC.position.Y + (NPC.height / 2f)), 1, 1, DustID.Wraith, (Main.rand.NextFloat()-0.5f)*3f, (Main.rand.NextFloat()-0.5f)*8f, 0, default, 1f)];
					spawnedDust.noGravity = true;
					Dust spawnedDust2 = Main.dust[Dust.NewDust(new Vector2(NPC.position.X + (NPC.width / 2f), NPC.position.Y + (NPC.height / 2f)), 1, 1, DustID.Vortex, (Main.rand.NextFloat()-0.5f)*3f, (Main.rand.NextFloat()-0.5f)*8f, 0, default, 1.6f)];
					spawnedDust2.noGravity = true;
				}
		}

		public override void OnKill()
		{
			// Have death curse shoot out
			if (!Main.dedServ)
			{
				for (int n = 0; n < 20; n++)
				{
					Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(NPC.position.X + (NPC.width / 2f), NPC.position.Y + (NPC.height / 2f)), 1, 1, DustID.Wraith, (Main.rand.NextFloat() - 0.5f) * 3f, (Main.rand.NextFloat() - 0.5f) * 3f, 0, default, 2f)];
					spawnedDust.noGravity = true;
					Dust spawnedDust2 = Main.dust[Dust.NewDust(new Vector2(NPC.position.X + (NPC.width / 2f), NPC.position.Y + (NPC.height / 2f)), 1, 1, DustID.Vortex, (Main.rand.NextFloat() - 0.5f) * 3f, (Main.rand.NextFloat() - 0.5f) * 3f, 0, default, 2.6f)];
					spawnedDust2.noGravity = true;
				}
				Main.NewText(Mod.CustomLocalization("NPCs.Edgelord.DeathCurse").Value, Color.DarkCyan);
			}
			else
			{
				foreach (Player player in Main.ActivePlayers)
				{
					ChatHelper.SendChatMessageToClient(Mod.CustomLocalization("NPCs.Edgelord.DeathCurse").ToNetworkText(), Color.DarkCyan, Main.player.IndexOf(player)); 
				}
			}
			Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.position.X + (NPC.width / 2f), NPC.position.Y + (NPC.height / 2f), 0f, -10f, ModContent.ProjectileType<DeathCurse>(), 0, 0f, -1, default);
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
			button = "Help";
			button2 = null;
		}
		public override void OnChatButtonClicked(bool firstButton, ref string shopName)
		{
			if (firstButton)
			{
				Main.npcChatText = GetChatHelpButton();
			}
		}
		public string GetChatHelpButton()
		{
			List<string> options = [];
			if (false) // add conditions later
			{
				options.Add(this.GetLocalizedValue("Help.DarkSeaWarning"));
			}
			if (BossesKilled == 0)
			{
				options.Add(this.GetLocalizedValue("Help.Early1"));
				options.Add(this.GetLocalizedValue("Help.Early2"));
				options.Add(this.GetLocalizedValue("Help.WorldofMagic"));
			}

			if (Main.hardMode && !NPC.downedMechBossAny)
			{
				options.Add(this.GetLocalizedValue("Help.EarlyHard1"));
				options.Add(this.GetLocalizedValue("Help.EarlyHard2"));
			}	

			if (!Main.hardMode)
			{
				options.Add(this.GetLocalizedValue("Help.PreHard1"));
				options.Add(this.GetLocalizedValue("Help.PreHard2"));
			}

			if (!NPC.downedAncientCultist && NPC.downedGolemBoss)
			{
				options.Add(this.GetLocalizedValue("Help.CultistTip"));
			}

			if (!NPC.downedPlantBoss && NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3)
			{
				options.Add(this.GetLocalizedValue("Help.PlantTip"));
			}

			options.RemoveAll(e => e == Main.LocalPlayer.GetModPlayer<MordenDialogue>().LastHelp);

            if (options.Count == 0)
                return this.GetLocalizedValue("Help.NothingToSay");

            string chosen = Main.rand.Next(options);
			Main.LocalPlayer.GetModPlayer<MordenDialogue>().LastHelp = chosen;
            return chosen;
		}

		public override string GetChat()
		{
			List<string> options = [];
			if (BossesKilled == 0)
			{
				options.Add(this.GetLocalizedValue("Chat.Intro").Replace("{PlayerName}", Main.LocalPlayer.name));
				options.Add(this.GetLocalizedValue("Chat.Grave"));
			}
			else
				options.Add(this.GetLocalizedValue("Chat.Hello"));
			options.Add(this.GetLocalizedValue("Chat.AskHelp"));
			if (BossesKilled > 0 && !NPC.downedBoss3) 
			{
				options.Add(this.GetLocalizedValue("Chat.OldManTalk"));
            }

            options.RemoveAll(e => e == Main.LocalPlayer.GetModPlayer<MordenDialogue>().LastDialogue);

            if (options.Count == 0)
                return this.GetLocalizedValue("Chat.Hello");

            string chosen = Main.rand.Next(options);
            Main.LocalPlayer.GetModPlayer<MordenDialogue>().LastDialogue = chosen;
            return chosen;
            return Main.rand.Next(options);
		}

		public override bool CanTownNPCSpawn(int numTownNPCs) => true;

		public override bool CanGoToStatue(bool toKingStatue) => toKingStatue;
	}

	public class MordenDialogue : ModPlayer
	{
		public string LastDialogue = "";
		public string LastHelp = "";
	}
}
