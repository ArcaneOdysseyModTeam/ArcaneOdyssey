#if VSDEBUGMODE
using ArcaneOdyssey.Content.Items.Magic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.Personalities;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.NPCS
{
	public class Edgelord : ModNPC
	{
		public override void SetDefaults()
		{
			NPC.townNPC = true;
			NPC.friendly = true;
			NPC.height = 40;
			NPC.width = 18;
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

        public override List<string> SetNPCNameList()
		{
			return ["Morden"];
		}

		public override bool CanBeHitByNPC(NPC attacker)
		{
			return !attacker.IsDamageDodgeable();
		}

		public override bool? CanBeHitByItem(Player player, Item item)
		{
			return item.TryGetImbue(player, out _) ? true : null; // no need to do more than this, flymeal is melee and rotten eggs are ranged
		}

        public override void OnHitByItem(Player player, Item item, NPC.HitInfo hit, int damageDone)
        {
			// do death magic dust here red
        }

        public override void OnHitByProjectile(Projectile projectile, NPC.HitInfo hit, int damageDone)
        {
            // do death magic dust here red
        }

        public override void ModifyTypeName(ref string typeName)
        {
			typeName = Mod.CustomLocalization($"NPCs.{Name}.DisplayNam{(Main.IsItDay() ? "e" : "e1")}").Value;
        }
    }
}
#endif