using ArcaneOdyssey.Content.Buffs.Base;
using Microsoft.Xna.Framework;
using System.Linq;
using Terraria;
using Terraria.ID;

namespace ArcaneOdyssey.Content.Buffs.Helpers
{
	public class PheonixHealing : AODebuff
	{
		public const float HealDistance = 700f;
		public override void Update(NPC npc, ref int buffIndex)
		{
			foreach (var player in Main.ActivePlayers)
			{
				if (npc.Hitbox.Distance(player.Center) <= HealDistance && (!AOUtils.BossAlive() || npc.boss))
				{
					player.ArcaneOdyssey().pheonixHealing += npc.boss ? 2 : 1;
					if (!Main.dedServ)
						HealEffect(player, npc);
				}
			}
		}

		public static void HealEffect(Player player, NPC npc)
		{
			if (Main.GameUpdateCount % 3 == 0)
			{
				Dust.NewDust(player.position, player.width, player.height, DustID.BlueTorch, Scale: 1.5f);
				Dust.NewDust(player.position, player.width, player.height, DustID.YellowTorch, Scale: 1.5f);
			}
			for (float i = 0; i < player.MountedCenter.Distance(npc.Center).Round(); i++)
			{
				var progressed = i >= player.MountedCenter.Distance(npc.Center).Round() / 2f;
				float progress;
				Vector2 dustpos;

				var offsetpoint = Vector2.Lerp(player.MountedCenter, npc.Center, .5f);

				if (!progressed)
				{
					progress = MathHelper.Clamp(i / (player.MountedCenter.Distance(npc.Center) / 2f), 0, 1);
				}
				else
				{
					progress = 1f - MathHelper.Clamp((i - (player.MountedCenter.Distance(npc.Center) / 2f)) / (player.MountedCenter.Distance(npc.Center) / 2f), 0, 1);
				}

				offsetpoint.Y -= player.MountedCenter.Distance(npc.Center) / 5f * progress * Main.rand.NextFloat();

				if (!progressed)
				{
					dustpos = Vector2.Lerp(player.MountedCenter, offsetpoint, progress);
				}
				else
				{
					dustpos = Vector2.Lerp(npc.Center, offsetpoint, progress);
				}

				if (i % 5 == 0)
				{
					var dust = Dust.NewDustPerfect(dustpos, Main.rand.Next(new int[] { DustID.BlueTorch, DustID.YellowTorch }));
					dust.noGravity = true;
				}
			}
		}
	}
}
