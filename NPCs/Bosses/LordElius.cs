using ArcaneOdyssey.Biomes;
using ArcaneOdyssey.Items.Armour.RavennaNoble;
using ArcaneOdyssey.Items.Weapons.RavennaNoble;
using Microsoft.Xna.Framework;
using Terraria;
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
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange([
				new FlavorTextBestiaryInfoElement($"Mods.{Mod.Name}.Bestiary.{Name}")
			]);
		}

		public override void SetDefaults()
		{
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
			Music = AOUtils.GetMusic("Elius");
			SpawnModBiomes = [AOUtils.BiomeType<EliusArena>()];
		}

		public bool sentMessage = false;

		public override void AI()
		{
			Main.raining = true;
			Main.rainTime = 2;
			Main.windSpeedTarget = MathHelper.Lerp(-.8f, .4f, NPC.life / (float)NPC.lifeMax);
			Main.maxRaining = MathHelper.Lerp(1, .7f, NPC.life / (float)NPC.lifeMax);


			if (!sentMessage)
			{
				Main.NewText(Mod.CustomLocalization(LocalizationCategory + "." + Name + ".SpawnMessage"), Color.MediumPurple);
				CombatText.NewText(NPC.Hitbox, Color.MediumPurple, Mod.CustomLocalization(LocalizationCategory + "." + Name + ".SpawnMessage").Value, true);
				sentMessage = true;
			}

			// ai here, red
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(AOUtils.Common<EliusBoots>(6));
			npcLoot.Add(AOUtils.Common<EliusChest>(6));
			npcLoot.Add(AOUtils.Common<EliusHelm>(6));
			npcLoot.Add(AOUtils.Common<NobleThunderspear>(6));
			npcLoot.Add(AOUtils.Common<ScimitarofStorm>(6));
			npcLoot.Add(AOUtils.Common<StormCaller>(6));
		}

		public override void OnKill()
		{
			DownedBosses.downedElius = true;
			Main.windSpeedTarget = -.1f;
			if (Main.dedServ)
			{
				NetMessage.SendData(MessageID.WorldData);
			}
		}
	}
}
