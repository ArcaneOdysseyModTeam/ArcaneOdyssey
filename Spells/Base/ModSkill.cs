using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Items.Base;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Spells.Base
{
	public abstract class ModSkill : ModType
	{
		private static int count = 0;
		public int Type { get; private set; }

		public abstract SkillType SkillSlot { get; }

		public virtual int Scroll => 0;

		public virtual void Activate(Player player, Imbuable imbue)
		{

		}

		protected sealed override void Register()
		{
			Type = ++count;

			Sets.All[Type] = this;

			ModTypeLookup<ModSkill>.Register(this);
		}

		public sealed override void SetupContent()
		{
			switch (SkillSlot)
			{
				case SkillType.Attack:
					Sets.Attacks[Type] = true;
					break;
				case SkillType.Passive:
					Sets.Passives[Type] = true;
					break;
				case SkillType.Mobility:
					Sets.Mobilities[Type] = true;
					break;
				case SkillType.Dash:
					Sets.Dashes[Type] = true;
					break;
			}
			SetStaticDefaults();
		}

		public static string GetName(int id) => Sets.All[id].Name;

		[ReinitializeDuringResizeArrays]
		public static class Sets
		{
			public static ModSkill[] All = new ModSkill[count];

			public static SetFactory Factory = new(count, nameof(ModSkill), GetName);

			public static bool[] Attacks = Factory.CreateBoolSet();

			public static bool[] Passives = Factory.CreateBoolSet();

			public static bool[] Mobilities = Factory.CreateBoolSet();

			public static bool[] Dashes = Factory.CreateBoolSet();
		}

		public enum SkillType : byte
		{
			Attack, // attacks
			Passive, // things like aura
			Mobility, // wings or leaps
			Dash, // dashes
			Other
		}
	}
}
