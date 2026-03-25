#if VSDEBUGMODE
using ArcaneOdysseyMusic;
#endif
using Terraria;
using Terraria.ID;

namespace ArcaneOdyssey.NPCs
{
	public class DebuffDummy : BaseNPC
	{
		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[NPC.type] = 1;
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
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
			NPC.lavaImmune = false;
#if VSDEBUGMODE
			NPC.boss = true;
		}

		public override AOMusicTrack Theme => AOMusicTrack.Atlantean;
#else
		}
#endif


		public override void AI()
		{
			NPC.rotation = NPC.Center.DirectionTo(Main.LocalPlayer.MountedCenter).ToRotation();
			NPC.life = NPC.lifeMax;
			if (AOUtils.BossAlive())
				NPC.Kill();
		}
	}
}
