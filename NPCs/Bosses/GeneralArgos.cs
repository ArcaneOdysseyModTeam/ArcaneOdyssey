using ArcaneOdyssey.Biomes;
using ArcaneOdyssey.Items.Materials;
using ArcaneOdyssey.Items.Weapons.RavennaLion;
using ArcaneOdyssey.NPCs.Base;
using ArcaneOdysseyMusic;
using Terraria.Audio;
using Terraria.Chat;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;

namespace ArcaneOdyssey.NPCs.Bosses
{
	[AutoloadBossHead]
	public class GeneralArgos : BaseNPC
	{
		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[Type] = 1;
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			NPC.lifeMax = 3000;
			NPC.damage = 30;
			NPC.knockBackResist = 0f;
			NPC.defense = 25;
			NPC.width = 26;
			NPC.height = 50;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.friendly = false;
			NPC.boss = true;
			NPC.aiStyle = NPCAIStyleID.BrainOfCthulhu; // placeholder
			SpawnModBiomes = [AOUtils.BiomeType<TalosBiome>()];
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.Add(new FlavorTextBestiaryInfoElement($"Mods.{Mod.Name}.Bestiary.{Name}"));
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			// for some reason guards are 4 pixels in the ground instead of 2
			spriteBatch.Draw(Sprite.Value, NPC.Center + new Vector2(0, 2) - screenPos, NPC.frame, NPC.GetAlpha(drawColor), NPC.rotation, NPC.frame.Size() / 2f, NPC.scale, NPC.spriteDirection < 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
			return false;
		}

		public override MusicTrack Theme => MusicTrack.Argos;

		private bool playedSound = false;

		public override bool PreAI()
		{
			if (ArgosSpawningSystem.InProgress)
			{
				NPC.velocity.X = 0;
				NPC.GravityMultiplier *= 5;
				if (!playedSound && NPC.Distance(ArgosSpawningSystem.argosArena.Center()) > 16 * 3f && NPC.velocity.Y <= 0.1f)
				{
					SoundEngine.PlaySound(SoundID.DD2_ExplosiveTrapExplode with { Volume = 2f }, NPC.Bottom);
					Main.musicFade[Music] = 1f;
					playedSound = true;
					ArgosSpawningSystem.musicStopped = false;
				}
				return false;
			}
			else
			{
				return true;
			}
		}

		public override void AI()
		{

		}

		public override void OnKill()
		{
			DownedBosses.DownedArgos = true;
			ArgosSpawningSystem.hasArgosKey = false;
			if (Main.dedServ)
			{
				ChatHelper.BroadcastChatMessage(ArcaneOdysseyMod.Instance.CustomLocalization($"RandomWords.Downed", DisplayName.Value).ToNetworkText(), new Color(175, 75, 255));
				var packet = Mod.GetPacket();
				packet.Write(ArcaneOdysseyMod.PacketID.MarkGlobalDowned);
				packet.Write(Type);
				packet.Send();
			}
			else
			{
				GlobalData.MarkDefeated(this);
				Main.NewText(ArcaneOdysseyMod.Instance.CustomLocalization($"RandomWords.Downed", DisplayName.Value).Value, new Color(175, 75, 255));
			}
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			LeadingConditionRule leadingConditionRule1 = new(new Conditions.NotExpert());
			leadingConditionRule1.OnSuccess(new AnyDropHelper([
					ModContent.ItemType<LanceofLoyalty>(),
					ModContent.ItemType<LionsHalberd>(),
					], 4));
			leadingConditionRule1.OnSuccess(new CommonDrop(ModContent.ItemType<BronzeBar>(), 1, 30, 45));
			npcLoot.Add(leadingConditionRule1);
			// add boss bag later
		}
	}

	public class ArgosSpawningSystem : ModSystem
	{
		public static bool justStarted, hasArgosKey, musicStopped;
		public static bool InProgress => timeLeft > 0;
		public static int playerInAnimation, timeLeft;
		public static Rectangle argosArena => new Rectangle(855, 119, 148, 34).ToWorldRect();

		public override void NetSend(BinaryWriter writer)
		{
			writer.Write(hasArgosKey);
		}

		public override void NetReceive(BinaryReader reader)
		{
			hasArgosKey = reader.ReadBoolean();
		}

		public override void ModifyScreenPosition()
		{
			if (InProgress)
			{
				if (AOUtils.NPCAlive<GeneralArgos>(out var argos))
				{
					Main.screenPosition = argos.Center - (Main.ScreenSize.ToVector2() / 2f);
				}
			}
		}
		public static byte Packet { get; private set; }

		public override void Load()
		{
			ArcaneOdysseyMod.HandleExtraPacket += ArgosSpawnAnimation;
			Packet = ArcaneOdysseyMod.PacketID.ExtraHandle++;
		}

		public static void StartAnimation(int player = 0)
		{
			hasArgosKey = true;
			timeLeft = 40;
			musicStopped = true;
			justStarted = true;
			playerInAnimation = player;
			if (Main.netMode == NetmodeID.MultiplayerClient)
			{
				var packet = ArcaneOdysseyMod.Instance.GetPacket();
				packet.Write(Packet);
				packet.Write((byte)player);
				packet.Send();
			}
			else
			{
				NPC.SpawnBoss(argosArena.Center.X, argosArena.Center.Y, ModContent.NPCType<GeneralArgos>(), playerInAnimation);
			}
		}

		private static void ArgosSpawnAnimation(BinaryReader reader, int whoAmI, byte command)
		{
			if (command == Packet)
			{
				timeLeft = 40;
				justStarted = true;
				musicStopped = true;
				playerInAnimation = reader.ReadByte();

				if (Main.dedServ)
				{
					var packet = ArcaneOdysseyMod.Instance.GetPacket();
					packet.Write(Packet);
					packet.Write((byte)playerInAnimation);
					packet.Send(ignoreClient: whoAmI);

					NPC.SpawnBoss(argosArena.Center.X, argosArena.Center.Y, ModContent.NPCType<GeneralArgos>(), playerInAnimation);
				}
				else
				{
					Main.LocalPlayer.ChangeSpawn(argosArena.Left + (2 * Main.myPlayer), argosArena.Bottom - 2);
					if (Main.myPlayer != playerInAnimation)
					{
						Main.LocalPlayer.SetToSpawnPosition();
					}
				}
			}
		}

		public override void Unload()
		{
			ArcaneOdysseyMod.HandleExtraPacket -= ArgosSpawnAnimation;
			ArcaneOdysseyMod.PacketID.ExtraHandle--;
		}

		private static float ogZoom = 1f;

		public override void PreUpdatePlayers()
		{
			if (InProgress)
			{
				if (!Main.dedServ)
				{
					Main.ClosePlayerChat();
					if (musicStopped)
					{
						for (int i = 0; i < Main.musicFade.Length; i++)
						{
							Main.musicFade[i] = 0;
						}
					}
					if (justStarted)
					{
						ogZoom = Main.GameZoomTarget;
						justStarted = false;
					}
					Main.GameZoomTarget = 1.5f;
				}
			}
		}

		public override void PostUpdatePlayers()
		{
			if (InProgress)
			{
				timeLeft--;
				if (timeLeft <= 0)
				{
					Main.GameZoomTarget = ogZoom;
				}
				if (!Main.dedServ)
				{
					var player = Main.player[playerInAnimation];
					if (justStarted)
					{
						player.velocity = new(-10, -5);
						player.Bottom = argosArena.Bottom();
					}
				}
			}
		}
	}
}
