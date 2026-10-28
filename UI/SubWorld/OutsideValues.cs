using ArcaneOdyssey.Subworlds.Base;
using Terraria.UI;

namespace ArcaneOdyssey.UI.SubWorld;

public partial class SubWorldUI : UIState
{
	// Spoky 2026 Oct 25: Values or stuff that can be used anywhere in the entire UI

	protected Player Player;
	protected const float separation = 4f;
	protected readonly Vector2 DefaultImageSize = new(480, 270);


	/// <summary>
	/// The chosen <see cref="AOSubworld"/>
	/// </summary>
	public AOSubworld ChosenSubworld { get; protected set; }
}
