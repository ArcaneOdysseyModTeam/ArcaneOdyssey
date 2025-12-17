using System;
using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace ArcaneOdyssey.VFX.Dusts
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