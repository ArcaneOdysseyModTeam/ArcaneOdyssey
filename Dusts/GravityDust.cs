using System;

namespace ArcaneOdyssey.Dusts
{
	public class GravityDust : ModDust
	{
		public override bool MidUpdate(Dust dust)
		{
			dust.rotation += 0.2f;
			dust.noGravity = true;
			dust.position += new Vector2((float)Math.Cos(dust.rotation), (float)Math.Sin(dust.rotation));
			Lighting.AddLight(dust.Centre(), TorchID.Purple);
			return true;
		}
	}
}