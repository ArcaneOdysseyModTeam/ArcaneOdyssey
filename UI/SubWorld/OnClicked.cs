using ArcaneOdyssey.Subworlds.Base;
using SubworldLibrary;
using System.IO;
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
		writer.Write(current);
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
					current = AOSubworld.Get(subworld).FullName;
					var packet = ArcaneOdysseyMod.Instance.NewPacket(Packet);
					packet.Write(subworld);
					packet.Send();
				}
				else
				{
					ChatHelper.SendChatMessageToClient(ArcaneOdysseyMod.Instance.CustomLocalization("UI.Subworld.AdminOnly").ToNetworkText(), Color.Red, whoAmI);
				}
			}
			else
			{
				var subworld = reader.ReadByte();
				if (subworld < byte.MaxValue)
				{
					current = AOSubworld.Get(subworld).FullName;
					SubworldSystem.Enter(current);
				}
				else
				{
					SubworldSystem.Exit();
				}
			}
		}
	}

	public override void PostUpdateEverything()
	{
		if (!AOUtils.ServerOrSingleplayer)
		{
			if (SubworldSystem.AnyActive() && SubworldSystem.Current is AOSubworld)
			{
				if (!current.IsNullOrWhiteSpace())
				{
					SubworldSystem.Enter(current);
				}
				else
				{
					SubworldSystem.Exit();
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
