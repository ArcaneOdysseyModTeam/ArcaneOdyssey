using ArcaneOdyssey;
using ArcaneOdyssey.AOPlayers;
using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Projectiles.Berserker;
using ArcaneOdyssey.Skills.Base;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Items.Scrolls.Dashes.Common
{
	public class BreathtakerTechnique : CommonScroll
	{
		public override bool CanHaveFS => true;

		public override ModSkill Skill => ModContent.GetInstance<BreathtakerSkill>();

		public const int Cooldown = 60 * 10;
		public override bool MetConditions() => NPC.downedBoss2;
	}

	public class BreathtakerSkill : DashSkill
	{
		public override int Scroll => ModContent.ItemType<BreathtakerTechnique>();
		public override int Damage => 20;

		public override void Activate(Player player, Imbuable imbue)
		{
			player.ArcaneOdyssey()?.SetDash(new Breathtaker(imbue));
		}
	}

	public class Breathtaker(Imbuable scroll) : ModDash(scroll.Item)
	{
		public override bool Immune => true;
		public override float DashSpeed => 120;
		public override int DashMax => 2;
		public override bool LocksPlayer => true;
		public override int Cooldown => BreathtakerTechnique.Cooldown;
		public override DamageClass DamageType => AOUtils.TrueMeleeNoSpeed();

		public override bool OnHit(Player player, NPC target) => true;

		public override void OnEnd(Player player)
		{
			player.velocity *= .01f;
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
