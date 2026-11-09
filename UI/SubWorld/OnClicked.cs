using ArcaneOdyssey.Subworlds.Base;
using SubworldLibrary;
using Terraria.Audio;
using Terraria.Chat;
using Terraria.UI;

namespace ArcaneOdyssey.UI.SubWorld;

public partial class SubWorldUI : UIState
{
	private void EnterButton_OnLeftClick(UIMouseEvent evt, UIElement listeningElement)
	{
		if (ChosenSubworld is null) return;

		if (AOUtils.Singleplayer)
		{
			SubworldSystem.Enter(ChosenSubworld.FullName);
		}
		else
		{
			var packet = ArcaneOdysseyMod.Instance.NewPacket(PlayersSyncSubworld.Packet);
			packet.Write(ChosenSubworld.OrderNum);
			packet.Send();
		}
	}

	#region Close button and Close method
	private void CloseButton_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) => CommitSudoku();

	/// <summary>
	/// Funny, when the method name is funny, funny
	/// </summary>
	protected void CommitSudoku()
	{
		SoundEngine.PlaySound(SoundID.MenuClose, Main.LocalPlayer.position);
		ModContent.GetInstance<ModUISystem>().HideSubWorld();
	}
	#endregion

	#region AT EASE WITH THE HARMONY
	protected void AtEaseWithTheHarmonyIAm(SubworldPicker picker)
	{
		var harmony = picker.Subworld;
		if (harmony == null) return;
		if (harmony == ChosenSubworld) ChosenSubworld = null;
		else ChosenSubworld = harmony;


		if (ChosenSubworld == null)
		{
			SoundEngine.PlaySound(SoundID.MenuClose);

			TitleSelection.SetText($"");
			TitleImage.Color = new Color(0, 0, 0, 0);
			TitleDescription.SetText($"");

			EnterText.SetText("");
			EnterButton.IgnoresMouseInteraction = true;
			EnterButton.BackgroundColor = new(50, 70, 130);
		}
		else
		{
			SoundEngine.PlaySound(SoundID.MenuOpen);
			if (!ChosenSubworld.MetConditions)
			{
				SoundEngine.PlaySound(SoundID.MenuOpen, Player.position);

				TitleSelection.SetText("? ? ?");

				TitleImage.Color = new Color(60, 60, 60);
				SetImage();

				TitleDescription.SetText(harmony.UnlockCondition);

				DisableButton();
			}
			else
			{

				TitleSelection.SetText(harmony.DisplayName);

				TitleImage.Color = Color.White;
				SetImage();

				TitleDescription.SetText(harmony.Description);

				EnterText.SetText(Language.GetText("Mods.ArcaneOdyssey.UI.Subworld.Enter"));
				EnterButton.IgnoresMouseInteraction = false;
				EnterButton.BackgroundColor = new(73, 94, 171);
			}
		}

		void SetImage()
		{
			TitleImage.SetImage(harmony.MenuStyle?.BackgroundTexture ?? ModContent.Request<Texture2D>($"{harmony.Biome.BackgroundPath}", AssetRequestMode.ImmediateLoad));
			ReSetTitleImageSize();
		}

		void DisableButton()
		{
			EnterText.SetText("");
			EnterButton.IgnoresMouseInteraction = true;
			EnterButton.BackgroundColor = new(50, 70, 130);
		}
	}

	public void AtEaseWithTheHarmonyIAm(AOSubworld harmony, LocalizedText title, Asset<Texture2D> image, string description) => AtEaseWithTheHarmonyIAm(harmony, title.Value, image, description);
	public void AtEaseWithTheHarmonyIAm(AOSubworld harmony, string title, Asset<Texture2D> image, string description)
	{
		if (harmony == null) return;
		if (harmony == ChosenSubworld || !harmony.MetConditions) ChosenSubworld = null;
		else ChosenSubworld = harmony;

		if (ChosenSubworld == null)
		{
			SoundEngine.PlaySound(SoundID.MenuClose, Player.position);

			TitleSelection.SetText($"");
			TitleImage.Color = new Color(0, 0, 0, 0);
			TitleDescription.SetText($"");

			EnterText.SetText("");
			EnterButton.IgnoresMouseInteraction = true;
			EnterButton.BackgroundColor = new(50, 70, 130);
		}
		else
		{
			SoundEngine.PlaySound(SoundID.MenuOpen, Player.position);

			TitleSelection.SetText(title);

			TitleImage.Color = Color.White;
			TitleImage.SetImage(image);
			ReSetTitleImageSize();

			TitleDescription.SetText(description);

			EnterText.SetText(Language.GetText("Mods.ArcaneOdyssey.UI.Subworld.Enter"));
			EnterButton.IgnoresMouseInteraction = false;
			EnterButton.BackgroundColor = new(73, 94, 171);
		}
	}
	#endregion
}

