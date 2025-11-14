using ArcaneOdyssey.Content.Projectiles.Base;
using ArcaneOdyssey.Content.Projectiles.Weapons.Abilities;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Projectiles.Weapons
{
	public class SunkenStaffProjectile : BaseStaffProjectile
	{
		public override bool? Cold => true;
		public override float AOSpeed => .9f;
		public override float AOSize => 1.25f;
		public override float AODamage => 1f;
		public override AODebuffRequirement? Debuff => new(BuffID.Wet, 60 * 10);
		public override SoundStyle? DebuffApplySound => SoundID.Splash;

		public override void PostAI()
		{
			if (!Main.dedServ) 
			{
				// dust
				for (int dustCountInt = 0; dustCountInt < 2; dustCountInt++) 
				{
					Dust.NewDust(Projectile.Center, 3, 3, DustID.Water, 50f * (0.5f - Main.rand.NextFloat()) ,50f * (0.5f - Main.rand.NextFloat()), 255, default, 1.3f);
				}
			}
		}

		public override void EffectBeforeSpin(Player player)
		{
			if (Projectile.owner == Main.myPlayer)
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, 17.5f * AOSpeed * player.SafeDirectionTo(Main.MouseWorld), ModContent.ProjectileType<FuryoftheSea>(), Projectile.damage, 0f, Projectile.owner);
		}
	}
}
