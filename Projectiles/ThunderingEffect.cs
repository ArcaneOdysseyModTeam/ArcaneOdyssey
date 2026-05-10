using ArcaneOdyssey.Imbues.Magic.Normal;
using ArcaneOdyssey.Projectiles.Base;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Projectiles
{
	public class ThunderingEffect : PlayerProjectile
	{
		public override string Texture => AOUtils.BlankTexture;

		private bool hidden = false;

		public override bool CanHaveImbueVFX => !hidden;

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
			hidden = AOPlayerOwner.hiddenThunder;
			Imbue = ModContent.GetInstance<LightningMagic>();
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(hidden);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			hidden = reader.ReadBoolean();
		}

		public override bool? CanCutTiles() => false;
	}
}
