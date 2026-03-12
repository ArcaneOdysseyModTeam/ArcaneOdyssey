using ArcaneOdyssey.Items.Armour.RavennaNoble;
using ArcaneOdyssey.Items.Weapons.RavennaNoble;
using Microsoft.Xna.Framework;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.NPCs.Bosses
{
	public class LordElius : AOBaseNPC
	{
		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[NPC.type] = 1;
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
		}

		public bool sentMessage = false;

		public override void AI()
		{
			//if (!Main.raining)
			//{
			//	Main.StartRain();
			//}
			Main.raining = true;
			Main.windSpeedTarget = -.4f;
			Main.maxRaining = .7f;
			Main.rainTime = 2;

			if (!sentMessage)
			{
				Main.NewText(Mod.CustomLocalization(LocalizationCategory + "." + Name + ".SpawnMessage"), Color.MediumPurple);
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
	}
}
