using ArcaneOdyssey.Content.Items.Imbues.Relics;
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
	public class Laelus : AOMiniboss
	{
		public override List<int> MeleeProjectiles => [ModContent.ProjectileType<LaelusExplosion>()];
		public override List<int> RangedProjectiles => [ModContent.ProjectileType<LaelusBlast>()];

		public override void SetDefaults()
		{
			base.SetDefaults();
			NPC.damage = 65;
			NPC.defense = 15;
			NPC.width = 20;
			NPC.height = 44;
			//Sprite height 46
			//Sprite width 68
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.value = Item.buyPrice(gold: 2);
			//NPC.ai[0] state
			//NPC.ai[1] state time
		}

		public override float ShootSpeed => 7f * .9f;

		public override ref bool Downed => ref DownedBosses.downedLaelus;

		public override bool ExtraConditions => Main.dayTime;

		public override int AOHealth => 650;

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
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<TidestoneBand>()));
		}
	}
}
