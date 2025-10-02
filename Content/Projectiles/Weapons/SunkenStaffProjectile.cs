using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Projectiles.Base;
using ArcaneOdyssey.Content.Projectiles.Weapons.Abilities;
using Microsoft.Xna.Framework;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey.Content.Projectiles.Weapons
{
	public class SunkenStaffProjectile : BaseStaffProjectile
	{
		public override bool? Cold => true;
		public override float AOSpeed => .9f;
		public override float AOSize => 1.25f;
		public override float AODamage => 1f;
		public override AOWeaponTiers AOWeaponTier => AOWeaponTiers.Good;
		public override AODebuffRequirement Debuff => new(BuffID.Wet, 600);
		public override SoundStyle? DebuffApplySound => SoundID.Splash;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.height = Projectile.width = 120;
		}

		public override void PostAI()
		{
			if (!Main.dedServ) 
			{
				// dust
				for (int dustCountInt = 0;dustCountInt<2;dustCountInt++) 
				{
					Dust.NewDust(Projectile.Center, 3, 3, DustID.Water, 50f * (0.5f - Main.rand.NextFloat()) ,50f * (0.5f - Main.rand.NextFloat()), 255, default, 1.3f);
				}
			}
		}

		public override void EffectBeforeSpin(Player player, float spintime)
		{
			if (Projectile.owner == Main.myPlayer)
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, 17.5f * AOSpeed * player.SafeDirectionTo(Main.MouseWorld), ModContent.ProjectileType<FuryoftheSea>(), Projectile.damage, 0f, Projectile.owner, MathHelper.TwoPi * 2f / spintime * player.direction);
		}
	}
}
