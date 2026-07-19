using ArcaneOdyssey;
using ArcaneOdyssey.AOPlayers;
using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Projectiles.Berserker;
using System.Collections.Generic;
using Terraria.Audio;

namespace ArcaneOdyssey.Items.Scrolls.Equipment.Common
{
	public class BreathtakerTechnique : CommonScroll
	{
		public override bool CanHaveFS => true;
		public const int Cooldown = 60 * 10;
		public override bool MetConditions() => NPC.downedBoss2;
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.accessory = true;
			Item.damage = 20;
			Item.DamageType = AOUtils.TrueMeleeNoSpeed();
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			base.ModifyTooltips(tooltips);
			tooltips.RemoveAll((TooltipLine line) => line.Name == "Speed");
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			if (HasCorrectImbue)
			{
				player.ArcaneOdyssey()?.SetDash(new Breathtaker(this));
			}
		}
	}

	public class Breathtaker(Scroll scroll) : ModDash(scroll.Item)
	{
		public override bool Immune => true;
		public override float DashSpeed => 120;
		public override int DashMax => 2;
		public override bool LocksPlayer => true;
		public override int Cooldown => BreathtakerTechnique.Cooldown;

		public override bool OnHit(Player player, NPC target) => true;

		public override void OnEnd(Player player)
		{
			player.velocity *= .01f;
			scroll.ActivateAbility(player);
		}

		public override bool ContactDamage => false;

		public override void OnStart(Player player)
		{
			SoundEngine.PlaySound(SoundID.Item67);
			var proj = Projectile.NewProjectileDirect(Source.GetSource_ItemUse(player), player.Center, (player.velocity + player.ArcaneOdyssey().DashVelocity).SafeNormalize(Vector2.Zero), ModContent.ProjectileType<BreathtakerProjectile>(), Damage, Knockback, player.whoAmI);
			//proj.timeLeft = player.ArcaneOdyssey().DashLeft;
			// use newprojectile instead of shootprojectile because we actually dont wanna modify the velocity
		}

		public override int DisplayedCooldownID => ModContent.BuffType<BreathtakerCooldown>();
	}

	public class BreathtakerCooldown : DisplayedCooldown
	{
		public override string Texture => AOUtils.GetTexture<BreathtakerTechnique>();
	}
}
