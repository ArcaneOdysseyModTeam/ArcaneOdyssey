using ArcaneOdyssey.Content.Buffs.MagicMarks;
using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Imbues.Magic.Normal;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Imbues.Magic.Lost
{
	public class DarknessMagic : AOMagic
	{
		public override AOImbuableTier ImbuableTier => AOImbuableTier.Lost;
		public override float DashSpeed => 1.2f; // burst
		public override SoundStyle? ImbueSound => SoundID.Item8;
		public override Color ImbueColour => Color.Black;
		public override float AOImbueSpeed => 1.3f;
		public override float AOImbueSize => 1.3f;
		public override float AOImbueDamage => 1.3f;
		public override AODebuffRequirement[] ImbueDebuffs => [new AODebuffRequirement(ModContent.BuffType<DrainedEffect>(), (60 * 7.5f).Round())];
		public override SynergyEffects Effects => new(
			[ // these are debuffs cleared on hit
				
			],
			[
				new MagicBuffMultiplier(BuffID.Confused,1.2f),
				new MagicBuffMultiplier(ModContent.BuffType<Crystallized>(),0.7f),
				new MagicBuffMultiplier(ModContent.BuffType<BlindedEffect>(),0.7f),
			]
			);

		public override void SpawningEffects(Entity projectile)
		{
			for (int n = 0; n < 2; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Wraith, projectile.velocity.X * 2f, projectile.velocity.Y * 2f, 0, default, 3f)];
				spawnedDust.noGravity = true;
                Dust spawnedDust2 = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.VampireHeal, projectile.velocity.X * 2f, projectile.velocity.Y * 2f, Scale: 3f)];
                spawnedDust2.noGravity = true;
            }
		}

		public override void LingeringEffects(Entity projectile)
		{
			if (Main.GameUpdateCount % 2 == 0)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Wraith, 0f, 0f, 0, default, 2f)];
				spawnedDust.noGravity = true;
				Dust spawnedDust2 = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.VampireHeal, Scale: 3f)];
				spawnedDust2.noGravity = true;
			}
		}

		public override void ExplosionEffects(Entity projectile)
		{
			for (int n = 0; n < 2; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(projectile.Center, 1, 1, DustID.Wraith, (Main.rand.NextFloat() - 0.5f) * (35f * AOScrollSize), (Main.rand.NextFloat() - 0.5f) * (35f * AOScrollSize), 0, default, 3f)];
				spawnedDust.noGravity = true;
				Dust spawnedDust2 = Main.dust[Dust.NewDust(projectile.Center, 1, 1, DustID.VampireHeal, (Main.rand.NextFloat() - 0.5f) * (35f * AOScrollSize), (Main.rand.NextFloat() - 0.5f) * (35f * AOScrollSize), Scale: 3f)];
				spawnedDust2.noGravity = true;
			}
		}

		public override void KillEffects(Entity projectile)
		{
			for (int n = 0; n < 6; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Wraith, 8f * (Main.rand.NextFloat() - 0.5f), 8f * (Main.rand.NextFloat() - 0.5f), 0, default, 3f)];
				spawnedDust.noGravity = true;
				Dust spawnedDust2 = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.VampireHeal, 8f * (Main.rand.NextFloat() - 0.5f), 8f * (Main.rand.NextFloat() - 0.5f), Scale: 3f)];
				spawnedDust2.noGravity = true;
			}
			SoundEngine.PlaySound(ImbueSound, projectile.position, null);
		}

        public override void AddRecipes()
        {
            CreateLostRecipe(typeof(ShadowMagic));
        }
	}
}
