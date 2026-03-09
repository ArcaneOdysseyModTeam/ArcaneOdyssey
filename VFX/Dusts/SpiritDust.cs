using Terraria;

namespace ArcaneOdyssey.VFX.Dusts
{
	public class SpiritDust : PreDrawnDust
	{
		public override int Rows => 3;
		public override bool PreDraw(Dust dust)
		{
			Lighting.AddLight(dust.Centre(), dust.color.ToVector3());
			return base.PreDraw(dust);
		}
	}
}
