using ArcaneOdyssey.Imbues.Magic.Normal;
using ArcaneOdyssey.Projectiles.Base;
using Terraria;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Projectiles
{
	public class ThunderingEffect : PlayerProjectile
	{
		public override string Texture => AOUtils.BlankTexture;

		public override bool CanHaveImbueVFX => !AOPlayerOwner.hiddenThunder;

		public override bool PreAI()
		{
			Imbue = ModContent.GetInstance<LightningMagic>();
			return Imbue is not null;
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.friendly = true;
			Projectile.extraUpdates = 100;
			Projectile.height = Projectile.height = 1;
			Projectile.ignoreWater = true;
			Projectile.DamageType = DamageClass.Generic;
		}
	}
}
