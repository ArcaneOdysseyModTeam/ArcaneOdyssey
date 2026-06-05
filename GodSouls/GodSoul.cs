using Terraria.Localization;
using Terraria.ModLoader;

namespace ArcaneOdyssey.GodSouls
{
	public abstract class GodSoul : ModType, ILocalizedModType
	{
		private static byte amount = 0;

		public static GodSoul[] AllSouls = new GodSoul[byte.MaxValue];

		/// <summary>
		/// The ID of this god soul, max of 255
		/// <para/> 0 is reserved for <seealso cref="NoneSoul"/>
		/// </summary>
		public byte Type { get; private set; }

		/// <inheritdoc/>
		public virtual string LocalizationCategory => "GodSouls";

		public static NoneSoul None => ModContent.GetInstance<NoneSoul>();

		protected sealed override void Register()
		{
			ModTypeLookup<GodSoul>.Register(this);
			if (this is NoneSoul)
			{
				Type = 0;
			}
			else
			{
				Type = ++amount;
			}
			AllSouls[Type] = this;
		}

		public static ref GodSoul GetSoul(byte type) => ref AllSouls[type];

		public sealed override void SetupContent()
		{
			_ = DisplayName;
			SetStaticDefaults();
		}

		public virtual LocalizedText DisplayName => Mod.CoolCustomLocalization(LocalizationCategory + "." + Name + ".DisplayName", PrettyPrintName);
	}

	public sealed class NoneSoul : GodSoul
	{

	}
}
