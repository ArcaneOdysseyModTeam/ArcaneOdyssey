using ArcaneOdyssey.Buffs.Base;
using Microsoft.Xna.Framework;
using System;
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

			var length = player.MountedCenter.Distance(npc.Center);

			for (float i = 0; i < length; i += 5f)
			{

				var posy = (length / 10f) * MathF.Sin((i / length) * MathF.PI) * MathF.Cos(((2 * (Main.GameUpdateCount % 400f)) / 400f) * MathF.PI);
				Vector2 dustpos = player.MountedCenter + new Vector2(i, posy + Main.rand.NextFloat(length/-50f, length/50f)).RotatedBy(player.MountedCenter.AngleTo(npc.Center));

				var dust = Dust.NewDustPerfect(dustpos, Main.rand.Next(new int[2] { DustID.BlueTorch, DustID.YellowTorch }));
				dust.noGravity = true;
			}
		}
	}
}
