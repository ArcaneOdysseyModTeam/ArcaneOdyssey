using ArcaneOdyssey.Content.Items.Base;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace ArcaneOdyssey.Content.Items.Imbues.Magic.Lost
{
	public class PlantMagic : AOMagic
	{
		public override AOImbuableTier ImbuableTier => AOImbuableTier.Lost;
		public override Color ImbueColour => Color.ForestGreen;

		public override void LingeringEffects(Entity entity)
		{
			Gore.NewGore(entity.GetSource_FromThis(), entity.Center, entity.velocity / 4f, GoreID.TreeLeaf_Normal);
		}
	}
}
