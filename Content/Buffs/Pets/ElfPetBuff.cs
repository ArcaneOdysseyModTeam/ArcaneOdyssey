using Microsoft.Xna.Framework;
using ArcaneOdyssey.Content.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;
using ArcaneOdyssey.Content.Items.Equipment.Pets;

namespace ArcaneOdyssey.Content.Buffs.Pets
{
	public class ElfPetBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			Main.buffNoTimeDisplay[Type] = true;
			Main.vanityPet[Type] = true;
		}
		public override void Update(Player player, ref int buffIndex)
		{
			player.buffTime[buffIndex] = 18000;
			player.GetModPlayer<ThyPlayer>().elfPet = true;
			bool projectileNotSpawned = player.ownedProjectileCounts[ModContent.ProjectileType<ElfPetProjectile>()] <= 0;
			if (projectileNotSpawned && player.whoAmI == Main.myPlayer)
			{
				Projectile.NewProjectile(player.GetSource_Buff(buffIndex), player.Center, Vector2.Zero, ModContent.ProjectileType<ElfPetProjectile>(), 0, 0f, player.whoAmI);
			}
		}
	}
}