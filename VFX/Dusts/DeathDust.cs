using System;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using ArcaneOdyssey.Content.Items.Imbues.Magic.Ancient;

namespace ArcaneOdyssey.VFX.Dusts
{
	public class DeathDust : ModDust
	{
		public override bool MidUpdate(Dust dust)
		{
			dust.rotation += 0.2f;
			dust.noGravity = true;
			dust.position += new Vector2((float)Math.Cos(dust.rotation), (float)Math.Sin(dust.rotation));
			Lighting.AddLight(dust.Centre(), ModContent.GetInstance<DeathMagic>().ImbueColour.ToVector3());
			return true;
		}
	}
}