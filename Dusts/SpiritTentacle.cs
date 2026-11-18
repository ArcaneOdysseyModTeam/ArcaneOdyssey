namespace ArcaneOdyssey.Dusts
{
	public class SpiritTentacle : PreDrawnDust
	{
		public override bool PreDraw(Dust dust)
		{
			Lighting.AddLight(dust.position, TorchID.Ice);
			return base.PreDraw(dust);
		}
	}
}