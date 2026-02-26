using ArcaneOdyssey.Content.Buffs.DOT;
using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Imbues.Magic.Lost;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;


namespace ArcaneOdyssey.Content.Items.Imbues.Magic.Normal
{
	public class PoisonMagic : AOMagic
	{
		public override void RegisterMutations()
		{
			RegisterMutation<PoisonLightningMagic>();
		}
		public override bool Special => true;
		public override float DashSpeed => 1.2f; // burst
		public override SoundStyle? ImbueSound => SoundID.Item17;
		public override Color ImbueColour => new(105, 0, 105, 255);
		public override float AOImbueSpeed => 1f;
		public override float AOImbueSize => 1.11f;
		public override float AOImbueDamage => 0.825f;
		public override float AOScrollSpeed => 1f;
		public override float AOScrollSize => 1.15f;
		public override float AOScrollDamage => 0.75f;
		public override Debuff[] ImbueDebuffs => [Debuff.Create<AOPoisoned>()];
		//public override AODebuff ImbueDebuff2 => new AODebuff(BuffID.Stinky, 60*10);
		public override SynergyEffects Effects => new(
			[ // these are debuffs cleared on hit
			
			],
			[
				Synergy.Create<AOBleed>(1.075f),
				
				Synergy.Create<AOBurning>(.99f),
				Synergy.Create<Scalding>(0.9f)
			]
			);

		public override void SpawningEffects(Rectangle area, Vector2 direction)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Cloud, direction.X * 0.4f, direction.Y * 0.4f, 0, Color.Purple, 3f * area.RelativeScale())];
				spawnedDust.noGravity = true;
			}
		}

		public override void LingeringEffects(Rectangle area, Vector2? direction = null, Entity source = null)
		{
			Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Cloud, 0f, 0f, 0, Color.Purple, 2f * area.RelativeScale())];
			spawnedDust.noGravity = true;
		}
		public override void ExplosionEffects(Vector2 position, float intensity = 1f)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(position, 0, 0, DustID.Cloud, (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize * intensity), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize * intensity), 0, Color.Purple, 3f * intensity)];
				spawnedDust.noGravity = true;
			}
		}
		public override void KillEffects(Rectangle area, Entity source = null)
		{
			for (int n = 0; n < 30; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Cloud, 5f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 5f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 0, Color.Purple, 3f)];
				spawnedDust.noGravity = true;
				if (source is Projectile projectile && n / 2 >= 10)
					Projectile.NewProjectile(projectile.GetSource_FromThis(), new(area.X + area.Width * Main.rand.NextFloat(), area.Y + area.Height * Main.rand.NextFloat()), new(1.25f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 1.25f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f)), Main.rand.Next([ProjectileID.SporeGas, ProjectileID.SporeGas2, ProjectileID.SporeGas3]), 2 + AOUtils.BossesKilled, 0f);
			}
			SoundEngine.PlaySound(ImbueSound, area.Center());
		}
	}
}