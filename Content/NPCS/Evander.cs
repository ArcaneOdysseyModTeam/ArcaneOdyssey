using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.NPCS
{
	public class Evander : ModNPC
	{
		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[NPC.type] = 1;
		}
		public override void SetDefaults()
		{
			NPC.lifeMax = 10000;
			NPC.lifeRegen = 0;
			NPC.noGravity = false;
			NPC.damage = 100;
			NPC.knockBackResist = 0f;
			NPC.defense = 20;
			NPC.height = 48;
			NPC.width = 24;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.friendly = false;
			NPC.trapImmune = false;
			NPC.lavaImmune = false;
			NPC.aiStyle = 0;
		}

	}
}
