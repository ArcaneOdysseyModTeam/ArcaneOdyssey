using ArcaneOdyssey.Subworlds.Base;

namespace ArcaneOdyssey.Subworlds
{
	public class DjinSubworld : StructureHelperSubworld
	{
		public override string StructureName => "EliusRuins";

		public override ushort OrderNum => 0;

		public override bool MetConditions()
		{
			return true;
		}
	}
}
