using ArcaneOdyssey.Content.Buffs.MagicMarks;
using ArcaneOdyssey.Content.Items.Equipment.Scrolls;
using ArcaneOdyssey.Content.Items.Magic;
using ArcaneOdyssey.Content.Items.Materials;
using ArcaneOdyssey.Content.Items.Scrolls;
using ArcaneOdyssey.Content.Projectiles;
using ArcaneOdyssey.Content.Projectiles.Base;
using ArcaneOdyssey.Content.Projectiles.Magic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.Chat;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey.Content.Items.Base
{

	/// <summary>
	/// Imbue values are applied as multipliers to imbued projectiles,
	/// Magic values are applied as multipliers to projectiles created using spell scrolls
	/// </summary>
	public abstract class AOMagic : Imbuable, ILocalizedModType
	{
		public override string LocalizationCategory => "Magics";

		public static Projectile CreateMagicCircle(Item item, Player player, Imbuable magicToUse)
		{ // add explosion spell spawning stuff later
			if (magicToUse is AOMagic)
			{
				SoundEngine.PlaySound(SoundID.Item84 with { Pitch=magicToUse.AOScrollSpeed.MultiToPercent().PitchPerfect() });
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
					circleprojectile.ArcaneOdyssey().imbue = magicToUse;
					return circleprojectile;
				}
				else if (item.ModItem is LeapScroll)
				{
					var proj = Projectile.NewProjectileDirect(player.GetSource_FromThis(), player.Bottom, Vector2.Zero, ModContent.ProjectileType<MagicCircle1>(), 0, 0, player.whoAmI);
					proj.rotation = (-Vector2.UnitY).ToRotation();
					proj.Center = player.Bottom;
					((MagicCircle1)proj.ModProjectile).MarkedForDeath = true;
					return proj;
				}
			}
			return null;
		}

		// Dust stuff below for copy/paste
		// Dust spawnedDust = Dust.NewDustDirect(new Vector2(projectile.position.X+(projectile.width*Main.rand.NextFloat()),projectile.position.Y+(projectile.height*Main.rand.NextFloat())), 1, 1, DustID.Water);
	}
}