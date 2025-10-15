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
			NPC.damage = 100;
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
			// Select state
			NPC.ai[0] = 0;
			if (NPC.ai[0] == 0) //Chase
			{// Chase the nearest player
				NPC.TargetClosest();
				if (Main.player[NPC.target].Center.Distance(NPC.Center) <= 1000f)
				{ // Limit chasing distance
					NPC.velocity.X += NPC.direction * 0.2f;
					if (Main.player[NPC.target].Center.Distance(NPC.Center) <= 20f)
					{ // Move away
						NPC.velocity.X *= -1f;
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
				if (Math.Abs(NPC.velocity.X) < 0.2f)
				{
					NPC.frame.Y = 0;
				}
				else
				{
					if (NPC.frameCounter < 3) {
						if (NPC.frame.Y < 17) {
							NPC.frame.Y += 1;
						} else
                        {
							NPC.frame.Y = 1;
                        }
						NPC.frameCounter++;
					} else
                    {
						NPC.frameCounter = 0;
                    }
                }
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
