using ArcaneOdyssey.Content.Items.Armour.Vanity.Masks;
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
	public class Dusk : AOMiniboss
	{
		public override List<int> MeleeProjectiles => [ModContent.ProjectileType<DuskRaincloud>(), ModContent.ProjectileType<DuskBeam>()];
		public override List<int> RangedProjectiles => [ModContent.ProjectileType<DuskHound>()];

		public override void SetDefaults()
		{
			base.SetDefaults();
			NPC.damage = 65;
			NPC.defense = 12;
			NPC.width = Player.defaultWidth;
			NPC.height = Player.defaultHeight;
			//Sprite height 46
			//Sprite width 68
			NPC.HitSound = SoundID.NPCHit40;
			NPC.DeathSound = SoundID.NPCDeath42;
			NPC.value = Item.buyPrice(gold: 5);
			//NPC.ai[0] state
			//NPC.ai[1] state time
		}

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			Main.npcFrameCount[Type] = 28;
		}

		public override int WalkingSpriteCount => 12;

		public override float ShootSpeed => 7f * .9f;

		public override ref bool Downed => ref DownedBosses.downedDusk;

		public override bool ExtraConditions => NPC.downedBoss2 && !Main.dayTime;

		public override int AOHealth => 1700;

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
					Gore.NewGore(NPC.GetSource_FromThis(), NPC.Top, NPC.velocity, ModContent.GoreType<DuskHead>());
					Gore.NewGore(NPC.GetSource_FromThis(), NPC.Right, NPC.velocity, ModContent.GoreType<DuskArm>());
					Gore.NewGore(NPC.GetSource_FromThis(), NPC.Center, NPC.velocity, ModContent.GoreType<DuskCape>());
					Gore.NewGore(NPC.GetSource_FromThis(), NPC.Bottom, NPC.velocity, ModContent.GoreType<DuskRobe>());
					Gore.NewGore(NPC.GetSource_FromThis(), NPC.Center, NPC.velocity, ModContent.GoreType<DuskTorso>());
					for (int n = 0; n < 17; n++)
					{
						Dust.NewDust(new Vector2(NPC.position.X + (NPC.width / 2f), NPC.position.Y + (NPC.height / 2f)), 1, 1, DustID.Blood, (Main.rand.NextFloat() - 0.5f) * 3f, (Main.rand.NextFloat() - 0.5f) * 8f, Scale: 1f);
					}
				}
			}
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<NyxStaff>(), 4));
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<DuskMask>(), 4));
		}
	}
}
