using ArcaneOdyssey.Content.Buffs.DOT;
using ArcaneOdyssey.Content.Buffs.MagicMarks;
using ArcaneOdyssey.Content.Buffs.Stuns;
using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Imbues.Magic.Lost;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Imbues.Magic.Normal
{
	public class AshMagic : AOMagic
	{
		public override void RegisterMutations()
		{
			RegisterMutation<AetherMagic>();
			RegisterMutation<HeatMagic>();
			RegisterMutation<ShadowflameMagic>();
			RegisterMutation<PhoenixMagic>();
			RegisterMutation<SunMagic>();
		}
		public override bool Special => true;
		public override bool? Cold => false;
		public override bool CanBeWet => false;
		public override Color ImbueColour => new(235, 40, 0, 0);
		public override float AOImbueSpeed => 0.975f;
		public override float AOImbueSize => 1.22f;
		public override float AOImbueDamage => 0.95f;
		public override float AOScrollSpeed => 0.95f;
		public override float AOScrollSize => 1.25f;
		public override float AOScrollDamage => 0.875f;
		public override SoundStyle? ImbueSound => SoundID.Dig;
		public override Debuff[] ImbueDebuffs => [new(ModContent.BuffType<Petrified>(), 60 * 10, 33)];
		public override Combo[] CombinedDebuffs => [new(BuffID.OnFire3, ModContent.BuffType<Petrified>()), Combo.Create<Melting, Petrified>(), new(BuffID.OnFire, ModContent.BuffType<Petrified>()), Combo.Create<AOBurning, Petrified>(), new(BuffID.ShadowFlame, ModContent.BuffType<Petrified>()), new(ModContent.BuffType<CharredEffect>(), ModContent.BuffType<Petrified>()), new(ModContent.BuffType<Scalding>(), ModContent.BuffType<Petrified>()), new(ModContent.BuffType<Singed>(), ModContent.BuffType<Petrified>())];
		public override SynergyEffects Effects => new(
			[ // these are debuffs cleared on hit
				new(BuffID.Wet),
				ClearBuff.Create < SnowyEffect >(),
				ClearBuff.Create < FreezingEffect >(),
				new(BuffID.OnFire),
				ClearBuff.Create < AOBurning >(),
				new(BuffID.OnFire3),
				ClearBuff.Create < Melting >(),
				ClearBuff.Create < CharredEffect >(),
				new(BuffID.ShadowFlame),
				ClearBuff.Create < Singed >(),
				ClearBuff.Create < Scalding >()
			],
			[
				new(ModContent.BuffType<AOBleed>(),1.1f),
				new(BuffID.OnFire,1.02f),
				Synergy.Create<AOBurning>(1.02f),
				new(BuffID.Venom,1.075f),
				Synergy.Create<Corroding>(1.075f),
				new(ModContent.BuffType<Singed>(), 1.2f),
				new(BuffID.Slimed,1.075f),
				new(BuffID.Oiled,1.075f),
				new(BuffID.OnFire3,1.075f),
				Synergy.Create<Melting>(1.075f),
				new(BuffID.ShadowFlame,1.15f),
				new(BuffID.Wet,0.995f),
				new(ModContent.BuffType<FreezingEffect>(),0.99f),
				new(ModContent.BuffType<CharredEffect>(),1.01f),
				new(ModContent.BuffType<SandyEffect>(),1.125f),
				new(ModContent.BuffType<Scalding>(),1.2f),
				new(ModContent.BuffType<SearedEffect>(),1.15f)
			]
			);

		public override void SpawningEffects(Rectangle area, Vector2 direction)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Ash, direction.X * 2f, direction.Y * 2f, Scale: 3f * area.RelativeScale())];
				spawnedDust.noGravity = true;
				Dust spawnedDust2 = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.RedTorch, direction.X * 2f, direction.Y * 2f, Scale: 2f * area.RelativeScale())];
				spawnedDust2.noGravity = true;
			}
		}

		public override void LingeringEffects(Rectangle area, Vector2? direction = null, Entity source = null)
		{
			_ = Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.RedTorch, Scale: 1f * area.RelativeScale());
			Dust spawnedDust = Dust.NewDustDirect(area.TopLeft(), area.Width, area.Height, DustID.Ash, Scale: 2f * area.RelativeScale());
			spawnedDust.noGravity = true;
		}
		public override void ExplosionEffects(Vector2 position, float intensity = 1f)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(position, 0, 0, DustID.RedTorch, (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize * intensity), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize * intensity), Scale: intensity)];
				Dust spawnedDust2 = Main.dust[Dust.NewDust(position, 0, 0, DustID.Ash, (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize * intensity), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize * intensity), Scale: 2f * intensity)];
				spawnedDust2.noGravity = true;
			}
		}

		public override void KillEffects(Rectangle area, Entity source = null)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust spawnedDust = Dust.NewDustDirect(area.TopLeft(), area.Width, area.Height, DustID.Ash, 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), Scale: 3f * area.RelativeScale());
				spawnedDust.noGravity = true;
				Dust spawnedDust2 = Dust.NewDustDirect(area.TopLeft(), area.Width, area.Height, DustID.RedTorch, 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), Scale: 2f * area.RelativeScale());
				spawnedDust2.noGravity = true;
			}
			if (source is Projectile projectile)
			{
				for (int n = 0; n < 10; n++)
				{
					Projectile.NewProjectile(projectile.GetSource_FromThis(), new(area.X + area.Width * Main.rand.NextFloat(), area.Y + area.Height * Main.rand.NextFloat()), new(1.23f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 1.23f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f)), ProjectileID.SporeCloud, 2 + AOUtils.BossesKilled, 0f);
				}
			}
			SoundEngine.PlaySound(ImbueSound, area.Center());

		}

		public override bool PreEffects(Entity entity = null)
		{
			if (entity is Projectile projectile)
				return base.PreEffects(projectile) && projectile.type != ProjectileID.SporeCloud;
			return base.PreEffects(entity);
		}


	}
}