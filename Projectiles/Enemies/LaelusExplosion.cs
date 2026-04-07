using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Imbues.Relics;
using ArcaneOdyssey.Projectiles.Base;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.CameraModifiers;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Projectiles.Enemies
{
	public class LaelusExplosion : BaseProjectile
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
			if (!Main.dedServ && !sentMessage)
			{
				sentMessage = true;
				PunchCameraModifier modifier = new(Projectile.Center, (Main.rand.NextFloat() * MathHelper.TwoPi).ToRotationVector2(), 10f, 2f, 10, 100f, FullName);
				Main.instance.CameraModifiers.Add(modifier);
				if (ArcaneOdysseyClientConfig.Instance.AbilityText)
				{
					CombatText.NewText(Projectile.Hitbox, Imbue?.Colour ?? Color.White, (DisplayName + "!").Trim(), true);
				}
			}
			Imbue?.ExplosionEffects(Projectile.Center);
		}

		public override bool PreDraw(ref Color lightColor) => false;
	}
}
