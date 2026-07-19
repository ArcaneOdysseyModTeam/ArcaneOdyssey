namespace ArcaneOdyssey.Dusts
{
	public class RegulusDust : PreDrawnDust
	{
		public override int Rows => 2;

		public override bool PreDraw(Dust dust)
		{
			Lighting.AddLight(dust.Centre(), Color.Gold.ToVector3() * dust.scale);
			return base.PreDraw(dust);
		}
	}
}
