using ArcaneOdyssey.Content.Buffs.MagicMarks;
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
	public abstract class AOMagic : ModItem
	{
		/// <summary>
		/// magic works underwater
		/// </summary>
		public virtual bool CanBeWet => true;
		public virtual float AOImbueSpeed => .9f;
		public virtual float AOImbueSize => .9f;
		public virtual float AOImbueDamage => .9f;
		public virtual float AOMagicSpeed => AOImbueSpeed;
		public virtual float AOMagicSize => AOImbueSize;
		public virtual float AOMagicDamage => AOImbueDamage;
		public virtual AOMagicTier MagicTier => AOMagicTier.Normal;
		public virtual AODebuffRequirement MagicDebuff => null;

		/// <summary>
		/// used for having freezing and frozen on a single magic ect
		/// </summary>
		public virtual AODebuffRequirement MagicDebuff2 => null;
		public virtual MagicEffects Effects => null;
		public virtual Color MagicColour => Color.Transparent;
		public virtual CombinedDebuff[] CombinedDebuffs => null;
		public virtual SoundStyle? MagicSound => null;

		public virtual Dictionary<Type, int> Spells => [];

		public bool FirstFrame = true;
		public override void SetStaticDefaults()
		{
			ItemID.Sets.CanGetPrefixes[Type] = false;
			ItemID.Sets.ShimmerTransformToItem[Type] = Type;
			ItemID.Sets.ItemNoGravity[Item.type] = true;
		}

		public override void SetDefaults()
		{
			Item.useStyle = ItemUseStyleID.DrinkOld;
			Item.useTime = 60;
			Item.useAnimation = 60;
			Item.noUseGraphic = true;
			if (this is GlassMagic)
			{
				Item.alpha = (int)Math.Round(255 * .2f); // glass gets 20% less visible
			}
		}

		public override bool CanUseItem(Player player)
		{
			FirstFrame = true;
			return true;
		}

		public override void AddRecipes()
		{
			if (MagicTier == AOMagicTier.Normal)
			{
				CreateRecipe().AddIngredient<PoseidonChoice>().Register();
				Recipe.Create(ModContent.ItemType<PoseidonChoice>()).AddIngredient(Type).AddIngredient<Acrimony>().Register(); // replace with something better later
			}
		}

		public virtual void MagicRecipe() {}

		public override bool? UseItem(Player player)
		{
			if (FirstFrame && player.Imbue() != this)
			{
				CreateMagicCircle(Item, player, this);
			}
			if (this != player.Imbue() && FirstFrame)
			{
				FirstFrame = false;
				player.ArcaneOdyssey().imbue = this;
				LocalizedText chatmessage = Mod.CustomLocalization("ImbueStuff.ImbueChatMessage", [Item.Name]);
				if (Main.netMode == NetmodeID.SinglePlayer)
				{
					Main.NewText(chatmessage.Value, 13, 132, 168);
				}
				else if (Main.dedServ)
				{
					ChatHelper.SendChatMessageToClient(chatmessage.ToNetworkText(), new Color(13, 132, 168), Main.player.IndexOf(player));
				}
			}
			else if (FirstFrame)
			{
				FirstFrame = false;
				player.ArcaneOdyssey().imbue = null;
				LocalizedText chatmessage = Mod.CustomLocalization("ImbueStuff.UnimbueText");
				if (Main.netMode == NetmodeID.SinglePlayer)
				{
					Main.NewText(chatmessage.Value, 13, 132, 168);
				}
				else if (Main.dedServ)
				{
					ChatHelper.SendChatMessageToClient(chatmessage.ToNetworkText(), new Color(13, 132, 168), Main.player.IndexOf(player));
				}
			}
			return null;
		}

		public virtual bool PreEffects(Projectile projectile)
		{
			if (ImbueClassCheck(projectile))
			{
				if (projectile.ModProjectile is null || ArcaneOdysseyConfig.Instance.AffectsOtherMods)
				{
					return !Main.dedServ && ImbueClassCheck(projectile) && projectile.ModProjectile is not (MagicCircle or ExplosionTracker or MagicCircle2);
				}
				else if (projectile.ModProjectile is AOPlayerProjectile)
				{
					return !Main.dedServ && ImbueClassCheck(projectile) && projectile.ModProjectile is not (MagicCircle or ExplosionTracker or MagicCircle2);
				}
			}
			return false;
		}

		public virtual void SpawningEffects(Projectile projectile) { }
		public virtual void LingeringEffects(Projectile projectile) { }
		public virtual void KillEffects(Projectile projectile) { }
		public virtual void ExplosionEffects(Projectile projectile) { }

		public static Projectile CreateMagicCircle(Projectile projectile)
		{
			if (projectile.ModProjectile is BlastSpell)
			{
				Projectile circleprojectile = Main.projectile[Projectile.NewProjectile(projectile.GetSource_FromThis(), Main.player[projectile.owner].position.X + (Main.player[projectile.owner].width / 2f), Main.player[projectile.owner].position.Y + (Main.player[projectile.owner].height / 2f), 0f, 0f, ModContent.ProjectileType<MagicCircle>(), 0, 0f, projectile.owner)];
				circleprojectile.rotation = projectile.velocity.ToRotation();
				Vector2 circleVec = Vector2.Normalize(projectile.velocity) * 30f;
				circleprojectile.position += circleVec;
				circleprojectile.scale = projectile.scale;
				return circleprojectile;
			}
			else
				return null;
		}

		public static Projectile CreateMagicCircle(Item item, Player player, AOMagic magicToUse = null)
		{ // add explosion spell spawning stuff later
			if (item.ModItem is AOMagic)
			{
				return Main.projectile[Projectile.NewProjectile(player.GetSource_FromThis(), player.position.X + (player.width / 2f), player.position.Y + (player.height / 2f), 0f, 0f, ModContent.ProjectileType<MagicCircle2>(), 0, 0f, Main.player.IndexOf(player), ai1: 1, ai2: magicToUse is not null ? magicToUse.Type : 0)];
			}
			else if (item.ModItem is ExplosionScroll)
			{
				return Main.projectile[Projectile.NewProjectile(player.GetSource_FromThis(), Main.MouseWorld.X, Main.MouseWorld.Y, 0f, 0f, ModContent.ProjectileType<MagicCircle2>(), 0, 0f, Main.player.IndexOf(player), ai1: player.altFunctionUse, ai2: magicToUse is not null ? magicToUse.Type : 0)];
			}
			else
				return null;
		}

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            tooltips.Add(new TooltipLine(Mod, "MagicTier", Mod.CustomLocalization($"MagicTierLines.{MagicTier}").Value));
        }

		// Dust stuff below for copy/paste
		// hello it is me the code insect i eat your code
		// Dust spawnedDust = Main.dust[Dust.EATEN AHHAHAH(new Vector2(projectile.position.X+(projectile.width*(float)rand.NextDouble()),projectile.position.Y+(projectile.height*(float)rand.NextDouble())),1,1,DustID.Water,0f,0f,0,default,1f)];
	}
}