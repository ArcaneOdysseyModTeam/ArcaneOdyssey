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

		public static Projectile CreateMagicCircle(Item item, Player player, Imbuable magicToUse)
		{
			if (magicToUse is AOMagic && Main.myPlayer == player.whoAmI)
			{
				SoundEngine.PlaySound(SoundID.Item84 with { Pitch = magicToUse.AOScrollSpeed.MultiToPercent().Clamp(-1, 1) }, player.Center);
				if (item.ModItem is AOMagic)
				{
					var proj = Projectile.NewProjectileDirect(player.GetSource_FromThis(), player.MountedCenter, Vector2.Zero, ModContent.ProjectileType<MagicCircle2>(), 0, 0f, player.whoAmI, 1);
					proj.ArcaneOdyssey().Imbue = magicToUse;
					return proj;
				}
				else if (item.ModItem is ExplosionScroll)
				{
					var proj = Projectile.NewProjectileDirect(item.GetSource_FromThis(), player.MountedCenter, Vector2.Zero, ModContent.ProjectileType<MagicCircle2>(), 0, 0f, player.whoAmI, 0, player.altFunctionUse);
					return proj;
				}
				else if (item.ModItem is BlastScroll)
				{
					Projectile circleprojectile = Main.projectile[Projectile.NewProjectile(item.GetSource_FromThis(), player.MountedCenter, Vector2.Zero, ModContent.ProjectileType<MagicCircle1>(), item.damage, 0f, player.whoAmI)];
					circleprojectile.rotation = player.SafeDirectionTo(Main.MouseWorld).ToRotation();
					Vector2 circleVec = circleprojectile.rotation.ToRotationVector2() * 30f;
					circleprojectile.position += circleVec;
					((MagicCircle1)circleprojectile.ModProjectile).ChargingProjectile = magicToUse.GetSkill("Blast");
					circleprojectile.ArcaneOdyssey().Imbue = magicToUse;
					return circleprojectile;
				}
				else if (item.ModItem is CannonScroll)
				{
					Projectile circleprojectile = Main.projectile[Projectile.NewProjectile(item.GetSource_FromThis(), player.MountedCenter, Vector2.Zero, ModContent.ProjectileType<MagicCircle1>(), item.damage, 0f, player.whoAmI)];
					circleprojectile.rotation = player.SafeDirectionTo(Main.MouseWorld).ToRotation();
					Vector2 circleVec = circleprojectile.rotation.ToRotationVector2() * 30f;
					circleprojectile.position += circleVec;
					((MagicCircle1)circleprojectile.ModProjectile).ChargingProjectile = magicToUse.GetSkill("Cannon");
					circleprojectile.ArcaneOdyssey().Imbue = magicToUse;
					return circleprojectile;
				}
				else if (item.ModItem is PulsarScroll)
				{
					Projectile circleprojectile = Main.projectile[Projectile.NewProjectile(item.GetSource_FromThis(), player.MountedCenter, Vector2.Zero, ModContent.ProjectileType<MagicCircle1>(), item.damage, 0f, player.whoAmI)];
					circleprojectile.rotation = player.SafeDirectionTo(Main.MouseWorld).ToRotation();
					Vector2 circleVec = circleprojectile.rotation.ToRotationVector2() * 30f;
					circleprojectile.position += circleVec;
					((MagicCircle1)circleprojectile.ModProjectile).ChargingProjectile = magicToUse.GetSkill("Pulsar");
					circleprojectile.ArcaneOdyssey().Imbue = magicToUse;
					return circleprojectile;
				}
				else if (item.ModItem is BeamScroll)
				{
					Projectile circleprojectile = Main.projectile[Projectile.NewProjectile(item.GetSource_FromThis(), player.MountedCenter, Vector2.Zero, ModContent.ProjectileType<MagicCircle1>(), item.damage, 0f, player.whoAmI)];
					circleprojectile.rotation = player.SafeDirectionTo(Main.MouseWorld).ToRotation();
					Vector2 circleVec = circleprojectile.rotation.ToRotationVector2() * 30f;
					circleprojectile.position += circleVec;
					((MagicCircle1)circleprojectile.ModProjectile).ChargingProjectile = ModContent.ProjectileType<BeamSpell>();
					circleprojectile.ArcaneOdyssey().Imbue = magicToUse;
					return circleprojectile;
				}
				else if (item.ModItem is LeapScroll)
				{
					var proj = Projectile.NewProjectileDirect(item.GetSource_FromThis(), player.Bottom, Vector2.Zero, ModContent.ProjectileType<MagicCircle1>(), 0, 0, player.whoAmI);
					proj.rotation = MathHelper.PiOver2;
					proj.netUpdate = true;
					((MagicCircle1)proj.ModProjectile).MarkedForDeath = true;
					return proj;
				}
			}
			return null;
		}
	}
}