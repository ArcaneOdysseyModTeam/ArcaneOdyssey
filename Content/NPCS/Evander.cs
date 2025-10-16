using ArcaneOdyssey.Content.Items.Weapons;
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
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.Chat;
using ArcaneOdyssey.VFX.Gores;
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
			NPC.lifeMax = 5000;
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
							NPC.position.X += NPC.direction * 3;
							NPC.ai[0] = 2;
							NPC.frameCounter = 0;
							NPC.ai[1] = 0;
						}
					}
					else if (NPC.ai[1] > 130 && Main.player[NPC.target].Center.Distance(NPC.Center) <= 900f && Main.player[NPC.target].Center.Distance(NPC.Center) >= 100f)
					{
						NPC.position.X += NPC.direction * 3;
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
				bool tileUnderIsFlat = Main.tile[(int)(NPC.Bottom.X / 16f), (int)(NPC.Bottom.Y / 16f)].IsHalfBlock;
				bool tileNextToFlatTile = Main.tileSolid[Main.tile[(int)(NPC.Bottom.X / 16f)+NPC.direction, (int)(NPC.Bottom.Y / 16f)].TileType] && !Main.tile[(int)(NPC.Bottom.X / 16f)+NPC.direction, (int)(NPC.Bottom.Y / 16f)].IsActuated && !Main.tile[(int)(NPC.Bottom.X / 16f)+NPC.direction, (int)(NPC.Bottom.Y / 16f)].IsHalfBlock;
				// Jump if there's a block
				if (CheckTileToDir(NPC.direction, NPC.Bottom + new Vector2(0f, -16f)) && canJump)
				{
					NPC.velocity.Y = -5f;
				} else if (tileUnderIsFlat) {
					if (tileNextToFlatTile && (NPC.ai[1] % 5 == 1)) {
						NPC.velocity.Y = -2f;
					}
				} else if (NPC.wet && (NPC.ai[1] % 3 == 1))
				{
					NPC.velocity.Y = -1f;
				}
				canJump = (CheckTileToDir(0, NPC.Bottom) || CheckTileToDir(0, NPC.BottomLeft) || CheckTileToDir(0, NPC.BottomRight)) && Math.Abs(NPC.velocity.Y) < 0.01f;
			} 
			else if (NPC.ai[0] == 1 && NPC.HasValidTarget) // col cleave
			{
				NPC.ai[1]++;
				NPC.velocity.X *= 0.7f;
				if (NPC.ai[1] >= 20)
				{
					NPC.position.X -= NPC.direction * 3;
					NPC.ai[0] = 0;
					NPC.ai[1] = 0;
					NPC.frameCounter = 0;
				} 
				else if (NPC.HasValidTarget && NPC.ai[1] == 15)
				{
					Vector2 aimDir = NPC.Center.DirectionTo(Main.player[NPC.target].Center);
					var proj = Projectile.NewProjectileDirect(NPC.GetSource_FromThis(), NPC.position, aimDir * 5, ModContent.ProjectileType<EvanderSlash>(), 35, 4.5f);
					proj.Center = NPC.Center;
				}
			} 
			else if (NPC.ai[0] == 2 && NPC.HasValidTarget) //melee
			{
				NPC.ai[1]++;
				if(NPC.ai[1] >= 20)
				{
					NPC.position.X -= NPC.direction * 3;
					NPC.ai[1] = 0;
					NPC.frameCounter = 0;
					NPC.ai[0] = 0;
				} 
				else if (NPC.ai[1] == 10)
				{
					Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center.X, NPC.Center.Y, 0f, 0f, ModContent.ProjectileType<EvanderMelee>(), 75, 4.5f);
				}
			}
		}

		public static bool CheckTileToDir(int direction, Vector2 pos)
		{
			Tile targetTile = Main.tile[(int)(pos.X / 16f)+direction, (int)(pos.Y / 16f)];
			return targetTile != null && targetTile.HasTile && (Main.tileSolid[targetTile.TileType] && !targetTile.IsActuated);
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
            {
                for (int n = 0; n < 3; n++)
                {
                    Dust.NewDust(new Vector2(NPC.position.X + (NPC.width / 2f), NPC.position.Y + (NPC.height / 2f)), 1, 1, DustID.Blood, (Main.rand.NextFloat() - 0.5f) * 3f, (Main.rand.NextFloat() - 0.5f) * 8f, 0, default, 1f);
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
                        Dust.NewDust(new Vector2(NPC.position.X + (NPC.width / 2f), NPC.position.Y + (NPC.height / 2f)), 1, 1, DustID.Blood, (Main.rand.NextFloat() - 0.5f) * 3f, (Main.rand.NextFloat() - 0.5f) * 8f, 0, default, 1f);
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

		public override void ModifyHoverBoundingBox(ref Rectangle boundingBox)
		{
			boundingBox.Width = 30;
			boundingBox.Height = 50;
			boundingBox.X = (int)NPC.position.X;
			boundingBox.Y = (int)NPC.position.Y;
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			var con = new LeadingConditionRule(new FirstEvanderKill());
			con.OnSuccess(new HecateDropMultiHelper(ModContent.ItemType<ColossalGreatsword>()));
			npcLoot.Add(con);
		}

        public override void OnKill()
        {
			DownedBosses.downedEvander = true;
			if (!Main.dedServ)
			{
				Main.NewText(Mod.CustomLocalization("NPCs.Evander.DeathMessage").Value, new Color(175,75,255));
            }
			else
			{
				NetMessage.SendData(MessageID.WorldData);
				ChatHelper.BroadcastChatMessage(Mod.CustomLocalization("NPCs.Evander.DeathMessage").ToNetworkText(), new Color(175,75,255));
			}
        }
	}

	public class EvanderSpawning : ModSystem
	{
		public override void PostUpdateWorld()
		{
			if (AOUtils.ServerOrSingleplayer && (!DownedBosses.downedEvander) && Main.hardMode && Main.dayTime)
			{
				foreach (var player in Main.ActivePlayers)
				{
					if (player.ZoneForest && (!player.ShoppingZone_AnyBiome) && PlayerInOuterThirds(player) && (!BossAlive()) && Main.rand.NextBool(300 * 60))
					{
						NPC.SpawnBoss(player.position.X.Round(), player.position.Y.Round() - Main.screenHeight, ModContent.NPCType<Evander>(), player.whoAmI);
					}
				}
			}
		}

		public static bool BossAlive()
		{
			foreach (var npc in Main.ActiveNPCs)
			{
				if (npc.boss || npc.ModNPC is Evander)
				{
					return true;
				}
			}
			return false;
		}

		public static bool PlayerInOuterThirds(Player player)
		{
			var worldwidth = Main.maxTilesX * 16f;
			return player.position.X < worldwidth / 3 || player.position.X > worldwidth / 3 * 2;
		}
	}
}
