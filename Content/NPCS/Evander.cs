using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using ArcaneOdyssey.Content.Projectiles.Enemies;
using Terraria.DataStructures;

namespace ArcaneOdyssey.Content.NPCS
{
	public class Evander : ModNPC
	{
		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[Type] = 17;
            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new() { Direction = -1, Velocity = 1f };
            ExternalModSupport.DeclareMiniboss(Type);
		}
		public override void SetDefaults()
		{
			NPC.lifeMax = 10000;
			NPC.lifeRegen = 0;
			NPC.noGravity = false;
			NPC.damage = 0;
			NPC.knockBackResist = 0f;
			NPC.defense = 20;
			NPC.height = 44;
			NPC.width = 20;
			//Sprite height 96
			//Sprite width 76
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.friendly = false;
			NPC.trapImmune = false;
			NPC.lavaImmune = false;
			NPC.aiStyle = 0;
			NPC.frameCounter = 0;
			NPC.ai[0] = 0; // state
			NPC.ai[1] = 0; // state time
		}
		private bool canJump = false;

		public override void AI()
		{
			if (NPC.ai[0] == 0) //Chase
			{// Chase the nearest player
				NPC.ai[1]++;
				NPC.TargetClosest();
				if (Main.player[NPC.target].Center.Distance(NPC.Center) <= 1000f)
				{ // Limit chasing distance
					NPC.velocity.X += NPC.direction * 0.2f;
					if (Main.player[NPC.target].Center.Distance(NPC.Center) <= 50f)
					{ // Attack meelee or stop
						NPC.velocity.X = 0f;
						if (NPC.ai[1] >= 60) {
							NPC.ai[0] = 2;
							NPC.frameCounter = 0;
							NPC.ai[1] = 0;
						}
					} else if (NPC.ai[1] > 60 && Math.Abs(Main.player[NPC.target].Center.X - NPC.Center.X) <= 300f && Math.Abs(Main.player[NPC.target].Center.X - NPC.Center.X) >= 100f)
                    {
						NPC.ai[0] = 1;
						NPC.ai[1] = 0;
						NPC.frameCounter = 0;
                    }
				}
				if (Math.Abs(NPC.velocity.X) > 8f)
				{
					NPC.velocity.X *= 0.8f;
				}
				if (Math.Abs(NPC.velocity.X) < 0.2f)
				{
					NPC.velocity.X = 0f;
				}

				// Jump if there's a block
				if (checkTileToDir(NPC.direction, NPC.Bottom + new Vector2(0f, -16f)) && canJump)
				{
					NPC.velocity.Y = -5f;
				}
				canJump = (checkTileToDir(0, NPC.Bottom) && Math.Abs(NPC.velocity.Y) < 0.01f);
			} else if(NPC.ai[0] == 1) // col cleave
			{
				NPC.ai[1]++;
				NPC.velocity.X *= 0.7f;
				if (NPC.ai[1] >= 20)
                {
					NPC.ai[0] = 0;
					NPC.ai[1] = 0;
					NPC.frameCounter = 0;
                } else if (NPC.ai[1] == 15)
                {
					Main.NewText("Collossal Cleave!");
					Projectile.NewProjectile(NPC.GetSource_FromThis(),NPC.position.X + (NPC.direction * 3),NPC.position.Y,NPC.direction * 3f,0f,ModContent.ProjectileType<EvanderSlash>(),25,4.5f);
                }
            } else if (NPC.ai[0] == 2) //melee
            {
				NPC.ai[1]++;
				if(NPC.ai[1] >= 30)
                {
					NPC.ai[1] = 0;
					NPC.frameCounter = 0;
					NPC.ai[0] = 0;
                } else if(NPC.ai[1] == 15)
                {
					Main.NewText("Melee!");
                }
            }
		}
		public bool checkTileToDir(int direction, Vector2 pos)
		{
			Tile targetTile = Main.tile[(int)(pos.X / 16f)+direction, (int)(pos.Y / 16f)];
			return (targetTile != null && targetTile.HasTile && Main.tileSolid[targetTile.TileType]);
		}
        public override void FindFrame(int frameHeight)
        {
            if (NPC.ai[0] == 0)
            {
				if (Main.player[NPC.target].Center.Distance(NPC.Center) > 1000f)
				{
					NPC.frame.Y = 0;
				} else if (Main.player[NPC.target].Center.Distance(NPC.Center) <= 50f)
				{
					NPC.frame.Y = 0;
                }
				else
				{
					if (NPC.frameCounter > 3)
					{
						if (NPC.frame.Y < 16 * frameHeight)
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
            } else if (NPC.ai[0] == 1)
            {
				NPC.frame.Y = 0;
            } else if (NPC.ai[0] == 2)
            {
				NPC.frame.Y = 0;
            }
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange([
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
                new FlavorTextBestiaryInfoElement($"Mods.{Mod.Name}.Bestiary.{Name}")
            ]);
        }
    }
}
