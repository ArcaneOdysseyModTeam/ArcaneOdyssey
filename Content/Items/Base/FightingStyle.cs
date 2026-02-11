using ArcaneOdyssey.Content.Projectiles.Berserker;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Base
{
	public abstract class FightingStyle : Imbuable, ILocalizedModType
	{
		public override string LocalizationCategory => base.LocalizationCategory + ".FightingStyles." + ImbuableTier;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.DamageType = DamageClass.MeleeNoSpeed;
			Item.shoot = ModContent.ProjectileType<BasicStrike>();
			Item.autoReuse = true;
			Item.damage = 12;
			Item.shootSpeed = 2f;
			Item.knockBack = 10f;
		}
	}
}
