using ArcaneOdyssey.Buffs.DOT;
using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Imbues.Magic.Lost;
using ArcaneOdyssey.Projectiles.Magic.Effects;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;


namespace ArcaneOdyssey.Imbues.Magic.Normal
{
	public class PoisonMagic : MagicType
	{
		public override void RegisterMutations()
		{
			RegisterMutation<PoisonLightningMagic>();
		}
		public override bool Special => true;
		public override float DashSpeed => 1.2f; // burst
		public override SoundStyle? ImbueSound => SoundID.Item17;
		public override Color ImbueColour => new(105, 0, 105, 255);
		public override float ImbueSpeed => 1f;
		public override float ImbueSize => 1.11f;
		public override float ImbueDamage => 0.825f;
		public override float ScrollSpeed => 1f;
		public override float ScrollSize => 1.15f;
		public override float ScrollDamage => 0.75f;
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

		public override int BlastFrames => 7;

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
				Dust spawnedDust = Main.dust[Dust.NewDust(position, 0, 0, DustID.Cloud, (Main.rand.NextFloat() - 0.5f) * (20f * intensity), (Main.rand.NextFloat() - 0.5f) * (20f * intensity), 0, Color.Purple, 3f * intensity)];
				spawnedDust.noGravity = true;
			}
		}
		public override void KillEffects(Rectangle area, Entity source = null)
		{
			for (int n = 0; n < 30; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Cloud, 5f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 5f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 0, Color.Purple, 3f)];
				spawnedDust.noGravity = true;
			}
			if (source is Projectile projectile && Main.myPlayer == projectile.owner)
			{
				var proj = Projectile.NewProjectileDirect(projectile.GetSource_FromThis(), new(area.X + area.Width * Main.rand.NextFloat(), area.Y + area.Height * Main.rand.NextFloat()), Vector2.Zero, ModContent.ProjectileType<PoisonCloud>(), 2 * (AOUtils.BossesKilled + 1), 0f);
				proj.scale *= projectile.Hitbox.RelativeScale(max: 2f);
				proj.Hitbox = proj.Hitbox.Scaled(projectile.Hitbox.RelativeScale(max: 2f));
				proj.netUpdate = true;
			}
			SoundEngine.PlaySound(ImbueSound, area.Center());
		}
	}
}