using ArcaneOdyssey.AOPlayers;
using ArcaneOdyssey.Buffs.MagicMarks;
using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Items.Weapons.Sunken;
using ArcaneOdyssey.Projectiles.Abilities;
using ArcaneOdyssey.Projectiles.Base;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Projectiles.Weapons
{
	public class SunkenStaffProjectile : BaseStaffProjectile
	{
		public override bool? Cold => true;
		public override float Speed => .9f;
		public override float Size => 1.25f;
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
			if (!player.ArcaneOdyssey().OnCooldown<FuryoftheSeaCooldown>())
			{
				player.ArcaneOdyssey().SetCooldown<FuryoftheSeaCooldown>();
				if (Owner.PlayerItem()?.ModItem is Weapon weap)
				{
					weap.ActivateAbility(Owner, true);
				}
				if (Projectile.owner == Main.myPlayer)
					AOUtils.ShootProjectile(Projectile.GetSource_FromThis(), Projectile.Center, 17.5f * player.SafeDirectionTo(Main.MouseWorld), ModContent.ProjectileType<FuryoftheSea>(), Projectile.damage / 2, 0f, Projectile.owner, Imbue, SecondImbue);
			}
		}
	}

	public class FuryoftheSeaCooldown : DisplayedCooldown
	{
		public override string Texture => AOUtils.GetTexture<SunkenStaff>();

		public override int CooldownLength => 60;
	}
}
