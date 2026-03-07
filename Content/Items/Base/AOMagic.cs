using ArcaneOdyssey.Content.Items.Consumable;
using ArcaneOdyssey.Content.Items.Scrolls.Equipment.Common;
using ArcaneOdyssey.Content.Items.Scrolls.Usable.Common;
using ArcaneOdyssey.Content.Items.Scrolls.Usable.Lost;
using ArcaneOdyssey.Content.Items.Scrolls.Usable.Rare;
using ArcaneOdyssey.Content.Projectiles.Circles;
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
	public abstract class AOMagic : Imbuable
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			if (ArcaneOdysseyMod.Mutations.Length == ItemID.Count)
				ArcaneOdysseyMod.Mutations = ItemID.Sets.Factory.CreateCustomSet<List<int>>(null);
			ArcaneOdysseyMod.Mutations[Type] = [];
			RegisterMutations();
			ItemID.Sets.ItemNoGravity[Type] = true;
		}

		public virtual void RegisterMutations() { }

		public void RegisterMutation<T>() where T : AOMagic
		{
			ArcaneOdysseyMod.Mutations[Type].Add(ModContent.ItemType<T>());
		}

		/// <summary>
		/// Remove later
		/// </summary>
		public override void AddRecipes()
		{
			if (ImbuableTier != AOImbuableTier.Normal) 
				return;

			foreach (var mutation in ArcaneOdysseyMod.Mutations[Type])
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
			Item.damage = 10 + (100 * (int)ImbuableTier);
			Item.shootSpeed = 7f * AOScrollSpeed;
		}

		public override void ModifyManaCost(Player player, ref float reduce, ref float mult)
		{
			if (player.AltUse())
				mult *= 0;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			//foreach (var mutation in ArcaneOdysseyMod.Mutations[Type])
			//{
			//	Main.NewText(Lang.GetItemNameValue(mutation));
			//}
			CreateMagicCircle(Item, player, this, damage);
			return false;
		}

		public static Projectile CreateMagicCircle(Item item, Player player, Imbuable magicToUse, int damage = 0)
		{
			if (magicToUse is not null && Main.myPlayer == player.whoAmI)
			{
				var rot = player.SafeDirectionTo(Main.MouseWorld);
				if (item.ModItem is AOMagic)
				{
					if (player.PlayerItem()?.ModItem?.Type != magicToUse.Type || player.AltUse())
					{
						Projectile circleprojectile = Projectile.NewProjectileDirect(item.GetSource_ItemUse(player), player.MountedCenter, Vector2.Zero, ModContent.ProjectileType<RotatingMagicCircle>(), 0, 0f, player.whoAmI);
						((RotatingMagicCircle)circleprojectile.ModProjectile).MarkedForDeath = true;
						return circleprojectile;
					}
					else
					{
						Projectile circleprojectile = Projectile.NewProjectileDirect(item.GetSource_ItemUse(player), player.MountedCenter + (rot * 30), Vector2.Zero, ModContent.ProjectileType<BasicMagicCircle>(), damage, item.knockBack, player.whoAmI);
						circleprojectile.rotation = rot.ToRotation();
						if (magicToUse.DashSpeed < 1.4f)
						{
							((BasicMagicCircle)circleprojectile.ModProjectile).ChargingProjectile = magicToUse.GetSkill("Blast");
						}
						else
						{
							((BasicMagicCircle)circleprojectile.ModProjectile).ChargingProjectile = ModContent.ProjectileType<LesserBeam>();
						}
						return circleprojectile;
					}
				}
				else if (item.ModItem is JavelinSpell)
				{
					Projectile circleprojectile = Projectile.NewProjectileDirect(item.GetSource_ItemUse(player), player.MountedCenter + (rot * 30), Vector2.Zero, ModContent.ProjectileType<RotatingMagicCircle>(), damage, item.knockBack, player.whoAmI);
					circleprojectile.rotation = rot.ToRotation();
					return circleprojectile;
				}
				else if (item.ModItem is BarrageSpell)
				{
					Projectile circleprojectile = Projectile.NewProjectileDirect(item.GetSource_ItemUse(player), player.MountedCenter + (rot * 30), Vector2.Zero, ModContent.ProjectileType<BarrageMagicCircle>(), damage, item.knockBack, player.whoAmI);
					circleprojectile.rotation = rot.ToRotation();
					((BarrageMagicCircle)circleprojectile.ModProjectile).ChargingProjectile = magicToUse.GetSkill("Blast");
					((BarrageMagicCircle)circleprojectile.ModProjectile).ProjectileSpread = MathHelper.PiOver4 / 2f / magicToUse.AOScrollSpeed.FlipFloat();
					return circleprojectile;
				}
				else if (item.ModItem is RaySpell)
				{
					Projectile circleprojectile = Projectile.NewProjectileDirect(item.GetSource_ItemUse(player), player.MountedCenter + (rot * 30), Vector2.Zero, ModContent.ProjectileType<BarrageMagicCircle>(), damage, item.knockBack, player.whoAmI);
					circleprojectile.rotation = rot.ToRotation();
					((BarrageMagicCircle)circleprojectile.ModProjectile).ChargingProjectile = ModContent.ProjectileType<MagicRay>();
					return circleprojectile;
				}
				else if (item.ModItem is ExplosionScroll)
				{
					Projectile circleprojectile = Projectile.NewProjectileDirect(item.GetSource_ItemUse(player), player.MountedCenter, Vector2.Zero, ModContent.ProjectileType<RotatingMagicCircle>(), 0, 0f, player.whoAmI, 0, player.altFunctionUse);
					//player.ArcaneOdyssey().myCircle = circleprojectile;
					return circleprojectile;
				}
				else if (item.ModItem is CannonScroll)
				{
					Projectile circleprojectile = Projectile.NewProjectileDirect(item.GetSource_ItemUse(player), player.MountedCenter + (rot * 30), Vector2.Zero, ModContent.ProjectileType<BasicMagicCircle>(), damage, item.knockBack, player.whoAmI);
					circleprojectile.rotation = rot.ToRotation();
					((BasicMagicCircle)circleprojectile.ModProjectile).ChargingProjectile = magicToUse.GetSkill("Cannon");
					return circleprojectile;
				}
				else if (item.ModItem is PulsarScroll)
				{
					Projectile circleprojectile = Projectile.NewProjectileDirect(item.GetSource_ItemUse(player), player.MountedCenter + (rot * 30), Vector2.Zero, ModContent.ProjectileType<BasicMagicCircle>(), damage, item.knockBack, player.whoAmI);
					circleprojectile.rotation = rot.ToRotation();
					((BasicMagicCircle)circleprojectile.ModProjectile).originallyAltFire = player.AltUse();
					((BasicMagicCircle)circleprojectile.ModProjectile).ChargingProjectile = magicToUse.GetSkill("Pulsar");
					return circleprojectile;
				}
				else if (item.ModItem is BeamScroll)
				{
					Projectile circleprojectile = Projectile.NewProjectileDirect(item.GetSource_ItemUse(player), player.MountedCenter + (rot * 30), Vector2.Zero, ModContent.ProjectileType<BasicMagicCircle>(), damage, item.knockBack, player.whoAmI);
					circleprojectile.rotation = rot.ToRotation();
					((BasicMagicCircle)circleprojectile.ModProjectile).ChargingProjectile = ModContent.ProjectileType<BeamSpell>();
					return circleprojectile;
				}
				else if (item.ModItem is LeapScroll)
				{
					var proj = Projectile.NewProjectileDirect(item.GetSource_ItemUse(player), player.Bottom, Vector2.Zero, ModContent.ProjectileType<BasicMagicCircle>(), 0, 0, player.whoAmI);
					proj.rotation = MathHelper.PiOver2;
					((BasicMagicCircle)proj.ModProjectile).MarkedForDeath = true;
					return proj;
				}
				else if (item.ModItem is ArrayScroll)
				{
					Projectile circleprojectile = Projectile.NewProjectileDirect(item.GetSource_ItemUse(player), player.MountedCenter - (Vector2.UnitY * 30), Vector2.Zero, ModContent.ProjectileType<BasicMagicCircle>(), damage, item.knockBack, player.whoAmI);
					circleprojectile.rotation = MathHelper.PiOver2;
					((BasicMagicCircle)circleprojectile.ModProjectile).ChargingProjectile = magicToUse.GetSkill("Array");
					return circleprojectile;
				}
				else if (item.ModItem is AnnihilationScroll)
				{
					var proj = Projectile.NewProjectileDirect(item.GetSource_ItemUse(player), player.Bottom, Vector2.Zero, ModContent.ProjectileType<BasicMagicCircle>(), 0, 0, player.whoAmI);
					proj.rotation = MathHelper.PiOver2;
					((BasicMagicCircle)proj.ModProjectile).MarkedForDeath = true;
					return proj;
				}
			}
			return null;
		}
	}
}