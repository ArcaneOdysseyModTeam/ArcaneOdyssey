using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Projectiles.Base;
using ArcaneOdyssey.Content.Projectiles.Magic.Blasts;
using ArcaneOdyssey.Content.Projectiles.Magic.Cannons;
using ArcaneOdyssey.Content.Projectiles.Magic.Pulsars;
using System;
using System.Collections.Generic;
using ArcaneOdyssey.Content.Items.Materials;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using ArcaneOdyssey.Content.Projectiles.Magic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ArcaneOdyssey.Content.Buffs.MagicMarks;
using ArcaneOdyssey.Content.Buffs.DOT;
using Terraria.Audio;
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey.Content.Items.Magic
{
	public class PrismMagic : AOMagic
	{
		public static Color[] rainbowColors = [Color.Red, Color.Orange, Color.Green, Color.Blue, Color.Indigo, Color.Violet];
		public override SoundStyle? ImbueSound => SoundID.Shatter;
        public override Color ImbueColour => new(255,255,255);
        public override float AOImbueSpeed => 1.1f;
        public override float AOImbueDamage => 1.2f;
		public override float AOImbueSize => 1.15f;
		public override float AOScrollSpeed => 1.1f;
        public override float AOScrollDamage => 1.2f;
        public override float AOScrollSize => 1.15f;

		public override AOImbuableTier ImbuableTier => AOImbuableTier.Lost;
		public override AODebuffRequirement[] ImbueDebuffs => [new(ModContent.BuffType<BlindedEffect>(), 60*5),new(ModContent.BuffType<AOBleed>(), 60*10)];
		
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
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.Hitbox.Width, projectile.Hitbox.Height, DustID.AncientLight, (Main.rand.NextFloat() - 0.5f) * 3f, (Main.rand.NextFloat() - 0.5f) * 3f, 0, rainbowColors[rainbowStep % 6], 1f);
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
					Dust dust = Dust.NewDustDirect(projectile.position, projectile.Hitbox.Width, projectile.Hitbox.Height, DustID.AncientLight, (Main.rand.NextFloat() - 0.5f) * 3f, (Main.rand.NextFloat() - 0.5f) * 3f, 0, rainbowColors[((Projectile)projectile).numUpdates % 6], 1.4f);
					dust.noGravity = true;
                } else
                {
					Dust dust = Dust.NewDustDirect(projectile.position, projectile.Hitbox.Width, projectile.Hitbox.Height, DustID.AncientLight, (Main.rand.NextFloat() - 0.5f) * 3f, (Main.rand.NextFloat() - 0.5f) * 3f, 0, rainbowColors[Main.GameUpdateCount % 6], 1.4f);
					dust.noGravity = true;
                }
            } else
            {
				Dust dust = Dust.NewDustDirect(projectile.position, projectile.Hitbox.Width, projectile.Hitbox.Height, DustID.AncientLight, (Main.rand.NextFloat() - 0.5f) * 3f, (Main.rand.NextFloat() - 0.5f) * 3f, 0, rainbowColors[Main.GameUpdateCount % 6], 1.4f);
				dust.noGravity = true;
            }
		}
		public override void KillEffects(Entity projectile)
		{
			int rainbowStep = (int)Main.GameUpdateCount;
			for (int n = 0; n < 10; n++)
			{
				Dust dust = Dust.NewDustDirect(projectile.position, projectile.Hitbox.Width, projectile.Hitbox.Height, DustID.AncientLight, (Main.rand.NextFloat() - 0.5f) * 3f, (Main.rand.NextFloat() - 0.5f) * 3f, 0, rainbowColors[rainbowStep % 6], 2f);
				dust.noGravity = true;
				rainbowStep++;
				Dust.NewDust(projectile.position, projectile.Hitbox.Width, projectile.Hitbox.Height, DustID.Glass, 0f, 0f, 0,default, 1.2f);
			}
			SoundEngine.PlaySound(ImbueSound, projectile.position, null);
		}
        public override void ExplosionEffects(Entity projectile)
        {
			int rainbowStep = (int)Main.GameUpdateCount;
			Dust.NewDust(new Vector2(projectile.position.X + (projectile.width / 2f), projectile.position.Y + (projectile.height / 2f)), 1, 1, DustID.Glass, (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), 0, default, 0.9f);
			for (int n = 0; n < 10; n++)
            {
				Dust dust = Dust.NewDustDirect(new Vector2(projectile.position.X + (projectile.width / 2f), projectile.position.Y + (projectile.height / 2f)), 1, 1, DustID.AncientLight, (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), 0, rainbowColors[rainbowStep % 6], 1.3f);
				dust.noGravity = true;
				rainbowStep++;
            }
        }
        public override Dictionary<Type, int> Skills => new([KeyValuePair.Create(typeof(BlastSpell), ModContent.ProjectileType<PrismBlast>()), KeyValuePair.Create(typeof(PulsarSpell), ModContent.ProjectileType<PrismPulsar>()), KeyValuePair.Create(typeof(CannonSpell), ModContent.ProjectileType<PrismCannon>())]);
		
		public override void AddRecipes()
        {
			CreateRecipe().AddIngredient<HecateShard>().AddIngredient<LightMagic>().Register();
			CreateRecipe().AddIngredient<HecateShard>().AddIngredient<GlassMagic>().Register();
			CreateRecipe().AddIngredient<HecateShard>().AddIngredient<CrystalMagic>().Register();
        }
	}
}