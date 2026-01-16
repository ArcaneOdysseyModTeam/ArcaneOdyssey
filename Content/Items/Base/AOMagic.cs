using ArcaneOdyssey.Content.Items.Equipment.Scrolls;
using ArcaneOdyssey.Content.Items.Materials;
using ArcaneOdyssey.Content.Items.Weapons.Scrolls;
using ArcaneOdyssey.Content.Projectiles;
using ArcaneOdyssey.Content.Projectiles.Magic;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using System;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Mono.Cecil.Cil;

namespace ArcaneOdyssey.Content.Items.Base
{

	/// <summary>
	/// Imbue values are applied as multipliers to imbued projectiles,
	/// Magic values are applied as multipliers to projectiles created using spell scrolls
	/// </summary>
	public abstract class AOMagic : Imbuable, ILocalizedModType
	{
		public override string LocalizationCategory => base.LocalizationCategory + ".Magic." + ImbuableTier;

		public void CreateLostRecipe(params Type[] imbues)
		{
			if (imbues.Length > 1)
			{
				List<int> types = [];
				foreach (var type in imbues)
				{
					types.Add(Mod.Find<ModItem>(type.Name).Type);
				}
				var group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " " + DisplayName.Value + " " + Mod.CustomLocalization("RandomWords.Material").Value, [.. types]);
				RecipeGroup.RegisterGroup(Mod.Name + ":" + Name + "Material", group);
				var rec = Recipe.Create(Type);
				rec.AddRecipeGroup(group);
				rec.AddIngredient<HecateShard>();
				rec.DisableDecraft();
				rec.Register();
			}
			else if (imbues.Length == 1)
			{
				var rec = CreateRecipe();
				rec.AddIngredient(Mod.Find<ModItem>(imbues[0].Name).Type);
				rec.AddIngredient<HecateShard>();
				rec.DisableDecraft();
				rec.Register();
			}
		}

		public void CreateAncientRecipe(params Type[] imbues)
		{
			//if (imbues.Length > 1)
			//{
			//	List<int> types = [];
			//	foreach (var type in imbues)
			//	{
			//		types.Add(Mod.Find<ModItem>(type.Name).Type);
			//	}
			//	var group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " " + DisplayName.Value + " " + Mod.CustomLocalization("RandomWords.Material").Value, [.. types]);
			//	RecipeGroup.RegisterGroup(Mod.Name + ":" + Name + "Material", group);
			//	var rec = Recipe.Create(Type);
			//	rec.AddRecipeGroup(group);
			//	rec.AddIngredient<AncientHecateOrb>();
			//	rec.DisableDecraft();
			//	rec.Register();
			//}
			//else if (imbues.Length == 1)
			//{
			//	var rec = CreateRecipe();
			//	rec.AddIngredient(Mod.Find<ModItem>(imbues[0].Name).Type);
			//	rec.AddIngredient<AncientHecateOrb>();
			//	rec.DisableDecraft();
			//	rec.Register();
			//}
		}

		public static Projectile CreateMagicCircle(Item item, Player player, Imbuable magicToUse, int damage = 0)
		{
			if (magicToUse is AOMagic && Main.myPlayer == player.whoAmI)
			{
				SoundEngine.PlaySound(SoundID.Item84 with { Pitch = magicToUse.AOScrollSpeed.MultiToPercent().Clamp(-1, 1) }, player.Center);
				var rot = player.SafeDirectionTo(Main.MouseWorld);
				if (item.ModItem is AOMagic)
				{
					return Projectile.NewProjectileDirect(item.GetSource_ItemUse(player), player.MountedCenter, Vector2.Zero, ModContent.ProjectileType<MagicCircle2>(), 0, 0f, player.whoAmI, 1);
				}
				else if (item.ModItem is ExplosionScroll)
				{
					return Projectile.NewProjectileDirect(item.GetSource_ItemUse(player), player.MountedCenter, Vector2.Zero, ModContent.ProjectileType<MagicCircle2>(), 0, 0f, player.whoAmI, 0, player.altFunctionUse);
				}
				else if (item.ModItem is BlastScroll)
				{
					Projectile circleprojectile = Projectile.NewProjectileDirect(item.GetSource_ItemUse(player), player.MountedCenter + (rot * 30), Vector2.Zero, ModContent.ProjectileType<MagicCircle1>(), damage, item.knockBack, player.whoAmI);
					circleprojectile.rotation = rot.ToRotation();
					((MagicCircle1)circleprojectile.ModProjectile).ChargingProjectile = magicToUse.GetSkill("Blast");
					return circleprojectile;
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
			}
			return null;
		}
	}
}