using System;
using Terraria;
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
            dust.position += new Vector2((float)Math.Cos((float)dust.rotation),(float)Math.Sin((float)dust.rotation));
            return true;
        }
    }
}