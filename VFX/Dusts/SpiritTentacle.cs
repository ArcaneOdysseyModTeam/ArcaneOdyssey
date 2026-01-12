using Terraria;
using Terraria.ID;

namespace ArcaneOdyssey.VFX.Dusts
{
	public class SpiritTentacle : PreDrawnDust
	{
		public override int Rows => 2;
		public override int Columns => 2;

		public override bool PreDraw(Dust dust)
		{
			Lighting.AddLight(dust.Centre(), TorchID.Ice);
			return base.PreDraw(dust);
		}
	}
}