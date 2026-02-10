using Terraria;


namespace ArcaneOdyssey.VFX.Dusts
{
	public class SlashDust : PreDrawnDust
	{
		public override int Rows => 4;
		public override int Columns => 4;


		public override bool MidUpdate(Dust dust)
		{
			dust.noGravity = true;
			return true;
		}
	}
}
