namespace ArcaneOdyssey.Dusts
{
	public class SlashDust : PreDrawnDust
	{
		public override int Rows => 2;

		public override bool PreDraw(Dust dust)
		{
			Lighting.AddLight(dust.position, Color.Lime.ToVector3() * dust.scale);
			return base.PreDraw(dust);
		}
	}
}
