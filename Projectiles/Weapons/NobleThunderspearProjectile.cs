using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Items.Weapons.RavennaNoble;
using ArcaneOdyssey.Projectiles.Abilities;
using ArcaneOdyssey.Projectiles.Base;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Projectiles.Weapons
{
	public class NobleThunderspearProjectile : BaseSpearProjectile
	{
		public override string Texture => AOUtils.GetTexture<NobleThunderspear>();
		public override ItemTiers AOWeaponTier => ItemTiers.Average;

		public override float Size => .85f;

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
				if (Owner.PlayerItem()?.ModItem is Weapon weap)
				{
					weap.ActivateAbility(Owner, false);
				}
				AOPlayerOwner.SetCooldown<SparrowThrustCooldown>();
				SoundEngine.PlaySound(SoundID.DD2_ExplosiveTrapExplode, Projectile.Center);

				AOUtils.ShootProjectile(Projectile.GetSource_FromThis(), Projectile.Center, (Projectile.rotation - MathHelper.PiOver4).ToRotationVector2() * 7.5f, ModContent.ProjectileType<SparrowThrust>(), Projectile.damage, Projectile.knockBack, Projectile.owner, Imbue, SecondImbue);
			}
		}
	}
}
