using ArcaneOdyssey.Content.Buffs.DOT;
using ArcaneOdyssey.Content.Buffs.MagicMarks;
using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Imbues.Magic.Ancient;
using ArcaneOdyssey.Content.Projectiles.Magic.Effects;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Imbues.Magic.Lost
{
	public class AetherMagic : AOMagic
	{
		public override void RegisterMutations()
		{
			RegisterMutation<IonMagic>();
		}
		
		public override float DashSpeed => 1.5f; // instant
		public override SoundStyle? ImbueSound => SoundID.Item9;
		public override Color ImbueColour => new(255, 255, 0);
		public override bool? Cold => false;
		public override bool CanBeWet => false;
		public override float AOImbueSpeed => 1.28f;
		public override float AOImbueSize => 1.2f;
		public override float AOImbueDamage => 1.15f;
		public override float AOScrollSpeed => 1.28f;
		public override float AOScrollSize => 1.2f;
		public override float AOScrollDamage => 1.15f;
		public override AOImbuableTier ImbuableTier => AOImbuableTier.Lost;
		public override AODebuffRequirement[] ImbueDebuffs => [new(ModContent.BuffType<CharredEffect>(), 60 * 10), new(ModContent.BuffType<BlindedEffect>(), 60 * 5)];
		public override SynergyEffects Effects => new(
			[ // these are debuffs cleared on hit
				ModContent.BuffType<FreezingEffect>(),
				ModContent.BuffType<SnowyEffect>(),
				BuffID.Wet
			],
			[
				new(ModContent.BuffType<AOBleed>(),1.01f),
				new(BuffID.OnFire,1.125f),
				new(BuffID.Venom,1.075f),
				new(ModContent.BuffType<FreezingEffect>(),1.01f),
				new(BuffID.OnFire3,1.075f),
				new(ModContent.BuffType<SnowyEffect>(),0.99f),
				new(BuffID.ShadowFlame,1.15f),
				new(BuffID.Wet,0.99f),
				new(BuffID.Oiled,1.075f),
				new(ModContent.BuffType<SandyEffect>(),0.99f),
				new(ModContent.BuffType<AOScalding>(),1.125f),
				new(ModContent.BuffType<SearedEffect>(),1.15f),
				new(ModContent.BuffType<Crystallized>(),1.075f),
				new(ModContent.BuffType<DrainedEffect>(),0.8f)
			]
			);


		public override void SpawningEffects(Rectangle area, Vector2 direction)
		{
			if (Main.dedServ)
				return;
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.YellowStarDust, direction.X * 0.2f, direction.Y * 0.2f, Scale: 3f * area.RelativeScale())];
				spawnedDust.noGravity = true;
				Dust spawnedDust2 = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.YellowTorch, direction.X * 0.2f, direction.Y * 0.2f, Scale: 3f * area.RelativeScale())];
				spawnedDust2.noGravity = true;
			}
		}

		public override void LingeringEffects(Rectangle area, Vector2? direction = null, Entity source = null)
		{
			Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.YellowStarDust, Scale: area.RelativeScale());
			Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.YellowTorch, Scale: 2f * area.RelativeScale())];
			spawnedDust.noGravity = true;
			Lighting.AddLight(area.Center(), 2, 2, 0);
		}

		public override void ExplosionEffects(Vector2 position, float intensity = 1f)
		{
			//for (int n = 0; n < 3; n++)
			//{
			//	Dust spawnedDust = Main.dust[Dust.NewDust(position, 0, 0, DustID.YellowStarDust, (Main.rand.NextFloat() - 0.5f) * (25f * intensity * AOScrollSize), (Main.rand.NextFloat() - 0.5f) * (25f * intensity * AOScrollSize), Scale: 3f * intensity)];
			//	spawnedDust.noGravity = true;
			//	Dust spawnedDust2 = Main.dust[Dust.NewDust(position, 0, 0, DustID.YellowTorch, (Main.rand.NextFloat() - 0.5f) * (25f * intensity * AOScrollSize), (Main.rand.NextFloat() - 0.5f) * (25f * intensity * AOScrollSize), Scale: 3f * intensity)];
			//	spawnedDust2.noGravity = true;
			//}
			Projectile.NewProjectile(Item.GetSource_FromThis(), position, Vector2.Zero, ModContent.ProjectileType<AetherExplosion>(), 0, 0, ai0: 2f * intensity);
		}

		public override void KillEffects(Rectangle area, Entity source = null)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.YellowStarDust, 28f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 28f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), Scale: 3f * area.RelativeScale())];
				spawnedDust.noGravity = true;
				Dust spawnedDust2 = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.YellowTorch, 28f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 28f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), Scale: 3f * area.RelativeScale())];
				spawnedDust2.noGravity = true;
			}
			SoundEngine.PlaySound(ImbueSound, area.Center());
			if (source is Projectile projectile && projectile.ModProjectile is not AetherExplosion)
			{
				if (projectile.owner == Main.myPlayer && AetherExplosion.Count < 3)
				{
					Projectile.NewProjectile(projectile.GetSource_FromThis(), area.Center(), Vector2.Zero, ModContent.ProjectileType<AetherExplosion>(), projectile.damage / 4, 0, projectile.owner);
				}
			}
		}

		public override bool PreEffects(Entity entity = null)
		{
			if (entity is Projectile projectile)
				if (projectile.ModProjectile is AetherExplosion)
					return false;
			return base.PreEffects(entity);
		}
	}
}
