using Terraria;
using Terraria.ID;

namespace ArcaneOdyssey.VFX.Dusts
{
	public class FlareDust : PreDrawnDust
	{
		public override int Rows => 2;
		public override int Columns => 2;
		public override bool PreDraw(Dust dust)
		{
			Lighting.AddLight(dust.Centre(), TorchID.Red);
			return base.PreDraw(dust);
		}
	}
}
