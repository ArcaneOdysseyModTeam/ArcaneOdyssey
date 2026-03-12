using ArcaneOdyssey.Buffs.Base;
using ArcaneOdyssey.Projectiles.Relics.Minions;
using Terraria;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Buffs.Minions
{
	public class SpiritMinionBuff : AOBaseBuff
	{
		public override string Texture => AOUtils.GetTexture<ElementalBuff>();
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
			Main.pvpBuff[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			if (player.ownedProjectileCounts[ModContent.ProjectileType<SpiritMinion>()] > 0)
			{
				player.buffTime[buffIndex] = 3600;
			}
			else
			{
				player.DelBuff(buffIndex);
				buffIndex--;
			}
		}
	}
}
