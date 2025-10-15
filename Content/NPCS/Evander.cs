using ArcaneOdyssey.Content.Projectiles.Enemies;
using Microsoft.Xna.Framework;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.NPCS
{
	public class Evander : ModNPC
	{
		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[Type] = 27;
			NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new() { Velocity = 1f };
			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
			ExternalModSupport.DeclareMiniboss(Type);
		}

		public override void SetDefaults()
		{
			NPC.lifeMax = 10000;
			NPC.knockBackResist = 0f;
			NPC.defense = 20;
			NPC.height = 44;
			NPC.width = 20;
			//Sprite height 96
			//Sprite width 76
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.aiStyle = 0;
			//NPC.ai[0] state
			//NPC.ai[1] state time
		}

		private bool canJump = false;

		public override void AI()
		{
			if (NPC.ai[0] == 0) //Chase
			{// Chase the nearest player
				NPC.ai[1]++;
				NPC.TargetClosest();
				if (NPC.HasValidTarget && Main.player[NPC.target].Center.Distance(NPC.Center) <= 1000f)
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
					}
					else if (NPC.ai[1] > 130 && Main.player[NPC.target].Center.Distance(NPC.Center) <= 900f && Main.player[NPC.target].Center.Distance(NPC.Center) >= 100f)
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
				if (CheckTileToDir(NPC.direction, NPC.Bottom + new Vector2(0f, -16f)) && canJump)
				{
					NPC.velocity.Y = -5f;
				}
				canJump = CheckTileToDir(0, NPC.Bottom) && Math.Abs(NPC.velocity.Y) < 0.01f;
			} 
			else if (NPC.ai[0] == 1 && NPC.HasValidTarget) // col cleave
			{
				NPC.ai[1]++;
				NPC.velocity.X *= 0.7f;
				if (NPC.ai[1] >= 20)
				{
					NPC.ai[0] = 0;
					NPC.ai[1] = 0;
					NPC.frameCounter = 0;
				} 
				else if (NPC.HasValidTarget && NPC.ai[1] == 15)
				{
					Vector2 aimDir = NPC.Center.DirectionTo(Main.player[NPC.target].Center);
					var proj = Projectile.NewProjectileDirect(NPC.GetSource_FromThis(), NPC.position, aimDir * 5, ModContent.ProjectileType<EvanderSlash>(), 25, 4.5f);
					proj.Center = NPC.Center;
				}
			} 
            else if (NPC.ai[0] == 2 && NPC.HasValidTarget) //melee
			{
				NPC.ai[1]++;
				if(NPC.ai[1] >= 20)
				{
					NPC.ai[1] = 0;
					NPC.frameCounter = 0;
					NPC.ai[0] = 0;
				} 
				else if (NPC.ai[1] == 10)
				{
					Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center.X, NPC.Center.Y, 0f, 0f, ModContent.ProjectileType<EvanderMelee>(), 50, 4.5f);
				}
			}
		}

		public static bool CheckTileToDir(int direction, Vector2 pos)
		{
			Tile targetTile = Main.tile[(int)(pos.X / 16f)+direction, (int)(pos.Y / 16f)];
			return targetTile != null && targetTile.HasTile && Main.tileSolid[targetTile.TileType];
		}

		public override void FindFrame(int frameHeight)
		{
			if(NPC.HasValidTarget) {
			if (NPC.HasValidTarget && NPC.ai[0] == 0)
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
						if (NPC.frame.Y < 16 * frameHeight && NPC.frame.Y > 0 * frameHeight)
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
			else if (NPC.ai[0] == 1)
			{
				if (NPC.frameCounter > 2)
				{
					if (NPC.frame.Y < 27 * frameHeight && NPC.frame.Y > 16 * frameHeight)
					{
						NPC.frame.Y += frameHeight;
					}
					else
					{
						NPC.frame.Y = frameHeight * 17;
					}
					NPC.frameCounter = 0;
				}
				NPC.frameCounter++;
			}
			else if (NPC.ai[0] == 2)
			{
				if (NPC.frameCounter > 2)
				{
					if (NPC.frame.Y < 27 * frameHeight && NPC.frame.Y > 16 * frameHeight)
					{
						NPC.frame.Y += frameHeight;
					}
					else
					{
						NPC.frame.Y = frameHeight * 17;
					}
					NPC.frameCounter = 0;
				}
				NPC.frameCounter++;
			}
			} else
            {
                NPC.frame.Y = 0;
            }
		}

		public override void HitEffect(NPC.HitInfo hit)
		{
			if (!Main.dedServ)
				for (int n = 0; n < 3; n++)
				{
					Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(NPC.position.X + (NPC.width / 2f), NPC.position.Y + (NPC.height / 2f)), 1, 1, DustID.Blood, (Main.rand.NextFloat() - 0.5f) * 3f, (Main.rand.NextFloat() - 0.5f) * 8f, 0, default, 1f)];
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
