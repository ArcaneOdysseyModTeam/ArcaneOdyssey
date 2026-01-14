using Terraria;
using Microsoft.Xna.Framework;
using System;


namespace ArcaneOdyssey.VFX.Dusts
{
	public class SlashDust : PreDrawnDust
	{
		public override int Rows => 2;
		public override int Columns => 2;

		
		public override bool MidUpdate(Dust dust)
		{
			dust.noGravity = true;
			return true;
		}
	}
}
