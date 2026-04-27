using Terraria;
using Terraria.ID;

namespace ArcaneOdyssey.Dusts
{
	public class HeatDust : PreDrawnDust
	{
		public override int Rows => 3;
		public override int Columns => 2;
		public override bool PreDraw(Dust dust)
		{
			Lighting.AddLight(dust.Centre(), TorchID.Orange);
			return base.PreDraw(dust);
		}
	}
}
