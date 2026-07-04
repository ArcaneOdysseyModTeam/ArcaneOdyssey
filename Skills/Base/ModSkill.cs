using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Imbues.Relics;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Skills.Base
{
	public abstract class ModSkill : ModType, ILocalizedModType
	{
		internal static int count = 0;

		public int Type { get; private set; }

		public abstract SkillType SkillSlot { get; }

		public abstract int Scroll { get; }

		public virtual string LocalizationCategory => $"Skills.{SkillSlot}";

		public virtual bool PreActivate(Player player, Imbuable imbue) => true;
		public abstract void Activate(Player player, Imbuable imbue);

		protected sealed override void Register()
		{
			Type = ++count;
			ModTypeLookup<ModSkill>.Register(this);
			if (this is AttackSkill attack)
			{
				ModTypeLookup<AttackSkill>.Register(attack);
			}
			if (this is DashSkill dash)
			{
				ModTypeLookup<DashSkill>.Register(dash);
			}
			if (this is PassiveSkill passive)
			{
				ModTypeLookup<PassiveSkill>.Register(passive);
			}
		}

		public virtual LocalizedText Description => this.GetLocalization("Description");
		public virtual LocalizedText DisplayName => this.GetLocalization("DisplayName", () => PrettyPrintName().Replace("Skill").Trim());
		public virtual LocalizedText Popup => this.GetLocalization("Popup", () => PrettyPrintName().Replace("Skill").Trim());

		public sealed override void SetupContent()
		{
			Sets.All[Type] = this;

			_ = DisplayName; // forces these to generate if they don't exist
			_ = Description;
			_ = Popup;

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

		public virtual bool MetConditions() => true;

		public void ActivateAbility(Player player, Imbuable imbue)
		{
			if (TryGetAbility(imbue, out var ability))
			{
				if (ArcaneOdysseyClientConfig.Instance.AbilityText && player is not null && player.active && !player.DeadOrGhost && Main.myPlayer == player.whoAmI)
				{
					CombatText.NewText(player.Hitbox, ability.Colour, ArcaneOdysseyMod.Instance.CustomLocalization("ImbueStuff.Exclaim", ability.Name).Value.Trim(), true);
				}
			}
		}

		public bool TryGetAbility(Imbuable Imbue, out WeaponAbility ability)
		{
			ability = default;
			if (Imbue is not null)
			{
				var SecondImbue = Imbue.Imbue;
				var ab = new WeaponAbility(Popup.Value, null, Imbue.Colour);
				if (Imbue is not (FightingStyle or SpiritEnergy))
				{
					ab.Name = ArcaneOdysseyMod.Instance.CustomLocalization("ImbueStuff.Space", Imbue.PrettySpellPrefix, ab.Name).Value.Trim();
				}
				else if (SecondImbue is not null)
				{
					ab.Colour = SecondImbue.Colour;
				}
				if (SecondImbue is not null)
				{
					ab.Name = ArcaneOdysseyMod.Instance.CustomLocalization("ImbueStuff.Space", SecondImbue.PrettyAttackPrefix, ab.Name).Value.Trim();
				}
				ability = ab;
				return true;
			}
			return false;
		}

		public static string GetName(int id) => Sets.All[id].Name;

		[ReinitializeDuringResizeArrays]
		public static class Sets
		{
			public static ModSkill[] All = new ModSkill[count + 1];

			public static SetFactory Factory = new(count + 1, nameof(ModSkill), GetName);

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
