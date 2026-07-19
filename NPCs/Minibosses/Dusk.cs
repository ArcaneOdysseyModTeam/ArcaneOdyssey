using ArcaneOdyssey.Gores.Dusk;
using ArcaneOdyssey.Imbues.Relics;
using ArcaneOdyssey.Items.Armour.Vanity.Masks;
using ArcaneOdyssey.Items.BossTrophies;
using ArcaneOdyssey.Items.Scrolls.Attacks.Common;
using ArcaneOdyssey.Projectiles.Enemies;
using System.Collections.Generic;

namespace ArcaneOdyssey.NPCs.Minibosses
{
	[AutoloadBossHead]
	public class Dusk : Miniboss
	{
		public override List<int> MeleeProjectiles => [ModContent.ProjectileType<DuskRaincloud>(), ModContent.ProjectileType<DuskBeam>()];
		public override List<int> RangedProjectiles => [ModContent.ProjectileType<DuskHound>(), ModContent.ProjectileType<DuskBeam>()];

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

		public override int WalkingSpriteCount => 13;

		public override int AttackingSpriteCount => 15;

		public override float ShootSpeed => 7f * .9f;


		public override bool Downed { get => DownedBosses.DownedDusk; set => DownedBosses.DownedDusk = value; }

		public override Color Motif => new(89, 0, 83);

		public override bool ExtraConditions => NPC.downedBoss2 && !Main.IsItDay();

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
			npcLoot.Add(AOUtils.Common<StaffofNight>());
			npcLoot.Add(AOUtils.Common<DuskMask>(4));
			npcLoot.Add(AnyDropHelper.Create(ModContent.ItemType<RainRite>(), ModContent.ItemType<HoundRite>()));
			npcLoot.Add(AOUtils.Common<DuskTrophy>(10));
		}
	}
}
