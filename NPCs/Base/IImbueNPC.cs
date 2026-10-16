using ArcaneOdyssey.Imbues.Base;

namespace ArcaneOdyssey.NPCs.Base
{
	public interface IImbueNPC : ILocalizedModType
	{
		public Imbuable Imbue { get; }
	}
}
