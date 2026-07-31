using ArcaneOdyssey.Imbues.Relics;
using ArcaneOdyssey.Items.BossTrophies;
using ArcaneOdyssey.Items.Scrolls.Attacks.Common;
using ArcaneOdyssey.Items.Weapons;
using ArcaneOdyssey.Projectiles.Enemies;
using System;
using System.Collections.Generic;
using Terraria.GameContent.ItemDropRules;

namespace ArcaneOdyssey.NPCs.Minibosses
{
	[AutoloadBossHead]
	public class Laelus : Miniboss
	{
		public override int AOHealth => 650;
		public override List<int> MeleeProjectiles => [ModContent.ProjectileType<LaelusExplosion>()];
		public override List<int> RangedProjectiles => [ModContent.ProjectileType<LaelusBlast>()];

		public override int WalkingSpriteCount => 15;
		public const int MeleeIndex = 15, RangedIndex = 25;

		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[Type] = 38;
			NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new() { Velocity = 1f };
			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
			ExternalModSupport.DeclareMiniboss(Type);
		}

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
		public override void FindFrame(int frameHeight)
		{
			if (NPC.HasValidTarget)
			{
				if (NPC.ai[0] == 0) // walking
				{
					if (Main.player[NPC.target].Center.Distance(NPC.Center) > 1000f)
					{
						NPC.frame.Y = 0;
					}
					else if (Main.player[NPC.target].Center.Distance(NPC.Center) <= 50f)
					{
						NPC.frame.Y = 0;
					}
					else
					{
						if (NPC.frameCounter > 3)
						{
							if (NPC.frame.Y < ((WalkingSpriteCount - 1) * frameHeight) && NPC.frame.Y >= 0)
							{
								NPC.frame.Y += frameHeight;
							}
							else
							{
								NPC.frame.Y = frameHeight;
							}
							NPC.frameCounter = 0;
						}
						NPC.frameCounter++;
					}
				}
				else if (NPC.ai[0] == 2) // melee
				{
					if (NPC.frameCounter++ > 2)
					{
						if (NPC.frame.Y < (RangedIndex * frameHeight) && NPC.frame.Y >= (MeleeIndex * frameHeight))
						{
							NPC.frame.Y += frameHeight;
						}
						else
						{
							if (NPC.frame.Y < (RangedIndex * frameHeight))
							{
								NPC.frame.Y = MeleeIndex * frameHeight;
							}
							else
							{
								NPC.ai[0] = 0;
								NPC.ai[1] = 0;
								NPC.frameCounter = 0;
							}
						}
						NPC.frameCounter = 0;
					}
				}
				else if (NPC.ai[0] == 1) // ranged
				{
					if (NPC.frameCounter++ > 2)
					{
						if (NPC.frame.Y < ((Main.npcFrameCount[Type] - 1) * frameHeight) && NPC.frame.Y >= (RangedIndex * frameHeight))
						{
							NPC.frame.Y += frameHeight;
						}
						else
						{
							if (NPC.frame.Y < ((Main.npcFrameCount[Type] - 1) * frameHeight))
							{
								NPC.frame.Y = RangedIndex * frameHeight;
							}
							else
							{
								NPC.ai[0] = 0;
								NPC.ai[1] = 0;
								NPC.frameCounter = 0;
							}
						}
						NPC.frameCounter = 0;
					}
				}
			}
			else
			{
				NPC.frame.Y = 0;
			}
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
