using ArcaneOdyssey.NPCs.Base;
using SubworldLibrary;
using System.Collections.Generic;

namespace ArcaneOdyssey.NPCs
{
	public class RowboatNPC : BaseNPC
	{
		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[Type] = 1;
			NPCID.Sets.ImmuneToAllBuffs[Type] = true;
			NPCID.Sets.NoTownNPCHappiness[Type] = true;
			NPCID.Sets.TeleportationImmune[Type] = true;
			NPCID.Sets.SpawnsWithCustomName[Type] = true;
		}

		public override List<string> SetNPCNameList()
		{
			return [Mod.CustomLocalization(this.GetLocalizationKey("BetterName"), Main.LocalPlayer.name).Value];
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			NPC.width = 100;
			NPC.height = 38;
			NPC.noGravity = true;
			NPC.noTileCollide = true;
			NPC.damage = 0;
			NPC.knockBackResist = 0f;
			NPC.defense = 0;
			NPC.lifeMax = 100;
			NPC.immortal = true;
			NPC.friendly = true;
			NPC.dontTakeDamageFromHostiles = true;
			NPC.trapImmune = false;
			NPC.lavaImmune = false;
		}

		public override bool CanChat() => true;

		public override bool CanHitPlayer(Player target, ref int cooldownSlot) => false;

		public override bool CanHitNPC(NPC target) => false;

		public override bool CanGoToStatue(bool toKingStatue) => false;

		public override bool CheckActive() => false;

		public override void OnChatButtonClicked(bool firstButton, ref string shopName)
		{
			SubworldSystem.Exit();
		}

		public override void SetChatButtons(ref string button, ref string button2)
		{
			button = Mod.CustomLocalization("RandomWords.Leave").Value;
		}

		public override string GetChat()
		{
			return Mod.CustomLocalization(this.GetLocalizationKey("AskToLeave")).Value;
		}
	}
}
