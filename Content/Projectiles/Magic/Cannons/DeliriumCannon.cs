using ArcaneOdyssey.Content.Projectiles.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Projectiles.Magic.Cannons
{
    public class DeliriumCannon : CannonSpell
    {
        public Texture2D BlastSprite => ModContent.Request<Texture2D>(Texture).Value;

        public float? timeLeftDefault = null;
        public override void SetStaticDefaults()
		{
			Main.projFrames[Type] = 7;
		}

        // All the pre draw stuff will go here later (oh god)
    }
}
