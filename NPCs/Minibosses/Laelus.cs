using ArcaneOdyssey.Imbues.Relics;
using ArcaneOdyssey.Items.BossTrophies;
using ArcaneOdyssey.Items.Scrolls.Attacks.Common;
using ArcaneOdyssey.Items.Weapons;
using ArcaneOdyssey.Projectiles.Enemies;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.NPCs.Minibosses
{
	[AutoloadBossHead]
	public class Laelus : Miniboss
	{
		public override int AOHealth => 650;
		public override List<int> MeleeProjectiles => [ModContent.ProjectileType<LaelusExplosion>()];
		public override List<int> RangedProjectiles => [ModContent.ProjectileType<LaelusBlast>()];

		public override int AttackingSpriteCount => 1;
		public override int WalkingSpriteCount => 1;


		public override void SetDefaults()
		{
			base.SetDefaults();
			NPC.damage = 35;
			NPC.defense = 3;
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

		public override bool Downed { get => DownedBosses.DownedLaelus; set => DownedBosses.DownedLaelus = value; }

		public override bool ExtraConditions => Main.dayTime || Main.remixWorld;

		public override Color Motif => new(0, 0, 214);

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
					//Gore.NewGore(NPC.GetSource_FromThis(), NPC.Top, NPC.velocity, ModContent.GoreType<EvanderHead>());
					//Gore.NewGore(NPC.GetSource_FromThis(), NPC.Right, NPC.velocity, ModContent.GoreType<EvanderRightArm>());
					//Gore.NewGore(NPC.GetSource_FromThis(), NPC.Left, NPC.velocity, ModContent.GoreType<EvanderLeftArm>());
					//Gore.NewGore(NPC.GetSource_FromThis(), NPC.Center, NPC.velocity, ModContent.GoreType<EvanderTorso>());
					//Gore.NewGore(NPC.GetSource_FromThis(), NPC.BottomLeft, NPC.velocity, ModContent.GoreType<EvanderLeg>());
					//Gore.NewGore(NPC.GetSource_FromThis(), NPC.BottomRight, NPC.velocity, ModContent.GoreType<EvanderLeg>());
					for (int n = 0; n < 17; n++)
					{
						Dust.NewDust(new Vector2(NPC.position.X + (NPC.width / 2f), NPC.position.Y + (NPC.height / 2f)), 1, 1, DustID.Blood, (Main.rand.NextFloat() - 0.5f) * 3f, (Main.rand.NextFloat() - 0.5f) * 8f, Scale: 1f);
					}
				}
			}
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<TidestoneBand>()));
			npcLoot.Add(AOUtils.Common<LaelusTrophy>(10));
			npcLoot.Add(AOUtils.Common<Sanguine>(4));
			npcLoot.Add(AnyDropHelper.Create(ModContent.ItemType<BlastScroll>(), ModContent.ItemType<ExplosionScroll>()));
		}
	}
}
