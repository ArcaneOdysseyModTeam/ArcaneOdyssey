using ArcaneOdyssey.Content.Buffs.Base;
using ArcaneOdyssey.Content.Items.Equipment.Pets;
using ArcaneOdyssey.Content.Projectiles.Pets;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Buffs.Pets
{
	public class ElfPetBuff : AOBaseBuff
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