using ArcaneOdyssey.Imbues.Magic.Normal;
using ArcaneOdyssey.Projectiles.Base;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Projectiles
{
	public class ThunderingEffect : PlayerProjectile
	{
		public override string Texture => AOUtils.BlankTexture;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.friendly = true;
			Projectile.extraUpdates = 100;
			Projectile.height = Projectile.width = 2;
			Projectile.ignoreWater = true;
			Projectile.DamageType = DamageClass.Generic;
		}

		public override void OnSpawn(IEntitySource source)
		{
			Imbue = ModContent.GetInstance<LightningMagic>();
		}

		public override bool? CanCutTiles() => false;
	}
}
