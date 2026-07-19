using ArcaneOdyssey.Buffs.Base;
using ArcaneOdyssey.Projectiles.Enemies.Effects;
using ArcaneOdyssey.Projectiles.Magic.Effects;

namespace ArcaneOdyssey.Buffs.Helpers
{
	public class ProminenceDebuff : MagicMark
	{
		private byte counter = 0;
		public override void Update(NPC npc, ref int buffIndex)
		{
			if (++counter > 120)
			{
				counter = 0;
				if (AOUtils.ServerOrSingleplayer)
					Projectile.NewProjectile(npc.GetSource_Buff(buffIndex), npc.Center.X, npc.Center.Y, (Main.rand.NextFloat() - 0.5f) * 5f, (Main.rand.NextFloat() - 0.5f) * 5f, ModContent.ProjectileType<ProminenceProjectile>(), (int)MathHelper.Clamp(npc.lifeMax * 0.005f, 17f, 1000f), 0);
			}
			if (!Main.dedServ)
			{
				Dust.NewDust(npc.position, npc.width, npc.height, DustID.Torch);
			}
		}

		public override void Update(Player player, ref int buffIndex)
		{
			if (++counter > 120)
			{
				counter = 0;
				if (AOUtils.ServerOrSingleplayer)
				{
					Rectangle rect;
					Vector2 pos;
					if (Main.dedServ)
					{
						pos = player.Center - new Vector2(Main.maxScreenW / 2f, Main.maxScreenH / 2f);
						rect = new Rectangle(pos.X.Round(), pos.Y.Round(), Main.maxScreenW, Main.maxScreenH);
					}
					else
					{
						rect = AOUtils.ScreenRect;
					}
					pos = rect.RandomBorder();

					Projectile.NewProjectile(player.GetSource_Buff(buffIndex), pos + (pos.DirectionFrom(player.Center) * 10f), pos.DirectionTo(player.Center) * 5f, ModContent.ProjectileType<EvilSun>(), 30, 0);
				}
			}
			if (!Main.dedServ)
			{
				Dust.NewDust(player.position, player.width, player.height, DustID.Torch);
			}
		}
	}
}
