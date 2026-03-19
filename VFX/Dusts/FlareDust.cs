using Terraria;
using Terraria.ID;

namespace ArcaneOdyssey.VFX.Dusts
{
	public class FlareDust : PreDrawnDust
	{
		public override bool PreDraw(Dust dust)
		{
			Lighting.AddLight(dust.Centre(), TorchID.Red);
			return base.PreDraw(dust);
		}
	}
}
