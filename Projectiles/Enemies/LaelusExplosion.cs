using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Imbues.Relics;
using ArcaneOdyssey.Projectiles.Base;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Projectiles.Enemies
{
	public class LaelusExplosion : AOBaseProjectile
	{
		public override string Texture => AOUtils.BlankTexture;
		public override void SetDefaults()
		{
			Projectile.DamageType = DamageClass.Summon;
			Projectile.timeLeft = 25;
			Projectile.hostile = true;
			Projectile.height = Projectile.width = 170;
			Projectile.tileCollide = false;
		}

		public Imbuable Imbue = ModContent.GetInstance<TidestoneBand>();

		public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)
		{
			modifiers = AOUtils.CalculateImbueDamage(Imbue, target, modifiers);
		}


		public bool sentMessage = false;
		public override void AI()
		{
			if (ArcaneOdysseyClientConfig.Instance.AbilityText && !Main.dedServ && !sentMessage)
			{
				sentMessage = true;
				CombatText.NewText(Projectile.Hitbox, Imbue?.Colour ?? Color.White, (DisplayName + "!").Trim(), true);
			}
			Imbue?.ExplosionEffects(Projectile.Center);
		}

		public override bool PreDraw(ref Color lightColor) => false;
	}
}
