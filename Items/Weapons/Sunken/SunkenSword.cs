using ArcaneOdyssey.AOPlayers;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Items.Materials;
using ArcaneOdyssey.Buffs.MagicMarks;
using ArcaneOdyssey.Items.Weapons.Bronze;
using ArcaneOdyssey.Projectiles.Abilities;


namespace ArcaneOdyssey.Items.Weapons.Sunken
{
	public class SunkenSword : AOWeapon
	{
		public override bool? Cold => true;
		public override float AOSpeed => 1.2f;
		public override float AOSize => .9f;
		public override float AODamage => 1f;
		public override int AOValue => 900;
		public override AORarities AORarity => AORarities.Rare;
		public override AOItemTiers AOWeaponTier => AOItemTiers.Good;
		public override Color Motif => Color.Aqua;
		public override SoundStyle UseSound => SoundID.SplashWeak;
		public override Debuff? WeaponDebuff => Debuff.Create<Soaked>(60 * 5);

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.width = 50;
			Item.height = 54;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.DamageType = AOUtils.TrueMelee();
			Item.autoReuse = true;
		}

		public override bool AltFunctionUse(Player player) => true;

		public override bool? UseItem(Player player)
		{
			if (!Main.dedServ)
			{
				// Particles from swinging
				Dust.NewDust(player.MountedCenter + new Vector2(player.direction * 3f * (Imbue?.AOImbueSize ?? 1f), 0f), 3, 3, DustID.Water, player.direction * 30f * (0.8f - Main.rand.NextFloat()) * (Imbue?.AOImbueSize ?? 1f), 30f * (0.5f - Main.rand.NextFloat()) * (Imbue?.AOImbueSpeed ?? 1f), 255, default, 1.3f);
			}
			return null;
		}

		public override void UseAnimation(Player player)
		{
			if (player.AltUse())
			{
				var dash = new RisingTide(Item);
				if (!dash.OnCooldown(player))
				{
					player.ArcaneOdyssey().StartDash(dash, -2, Imbue, true);
					ActivateAbility(player, false);
				}
			}
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient<RavennaSword>();
			recipe.AddIngredient<SunkenScrap>(2);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}

	public class RisingTide(Entity source) : DashSystem(source)
	{
		public override bool ContactDamage => false;
		public override float DashSpeed => 13;
		public override int DashMax => 30;
		public override bool LocksPlayer => true;
		public override bool Immune => false;
		public override int Cooldown => 60 * 3;
		public override bool OnHit(Player player, Entity target) => false;

		public override void DashEffect(Player player)
		{
			player.statDefense *= 1.15f;
			if (player.ArcaneOdyssey().DashLeft % 5 == 0)
			{
				player.direction *= -1;
			}
		}

		public override void OnStart(Player player)
		{
			if (!Main.dedServ)
			{
				SoundEngine.PlaySound(SoundID.Splash, player.position);
				// Adds dust
				//for (int dustCountInt = 0; dustCountInt < 50; dustCountInt++)
				//{
				//	Dust.NewDust(player.position + new Vector2(-20f + (40f * ((float)Math.Sin(dustCountInt * 3.0))), 0f), 3, 3, DustID.Water, player.velocity.X * dustCountInt * 0.02f, -1f * dustCountInt * player.gravDir, Scale: 1.3f);
				//	Dust.NewDust(player.position + new Vector2(20f + (40f * ((float)Math.Sin((dustCountInt * 3.0) + 3.14))), 0f), 3, 3, DustID.Water, player.velocity.X * dustCountInt * 0.02f, -1f * dustCountInt * player.gravDir, Scale: 1.3f);
				//	Dust.NewDust(player.position + new Vector2(-20f + (40f * ((float)Math.Sin(dustCountInt * 3.0))), 0f), 3, 3, DustID.DungeonWater, player.velocity.X * dustCountInt * 0.02f, -0.5f * dustCountInt * player.gravDir);
				//	Dust.NewDust(player.position + new Vector2(20f + (40f * ((float)Math.Sin((dustCountInt * 3.0) + 3.14))), 0f), 3, 3, DustID.DungeonWater, player.velocity.X * dustCountInt * 0.02f, -0.5f * dustCountInt * player.gravDir);
				//}
				AOUtils.ShootProjectile(source.GetSource_ItemUse(player), player.Center, Vector2.UnitY * -2f, ModContent.ProjectileType<RisingTideProjectile>(), Damage, Knockback, player.whoAmI, Imbue, SecondImbue);
			}
		}

		public override int DisplayedCooldownID => ModContent.BuffType<RisingTideCooldown>();
	}

	public class RisingTideCooldown : DisplayedCooldown
	{
		public override string ExtraIconTexture => AOUtils.GetTexture<SunkenSword>();
	}

}
