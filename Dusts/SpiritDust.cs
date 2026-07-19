namespace ArcaneOdyssey.Dusts
{
	public class SpiritDust : PreDrawnDust
	{
		public override int Rows => 3;
		public override bool PreDraw(Dust dust)
		{
			Lighting.AddLight(dust.Centre(), dust.color.ToVector3() * dust.scale);
			return base.PreDraw(dust);
		}
	}
}
