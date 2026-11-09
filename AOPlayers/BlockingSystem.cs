using ArcaneOdyssey.Items.Armour.Centurion;
using ArcaneOdyssey.Items.Scrolls;
using System;

namespace ArcaneOdyssey.AOPlayers
{
	public partial class AOPlayer
	{
		public bool Blocking { get; private set; }
		private bool wasBlocking;

		public bool Parrying => Blocking && blockingTime > (MaxBlock - 25);

		public byte blockingTime = MaxBlock;

		public const byte MaxBlock = 120;

		public void HandleBlocking()
		{
			CheckBlocking();
			if (Main.myPlayer == Player.whoAmI)
			{
				if (Blocking != wasBlocking)
				{
					wasBlocking = Blocking;
					if (!AOUtils.Singleplayer)
					{
						var packet = ArcaneOdysseyMod.NewPacket(Packet);
						packet.Write(Blocking);
						packet.Write(blockingTime);
						packet.Send();
					}
				}
			}
		}

		public static byte Packet;

		public void RegisterPacket()
		{
			ArcaneOdysseyMod.HandleExtraPacket += SetBlockState;
			Packet = ArcaneOdysseyMod.PacketID.ExtraHandle++;
		}

		public override void Unload()
		{
			ArcaneOdysseyMod.HandleExtraPacket -= SetBlockState;
			ArcaneOdysseyMod.PacketID.ExtraHandle--;
		}

		public override void FrameEffects()
		{
			if (!Main.dedServ)
			{
				if (Blocking)
				{
					if (Player.shield == -1)
					{
						Player.shield = EquipLoader.GetEquipSlot(Mod, typeof(RavennaBoots).Name, EquipType.Shield);
					}
				}
				if (Parrying)
				{
					Player.shieldRaised = true;
				}
			}
		}

		private static void SetBlockState(BinaryReader reader, int whoAmI, byte command)
		{
			if (command == Packet)
			{
				bool blocking;
				byte blockingTime;
				if (Main.dedServ)
				{
					blocking = reader.ReadBoolean();
					blockingTime = reader.ReadByte();
					Main.player[whoAmI].ArcaneOdyssey().Blocking = blocking;
					Main.player[whoAmI].ArcaneOdyssey().blockingTime = blockingTime;
					var packet = ArcaneOdysseyMod.NewPacket(Packet);
					packet.Write((byte)whoAmI);
					packet.Write(blocking);
					packet.Write(blockingTime);
					packet.Send(ignoreClient: whoAmI);
				}
				else
				{
					whoAmI = reader.ReadByte();
					blocking = reader.ReadBoolean();
					blockingTime = reader.ReadByte();
					Main.player[whoAmI].ArcaneOdyssey().Blocking = blocking;
					Main.player[whoAmI].ArcaneOdyssey().blockingTime = blockingTime;
				}
			}
		}

		public override void PostUpdateRunSpeeds()
		{
			if (Blocking)
			{
				Player.maxRunSpeed /= 2;
				Player.accRunSpeed /= 2;
				Player.runAcceleration /= 2;
			}
		}

		public override void OnEnterWorld()
		{
			Player.QuickSpawnItem(Player.GetSource_FromThis(), ModContent.ItemType<VariantSwapper>()); // remove later
			blockingTime = MaxBlock;
			Blocking = false;
		}

		private void CheckBlocking()
		{
			if (Main.myPlayer == Player.whoAmI)
			{
				if (!(Immobile || Player.CCed || HeavySkillActive))
				{
					if (AOKeybinds.BlockAoEAttack.Current)
					{
						if (blockingTime > 0)
						{
							blockingTime--;
							Blocking = true;
							return;
						}
					}
					else
					{
						blockingTime = (byte)Math.Clamp(blockingTime + 2, 0, MaxBlock);
					}
				}
				if (wasBlocking)
				{
					blockingTime = (byte)Math.Clamp(blockingTime - 50, 0, MaxBlock);
				}
				Blocking = false;
			}
			else
			{
				if (Blocking)
				{
					blockingTime--;
					if (blockingTime <= 0)
					{
						Blocking = false;
					}
				}
				else
				{
					blockingTime = (byte)Math.Clamp(blockingTime + 2, 0, MaxBlock);
				}
			}
		}
	}
}
