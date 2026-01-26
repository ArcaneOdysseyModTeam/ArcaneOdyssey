using ArcaneOdyssey.Content.Items.Weapons;
using ArcaneOdyssey.Content.Projectiles.Base;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Projectiles.Weapons
{
	public class NobleThunderspearProjectile : BaseSpearProjectile
	{
		public override string Texture => typeof(NobleThunderspear).Texture();
		public override AOItemTiers AOWeaponTier => AOItemTiers.Average;

		public override float AOSize => .85f;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.height = 70;
			Projectile.width = 72;
		}

		public override void EffectBeforeReelBack()
		{
			if (Projectile.ai[2] == 3)
			{
				AOPlayerOwner.SetCooldown(ModContent.BuffType<SparrowThrustCooldown>(), 60 * 5);
				SoundEngine.PlaySound(SoundID.DD2_ExplosiveTrapExplode, Projectile.Center);

				// sparrow thrust
			}
		}
	}
}
