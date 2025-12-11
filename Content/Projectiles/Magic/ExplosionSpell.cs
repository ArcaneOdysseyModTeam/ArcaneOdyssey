using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Projectiles.Base;
using Microsoft.Xna.Framework;
using Terraria;

namespace ArcaneOdyssey.Content.Projectiles.Magic
{
	public class ExplosionSpell : MagicSpell
	{
		// ai[0] will be damage multiplier
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = -1;
			Projectile.penetrate = -1;
			Projectile.height = Projectile.width = 200;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 30;
			Projectile.ownerHitCheck = true;
		}

		public override void AI()
		{
			if (Projectile.TryGetImbue(out Imbuable imbue) && imbue is AOMagic magic)
			{
				magic.ExplosionEffects(Projectile);
			}
		}

		public override void ModifyDamageHitbox(ref Rectangle hitbox)
		{
			if (Projectile.TryGetImbue(out Imbuable imbue) && imbue is AOMagic)
			{
				hitbox.Height = hitbox.Width = (int)(imbue.AOScrollSize * 200 * Projectile.ai[0]);
			}
		}

		public override bool PreDraw(ref Color lightColor) => false;
	}
}
