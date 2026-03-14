using System.Collections.Generic;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace ArcaneOdyssey
{
	public class DownedBosses : ModSystem
	{
		public static bool downedEvander;
		public static bool downedDusk;
		public static bool downedLaelus;
		public static bool downedCrone;
		public static bool downedDelamere;

		public static bool downedElius;
		public static bool downedAllanon;
		public static bool downedArgos;
		public static bool downedCalvus;

		public static bool downedEnragedEmpress;
		public static bool downedWorldEater;
		public static bool downedBrain;

		public static void ResetDefaults()
		{
			downedEvander = false;
			downedEnragedEmpress = false;
			downedDusk = false;
			downedLaelus = false;
			downedCrone = false;
			downedDelamere = false;
			downedElius = false;
			downedAllanon = false;
			downedArgos = false;
			downedCalvus = false;
			downedBrain = false;
			downedWorldEater = false;
		}

		public override void OnWorldLoad() => ResetDefaults();

		public override void OnWorldUnload() => ResetDefaults();

		public override void SaveWorldData(TagCompound tag)
		{
			List<string> downed = [];
			if (downedEvander)
				downed.Add("Evander");
			if (downedEnragedEmpress)
				downed.Add("EnragedEoL");
			if (downedDelamere)
				downed.Add("Delamere");
			if (downedDusk)
				downed.Add("Dusk");
			if (downedCrone)
				downed.Add("Crone");
			if (downedLaelus)
				downed.Add("Laelus");
			if (downedElius)
				downed.Add("Elius");
			if (downedAllanon)
				downed.Add("Allanon");
			if (downedArgos)
				downed.Add("Argos");
			if (downedCalvus)
				downed.Add("Calvus");
			if (downedWorldEater)
				downed.Add("EoW");
			if (downedBrain)
				downed.Add("Brain");

			tag["downed"] = downed;
		}

		public override void LoadWorldData(TagCompound tag)
		{
			var downed = tag.GetList<string>("downed");
			downedEvander = downed.Contains("Evander");
			downedDusk = downed.Contains("Dusk");
			downedCrone = downed.Contains("Crone");
			downedLaelus = downed.Contains("Laelus");
			downedDelamere = downed.Contains("Delamere");
			downedEnragedEmpress = downed.Contains("EnragedEoL");
			downedElius = downed.Contains("Elius");
			downedAllanon = downed.Contains("Allanon");
			downedArgos = downed.Contains("Argos");
			downedCalvus = downed.Contains("Calvus");
			downedWorldEater = downed.Contains("EoW");
			downedBrain = downed.Contains("Brain");
		}
	}
}
