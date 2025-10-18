using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using Terraria.DataStructures;

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