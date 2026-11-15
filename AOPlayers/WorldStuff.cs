using ArcaneOdyssey.Buffs;
using ArcaneOdyssey.NPCs.Bosses;
using ArcaneOdyssey.Subworlds;
using SubworldLibrary;
using System;
using Terraria.DataStructures;

namespace ArcaneOdyssey.AOPlayers
{
	public partial class AOPlayer
	{
		private byte _insanity = 0;
		/// <summary>
		/// Player insanity level, use += to increase
		/// </summary>
		public byte Insanity
		{
			get
			{
				return Math.Clamp(_insanity, (byte)0, (byte)5);
			}
			set
			{
				_insanity = Math.Clamp(value, (byte)0, (byte)5);
			}
		}

		private byte _banishment = 0;
		public byte Banishment
		{
			get
			{
				return Math.Clamp(_banishment, (byte)0, (byte)5);
			}
			set
			{
				_banishment = Math.Clamp(value, (byte)0, (byte)5);
			}
		}

		public byte BronzeSealed { get; internal set; } = 0;
		public byte DarkSealed { get; internal set; } = 0;
		public byte NimbusSealed { get; internal set; } = 0;
		public byte[] AllSealed
		{
			get
			{
				return [BronzeSealed, DarkSealed, NimbusSealed];
			}
			internal set
			{
				BronzeSealed = value[0];
				DarkSealed = value[1];
				NimbusSealed = value[2];
			}
		}

		public const byte MaxSealedChests = byte.MaxValue;

		public void AddBronzeSealed(int amount)
		{ 
			var newval = BronzeSealed + amount;
			if (newval >= MaxSealedChests)
			{
				Main.NewText(Mod.CustomLocalization($"{Name}.SealedChestsFull"), Color.Red);
				newval = MaxSealedChests;
			}
			BronzeSealed = (byte)newval;
			SyncChests();
		}

		public void AddNimbusSealed(int amount)
		{
			var newval = NimbusSealed + amount;
			if (newval >= MaxSealedChests)
			{
				Main.NewText(Mod.CustomLocalization($"{Name}.SealedChestsFull"), Color.Red);
				newval = MaxSealedChests;
			}
			NimbusSealed = (byte)newval;
			SyncChests();
		}

		public void AddDarkSealed(int amount)
		{
			var newval = DarkSealed + amount;
			if (newval >= MaxSealedChests)
			{
				Main.NewText(Mod.CustomLocalization($"{Name}.SealedChestsFull"), Color.Red);
				newval = MaxSealedChests;
			}
			DarkSealed = (byte)newval;
			SyncChests();
		}
		public static byte SyncChestsPacket => ModContent.GetInstance<SyncSealedChestsSystem>().Packet;

		internal void SyncChests()
		{
			if (!AOUtils.Singleplayer)
			{
				var packet = Mod.NewPacket(SyncChestsPacket);
				packet.Write(AllSealed);
				packet.Send();
			}
		}

		public override void UpdateDead()
		{
			BronzeSealed = 0;
			DarkSealed = 0;
			NimbusSealed = 0;
			Blocking = false;
		}

		public ushort eliusArenaCounter = 0;

		public void AddInsanityDebuff()
		{
			switch (Insanity)
			{
				case 1:
					Player.AddBuff(ModContent.BuffType<InsanityOne>(), 2);
					break;
				case 2:
					Player.AddBuff(ModContent.BuffType<InsanityTwo>(), 2);
					break;
				case 3:
					Player.AddBuff(ModContent.BuffType<InsanityThree>(), 2);
					break;
				case 4:
					Player.AddBuff(ModContent.BuffType<InsanityFour>(), 2);
					break;
				case 5:
					Player.AddBuff(ModContent.BuffType<InsanityFive>(), 2);
					break;
			}
		}

		public void SubworldEffects()
		{
			Player.noBuilding = true;
			Player.noFallDmg = false;
			Player.equippedWings = null;
			Player.slowFall = false;
			Player.rocketBoots = 0;
			if (jumpCount > 0)
				Player.blockExtraJumps = true;
			Player.breathEffectiveness *= 0f;
			Player.waterWalk = false;
			Player.waterWalk2 = false;
			Player.canFloatInWater = false;
			Player.wingsLogic = 0;
			Player.wings = 0;
			Player.mount?.Dismount(Player);
			Player.RemoveAllGrapplingHooks();

			if (Main.myPlayer == Player.whoAmI && SubworldSystem.Current is TalosSubworld && ArgosSpawningSystem.hasArgosKey)
			{
				if (ArgosSpawningSystem.argosArena.Bottom().Distance(Player.Bottom) < 16f)
				{
					if (!ArgosSpawningSystem.InProgress && !AOUtils.BossAlive)
					{
						ArgosSpawningSystem.StartAnimation(Main.myPlayer);
					}
				}
			}
		}

		private static void DarkSeaNoTeleport(On_Player.orig_ItemCheck_UseTeleportRod orig, Player self, Item sItem)
		{
			if (!AOUtils.InAOSubworld)
				orig.Invoke(self, sItem);
		}
	}

	public class SyncSealedChestsSystem : AONetSystem
	{
		protected override void HandlePacket(BinaryReader reader, int whoAmI, byte command)
		{
			if (command == Packet)
			{
				if (Main.dedServ)
				{
					var player = Main.player[whoAmI].ArcaneOdyssey();
					player.AllSealed = reader.ReadBytes(3);
					var packet = Mod.NewPacket(Packet);
					packet.Write((byte)whoAmI);
					packet.Write(player.AllSealed);
					packet.Send(ignoreClient: whoAmI);
				}
				else
				{
					var player = Main.player[reader.ReadByte()].ArcaneOdyssey();
					player.AllSealed = reader.ReadBytes(3);
				}
			}
		}
	}

	public class UpdateSubworldSystem : ModSystem
	{
		public override void PreUpdateWorld()
		{
			if (AOUtils.InAOSubworld)
			{
				if (Main.dayTime)
				{
					Main.time += Main.dayRate;
					if (Main.time > Main.dayLength)
					{
						Main.dayTime = false;
						Main.time = 0;
					}
				}
				else
				{
					Main.time += Main.dayRate;
					if (Main.time > Main.nightLength)
					{
						Main.dayTime = true;
						Main.time = 0;
					}
				}

				// Update mechanisms
				Wiring.UpdateMech();

				// Update tile entities
				TileEntity.UpdateStart();
				foreach (TileEntity te in TileEntity.ByID.Values)
				{
					te.Update();
				}
				TileEntity.UpdateEnd();

				// do NOT update liquid
				//if (++Liquid.skipCount > 1)
				//{
				//	Liquid.UpdateLiquid();
				//	Liquid.skipCount = 0;
				//}
			}
		}
	}
}
