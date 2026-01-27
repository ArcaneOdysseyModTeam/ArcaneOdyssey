using ArcaneOdyssey.Content.Items.Weapons.RavennaNoble;
using ArcaneOdyssey.Content.Projectiles.Base;
using ArcaneOdyssey.Content.Projectiles.Weapons.Abilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Projectiles.Weapons
{
	public class NobleThunderspearProjectile : BaseSpearProjectile
	{
		public override string Texture => AOUtils.GetTexture<NobleThunderspear>();
		public override AOItemTiers AOWeaponTier => AOItemTiers.Average;

		public override float AOSize => .85f;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.height = Projectile.width = 70;
		}

		public override void AI()
		{
			base.AI();
			if (Main.myPlayer == Projectile.owner && Projectile.ai[2] == 3)
				AOPlayerOwner.HeavySkillActive = true;
		}

		public override void EffectBeforeReelBack()
		{
			if (Main.myPlayer == Projectile.owner && Projectile.ai[2] == 3)
			{
				AOPlayerOwner.SetCooldown(ModContent.BuffType<SparrowThrustCooldown>(), 60 * 5);
				SoundEngine.PlaySound(SoundID.DD2_ExplosiveTrapExplode, Projectile.Center);

				AOUtils.ShootProjectile(Projectile.GetSource_FromThis(), Projectile.Center, (Projectile.rotation - MathHelper.PiOver4).ToRotationVector2() * 7.5f, ModContent.ProjectileType<SparrowThrust>(), Projectile.damage, Projectile.knockBack, Projectile.owner, Imbue, SecondImbue);
			}
		}
	}
}
