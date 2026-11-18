namespace ArcaneOdyssey.Dusts
{
	public class FlareDust : PreDrawnDust
	{
		public override bool PreDraw(Dust dust)
		{
			Lighting.AddLight(dust.position, TorchID.Red);
			return base.PreDraw(dust);
		}
	}
}
