using ArcaneOdyssey.Buffs.Base;
using ArcaneOdyssey.Projectiles.Pets;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Buffs.Pets
{
	public class IrisBuff : BaseBuff
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			Main.buffNoTimeDisplay[Type] = true;
			Main.lightPet[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.buffTime[buffIndex] = 18000;
			bool projectileNotSpawned = player.ownedProjectileCounts[ModContent.ProjectileType<Iris>()] <= 0;
			if (projectileNotSpawned && player.whoAmI == Main.myPlayer)
			{
				Projectile.NewProjectile(player.GetSource_Buff(buffIndex), player.Center, Vector2.Zero, ModContent.ProjectileType<Iris>(), 0, 0f, player.whoAmI);
			}
		}
	}
}
