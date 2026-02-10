using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Chat;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.NPCS
{
	[AutoloadBossHead]
	public abstract class AOMiniboss : ModNPC
	{
		public abstract int AOHealth { get; }

		public override void SetStaticDefaults()
		{
			MinibossSpawning.AllMinibosses.Add(this);
			Main.npcFrameCount[Type] = 27;
			NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new() { Velocity = 1f };
			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
			ExternalModSupport.DeclareMiniboss(Type);
		}

		public Texture2D Sprite => ModContent.Request<Texture2D>(Texture).Value;

		public override bool CanHitPlayer(Player target, ref int cooldownSlot) => canMelee;

		public bool canMelee = false;

		public abstract List<int> RangedProjectiles { get; }

		public abstract List<int> MeleeProjectiles { get; }

		public abstract bool Downed { get; set; }

		public virtual float ShootSpeed => 5f;

		public virtual float MovespeedMulti => 1f;

		public override void SetDefaults()
		{
			NPC.aiStyle = NPCAIStyleID.FaceClosestPlayer;
			NPC.knockBackResist = 0f;
			NPC.lifeMax = AOHealth / 2;
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
			boundingBox = NPC.Hitbox;
		}

		public abstract bool ExtraConditions { get; }

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
						if (NPC.ai[1] >= 60)
						{

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
				bool tileUnderIsFlat = Main.tile[(int)(NPC.Bottom.X / 16f), (int)(NPC.Bottom.Y / 16f)].IsHalfBlock;
				bool tileNextToFlatTile = Main.tileSolid[Main.tile[(int)(NPC.Bottom.X / 16f) + NPC.direction, (int)(NPC.Bottom.Y / 16f)].TileType] && !Main.tile[(int)(NPC.Bottom.X / 16f) + NPC.direction, (int)(NPC.Bottom.Y / 16f)].IsActuated && !Main.tile[(int)(NPC.Bottom.X / 16f) + NPC.direction, (int)(NPC.Bottom.Y / 16f)].IsHalfBlock;
				// Jump if there's a block
				if (CheckTileToDir(NPC.direction, NPC.Bottom + new Vector2(0f, -16f)) && canJump)
				{
					NPC.velocity.Y = -7f;
				}
				else if (tileUnderIsFlat)
				{
					if (tileNextToFlatTile && (NPC.ai[1] % 5 == 1))
					{
						NPC.velocity.Y = -2f;
					}
				}
				else if (NPC.wet && (NPC.ai[1] % 3 == 1))
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

					NPC.ai[0] = 0;
					NPC.ai[1] = 0;
					NPC.frameCounter = 0;
				}
				else if (NPC.HasValidTarget && NPC.ai[1] == 15 && Main.netMode != NetmodeID.MultiplayerClient)
				{
					Vector2 aimDir = NPC.Center.DirectionTo(Main.player[NPC.target].Center);
					Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, aimDir * ShootSpeed, Main.rand.Next(RangedProjectiles), NPC.damage, 4.5f);
				}
			}
			else if (NPC.ai[0] == 2 && NPC.HasValidTarget) //melee
			{
				NPC.ai[1]++;
				if (NPC.ai[1] >= 20)
				{

					NPC.ai[1] = 0;
					NPC.frameCounter = 0;
					NPC.ai[0] = 0;
				}
				else if (NPC.ai[1] == 10 && Main.netMode != NetmodeID.MultiplayerClient)
				{
					List<int> melee = [.. MeleeProjectiles];
					List<int> activetypes = [];
					foreach (var a in Main.ActiveProjectiles)
					{
						activetypes.Add(a.type);
					}
					melee.RemoveAll(activetypes.Contains);
					canMelee = !(melee.Count > 0);
					if (!canMelee)
						Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.Zero, Main.rand.Next(melee), NPC.damage, 4.5f);
				}
			}
		}

		public override void OnKill()
		{
			Downed = true;
			if (!Main.dedServ)
			{
				Main.NewText(Mod.CustomLocalization($"NPCs.{Name}.DeathMessage").Value, new Color(175, 75, 255));
			}
			else
			{
				NetMessage.SendData(MessageID.WorldData);
				ChatHelper.BroadcastChatMessage(Mod.CustomLocalization($"NPCs.{Name}.DeathMessage").ToNetworkText(), new Color(175, 75, 255));
			}
		}

		public static bool CheckTileToDir(int direction, Vector2 pos)
		{
			Tile targetTile = Main.tile[(int)(float.Floor(pos.X / 16f)) + direction, (pos.Y / 16f).Round()];
			return targetTile != null && targetTile.HasTile && (Main.tileSolid[targetTile.TileType] && !targetTile.IsActuated);
		}

		public override void FindFrame(int frameHeight)
		{
			if (NPC.HasValidTarget)
			{
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
			}
			else
			{
				NPC.frame.Y = 0;
			}
		}
	}

	public class MinibossSpawning : ModSystem
	{
		internal static List<AOMiniboss> AllMinibosses = [];

		public override void PostUpdateWorld()
		{
			if (AOUtils.ServerOrSingleplayer && Main.hardMode)
			{
				foreach (var miniboss in AOUtils.ShuffledList(AllMinibosses))
				{
					foreach (var player in Main.ActivePlayers)
					{
						if (miniboss.ExtraConditions && player.ZoneForest && (!player.ShoppingZone_AnyBiome) && PlayerInOuterThirds(player) && (!AOMinibossOrBossAlive()) && Main.rand.NextBool(miniboss.Downed ? 600 * 60 : 300 * 60))
						{
							NPC.SpawnOnPlayer(player.whoAmI, miniboss.Type);
						}
					}
				}
			}
		}

		public static bool AOMinibossOrBossAlive()
		{
			foreach (var npc in Main.ActiveNPCs)
			{
				if (npc.boss || npc.ModNPC is AOMiniboss)
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
