using ArcaneOdyssey.Content.Buffs.MagicMarks;
using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Projectiles.Abilities;
using ArcaneOdyssey.Content.Projectiles.Base;
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
		public override Debuff? ProjectileDebuff => Debuff.Create<Soaked>();
		public override SoundStyle? HitSound => SoundID.Splash;

		public override void PostAI()
		{
			base.PostAI();
			if (!Main.dedServ)
			{
				// dust
				for (int dustCountInt = 0; dustCountInt < 2; dustCountInt++)
				{
					Dust.NewDust(Projectile.Center, 3, 3, DustID.Water, 50f * (0.5f - Main.rand.NextFloat()), 50f * (0.5f - Main.rand.NextFloat()), 255, default, 1.3f);
				}
			}
		}

		public override void EffectBeforeSpin(Player player)
		{
			if (Owner.PlayerItem()?.ModItem is AOWeapon weap)
			{
				weap.ActivateAbility(Owner, true);
			}
			if (Projectile.owner == Main.myPlayer)
				AOUtils.ShootProjectile(Projectile.GetSource_FromThis(), Projectile.Center, 17.5f * player.SafeDirectionTo(Main.MouseWorld), ModContent.ProjectileType<FuryoftheSea>(), Projectile.damage / 2, 0f, Projectile.owner, Imbue, SecondImbue);
		}
	}
}
