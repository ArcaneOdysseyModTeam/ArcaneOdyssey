using ArcaneOdyssey.Biomes;
using ArcaneOdyssey.Items.Pickups;
using ArcaneOdyssey.NPCs.Base;
using ArcaneOdyssey.Subworlds;
using ArcaneOdyssey.UI.SubWorld;
using SubworldLibrary;
using System;
using System.Linq;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;

namespace ArcaneOdyssey.NPCs.Enemy
{
	public class TalosGuard : BaseNPC
	{
		public const int Frames = 14;

		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[Type] = Frames;
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			NPC.defense = 8;
			NPC.lifeMax = 75;
			NPC.width = 26;
			NPC.height = 58;
			NPC.damage = 25;
			NPC.knockBackResist = .25f;
			NPC.HitSound = SoundID.NPCHit4;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.friendly = false;
			SpawnModBiomes = [AOUtils.BiomeType<TalosBiome>()];
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.Add(new FlavorTextBestiaryInfoElement($"Mods.{Mod.Name}.Bestiary.{Name}"));
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(new AnyDropHelper([ItemID.GladiatorHelmet, ItemID.GladiatorBreastplate, ItemID.GladiatorLeggings], 5));
			LeadingConditionRule keycondition = new(new ArgosKeyDrop());
			keycondition.OnSuccess(AOUtils.Common<ArgosKey>(6));
			npcLoot.Add(keycondition);
		}

