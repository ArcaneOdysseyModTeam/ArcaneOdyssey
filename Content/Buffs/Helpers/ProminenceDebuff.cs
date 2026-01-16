using ArcaneOdyssey.Content.Buffs.Base;
using Terraria;
using Terraria.ID;
using System;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using ArcaneOdyssey.Content.Items.Imbues.Magic.Lost;
using ArcaneOdyssey.Content.Projectiles.Magic.Effects;

namespace ArcaneOdyssey.Content.Buffs.Helpers
{
	public class ProminenceDebuff : AODebuff
	{
        private int counter = 0;
		public override void Update(NPC npc, ref int buffIndex)
		{
			counter++;
            if (counter > 120)
            {
                counter = 0;
                Projectile.NewProjectile(npc.GetSource_FromThis(),npc.position.X,npc.position.Y,(Main.rand.NextFloat()-0.5f)*5f,(Main.rand.NextFloat()-0.5f)*5f,ModContent.ProjectileType<ProminenceProjectile>(),50,0,-1);
            }
            if (!Main.dedServ)
            {
                Dust newDust = Dust.NewDustDirect(npc.position, npc.Hitbox.Width, npc.Hitbox.Height, DustID.Torch, (0.5f - Main.rand.NextFloat()) * 2f, (0.5f - Main.rand.NextFloat()) * 2f, 1, default, 1f);
            }
		}
	}
}
