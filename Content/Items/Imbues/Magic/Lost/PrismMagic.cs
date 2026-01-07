using ArcaneOdyssey.Content.Buffs.DOT;
using ArcaneOdyssey.Content.Buffs.MagicMarks;
using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Imbues.Magic.Normal;
using ArcaneOdyssey.Content.Projectiles.Magic.MagicEffects;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Imbues.Magic.Lost
{
	public class PrismMagic : AOMagic
	{
		public override float? DashResist => 1.15f;
		public override float ItemInvisibility => .5f;

		internal static readonly Color[] rainbowColors = [new Color(255, 71, 124), new Color(94, 61, 255), new Color(87, 219, 255), new Color(100, 255, 93)];

		public override SoundStyle? ImbueSound => SoundID.Shatter;
		public override Color ImbueColour => Color.White;
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
				new(ModContent.BuffType<Crystallized>(),1.075f),
				new(ModContent.BuffType<DrainedEffect>(),0.8f),
				new(BuffID.Venom,1.05f),
				new(ModContent.BuffType<FreezingEffect>(),1.075f),
				new(ModContent.BuffType<SandyEffect>(),1.1f),
				new(BuffID.OnFire3,1.05f)
			]
			);

		public override void SpawningEffects(Entity entity)
		{
			if (Main.dedServ)
				return;
			int rainbowStep = (int)Main.GameUpdateCount;
			for (int n = 0; n < 3; n++)
			{
				Dust dust = Dust.NewDustDirect(entity.position, entity.Hitbox.Width, entity.Hitbox.Height, DustID.AncientLight, (Main.rand.NextFloat() - 0.5f) * 3f, (Main.rand.NextFloat() - 0.5f) * 3f, 0, rainbowColors[rainbowStep % 3], 1f);
				dust.noGravity = true;
				rainbowStep++;
				Dust.NewDust(entity.position, entity.Hitbox.Width, entity.Hitbox.Height, DustID.Glass, 0f, 0f, 0, default, 1f);
			}
		}

		public override void LingeringEffects(Entity entity)
		{
			if (Main.dedServ)
				return;
			Dust.NewDust(entity.position, entity.Hitbox.Width, entity.Hitbox.Height, DustID.Glass, 0f, 0f, 0, default, 0.5f);
			if (entity is Projectile projectile)
			{
				if (projectile.extraUpdates > 0)
				{
					Dust dust = Dust.NewDustDirect(entity.position, entity.Hitbox.Width, entity.Hitbox.Height, DustID.AncientLight, (Main.rand.NextFloat() - 0.5f) * 3f, (Main.rand.NextFloat() - 0.5f) * 3f, 0, rainbowColors[projectile.numUpdates % 3], 1.4f);
					dust.noGravity = true;
				}
				else
				{
					Dust dust = Dust.NewDustDirect(entity.position, entity.Hitbox.Width, entity.Hitbox.Height, DustID.AncientLight, (Main.rand.NextFloat() - 0.5f) * 3f, (Main.rand.NextFloat() - 0.5f) * 3f, 0, rainbowColors[Main.GameUpdateCount % 3], 1.4f);
					dust.noGravity = true;
				}
			}
			else
			{
				Dust dust = Dust.NewDustDirect(entity.position, entity.Hitbox.Width, entity.Hitbox.Height, DustID.AncientLight, (Main.rand.NextFloat() - 0.5f) * 3f, (Main.rand.NextFloat() - 0.5f) * 3f, 0, rainbowColors[Main.GameUpdateCount % 3], 1.4f);
				dust.noGravity = true;
			}
		}

		public override void KillEffects(Entity entity)
		{
			if (Main.dedServ)
				return;
			int rainbowStep = (int)Main.GameUpdateCount;
			for (int n = 0; n < 10; n++)
			{
				Dust dust = Dust.NewDustDirect(entity.position, entity.Hitbox.Width, entity.Hitbox.Height, DustID.AncientLight, (Main.rand.NextFloat() - 0.5f) * 3f, (Main.rand.NextFloat() - 0.5f) * 3f, 0, rainbowColors[rainbowStep % 3], 2f);
				dust.noGravity = true;
				rainbowStep++;
				Dust.NewDust(entity.position, entity.Hitbox.Width, entity.Hitbox.Height, DustID.Glass, 0f, 0f, 0, default, 1.2f);
			}
			SoundEngine.PlaySound(ImbueSound, entity.Center, null);
			if (entity is Projectile projectile && projectile.owner == Main.myPlayer && projectile.GetOwner().ownedProjectileCounts[ModContent.ProjectileType<PrismLinger>()] < 3)
				Projectile.NewProjectile(entity.GetSource_FromThis(), entity.Center, Vector2.Zero, ModContent.ProjectileType<PrismLinger>(), projectile.damage / 6, 0, projectile.owner);
		}

		public override bool PreEffects(Entity entity)
		{
			if (entity is Projectile projectile)
				if (projectile.ModProjectile is PrismLinger)
					return false;
			return base.PreEffects(entity);
		}

		public override void ExplosionEffects(Entity entity)
		{
			if (Main.dedServ)
				return;
			int rainbowStep = (int)Main.GameUpdateCount;
			Dust.NewDust(entity.Center, 0, 0, DustID.Glass, (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), 0, default, 0.9f);
			for (int n = 0; n < 10; n++)
			{
				Dust dust = Dust.NewDustDirect(entity.Center, 0, 0, DustID.AncientLight, (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), 0, rainbowColors[rainbowStep % 3], 1.3f);
				dust.noGravity = true;
				rainbowStep++;
			}
		}

		public override void AddRecipes()
		{
			CreateLostRecipe(typeof(LightMagic), typeof(GlassMagic), typeof(CrystalMagic));
		}
	}
}