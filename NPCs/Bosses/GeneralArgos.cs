using ArcaneOdyssey.NPCs.Base;
using ArcaneOdysseyMusic;
using System.IO;
using Terraria.Audio;

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
			NPC.lifeMax = 3000;
			NPC.damage = 0;
			NPC.knockBackResist = 0f;
			NPC.defense = 25;
			NPC.width = Player.defaultWidth;
			NPC.height = Player.defaultHeight;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.friendly = false;
			NPC.boss = true;
		}

		public override MusicTrack Theme => MusicTrack.Argos;

		private bool playedSound = false;
		public override void AI()
		{
			if (ArgoSpawningAnimation.InProgress)
			{
				NPC.velocity.X = 0;
				NPC.GravityMultiplier *= 5;
				if (!playedSound && NPC.Distance(ArgoSpawningAnimation.argosArena.Center()) > 16 * 3f && NPC.velocity.Y <= 0.1f)
				{
					SoundEngine.PlaySound(SoundID.DD2_ExplosiveTrapExplode, NPC.Bottom);
					playedSound = true;
				}
			}
			else
			{
				Music = Theme.MusicSlot;
			}
		}

		public override void OnKill()
		{

		}
	}

	public class ArgoSpawningAnimation : ModSystem
	{
		public static bool justStarted, hasArgosKey;
		public static bool InProgress => timeLeft > 0;
		public static int playerInAnimation;
		public static Rectangle argosArena => new Rectangle(855, 119, 148, 34).ToWorldRect();

		public static float timeLeft;

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
				playerInAnimation = reader.ReadByte();

				if (Main.dedServ)
				{
					var packet = ArcaneOdysseyMod.Instance.GetPacket();
					packet.Write(Packet);
					packet.Write((byte)playerInAnimation);
					packet.Send(ignoreClient: whoAmI);

					NPC.SpawnBoss(argosArena.Center.X, argosArena.Center.Y, ModContent.NPCType<GeneralArgos>(), playerInAnimation);
				}
			}
		}

		public override void Unload()
		{
			ArcaneOdysseyMod.HandleExtraPacket -= ArgosSpawnAnimation;
			ArcaneOdysseyMod.PacketID.ExtraHandle--;
		}

		private static float ogZoom;

		public override void PreUpdatePlayers()
		{
			if (InProgress)
			{
				if (!Main.dedServ)
				{
					for (int i = 0; i < Main.musicFade.Length; i++)
					{
						Main.musicFade[i] = 0;
					}
					var player = Main.player[playerInAnimation];
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
