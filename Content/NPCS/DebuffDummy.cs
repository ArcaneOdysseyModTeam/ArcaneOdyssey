using Terraria;
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
			NPC.lifeMax = int.MaxValue / 100;
			NPC.noGravity = true;
			NPC.damage = 0;
			NPC.knockBackResist = 0f;
			NPC.defense = 0;
			NPC.height = NPC.width = 80;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.friendly = false;
			NPC.trapImmune = false;
			Music = AOUtils.GetMusic("Atlantean");
			NPC.lavaImmune = false;
			NPC.boss = true;
		}

		public override void AI()
		{
			NPC.rotation = NPC.Center.DirectionTo(Main.LocalPlayer.MountedCenter).ToRotation();
			NPC.life = NPC.lifeMax;
			if (AOUtils.BossAlive())
				NPC.Kill();
		}
	}
}
