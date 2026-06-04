using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Chat;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.NPCs.Minibosses
{
	public abstract class Miniboss : BaseNPC
	{
		public abstract int AOHealth { get; }

		public virtual int WalkingSpriteCount => 16;
		public virtual int AttackingSpriteCount => 8;

		public float MoveSpeed => .2f * MovespeedMulti;

		public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
		{
			NPC.lifeMax = (int)(NPC.lifeMax * 0.8f * balance);
		}

		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[Type] = WalkingSpriteCount + AttackingSpriteCount;
			NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new() { Velocity = 1f };
			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
			ExternalModSupport.DeclareMiniboss(Type);
		}

		public Texture2D Sprite => ModContent.Request<Texture2D>(Texture).Value;

		public override bool CanHitPlayer(Player target, ref int cooldownSlot) => false;

		public abstract List<int> RangedProjectiles { get; }

		public abstract List<int> MeleeProjectiles { get; }

		public abstract bool Downed { get; set; }

		public virtual float ShootSpeed => 5f;

		public virtual float MovespeedMulti => 1f;
		public float MaxSpeed => 40 * MoveSpeed;

		public override void SetDefaults()
		{
			base.SetDefaults();
			NPC.aiStyle = NPCAIStyleID.FaceClosestPlayer;
			NPC.knockBackResist = 0.1f;
			NPC.lifeMax = (AOHealth / 2) + (AOHealth / 4);
			NPC.rarity = 4;
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange([
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
				new FlavorTextBestiaryInfoElement($"Mods.{Mod.Name}.Bestiary.{Name}")
			]);
		}

		public override bool? CanFallThroughPlatforms() => (NPC.HasValidTarget && ((Main.player[NPC.target].Bottom.Y - 10) > NPC.Bottom.Y)) || NPC.noTileCollide;


		public override void ModifyHoverBoundingBox(ref Rectangle boundingBox)
		{
			boundingBox = NPC.Hitbox.Scaled(2.5f);
		}

		public static bool AOMinibossOrBossAlive()
		{
			foreach (var npc in Main.ActiveNPCs)
			{
				if (npc.boss || npc.ModNPC is Miniboss)
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

		public static bool TileInOuterThirds(int i, int j)
		{
			return (i < (Main.maxTilesX / 3) || i > (Main.maxTilesX / 3 * 2)) && j < Main.UnderworldLayer;
		}

		public abstract bool ExtraConditions { get; }

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (TileInOuterThirds(spawnInfo.SpawnTileX, spawnInfo.SpawnTileY) || !Downed)
			{
				if (!AOMinibossOrBossAlive())
				{
					if (spawnInfo.SpawnTileY < Main.UnderworldLayer)
					{
						if (ExtraConditions)
						{
							if (!spawnInfo.Player.ZoneRockLayerHeight)
							{
								if (!spawnInfo.SafeRangeX)
								{
									if (!spawnInfo.PlayerSafe)
									{
										if (!spawnInfo.Invasion)
										{
											if (!spawnInfo.Water)
											{
												if (!spawnInfo.Sky)
												{
													return 1f / 150f;
												}
											}
										}
									}
								}
							}
						}
					}
				}
			}
			return 0f;
		}

		public abstract Color Motif { get; }

		public bool hasSaidMesage = false;

		private bool canJump = false;

		public override void AI()
		{
			if (!hasSaidMesage)
			{
				NPC.NPCDialogue(this.GetLocalizedValue("SpawnMessage"), Motif);
				hasSaidMesage = true;
			}
			bool stuckintile = Main.tile[(int)(NPC.Center.X / 16f), (int)(NPC.Center.Y / 16f)].IsTileReallySolidGround();
			if (NPC.ai[0] == 0) //Chase
			{
				// Chase the nearest player
				NPC.ai[1]++;
				NPC.TargetClosest();
				if (NPC.HasValidTarget && Main.player[NPC.target].Center.Distance(NPC.Center) <= 1000f)
				{
					// Limit chasing distance
					NPC.velocity.X += NPC.direction * MoveSpeed;
					if (NPC.ai[2] == 0 && !stuckintile && Main.player[NPC.target].Center.Distance(NPC.Center) <= 50f)
					{
						// Attack meelee or stop
						NPC.velocity.X = 0f;
						if (NPC.ai[1] >= 60)
						{
							NPC.ai[0] = 2;
							NPC.frameCounter = 0;
							NPC.ai[1] = 0;
						}
					}
					else if (NPC.ai[2] == 0 && !stuckintile && NPC.ai[1] > 130 && Main.player[NPC.target].Center.Distance(NPC.Center) <= 900f && Main.player[NPC.target].Center.Distance(NPC.Center) >= 100f)
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

				if (Math.Abs(NPC.velocity.X) < MoveSpeed)
				{
					NPC.velocity.X = 0f;
				}

				if (Math.Abs(NPC.velocity.X) <= MoveSpeed && NPC.HasValidTarget && (Math.Abs(NPC.Center.Y - Main.player[NPC.target].Center.Y) > 16))
				{
					NPC.ai[2]++; // stuck counter
				}
				else
				{
					NPC.ai[2] = 0; // stuck counter
				}

				if (NPC.HasValidTarget && (NPC.ai[2] >= 100 || stuckintile))
				{
					NPC.noTileCollide = true;
					NPC.noGravity = true;
					NPC.velocity = NPC.SafeDirectionTo(Main.player[NPC.target].Center) * 3f;
					NPC.velocity.Y -= 2f;
				}
				else
				{
					NPC.noTileCollide = false;
					NPC.noGravity = false;
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
			}
			else if (NPC.ai[0] == 1 && NPC.HasValidTarget) // ranged
			{
				NPC.ai[1]++;
				NPC.velocity.X *= 0.7f;
				if (NPC.HasValidTarget && NPC.ai[1] == ((Main.npcFrameCount[Type] - WalkingSpriteCount) * 2) && Main.netMode != NetmodeID.MultiplayerClient)
				{
					Vector2 aimDir = NPC.SafeDirectionTo(Main.player[NPC.target].Center + (Main.player[NPC.target].velocity * 20f));
					Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, aimDir * ShootSpeed, Main.rand.Next(AOUtils.ShuffledList(RangedProjectiles)), NPC.damage, 4.5f);
				}
			}
			else if (NPC.ai[0] == 2 && NPC.HasValidTarget) //melee
			{
				NPC.ai[1]++;
				if (NPC.ai[1] == ((Main.npcFrameCount[Type] - WalkingSpriteCount) * 2) && Main.netMode != NetmodeID.MultiplayerClient)
				{
					List<int> melee = [.. MeleeProjectiles];
					List<int> activetypes = [];
					foreach (var a in Main.ActiveProjectiles)
					{
						activetypes.Add(a.type);
					}
					melee.RemoveAll(activetypes.Contains);
					if (melee.Count > 0)
						Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.Zero, Main.rand.Next(AOUtils.ShuffledList(melee)), NPC.damage, 4.5f);
				}
			}
		}

		public override void OnKill()
		{
			Downed = true;
			if (!Main.dedServ)
			{
				Main.NewText(ArcaneOdysseyMod.Instance.CustomLocalization($"RandomWords.Downed", DisplayName.Value).Value, new Color(175, 75, 255));
			}
			else
			{
				ChatHelper.BroadcastChatMessage(ArcaneOdysseyMod.Instance.CustomLocalization($"RandomWords.Downed", DisplayName.Value).ToNetworkText(), new Color(175, 75, 255));
			}
		}

		public static bool CheckTileToDir(int direction, Vector2 pos)
		{
			Tile targetTile = Main.tile[(int)float.Floor(pos.X / 16f) + direction, (pos.Y / 16f).Round()];
			return targetTile != null && targetTile.HasTile && Main.tileSolid[targetTile.TileType] && !targetTile.IsActuated;
		}

		public override void FindFrame(int frameHeight)
		{
			if (NPC.HasValidTarget)
			{
				if (NPC.ai[0] == 0)
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
								NPC.frame.Y = 0;
							}
							NPC.frameCounter = 0;
						}
						NPC.frameCounter++;
					}
				}
				else if (NPC.ai[0] == 1)
				{
					if (NPC.frameCounter++ > 2)
					{
						if (NPC.frame.Y < ((Main.npcFrameCount[Type] - 1) * frameHeight) && NPC.frame.Y >= ((WalkingSpriteCount - 1) * frameHeight))
						{
							NPC.frame.Y += frameHeight;
						}
						else
						{
							if (NPC.frame.Y < ((Main.npcFrameCount[Type] - 1) * frameHeight))
							{
								NPC.frame.Y = (WalkingSpriteCount - 1) * frameHeight;
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
				else if (NPC.ai[0] == 2)
				{
					if (NPC.frameCounter++ > 2)
					{
						if (NPC.frame.Y >= ((WalkingSpriteCount - 1) * frameHeight) && NPC.frame.Y < ((Main.npcFrameCount[Type] - 1) * frameHeight))
						{
							NPC.frame.Y += frameHeight;
						}
						else
						{
							if (NPC.frame.Y < ((Main.npcFrameCount[Type] - 1) * frameHeight))
							{
								NPC.frame.Y = (WalkingSpriteCount - 1) * frameHeight;
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
	}
}
