using ArcaneOdyssey.Buffs.Base;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace ArcaneOdyssey.Buffs.DOT
{
	public class PhoenixHealing : MagicMark
	{
		public const int HealDistance = 700;

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ExternalModSupport.RegisterDoT(Type);
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.onFire2 = true;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			if (npc.wet && !npc.lavaWet)
			{
				npc.DelBuff(buffIndex);
				buffIndex--;
				return;
			}
			bool noPlayerFound = true;
			npc.ArcaneOdyssey().phoenixDrain = true;
			foreach (var player in Main.ActivePlayers)
			{
				if (npc.Hitbox.Distance(player.Center) <= HealDistance && (!AOUtils.BossAlive || npc.boss))
				{
					noPlayerFound = false;
					if (!ArcaneOdysseyMod.Sets.phoenixAffected[npc.type] || !npc.boss)
						player.ArcaneOdyssey().pheonixHealing += npc.boss ? 2 : 1;
					npc.ArcaneOdyssey().lesserPhoenixDrain++;
					if (!Main.dedServ)
						HealEffect(player, npc);
				}
			}
			if (noPlayerFound)
			{
				if (!Main.dedServ && Main.GameUpdateCount % 4 == 0)
				{
					Dust.NewDust(npc.position, npc.width, npc.height, DustID.BlueTorch, Scale: 1.4f);
					Dust.NewDust(npc.position, npc.width, npc.height, DustID.YellowTorch, Scale: 1.4f);
				}
			}
			else
			{
				ArcaneOdysseyMod.Sets.phoenixAffected[npc.type] = true;
			}
		}

		public static void HealEffect(Player player, NPC npc)
		{
			if (Main.GameUpdateCount % 2 == 0 && player.ArcaneOdyssey().pheonixHealing < 3)
			{
				Dust.NewDust(player.position, player.width, player.height, DustID.BlueTorch, Scale: 1.4f);
				Dust.NewDust(player.position, player.width, player.height, DustID.YellowTorch, Scale: 1.4f);
			}
			for (float i = 0; i < player.MountedCenter.Distance(npc.Center).Round(); i++)
			{
				if (!Main.rand.NextBool(10))
					continue;
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
					progress = 1f - MathHelper.Clamp((i - player.MountedCenter.Distance(npc.Center) / 2f) / (player.MountedCenter.Distance(npc.Center) / 2f), 0, 1);
				}

				offsetpoint += (npc.SafeDirectionTo(player.MountedCenter).ToRotation() - MathHelper.PiOver2).ToRotationVector2() * player.MountedCenter.Distance(npc.Center) * .1f * progress.FlipFloat() * Main.rand.NextFloat(-1f, 1f);

				if (!progressed)
				{
					dustpos = Vector2.Lerp(player.MountedCenter, offsetpoint, progress);
				}
				else
				{
					dustpos = Vector2.Lerp(npc.Center, offsetpoint, progress);
				}

				var dust = Dust.NewDustPerfect(dustpos, Main.rand.Next(new int[] { DustID.BlueTorch, DustID.YellowTorch }));
				dust.noGravity = true;
			}
		}
	}
}
