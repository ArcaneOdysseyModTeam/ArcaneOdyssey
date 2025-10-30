using ArcaneOdyssey.Content.Items.Equipment.Scrolls;
using ArcaneOdyssey.Content.Items.Materials;
using ArcaneOdyssey.Content.Items.Weapons.Scrolls;
using ArcaneOdyssey.Content.Projectiles;
using ArcaneOdyssey.Content.Projectiles.Base;
using ArcaneOdyssey.Content.Projectiles.Magic;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using System;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Base
{

	/// <summary>
	/// Imbue values are applied as multipliers to imbued projectiles,
	/// Magic values are applied as multipliers to projectiles created using spell scrolls
	/// </summary>
	public abstract class AOMagic : Imbuable, ILocalizedModType
	{
		public override string LocalizationCategory => "Magic." + ImbuableTier;

        public void CreateLostRecipe(params Type[] imbues)
        {
            List<int> types = [];
            foreach (var type in imbues)
            {
                types.Add(Mod.Find<ModItem>(type.Name).Type);
            }
            var group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " " + DisplayName.Value + " " + Mod.CustomLocalization("RandomWords.Material").Value, [.. types]);
            RecipeGroup.RegisterGroup(nameof(ArcaneOdyssey) + ":" + Name + "Material", group);
            var rec = Recipe.Create(Type);
            rec.AddRecipeGroup(group);
            rec.AddIngredient<HecateShard>();
            rec.DisableDecraft();
            rec.Register();
        }

        public void CreateAncientRecipe(params Type[] imbues)
        {
            List<int> types = [];
            foreach (var type in imbues)
            {
                types.Add(Mod.Find<ModItem>(type.Name).Type);
            }
            var group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " " + DisplayName.Value + " " + Mod.CustomLocalization("RandomWords.Material").Value, [.. types]);
            RecipeGroup.RegisterGroup(nameof(ArcaneOdyssey) + ":" + Name + "Material", group);
            var rec = Recipe.Create(Type);
            rec.AddRecipeGroup(group);
            rec.AddIngredient<AncientHecateOrb>();
            rec.DisableDecraft();
            rec.Register();
        }

        public static Projectile CreateMagicCircle(Item item, Player player, Imbuable magicToUse)
		{
			if (magicToUse is AOMagic)
			{
				SoundEngine.PlaySound(SoundID.Item84 with { Pitch=magicToUse.AOScrollSpeed.MultiToPercent().Clamp(-1, 1) }, player.Center);
				if (item.ModItem is AOMagic)
				{
					return Main.projectile[Projectile.NewProjectile(player.GetSource_FromThis(), player.MountedCenter.X, player.MountedCenter.Y, 0f, 0f, ModContent.ProjectileType<MagicCircle2>(), 0, 0f, player.whoAmI, 1, 0, magicToUse.Type)];
				}
				else if (item.ModItem is ExplosionScroll)
				{
					return Main.projectile[Projectile.NewProjectile(player.GetSource_FromThis(), player.Center.X, player.Center.Y, 0f, 0f, ModContent.ProjectileType<MagicCircle2>(), 0, 0f, player.whoAmI, 0, player.altFunctionUse, magicToUse.Type)];
				}
				else if (item.ModItem is BlastScroll)
				{
					Projectile circleprojectile = Main.projectile[Projectile.NewProjectile(item.GetSource_FromThis(), player.position.X + (player.width / 2f), player.position.Y + (player.height / 2f), 0f, 0f, ModContent.ProjectileType<MagicCircle1>(), item.damage, 0f, player.whoAmI)];
					circleprojectile.rotation = player.SafeDirectionTo(Main.MouseWorld).ToRotation();
					Vector2 circleVec = circleprojectile.rotation.ToRotationVector2() * 30f;
					circleprojectile.position += circleVec;
					((MagicCircle1)circleprojectile.ModProjectile).ChargingProjectile = magicToUse.Skills.GetValueOrDefault(typeof(BlastSpell), ProjectileID.WoodenArrowFriendly);
					circleprojectile.ArcaneOdyssey().Imbue = magicToUse;
					return circleprojectile;
				}
				else if (item.ModItem is CannonScroll)
				{
					Projectile circleprojectile = Main.projectile[Projectile.NewProjectile(item.GetSource_FromThis(), player.position.X + (player.width / 2f), player.position.Y + (player.height / 2f), 0f, 0f, ModContent.ProjectileType<MagicCircle1>(), item.damage, 0f, player.whoAmI)];
					circleprojectile.rotation = player.SafeDirectionTo(Main.MouseWorld).ToRotation();
					Vector2 circleVec = circleprojectile.rotation.ToRotationVector2() * 30f;
					circleprojectile.position += circleVec;
					((MagicCircle1)circleprojectile.ModProjectile).ChargingProjectile = magicToUse.Skills.GetValueOrDefault(typeof(CannonSpell), ProjectileID.WoodenArrowFriendly);
					circleprojectile.ArcaneOdyssey().Imbue = magicToUse;
					return circleprojectile;
				}
				else if (item.ModItem is PulsarScroll)
				{
					Projectile circleprojectile = Main.projectile[Projectile.NewProjectile(item.GetSource_FromThis(), player.position.X + (player.width / 2f), player.position.Y + (player.height / 2f), 0f, 0f, ModContent.ProjectileType<MagicCircle1>(), item.damage, 0f, player.whoAmI)];
					circleprojectile.rotation = player.SafeDirectionTo(Main.MouseWorld).ToRotation();
					Vector2 circleVec = circleprojectile.rotation.ToRotationVector2() * 30f;
					circleprojectile.position += circleVec;
					((MagicCircle1)circleprojectile.ModProjectile).ChargingProjectile = magicToUse.Skills.GetValueOrDefault(typeof(PulsarSpell), ProjectileID.WoodenArrowFriendly);
					circleprojectile.ArcaneOdyssey().Imbue = magicToUse;
					return circleprojectile;
				}
				else if (item.ModItem is BeamScroll)
				{
					Projectile circleprojectile = Main.projectile[Projectile.NewProjectile(item.GetSource_FromThis(), player.position.X + (player.width / 2f), player.position.Y + (player.height / 2f), 0f, 0f, ModContent.ProjectileType<MagicCircle1>(), item.damage, 0f, player.whoAmI)];
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
					proj.Center = player.Bottom;
					((MagicCircle1)proj.ModProjectile).MarkedForDeath = true;
					return proj;
				}
			}
			return null;
		}
    }
}