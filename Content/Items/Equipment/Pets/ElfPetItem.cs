using Terraria.ModLoader;
using Terraria;
using ArcaneOdyssey.Content.Projectiles.Pets;
using ArcaneOdyssey.Content.Buffs.Pets;
using Microsoft.Xna.Framework;
using ArcaneOdyssey.Content.Items.Base;
using Terraria.ID;
using Terraria.Audio;

namespace ArcaneOdyssey.Content.Items.Equipment.Pets
{
	public class ElfPetItem : AOBaseItem, ILocalizedModType
	{
		public override string LocalizationCategory => base.LocalizationCategory + ".Pets";
		public override AORarities AORarity => AORarities.Special;

		public override void SetDefaults()
		{
			Item.shoot = ModContent.ProjectileType<ElfPetProjectile>();
			Item.useStyle = ItemUseStyleID.Swing;
			Item.buffType = ModContent.BuffType<ElfPetBuff>();
			Item.noMelee = true;
			Item.useAnimation = 20;
			Item.useTime = 20;
			Item.UseSound = SoundID.Meowmere;
		}

		public override void UseStyle(Player player, Rectangle heldItemFrame)
		{
			if (player.whoAmI == Main.myPlayer && player.ItemTimeIsZero)
			{
				player.AddBuff(Item.buffType, 3600, true);
			}
		}
	}

	public class ThyPlayer : ModPlayer
	{
		public bool elfPet;
		public override void ResetEffects()
		{
			elfPet = false;
			if (!Player.dead)
				madeDeathSound = false;
		}

		public bool madeDeathSound;

		public static readonly SoundStyle ElfDeathSound = new(ArcaneOdysseyMod.InternalName + "/Sounds/ElfPetDeath");

		public override void UpdateDead()
		{
			if (!madeDeathSound && elfPet)
			{
				madeDeathSound = true;
				SoundEngine.PlaySound(ElfDeathSound);
			}
		}
	}
}