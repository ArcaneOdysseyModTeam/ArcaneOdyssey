using ArcaneOdyssey.Content.Items.Equipment.Scrolls;
using ArcaneOdyssey.Content.Items.Imbues.Magic.Lost;
using ArcaneOdyssey.Content.Items.Materials;
using ArcaneOdyssey.Content.Items.Weapons.Scrolls;
using ArcaneOdyssey.Content.Projectiles;
using ArcaneOdyssey.Content.Projectiles.Magic;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Base
{
	public abstract class AOMagic : Imbuable, ILocalizedModType
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			RegisterMutations();
			ItemID.Sets.ItemNoGravity[Type] = true;

			if (this is not (SoundMagic or SlashMagic))
			{
				var texture = AOUtils.GetTexture<AnnihilationSpell>().Replace("AnnihilationSpell", $"Annihilations/{ImbuableTier}/{AttackPrefix}Annihilation");
				if (!ModContent.HasAsset(texture))
				{
					ArcaneOdysseyMod.NoticeQueue.Add(DisplayName.Value + " is missing Annihilation sprite.");
				}
			}
		}

		public virtual void RegisterMutations() { }

		public void RegisterMutation<T>() where T : AOMagic
		{
			Mutations.Add(ModContent.ItemType<T>());
		}

		public List<int> Mutations = [];

		public override void Load()
		{
			Mutations = [];
		}

		public override void Unload()
		{
			Mutations = [];
		}

		public override string LocalizationCategory => base.LocalizationCategory + ".Magic." + ImbuableTier;

		/// <summary>
		/// Remove later
		/// </summary>
		public override void AddRecipes()
		{
			if (ImbuableTier != AOImbuableTier.Normal) return;
			foreach (var mutation in Mutations)
			{
				Recipe.Create(mutation).AddIngredient(Type).AddIngredient<HecateShard>().DisableDecraft().Register();
			}	
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.mana = (10 * AOScrollSpeed.FlipFloat()).Round();
			Item.DamageType = DamageClass.Magic;
			Item.shoot = GetSkill("Blast");
			Item.autoReuse = true;
			Item.damage = 10;
			Item.shootSpeed = 7f * AOScrollSpeed;
		}

		public override void ModifyManaCost(Player player, ref float reduce, ref float mult)
		{
			if (player.AltUse())
				mult *= 0;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			CreateMagicCircle(Item, player, this, damage);
			return false;
		}

		public static Projectile CreateMagicCircle(Item item, Player player, Imbuable magicToUse, int damage = 0)
		{
			if (magicToUse is AOMagic && Main.myPlayer == player.whoAmI)
			{
				var rot = player.SafeDirectionTo(Main.MouseWorld);
				if (item.ModItem is AOMagic)
				{
					if (player.PlayerItem()?.ModItem?.Type != magicToUse.Type || player.AltUse())
						return Projectile.NewProjectileDirect(item.GetSource_ItemUse(player), player.MountedCenter, Vector2.Zero, ModContent.ProjectileType<MagicCircle2>(), 0, 0f, player.whoAmI, 1);
					else
					{
						Projectile circleprojectile = Projectile.NewProjectileDirect(item.GetSource_ItemUse(player), player.MountedCenter + (rot * 30), Vector2.Zero, ModContent.ProjectileType<MagicCircle1>(), damage, item.knockBack, player.whoAmI);
						circleprojectile.rotation = rot.ToRotation();
						if (magicToUse.DashSpeed < 1.5f)
						{
							((MagicCircle1)circleprojectile.ModProjectile).ChargingProjectile = magicToUse.GetSkill("Blast");
						}
						else
							((MagicCircle1)circleprojectile.ModProjectile).ChargingProjectile = ModContent.ProjectileType<LesserBeam>();
						return circleprojectile;
					}
				}
				else if (item.ModItem is ExplosionScroll)
				{
					return Projectile.NewProjectileDirect(item.GetSource_ItemUse(player), player.MountedCenter, Vector2.Zero, ModContent.ProjectileType<MagicCircle2>(), 0, 0f, player.whoAmI, 0, player.altFunctionUse);
				}
				else if (item.ModItem is CannonScroll)
				{
					Projectile circleprojectile = Projectile.NewProjectileDirect(item.GetSource_ItemUse(player), player.MountedCenter + (rot * 30), Vector2.Zero, ModContent.ProjectileType<MagicCircle1>(), damage, item.knockBack, player.whoAmI);
					circleprojectile.rotation = rot.ToRotation();
					((MagicCircle1)circleprojectile.ModProjectile).ChargingProjectile = magicToUse.GetSkill("Cannon");
					return circleprojectile;
				}
				else if (item.ModItem is PulsarScroll)
				{
					Projectile circleprojectile = Projectile.NewProjectileDirect(item.GetSource_ItemUse(player), player.MountedCenter + (rot * 30), Vector2.Zero, ModContent.ProjectileType<MagicCircle1>(), damage, item.knockBack, player.whoAmI);
					circleprojectile.rotation = rot.ToRotation();
					((MagicCircle1)circleprojectile.ModProjectile).originallyAltFire = player.AltUse();
					((MagicCircle1)circleprojectile.ModProjectile).ChargingProjectile = magicToUse.GetSkill("Pulsar");
					return circleprojectile;
				}
				else if (item.ModItem is BeamScroll)
				{
					Projectile circleprojectile = Projectile.NewProjectileDirect(item.GetSource_ItemUse(player), player.MountedCenter + (rot * 30), Vector2.Zero, ModContent.ProjectileType<MagicCircle1>(), damage, item.knockBack, player.whoAmI);
					circleprojectile.rotation = rot.ToRotation();
					((MagicCircle1)circleprojectile.ModProjectile).ChargingProjectile = ModContent.ProjectileType<BeamSpell>();
					return circleprojectile;
				}
				else if (item.ModItem is LeapScroll)
				{
					var proj = Projectile.NewProjectileDirect(item.GetSource_ItemUse(player), player.Bottom, Vector2.Zero, ModContent.ProjectileType<MagicCircle1>(), 0, 0, player.whoAmI);
					proj.rotation = MathHelper.PiOver2;
					((MagicCircle1)proj.ModProjectile).MarkedForDeath = true;
					return proj;
				}
				else if (item.ModItem is ArrayScroll)
				{
					Projectile circleprojectile = Projectile.NewProjectileDirect(item.GetSource_ItemUse(player), player.MountedCenter - (Vector2.UnitY * 30), Vector2.Zero, ModContent.ProjectileType<MagicCircle1>(), damage, item.knockBack, player.whoAmI);
					circleprojectile.rotation = MathHelper.PiOver2;
					((MagicCircle1)circleprojectile.ModProjectile).ChargingProjectile = magicToUse.GetSkill("Array");
					return circleprojectile;
				}
				else if (item.ModItem is AnnihilationScroll)
				{
					var proj = Projectile.NewProjectileDirect(item.GetSource_ItemUse(player), player.Bottom, Vector2.Zero, ModContent.ProjectileType<MagicCircle1>(), 0, 0, player.whoAmI);
					proj.rotation = MathHelper.PiOver2;
					((MagicCircle1)proj.ModProjectile).MarkedForDeath = true;
					return proj;
				}
			}
			return null;
		}
	}
}