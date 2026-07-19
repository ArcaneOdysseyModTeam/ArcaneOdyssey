using ArcaneOdysseyMusic;

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
			NPC.boss = true;
		}

		public override MusicTrack Theme => MusicTrack.Atlantean;


		public override void AI()
		{
			NPC.rotation = NPC.SafeDirectionTo(Main.LocalPlayer.MountedCenter).ToRotation();
			NPC.life = NPC.lifeMax;
		}
	}
}
