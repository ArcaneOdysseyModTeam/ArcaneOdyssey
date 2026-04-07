using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace ArcaneOdyssey
{
	public class DownedBosses : ModSystem
	{
		private static bool _downedEvander;
		private static bool _downedDusk;
		private static bool _downedLaelus;
		private static bool _downedCrone;
		private static bool _downedDelamere;

		private static bool _downedElius;
		private static bool _downedAllanon;
		private static bool _downedArgos;
		private static bool _downedCalvus;

		public static bool DownedBrain { get; set; }

		public static bool DownedWorldEater { get; set; }

		public static bool DownedEnragedEmpress { get; set; }

		public static bool DownedCalvus { get => _downedCalvus; set
			{
				if (!value)
				{
					_downedCalvus = value;
				}
				else
				{
					NPC.SetEventFlagCleared(ref _downedCalvus, -1);
					if (Main.dedServ)
					{
						NetMessage.SendData(MessageID.WorldData);
					}
				}
			}
		}

		public static bool DownedArgos { get => _downedArgos; set
			{
				if (!value)
				{
					_downedArgos = value;
				}
				else
				{
					NPC.SetEventFlagCleared(ref _downedArgos, -1);
					if (Main.dedServ)
					{
						NetMessage.SendData(MessageID.WorldData);
					}
				}
			}
		}

		public static bool DownedAllanon { get => _downedAllanon; set
			{
				if (!value)
				{
					_downedAllanon = value;
				}
				else
				{
					NPC.SetEventFlagCleared(ref _downedAllanon, -1);
					if (Main.dedServ)
					{
						NetMessage.SendData(MessageID.WorldData);
					}
				}
			}
		}

		public static bool DownedElius { get => _downedElius; set
			{
				if (!value)
				{
					_downedElius = value;
				}
				else
				{
					NPC.SetEventFlagCleared(ref _downedElius, -1);
					if (Main.dedServ)
					{
						NetMessage.SendData(MessageID.WorldData);
					}
				}
			}
		}

		public static bool DownedDelamere { get => _downedDelamere; set
			{
				if (!value)
				{
					_downedDelamere = value;
				}
				else
				{
					NPC.SetEventFlagCleared(ref _downedDelamere, -1);
					if (Main.dedServ)
					{
						NetMessage.SendData(MessageID.WorldData);
					}
				}
			}
		}

		public static bool DownedCrone { get => _downedCrone; set
			{
				if (!value)
				{
					_downedCrone = value;
				}
				else
				{
					NPC.SetEventFlagCleared(ref _downedCrone, -1);
					if (Main.dedServ)
					{
						NetMessage.SendData(MessageID.WorldData);
					}
				}
			}
		}

		public static bool DownedLaelus { get => _downedLaelus; set
			{
				if (!value)
				{
					_downedLaelus = value;
				}
				else
				{
					NPC.SetEventFlagCleared(ref _downedLaelus, -1);
					if (Main.dedServ)
					{
						NetMessage.SendData(MessageID.WorldData);
					}
				}
			}
		}

		public static bool DownedDusk { get => _downedDusk; set
			{
				if (!value)
				{
					_downedDusk = value;
				}
				else
				{
					NPC.SetEventFlagCleared(ref _downedDusk, -1);
					if (Main.dedServ)
					{
						NetMessage.SendData(MessageID.WorldData);
					}
				}
			}
		}

		public static bool DownedEvander
		{
			get => _downedEvander; set
			{
				if (!value)
				{
					_downedEvander = value;
				}
				else
				{
					NPC.SetEventFlagCleared(ref _downedEvander, -1);
					if (Main.dedServ)
					{
						NetMessage.SendData(MessageID.WorldData);
					}
				}
			}
		}

		public static void ResetDefaults()
		{
			DownedEvander = false;
			DownedEnragedEmpress = false;
			DownedDusk = false;
			DownedLaelus = false;
			DownedCrone = false;
			DownedDelamere = false;
			DownedElius = false;
			DownedAllanon = false;
			DownedArgos = false;
			DownedCalvus = false;
			DownedBrain = false;
			DownedWorldEater = false;
		}

		public override void OnWorldLoad() => ResetDefaults();

		public override void OnWorldUnload() => ResetDefaults();

		public override void SaveWorldData(TagCompound tag)
		{
			List<string> downed = [];
			if (DownedEvander)
				downed.Add(DownedFlagID.Evander);
			if (DownedEnragedEmpress)
				downed.Add(DownedFlagID.DaytimeEmpress);
			if (DownedDelamere)
				downed.Add(DownedFlagID.Delamere);
			if (DownedDusk)
				downed.Add(DownedFlagID.Dusk);
			if (DownedCrone)
				downed.Add(DownedFlagID.TheCrone);
			if (DownedLaelus)
				downed.Add(DownedFlagID.Laelus);
			if (DownedElius)
				downed.Add(DownedFlagID.LordElius);
			if (DownedAllanon)
				downed.Add(DownedFlagID.Allanon);
			if (DownedArgos)
				downed.Add(DownedFlagID.Argos);
			if (DownedCalvus)
				downed.Add(DownedFlagID.Calvus);
			if (DownedWorldEater)
				downed.Add(DownedFlagID.EaterofWorlds);
			if (DownedBrain)
				downed.Add(DownedFlagID.CrimsonBrain);
			tag["downed"] = downed;
		}

		public override void LoadWorldData(TagCompound tag)
		{
			var downed = tag.GetList<string>("downed");
			DownedEvander = downed.Contains(DownedFlagID.Evander);
			DownedDusk = downed.Contains(DownedFlagID.Dusk);
			DownedCrone = downed.Contains(DownedFlagID.TheCrone);
			DownedLaelus = downed.Contains(DownedFlagID.Laelus);
			DownedDelamere = downed.Contains(DownedFlagID.Delamere);
			DownedEnragedEmpress = downed.Contains(DownedFlagID.DaytimeEmpress);
			DownedElius = downed.Contains(DownedFlagID.LordElius);
			DownedAllanon = downed.Contains(DownedFlagID.Allanon);
			DownedArgos = downed.Contains(DownedFlagID.Argos);
			DownedCalvus = downed.Contains(DownedFlagID.Calvus);
			DownedWorldEater = downed.Contains(DownedFlagID.EaterofWorlds);
			DownedBrain = downed.Contains(DownedFlagID.CrimsonBrain);
		}

		public override void NetSend(BinaryWriter writer)
		{
			writer.Write(DownedEvander);
			writer.Write(DownedDusk);
			writer.Write(DownedCrone); 
			writer.Write(DownedLaelus); 
			writer.Write(DownedDelamere);
			writer.Write(DownedEnragedEmpress);
			writer.Write(DownedElius);
			writer.Write(DownedAllanon);
			writer.Write(DownedArgos);
			writer.Write(DownedCalvus);
			writer.Write(DownedWorldEater);
			writer.Write(DownedBrain);
		}

		public override void NetReceive(BinaryReader reader)
		{
			DownedEvander = reader.ReadBoolean();
			DownedDusk = reader.ReadBoolean();
			DownedCrone = reader.ReadBoolean();
			DownedLaelus = reader.ReadBoolean();
			DownedDelamere = reader.ReadBoolean();
			DownedEnragedEmpress = reader.ReadBoolean();
			DownedElius = reader.ReadBoolean();
			DownedAllanon = reader.ReadBoolean();
			DownedArgos = reader.ReadBoolean();
			DownedCalvus = reader.ReadBoolean();
			DownedWorldEater = reader.ReadBoolean();
			DownedBrain = reader.ReadBoolean();
		}

		public class DownedFlagID
		{
			public const string Evander = "Evander";
			public const string DaytimeEmpress = "EnragedEoL";
			public const string Delamere = "Delamere";
			public const string Dusk = "Dusk";
			public const string TheCrone = "Crone";
			public const string Laelus = "Laelus";
			public const string LordElius = "Elius";
			public const string Allanon = "Allanon";
			public const string Argos = "Argos";
			public const string Calvus = "Calvus";
			public const string EaterofWorlds = "EoW";
			public const string CrimsonBrain = "Brain";
		}
	}
}
