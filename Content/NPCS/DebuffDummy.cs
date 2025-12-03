using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.NPCS
{
	public class DebuffDummy : ModNPC
	{
		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[NPC.type] = 1;
		}

		public override void SetDefaults()
		{
			NPC.lifeMax = 999999999;
			NPC.noGravity = true;
			NPC.damage = 0;
			NPC.knockBackResist = 0f;
			NPC.defense = 0;
			NPC.height = NPC.width = 80;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.friendly = false;
			NPC.trapImmune = false;
			NPC.lavaImmune = false;
			NPC.aiStyle = 0;
		}
	}
}
