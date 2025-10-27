using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Projectiles.Base;
using System;
using System.Collections.Generic;
using ArcaneOdyssey.Content.Items.Materials;
using Microsoft.Xna.Framework;
using ArcaneOdyssey.Content.Projectiles.Magic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ArcaneOdyssey.Content.Buffs.MagicMarks;
using ArcaneOdyssey.Content.Buffs.DOT;
using Terraria.Audio;
using static ArcaneOdyssey.AOUtils;
using ArcaneOdyssey.Content.Items.Imbues.Magic.Normal;
using ArcaneOdyssey.Content.Projectiles.Magic.Blasts.Lost;
using ArcaneOdyssey.Content.Projectiles.Magic.Cannons.Lost;
using ArcaneOdyssey.Content.Projectiles.Magic.Pulsars.Lost;

namespace ArcaneOdyssey.Content.Items.Imbues.Magic.Lost
{
	public class PrismMagic : AOMagic
	{
		public static Color[] rainbowColors = [new Color(255, 71, 124),new Color(94, 61, 255),new Color(87, 219, 255),new Color(100, 255, 93)];
		public override SoundStyle? ImbueSound => SoundID.Shatter;
        public override Color ImbueColour => new(255,255,255);
        public override float AOImbueSpeed => 1.1f;
        public override float AOImbueDamage => 1.2f;
		public override float AOImbueSize => 1.15f;
		public override float AOScrollSpeed => 1.1f;
        public override float AOScrollDamage => 1.2f;
        public override float AOScrollSize => 1.15f;

		public override AOImbuableTier ImbuableTier => AOImbuableTier.Lost;
        public override AODebuffRequirement[] ImbueDebuffs => [new(ModContent.BuffType<BlindedEffect>(), 60 * 5), new(ModContent.BuffType<AOBleed>(), 60 * 10)];
		
		public override SynergyEffects Effects => new(
			[ // these are debuffs cleared on hit
				
			],
			[
				new MagicBuffMultiplier(ModContent.BuffType<Crystallized>(),1.075f),
				new MagicBuffMultiplier(ModContent.BuffType<DrainedEffect>(),0.8f),
				new MagicBuffMultiplier(BuffID.Venom,1.05f),
				new MagicBuffMultiplier(ModContent.BuffType<FreezingEffect>(),1.075f),
				new MagicBuffMultiplier(ModContent.BuffType<SandyEffect>(),1.1f),
				new MagicBuffMultiplier(BuffID.OnFire3,1.05f)
			]
			);
		public override void SpawningEffects(Entity projectile)
		{
			int rainbowStep = (int)Main.GameUpdateCount;
			for (int n = 0; n < 3; n++)
            {
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.Hitbox.Width, projectile.Hitbox.Height, DustID.AncientLight, (Main.rand.NextFloat() - 0.5f) * 3f, (Main.rand.NextFloat() - 0.5f) * 3f, 0, rainbowColors[rainbowStep % 3], 1f);
				dust.noGravity = true;
				rainbowStep++;
				Dust.NewDust(projectile.position, projectile.Hitbox.Width, projectile.Hitbox.Height, DustID.Glass, 0f, 0f, 0,default, 1f);
            }
		}
		public override void LingeringEffects(Entity projectile)
		{
			Dust.NewDust(projectile.position, projectile.Hitbox.Width, projectile.Hitbox.Height, DustID.Glass, 0f, 0f, 0,default, 0.5f);
			if (projectile is Projectile)
            {
				if (((Projectile)projectile).type == ModContent.ProjectileType<BeamSpell>())
                {
					Dust dust = Dust.NewDustDirect(projectile.position, projectile.Hitbox.Width, projectile.Hitbox.Height, DustID.AncientLight, (Main.rand.NextFloat() - 0.5f) * 3f, (Main.rand.NextFloat() - 0.5f) * 3f, 0, rainbowColors[((Projectile)projectile).numUpdates % 3], 1.4f);
					dust.noGravity = true;
                } else
                {
					Dust dust = Dust.NewDustDirect(projectile.position, projectile.Hitbox.Width, projectile.Hitbox.Height, DustID.AncientLight, (Main.rand.NextFloat() - 0.5f) * 3f, (Main.rand.NextFloat() - 0.5f) * 3f, 0, rainbowColors[Main.GameUpdateCount % 3], 1.4f);
					dust.noGravity = true;
                }
            } else
            {
				Dust dust = Dust.NewDustDirect(projectile.position, projectile.Hitbox.Width, projectile.Hitbox.Height, DustID.AncientLight, (Main.rand.NextFloat() - 0.5f) * 3f, (Main.rand.NextFloat() - 0.5f) * 3f, 0, rainbowColors[Main.GameUpdateCount % 3], 1.4f);
				dust.noGravity = true;
            }
		}
		public override void KillEffects(Entity projectile)
		{
			int rainbowStep = (int)Main.GameUpdateCount;
			for (int n = 0; n < 10; n++)
			{
				Dust dust = Dust.NewDustDirect(projectile.position, projectile.Hitbox.Width, projectile.Hitbox.Height, DustID.AncientLight, (Main.rand.NextFloat() - 0.5f) * 3f, (Main.rand.NextFloat() - 0.5f) * 3f, 0, rainbowColors[rainbowStep % 3], 2f);
				dust.noGravity = true;
				rainbowStep++;
				Dust.NewDust(projectile.position, projectile.Hitbox.Width, projectile.Hitbox.Height, DustID.Glass, 0f, 0f, 0,default, 1.2f);
			}
			SoundEngine.PlaySound(ImbueSound, projectile.position, null);
		}
        public override void ExplosionEffects(Entity projectile)
        {
			int rainbowStep = (int)Main.GameUpdateCount;
			Dust.NewDust(new Vector2(projectile.position.X + projectile.width / 2f, projectile.position.Y + projectile.height / 2f), 1, 1, DustID.Glass, (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), 0, default, 0.9f);
			for (int n = 0; n < 10; n++)
            {
				Dust dust = Dust.NewDustDirect(new Vector2(projectile.position.X + projectile.width / 2f, projectile.position.Y + projectile.height / 2f), 1, 1, DustID.AncientLight, (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), 0, rainbowColors[rainbowStep % 3], 1.3f);
				dust.noGravity = true;
				rainbowStep++;
            }
        }
        public override Dictionary<Type, int> Skills => new([KeyValuePair.Create(typeof(BlastSpell), ModContent.ProjectileType<PrismBlast>()), KeyValuePair.Create(typeof(PulsarSpell), ModContent.ProjectileType<PrismPulsar>()), KeyValuePair.Create(typeof(CannonSpell), ModContent.ProjectileType<PrismCannon>())]);
		
		public override void AddRecipes()
        {
            this.CreateLostRecipe(typeof(LightMagic), typeof(GlassMagic), typeof(CrystalMagic));
        }
	}
}