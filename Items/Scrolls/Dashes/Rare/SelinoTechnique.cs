using ArcaneOdyssey.AOPlayers;
using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Projectiles.Berserker;
using ArcaneOdyssey.Skills.Base;

namespace ArcaneOdyssey.Items.Scrolls.Dashes.Rare
{
	public class SelinoTechnique : RareScroll
	{
		public const int Cooldown = 60 * 10;

		public override bool CanHaveFS => true;

		public override ModSkill Skill => ModContent.GetInstance<SelinoSkill>();
	}

	public class SelinoSkill : DashSkill
	{
		public override int Damage => 50;
		public override int Scroll => ModContent.ItemType<SelinoTechnique>();
		public override float Knockback => 8f;

		public override void Activate(Player player, Imbuable imbue)
		{
			player.ArcaneOdyssey()?.SetDash(new Selino1(imbue));
		}
	}

	public class Selino1(Imbuable scroll) : ModDash(scroll.Item)
	{
		public override DamageClass DamageType => AOUtils.TrueMeleeNoSpeed();
		public override bool ContactDamage => false;
		public override int Cooldown => SelinoTechnique.Cooldown;

		public override bool LocksPlayer => true;

		public override bool OnHit(Player player, NPC target) => false;

		public override void OnEnd(Player player)
		{
			Imbue?.Dash?.ActivateAbility(player, Imbue);
			var dash = new Selino2(Source);
			player.ArcaneOdyssey().StartDash(dash, 0, Imbue, true);
			AOUtils.ShootProjectile(Source.GetSource_ItemUse(player), player.Center, player.SafeDirectionTo(Main.MouseWorld, Vector2.UnitX), ModContent.ProjectileType<ShockwaveSmash>(), Damage, Knockback, player.whoAmI, Imbue, SecondImbue, true);
		}

		public override float DashSpeed => 8;

		public override int DashMax => 15;

		public override bool Immune => true;

		public override int DisplayedCooldownID => ModContent.BuffType<SelinoCooldown>();
	}

	public class Selino2(Entity source) : ModDash(source)
	{
		public override DamageClass DamageType => AOUtils.TrueMeleeNoSpeed();
		public override bool ContactDamage => false;
		public override int Cooldown => SelinoTechnique.Cooldown;

		public override bool LocksPlayer => true;

		public override bool OnHit(Player player, NPC target) => false;

		public override void OnEnd(Player player)
		{
			var dash = new Selino3(Source);
			player.ArcaneOdyssey().StartDash(dash, 0, Imbue, true);
			AOUtils.ShootProjectile(Source.GetSource_ItemUse(player), player.Center, player.SafeDirectionTo(Main.MouseWorld, Vector2.UnitX), ModContent.ProjectileType<ShockwaveSmash>(), Damage, Knockback, player.whoAmI, Imbue, SecondImbue, true);
		}

		public override float DashSpeed => 8;

		public override int DashMax => 15;

		public override bool Immune => true;

		public override int DisplayedCooldownID => ModContent.BuffType<SelinoCooldown>();
	}

	public class Selino3(Entity source) : ModDash(source)
	{
		public override DamageClass DamageType => AOUtils.TrueMeleeNoSpeed();
		public override bool ContactDamage => false;
		public override int Cooldown => SelinoTechnique.Cooldown;

		public override bool LocksPlayer => true;

		public override bool OnHit(Player player, NPC target) => false;

		public override void OnEnd(Player player)
		{
			player.velocity *= .25f;
			AOUtils.ShootProjectile(Source.GetSource_ItemUse(player), player.Center, player.SafeDirectionTo(Main.MouseWorld, Vector2.UnitX), ModContent.ProjectileType<Selino>(), Damage, Knockback, player.whoAmI, Imbue, SecondImbue, true);
		}

		public override float DashSpeed => 8;

		public override int DashMax => 15;

		public override float Knockback => base.Knockback * 4f;

		public override bool Immune => true;

		public override int DisplayedCooldownID => ModContent.BuffType<SelinoCooldown>();
	}

	public class SelinoCooldown : DisplayedCooldown
	{
		public override string Texture => AOUtils.GetTexture<SelinoTechnique>();
	}
}
