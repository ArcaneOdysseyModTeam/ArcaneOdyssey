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
				SetNPCAffection(NPCID.TaxCollector, AffectionLevel.Dislike).
				SetNPCAffection(NPCID.Pirate, AffectionLevel.Like).
				SetNPCAffection(NPCID.Wizard, AffectionLevel.Love);
			NPCID.Sets.AttackFrameCount[Type] = 4; // morden doesnt attack but im keeping this

		}

		public override List<string> SetNPCNameList() => ["Morden"];

		public override bool CanBeHitByNPC(NPC attacker) => !attacker.IsDamageDodgeable();

        /// <summary>
        /// no need to do more than this, flymeal is melee and rotten eggs are ranged
        /// </summary>
        public override bool? CanBeHitByItem(Player player, Item item) => (item.TryGetImbue(out _) || (item.DamageType == DamageClass.Magic || item.DamageType == DamageClass.MagicSummonHybrid) ? true : null);


  		public override bool? CanBeHitByProjectile(Projectile projectile) => (projectile.TryGetImbue(out _) || ((projectile.DamageType == DamageClass.Magic || projectile.DamageType == DamageClass.MagicSummonHybrid) && projectile.hostile) ? true : null);


        /// <summary>
        /// do death magic dust here red
        /// </summary>
        public override void OnHitByItem(Player player, Item item, NPC.HitInfo hit, int damageDone)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(NPC.position.X + (NPC.width / 2f), NPC.position.Y + (NPC.height / 2f)), 1, 1, DustID.Wraith, (Main.rand.NextFloat()-0.5f)*3f, (Main.rand.NextFloat()-0.5f)*3f, 0, default, 1f)];
				spawnedDust.noGravity = true;
				Dust spawnedDust2 = Main.dust[Dust.NewDust(new Vector2(NPC.position.X + (NPC.width / 2f), NPC.position.Y + (NPC.height / 2f)), 1, 1, DustID.Vortex, (Main.rand.NextFloat()-0.5f)*3f, (Main.rand.NextFloat()-0.5f)*3f, 0, default, 1.6f)];
				spawnedDust2.noGravity = true;
			}
		}

		/// <summary>
		/// do death magic dust here red
		/// </summary>
		public override void OnHitByProjectile(Projectile projectile, NPC.HitInfo hit, int damageDone)
		{
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
			for (int n = 0; n < 20; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(NPC.position.X + (NPC.width / 2f), NPC.position.Y + (NPC.height / 2f)), 1, 1, DustID.Wraith, (Main.rand.NextFloat() - 0.5f) * 3f, (Main.rand.NextFloat() - 0.5f) * 3f, 0, default, 2f)];
				spawnedDust.noGravity = true;
				Dust spawnedDust2 = Main.dust[Dust.NewDust(new Vector2(NPC.position.X + (NPC.width / 2f), NPC.position.Y + (NPC.height / 2f)), 1, 1, DustID.Vortex, (Main.rand.NextFloat() - 0.5f) * 3f, (Main.rand.NextFloat() - 0.5f) * 3f, 0, default, 2.6f)];
				spawnedDust2.noGravity = true;
			}
			Projectile.NewProjectile(NPC.GetSource_FromThis(),NPC.position.X + (NPC.width / 2f),NPC.position.Y + (NPC.height / 2f),(Main.rand.NextFloat() - 0.5f) * 3f,-30f,ModContent.ProjectileType<Projectiles.DeathCurse>(),0,0f,-1,default);
        }
		public override void ModifyTypeName(ref string typeName) => typeName = Mod.CustomLocalization($"NPCs.{Name}.DisplayNam{(Main.IsItDay() ? "e" : "e1")}").Value;

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
				Main.npcChatText = this.GetChatHelpButton();
			}
        }
		public string GetChatHelpButton()
		{
			List<string> options = [];
			if (false) // add conditions later
			{
				options.Add(this.GetLocalizedValue("Help.DarkSeaWarning"));
			}
			if (GetBossKillCount() == 0)
			{
				options.Add(this.GetLocalizedValue("Help.Early1"));
				options.Add(this.GetLocalizedValue("Help.Early2"));
			}
			if (options.Count == 0)
				return this.GetLocalizedValue("Help.NothingToSay");
			return Main.rand.Next(options);
		}
        public override string GetChat()
		{
			return this.GetLocalizedValue("Help.Intro").Replace("{PlayerName}", Main.LocalPlayer.name);
		}

		public override bool CanGoToStatue(bool toKingStatue) => toKingStatue;
	}
}
