using ArcaneOdyssey.Content.Items.Scrolls.Equipment.Rare;
using ArcaneOdyssey.Content.Projectiles.Base;
using Microsoft.Xna.Framework;
using Terraria;

namespace ArcaneOdyssey.Content.Projectiles.Berserker
{
	public class BreathtakerProjectile : StrengthTechnique
	{
		public override string Texture => AOUtils.BlankTexture;

		public override bool CanHaveImbueVFX => false;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = Projectile.height = 2;
			Projectile.extraUpdates = 1000;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.ownerHitCheck = true;
		}

		public override bool? CanCutTiles() => false;

		public override bool? CanHitNPC(NPC target) => target.active && target.Hitbox.Distance(Owner.Center) < 100f && !target.friendly && target.immune[Owner.whoAmI] == 0 && !target.dontTakeDamage;

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			target.ArcaneOdyssey().LowerDefense(2, target.Hitbox);
		}
	}
}