		public byte roomIndex = byte.MaxValue;

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(roomIndex);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			roomIndex = reader.ReadByte();
		}

		private bool justSpawned = true;

		public override bool CheckActive() => roomIndex == byte.MaxValue;

		public override void AI()
		{
			if (NPC.HasValidTarget)
			{
				var player = Main.player[NPC.target];
				var targetX = (player.Center.X - NPC.Center.X).Normalize() * 1.5f;
				if (MathF.Abs(player.Center.X - NPC.Center.X) < 16 && player.Bottom.Y + 16 < NPC.Bottom.Y && NPC.velocity.Y == 0)
				{
					NPC.velocity.Y -= 6;
				}
				NPC.velocity = NPC.velocity.MoveTowards(new(targetX, NPC.velocity.Y), .2f);
			}

			NPC.direction = (NPC.velocity.X > 0).ToDirectionInt();
			NPC.spriteDirection = NPC.direction;

			if (roomIndex == byte.MaxValue)
			{
				if (!NPC.HasValidTarget)
				{
					NPC.TargetClosest();
				}
				else
				{
					if (NPC.position == NPC.oldPosition)
					{
						NPC.velocity.Y -= 6;
					}
				}

				return;
			}

			if (AOUtils.BossAlive)
			{
				Despawn();
			}

			if (justSpawned)
			{
				justSpawned = false;
				if (!Main.dedServ)
				{
					if (AOUtils.OnScreen(NPC.Hitbox))
					{
						for (int n = 0; n < 3; n++)
						{
							Dust spawnedDust = Main.dust[Dust.NewDust(NPC.Hitbox.TopLeft(), NPC.Hitbox.Width, NPC.Hitbox.Height, DustID.BubbleBurst_White, Scale: 3f * NPC.Hitbox.RelativeScale())];
							spawnedDust.noGravity = true;
						}
						SoundEngine.PlaySound(SoundID.AbigailSummon with { Volume = .5f }, NPC.Center);
					}
				}
			}

			if (NPC.HasValidTarget)
			{
				var player = Main.player[NPC.target];

				for (int i = 0; i < TalosSubworld.openRooms.Length; i++)
				{
					if (!player.Hitbox.ToTileRect().Intersects(TalosSubworld.openRooms[i]))
					{
						NPC.TargetClosest();
					}
					else
					{
						break;
					}
				}

				if (!NPC.HasValidTarget) Despawn();
			}
			else
			{
				NPC.TargetClosest();
				if (!NPC.HasValidTarget) Despawn();
			}

			void Despawn()
			{
				NPC.active = false;
				if (!Main.dedServ)
				{
					Dust spawnedDust = Main.dust[Dust.NewDust(NPC.Hitbox.TopLeft(), NPC.Hitbox.Width, NPC.Hitbox.Height, DustID.BubbleBurst_White, Scale: 3f * NPC.Hitbox.RelativeScale())];
					spawnedDust.noGravity = true;
				}
			}
		}

		public override void FindFrame(int frameHeight)
		{
			NPC.frameCounter += MathF.Abs(NPC.velocity.X);
			if (NPC.frameCounter >= 8)
			{
				NPC.frameCounter = 0;
				NPC.frame.Y += frameHeight;
				if (NPC.frame.Y >= frameHeight * Frames)
				{
					NPC.frame.Y = 0;
				}
			}
		}

		public bool TalosGuardTarget()
		{
			if (roomIndex == byte.MaxValue)
			{
				return false;
			}
			var playersSorted = Main.player.Where(e => e.active).OrderBy(e => e.Distance(NPC.Center) - e.aggro);
			foreach (var player in playersSorted)
			{
				if (!player.Hitbox.ToTileRect().Intersects(TalosSubworld.openRooms[roomIndex]))
				{
					continue;
				}
				else
				{
					NPC.target = player.whoAmI;
					if (NPC.target != NPC.oldTarget)
						NPC.netUpdate = true;
					return true;
				}
			}
			NPC.target = -1;
			if (NPC.target != NPC.oldTarget)
				NPC.netUpdate = true;
			return true;
		}

		public override void Load()
		{
			On_NPC.TargetClosest += On_NPC_TargetClosest;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			// for some reason guards are 4 pixels in the ground instead of 2
			spriteBatch.Draw(Sprite.Value, NPC.Center + new Vector2(0, 2) - screenPos, NPC.frame, NPC.GetAlpha(drawColor), NPC.rotation, NPC.frame.Size() / 2f, NPC.scale, NPC.spriteDirection < 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
			return false;
		}

		private static void On_NPC_TargetClosest(On_NPC.orig_TargetClosest orig, NPC self, bool faceTarget)
		{
			if (self.ModNPC is TalosGuard guard)
			{
				if (guard.TalosGuardTarget())
				{
					return;
				}
			}
			orig(self, faceTarget);
		}
	}

	public class TalosGuardSpawningSystem : ModSystem
	{
		private static int spawncd = 0;
		public override void PreUpdateWorld()
		{
			if (!AOUtils.BossAlive)
			{
				if ((AOUtils.Singleplayer && SubworldSystem.IsActive<TalosSubworld>()) || (!AOUtils.Singleplayer && PlayersSyncSubworld.current == ModContent.GetInstance<TalosSubworld>().FullName))
				{
					spawncd--;
					foreach (var player in Main.player.Where(e => e.active && !e.DeadOrGhost).OrderBy(e => e.aggro))
					{
						for (byte i = 0; i < TalosSubworld.openRooms.Length; i++)
						{
							if (player.Hitbox.ToTileRect().Intersects(TalosSubworld.openRooms[i]))
							{
								if (spawncd <= 0 && Main.rand.NextBool(60)) // every 12 seconds about
								{
									int x;
									do
									{
										x = Main.rand.Next(TalosSubworld.openRooms[i].Left, TalosSubworld.openRooms[i].Right) * 16;
									}
									while (player.Center.Distance(new Vector2(x, TalosSubworld.openRooms[i].Bottom * 16)) < (4 * 16));
									spawncd = 60 * 12;
									var guard = NPC.NewNPCDirect(new EntitySource_SpawnNPC(), x, TalosSubworld.openRooms[i].Bottom * 16, ModContent.NPCType<TalosGuard>(), target: player.whoAmI).ModNPC as TalosGuard;
									guard.roomIndex = i;
									return;
								}
							}
						}
					}
				}
			}
		}
	}
}
