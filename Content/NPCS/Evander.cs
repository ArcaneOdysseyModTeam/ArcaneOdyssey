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
			Main.npcFrameCount[NPC.type] = 17;
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
			NPC.ai[0] = 0;
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

	}
}
