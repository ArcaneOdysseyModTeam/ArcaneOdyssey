namespace ArcaneOdyssey.Dusts
{
	public class SpiritTentacle : PreDrawnDust
	{
		public override bool PreDraw(Dust dust)
		{
			Lighting.AddLight(dust.Centre(), TorchID.Ice);
			return base.PreDraw(dust);
		}
	}
}