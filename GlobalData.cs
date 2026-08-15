using System.Collections.Generic;
using System.IO;

namespace ArcaneOdyssey
{
	public class GlobalData : ModSystem
	{
		private static List<string> globalDefeatCache = [];
		public static string GlobalDefeatFile => Path.Combine(Main.SavePath, "GlobalAOModDefeatList.txt");

		public override void OnModLoad()
		{
			if (!File.Exists(GlobalDefeatFile))
				File.WriteAllText(GlobalDefeatFile, string.Empty);
			else
				globalDefeatCache = [.. File.ReadAllLines(GlobalDefeatFile)];
		}

		public static bool IsDefeated(int npcID) => globalDefeatCache.Contains(npcID.ToString());

		public static bool IsDefeated(NPC npc)
		{
			if (npc.ModNPC is null)
			{
				return IsDefeated(npc.type);
			}
			else
			{
				return IsDefeated(npc.FullName);
			}
		}

		public static bool IsDefeated(string name) => globalDefeatCache.Contains(name);

		public static bool IsDefeated<T>() where T : ModNPC
		{
			var instance = ModContent.GetInstance<T>();
			if (instance is null)
				return false;

			return globalDefeatCache.Contains(instance.FullName);
		}

		private static void MarkDefeated(string key)
		{
			if (globalDefeatCache.Contains(key))
				return;

			File.AppendAllLines(GlobalDefeatFile, [key]);
			globalDefeatCache.Add(key);
		}

		public static void MarkDefeated(int npcID) => MarkDefeated(npcID.ToString());

		public static void MarkDefeated<T>() where T : ModNPC => MarkDefeated(ModContent.GetInstance<T>().FullName);

		public static void MarkDefeated(NPC npc)
		{
			if (npc.ModNPC is null)
				MarkDefeated(npc.type);
			else
				MarkDefeated(npc.ModNPC.FullName);
		}

		public static void MarkDefeated(ModNPC npc) => MarkDefeated(npc.NPC);

		public static void IsDefeated(ModNPC npc) => IsDefeated(npc.NPC);
	}
}
