using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Skills.Base;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Items.Scrolls.Equipment.Common
{
	public class HoverScroll : CommonScroll
	{
		public override bool CanHaveMagic => true;

		public override ModSkill Skill => ModContent.GetInstance<HoverSkill>();
	}

	public class HoverSkill : ModSkill
	{
		public override SkillType SkillSlot => SkillType.Mobility;

		public override void Activate(Player player, Imbuable Imbue)
		{
			player.carpet = true;
			player.GetModPlayer<HoverPlayer>().hasHoverEquipped = true;
			if (player.carpetTime > 0 && player.controlJump)
			{
				player.moveSpeed += Imbue.ScrollSpeed.MultiToPercent();
				Imbue.LingeringEffects(player.Hitbox);
			}
			else
				player.carpetTime = (player.carpetTime * Imbue.ScrollDamage).Round();
		}

		public override int Scroll => ModContent.ItemType<HoverScroll>();
	}

	public class HoverPlayer : ModPlayer
	{
		public bool hasHoverEquipped = false;

		public override void PostUpdateMiscEffects()
		{
			if ((!Main.dedServ) && Main.myPlayer == Player.whoAmI)
			{
				Asset<Texture2D> carpetNoneLol = Mod.Assets.Request<Texture2D>("Assets/BlankCarpet");
				Asset<Texture2D> carpetOriginal = IHATECARPETS.carpet;
				TextureAssets.FlyingCarpet = hasHoverEquipped ? carpetNoneLol : carpetOriginal;
			}
		}

		public override void ResetEffects()
		{
			hasHoverEquipped = false;
		}
	}

	public class IHATECARPETS : ModSystem
	{
		public static Asset<Texture2D> carpet;
		public override void Load()
		{
			carpet = TextureAssets.FlyingCarpet;
		}
	}
}