public class PlayersSyncSubworld : ModSystem
{
	public static byte Packet;
	public static string current;
	public override void Load()
	{
		ArcaneOdysseyMod.HandleExtraPacket += AllPlayersLeaveOrExit;
		Packet = ArcaneOdysseyMod.PacketID.ExtraHandle++;
	}

	public override void NetSend(BinaryWriter writer)
	{
		writer.Write(current ?? "");
	}

	public override void NetReceive(BinaryReader reader)
	{
		current = reader.ReadString();
	}

	private static void AllPlayersLeaveOrExit(BinaryReader reader, int whoAmI, byte command)
	{
		if (command == Packet)
		{
			if (Main.dedServ)
			{
				var subworld = reader.ReadByte();
				if (NetMessage.DoesPlayerSlotCountAsAHost(whoAmI))
				{
					if (subworld < byte.MaxValue)
					{
						current = AOSubworld.Get(subworld).FullName;
						foreach (var player in Main.ActivePlayers)
							SubworldSystem.MovePlayerToSubworld(current, player.whoAmI);
					}
					else
					{
						current = null;
						foreach (var player in Main.ActivePlayers)
							SubworldSystem.MovePlayerToMainWorld(player.whoAmI);
					}
				}
				else
				{
					ChatHelper.SendChatMessageToClient(ArcaneOdysseyMod.Instance.CustomLocalization("UI.Subworld.AdminOnly").ToNetworkText(), Color.Red, whoAmI);
				}
			}
		}
	}

	public override void PostUpdateEverything()
	{
		if (!AOUtils.ServerOrSingleplayer)
		{
			if (!current.IsNullOrWhiteSpace())
			{
				if (!SubworldSystem.IsActive(current))
				{
					Mod.NewPacket(SendMeToSubworldPlsSystem.Packet).Send();
				}
			}
			else
			{
				if (SubworldSystem.AnyActive() && SubworldSystem.Current is AOSubworld)
				{
					Mod.NewPacket(SendMeToSubworldPlsSystem.Packet).Send();
				}
			}
		}
	}

	public override void Unload()
	{
		ArcaneOdysseyMod.HandleExtraPacket -= AllPlayersLeaveOrExit;
		ArcaneOdysseyMod.PacketID.ExtraHandle--;
	}
}

public class SendMeToSubworldPlsSystem : ModSystem
{
	public static byte Packet;

	public override void Load()
	{
		ArcaneOdysseyMod.HandleExtraPacket += SendPlayerToSubworld;
		Packet = ArcaneOdysseyMod.PacketID.ExtraHandle++;
	}
	public override void Unload()
	{
		ArcaneOdysseyMod.HandleExtraPacket -= SendPlayerToSubworld; ;
		ArcaneOdysseyMod.PacketID.ExtraHandle--;
	}

	private static void SendPlayerToSubworld(BinaryReader reader, int whoAmI, byte command)
	{
		if (command == Packet)
		{
			if (Main.dedServ)
			{
				if (PlayersSyncSubworld.current.IsNullOrWhiteSpace())
					SubworldSystem.MovePlayerToMainWorld(whoAmI);
				else
					SubworldSystem.MovePlayerToSubworld(PlayersSyncSubworld.current, whoAmI);
			}
		}
	}
}
