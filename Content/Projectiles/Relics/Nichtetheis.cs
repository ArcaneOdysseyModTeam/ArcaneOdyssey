using ArcaneOdyssey.Content.Buffs.MagicMarks;
using ArcaneOdyssey.Content.Projectiles.Base;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Projectiles.Relics
{
	public class Nichtetheis : SpiritProjectile
	{
		public override string Texture => Mod.Name + "/Backgrounds/Blank";
		public override AODebuffRequirement? Debuff => new(ModContent.BuffType<DrainedEffect>(), 60 * 5);
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.height = Projectile.width = 40; // hitscan
			Projectile.extraUpdates = 100;
			Projectile.timeLeft = 60;
		}

		public override bool PreDraw(ref Color lightColor) => false;
	}
}
