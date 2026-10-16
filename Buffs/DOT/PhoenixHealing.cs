using ArcaneOdyssey.Buffs.Base;
using ArcaneOdyssey.Imbues.Magic.Lost;
using System;
using System.Linq;

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

		private bool vfxSet = false;
		public Color[] dustColour = [Color.Yellow, Color.LightBlue];

		public override bool ReApply(NPC npc, int time, int buffIndex)
		{
			vfxSet = false;
			return base.ReApply(npc, time, buffIndex);
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			if (!Main.dedServ)
			{
				if (!vfxSet)
				{
					if (Main.netMode == NetmodeID.SinglePlayer)
					{
						if (Main.LocalPlayer.HasTypeInInventory<PhoenixMagic>(out var imbue))
						{
							dustColour = [imbue.VariantColour, imbue.VariantColour2.GetValueOrDefault(imbue.VariantColour)];
						}
					}
					else
					{
						var players = Main.player.Where(e => e.active).OrderBy(e => e.Distance(npc.Center));
						foreach (var player in players)
						{
							if (player is not null && player.HasTypeInInventory<PhoenixMagic>(out var imbue))
							{
								dustColour = [imbue.VariantColour, imbue.VariantColour2.GetValueOrDefault(imbue.VariantColour)];
								break;
							}
						}
					}
					vfxSet = true;
				}
			}
			if (npc.wet && !npc.lavaWet)
			{
				if (AOUtils.ServerOrSingleplayer)
				{
					npc.DelBuff(buffIndex);
					buffIndex--;
				}
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
					Dust.NewDust(npc.position, npc.width, npc.height, DustID.WhiteTorch, newColor: dustColour[0], Scale: 1.4f);
					Dust.NewDust(npc.position, npc.width, npc.height, DustID.WhiteTorch, newColor: dustColour[1], Scale: 1.4f);
				}
			}
			else
			{
				ArcaneOdysseyMod.Sets.phoenixAffected[npc.type] = true;
			}
		}

		public void HealEffect(Player player, NPC npc)
		{
			if (Main.GameUpdateCount % 2 == 0 && player.ArcaneOdyssey().pheonixHealing < 3)
			{
				Dust.NewDust(player.position, player.width, player.height, DustID.WhiteTorch, newColor: dustColour[0], Scale: 1.4f);
				Dust.NewDust(player.position, player.width, player.height, DustID.WhiteTorch, newColor: dustColour[1], Scale: 1.4f);
			}

			var length = player.MountedCenter.Distance(npc.Center);

			for (float i = 0; i < length; i += 5f)
			{

				var posy = (length / 10f) * MathF.Sin((i / length) * MathF.PI) * MathF.Cos(((2 * (Main.GameUpdateCount % 400f)) / 400f) * MathF.PI);
				Vector2 dustpos = player.MountedCenter + new Vector2(i, posy + Main.rand.NextFloat(length / -50f, length / 50f)).RotatedBy(player.MountedCenter.AngleTo(npc.Center));

				var dust = Dust.NewDustPerfect(dustpos, DustID.WhiteTorch, newColor: Main.rand.Next(dustColour));
				dust.noGravity = true;
			}
		}
	}
}
