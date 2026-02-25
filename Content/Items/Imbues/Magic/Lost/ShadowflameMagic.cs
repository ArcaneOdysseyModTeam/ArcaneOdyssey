using ArcaneOdyssey.Content.Buffs.DOT;
using ArcaneOdyssey.Content.Buffs.MagicMarks;
using ArcaneOdyssey.Content.Buffs.Stuns;
using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Imbues.Magic.Ancient;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;


namespace ArcaneOdyssey.Content.Items.Imbues.Magic.Lost
{
	public class ShadowflameMagic : AOMagic
	{
		public override void RegisterMutations()
		{
			RegisterMutation<DeathMagic>();
			RegisterMutation<IonMagic>();
		}
		
		public override float DashSpeed => 1.2f; // burst
		public override bool? Cold => false;
		public override SoundStyle? ImbueSound => SoundID.Item20;
		public override Color ImbueColour => new(255, 100, 255);
		public override AOImbuableTier ImbuableTier => AOImbuableTier.Lost;
		public override bool CanBeWet => true;
		public override float AOImbueSpeed => 1.1f;
		public override float AOImbueSize => 1.15f;
		public override float AOImbueDamage => 1.1f;
		public override float AOScrollSpeed => 1.1f;
		public override float AOScrollSize => 1.15f;
		public override float AOScrollDamage => 1.05f;
		public override Debuff[] ImbueDebuffs => [Debuff.Create<MagicShadowflame>(60 * 10)];
		public override Combo[] CombinedDebuffs => [Combo.Create<CharredEffect, Petrified>()];
		public override SynergyEffects Effects => new(
			[ // these are debuffs cleared on hit
				ClearBuff.Create<AOBleed>(),
				ClearBuff.Create < FreezingEffect >(),
				ClearBuff.Create < SnowyEffect >(),
				ClearBuff.Create < CharredEffect >()
			],
			[
				new(ModContent.BuffType<AOBleed>(),1.15f),
				new(ModContent.BuffType<CharredEffect>(),1.01f),
				new(BuffID.Venom,1.05f),
				Synergy.Create<Corroding>(1.05f),
				new(ModContent.BuffType<Crystallized>(),0.85f),
				new(ModContent.BuffType<FreezingEffect>(),0.99f),
				new(ModContent.BuffType<SnowyEffect>(),0.99f),
				new(BuffID.Wet,0.99f),
				new(BuffID.OnFire3,1.05f),
				Synergy.Create<Melting>(1.05f),
				new(BuffID.Poisoned,1.05f),
				Synergy.Create<AOPoisoned>(1.05f),
				new(BuffID.OnFire,1.1f),
				Synergy.Create<AOBurning>(1.1f),
				new(BuffID.Slimed,1.075f),
				new(BuffID.Oiled,1.075f),
				new(ModContent.BuffType<SandyEffect>(),0.98f),
				new(ModContent.BuffType<Scalding>(),1.1f),
				new(ModContent.BuffType<SearedEffect>(),1.1f)

			]
			);

		public override void SpawningEffects(Rectangle area, Vector2 direction)
		{
			if (Main.dedServ)
				return;
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.FireworkFountain_Pink, direction.X * 2f, direction.Y * 2f, Scale: area.RelativeScale())];
				spawnedDust.noGravity = true;
				Dust spawnedDust2 = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Shadowflame, 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), Scale: 2.4f * area.RelativeScale())];
				spawnedDust2.noGravity = true;
			}
		}

		public override void LingeringEffects(Rectangle area, Vector2? direction = null, Entity source = null)
		{
			if (Main.dedServ)
				return;
			Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Shadowflame, Scale: 1.6f * area.RelativeScale());
			Dust spawnedDust = Dust.NewDustDirect(area.TopLeft(), area.Width, area.Height, DustID.FireworkFountain_Pink, Scale: 0.8f * area.RelativeScale());
			spawnedDust.noGravity = true;
		}
		public override void ExplosionEffects(Vector2 position, float intensity = 1f)
		{
			if (Main.dedServ)
				return;
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(position, 0, 0, DustID.FireworkFountain_Pink, (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize * intensity), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize * intensity), Scale: 1.3f * intensity)];
				spawnedDust.noGravity = true;
				Dust spawnedDust2 = Main.dust[Dust.NewDust(position, 0, 0, DustID.Shadowflame, (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize * intensity), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize * intensity), Scale: 2.8f * intensity)];
				spawnedDust2.noGravity = true;
			}
		}
		public override void KillEffects(Rectangle area, Entity source = null)
		{
			if (Main.dedServ)
				return;
			for (int n = 0; n < 10; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.FireworkFountain_Pink, 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), Scale: 2f * area.RelativeScale())];
				spawnedDust.noGravity = true;
				Dust spawnedDust2 = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Shadowflame, 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), Scale: 2.8f * area.RelativeScale())];
				spawnedDust2.noGravity = true;
			}
			SoundEngine.PlaySound(ImbueSound, area.Center());
		}
	}
}