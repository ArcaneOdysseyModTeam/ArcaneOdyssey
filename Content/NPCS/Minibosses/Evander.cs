using ArcaneOdyssey.Content.Items.Accessories;
using ArcaneOdyssey.Content.Items.BossTrophies;
using ArcaneOdyssey.Content.Items.Weapons;
using ArcaneOdyssey.Content.Projectiles.Enemies;
using ArcaneOdyssey.VFX.Gores;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.NPCS.Minibosses
{
	[AutoloadBossHead]
	public class Evander : AOMiniboss
	{
		public override int AOHealth => 5000;
		public override List<int> MeleeProjectiles => [ModContent.ProjectileType<EvanderMelee>()];
		public override List<int> RangedProjectiles => [ModContent.ProjectileType<EvanderSlash>()];

		public override void SetDefaults()
		{
			base.SetDefaults();
			NPC.lifeMax = 5000;
			NPC.damage = 75;
			NPC.defense = 20;
			NPC.width = 20;
			NPC.height = 44;
			//Sprite height 96
			//Sprite width 76
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.value = Item.buyPrice(gold: 10);
			//NPC.ai[0] state
			//NPC.ai[1] state time
		}

		public override ref bool Downed => ref DownedBosses.downedEvander;

		public override bool ExtraConditions => Main.hardMode;

		public override void HitEffect(NPC.HitInfo hit)
		{
			if (!Main.dedServ)
			{
				for (int n = 0; n < 3; n++)
				{
					Dust.NewDust(new Vector2(NPC.position.X + (NPC.width / 2f), NPC.position.Y + (NPC.height / 2f)), 1, 1, DustID.Blood, (Main.rand.NextFloat() - 0.5f) * 3f, (Main.rand.NextFloat() - 0.5f) * 8f, Scale: 1f);
				}
				if (NPC.life <= 0)
				{
					Gore.NewGore(NPC.GetSource_FromThis(), NPC.Top, NPC.velocity, ModContent.GoreType<EvanderHead>(), 1f);
					Gore.NewGore(NPC.GetSource_FromThis(), NPC.Right, NPC.velocity, ModContent.GoreType<EvanderRightArm>(), 1f);
					Gore.NewGore(NPC.GetSource_FromThis(), NPC.Left, NPC.velocity, ModContent.GoreType<EvanderLeftArm>(), 1f);
					Gore.NewGore(NPC.GetSource_FromThis(), NPC.Center, NPC.velocity, ModContent.GoreType<EvanderTorso>(), 1f);
					Gore.NewGore(NPC.GetSource_FromThis(), NPC.BottomLeft, NPC.velocity, ModContent.GoreType<EvanderLeg>(), 1f);
					Gore.NewGore(NPC.GetSource_FromThis(), NPC.BottomRight, NPC.velocity, ModContent.GoreType<EvanderLeg>(), 1f);
					for (int n = 0; n < 17; n++)
					{
						Dust.NewDust(new Vector2(NPC.position.X + (NPC.width / 2f), NPC.position.Y + (NPC.height / 2f)), 1, 1, DustID.Blood, (Main.rand.NextFloat() - 0.5f) * 3f, (Main.rand.NextFloat() - 0.5f) * 8f, Scale: 1f);
					}
				}
			}
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<ColossalGreatsword>(), 4));
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<EvanderCape>(), 4));
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<EvanderGauntlet>(), 4));
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<EvanderTrophy>(), 10));
		}
	}
}
