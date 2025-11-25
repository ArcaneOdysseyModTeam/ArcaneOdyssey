using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Imbues.Magic.Normal;
using ArcaneOdyssey.Content.Projectiles.Magic.Blasts.Lost;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace ArcaneOdyssey.Content.Items.Imbues.Magic.Lost
{
    public class SoundMagic : AOMagic
    {
        public override float DashSpeed => 1.5f; // instant
        public override SoundStyle? ImbueSound => SoundID.Roar;
        public override AOImbuableTier ImbuableTier => AOImbuableTier.Lost;

        public override float AOImbueSpeed => 1.2f;
        public override float AOImbueSize => 1.2f;
        public override float AOImbueDamage => .95f;
        public override float AOScrollSpeed => 1.4f;
        public override float AOScrollSize => 1.25f;
        public override float AOScrollDamage => .9f;
        public override float KBMulti => 1.5f;


        public const int DustCount = 30;
        public override void KillEffects(Entity Projectile)
        {
            if (!Main.dedServ)
            {
                for (float i = 0; i < DustCount; i++)
                {
                    var centre = (MathHelper.TwoPi / DustCount * i).ToRotationVector2() * (Projectile.width * 3);
                    var dust = Dust.NewDustPerfect(Projectile.Center, DustID.MushroomTorch, centre / 14);
                    dust.noGravity = true;
                }
            }
            SoundEngine.PlaySound(ImbueSound, Projectile.Center);
        }

        public override void ExplosionEffects(Entity entity)
        {
            for (float e = 13; e < 18; e++)
            {
                if (!Main.dedServ)
                {
                    for (float i = 0; i < DustCount; i++)
                    {
                        var centre = (MathHelper.TwoPi / DustCount * i).ToRotationVector2() * (entity.width * 1.2f);
                        var dust = Dust.NewDustPerfect(entity.Center, DustID.MushroomTorch, centre / e);
                        dust.noGravity = true;
                    }
                }
            }
        }

        public override void LingeringEffects(Entity entity)
        {
            if (!Main.dedServ)
            {
                for (float i = 0; i < DustCount; i++)
                {
                    var centre = (MathHelper.TwoPi / DustCount * i).ToRotationVector2() * entity.width;
                    var dust = Dust.NewDustPerfect(entity.Center, DustID.MushroomTorch, centre / (DustCount * .75f));
                    dust.noGravity = true;
                }
            }
        }

        public override void SpawningEffects(Entity entity)
        {
            if (!Main.dedServ)
            {
                SoundEngine.PlaySound(ImbueSound, entity.Center);
            }
        }

        public override void AddRecipes()
        {
            CreateLostRecipe(typeof(WindMagic), typeof(GlassMagic), typeof(LightningMagic));
        }

        public override List<Type> Skills => [typeof(SoundBlast)];
    }
}
