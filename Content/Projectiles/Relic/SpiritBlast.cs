using ArcaneOdyssey.Content.Buffs.DOT;
using ArcaneOdyssey.Content.Buffs.MagicMarks;
using ArcaneOdyssey.Content.Buffs.Stuns;
using ArcaneOdyssey.Content.Projectiles.Base;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO.Enumeration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey.Content.Projectiles.Relic
{
	public class SpiritBlast : SpiritProjectile
    {
        public override AODebuffRequirement? Debuff => new AODebuffRequirement(ModContent.BuffType<AOParalyzed>(), 60, 33);
        public override CombinedDebuff[] CombinedDebuffs => [new(BuffID.Wet, ModContent.BuffType<AOParalyzed>())];

        public override SynergyEffects Effects => new( // copy of lightning lmao
            [ // these are debuffs cleared on hit
				ModContent.BuffType<AOPetrified>(), // petrified
				ModContent.BuffType<CharredEffect>(),
                ModContent.BuffType<SandyEffect>(),
                ModContent.BuffType<AOBleed>(),
                ModContent.BuffType<AOFrozen>()
            ],
            [
                new MagicBuffMultiplier(BuffID.Chilled, 1.2f), // frozen
				new MagicBuffMultiplier(ModContent.BuffType<AOBleed>(), 1.2f), // bleeding
				new MagicBuffMultiplier(BuffID.Burning, 1.15f), // scalding
				new MagicBuffMultiplier(BuffID.OnFire3, 1.075f), // melting/hellfire
				new MagicBuffMultiplier(BuffID.Venom, 1.075f), // venom acid
				new MagicBuffMultiplier(BuffID.Wet, 1.05f), // (add stunning later!)
				new MagicBuffMultiplier(BuffID.ShadowFlame,1.15f),
                new MagicBuffMultiplier(ModContent.BuffType<Crystallized>(),1.075f),
                new MagicBuffMultiplier(ModContent.BuffType<SearedEffect>(),1.15f)
            ]
            );

        public override void SetDefaults()
		{
			Projectile.width = Projectile.height = 64;
			Projectile.friendly = true;
		}

		public override void AI()
		{
			if (Projectile.ai[0] == 0)
			{
				Projectile.ai[0] = 1;
				Projectile.netUpdate = true;
			}

			if (!Main.dedServ)
			{
				for (float i = 0; i < 20; i++)
				{
					Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.width, DustID.IcyMerman, Projectile.velocity.X/2, Projectile.velocity.Y/2).noGravity = true;
				}
			}
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			width /= 4;
			height /= 4;
			fallThrough = true;
			return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
		}

		public override bool PreDraw(ref Color lightColor) => false;

        public override void ManageSynergies(ref NPC.HitModifiers modifiers)
        {
            base.ManageSynergies(ref modifiers);
        }
	}
}
