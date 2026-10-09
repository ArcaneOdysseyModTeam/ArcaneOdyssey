using System.Collections.Generic;

namespace ArcaneOdyssey.MagicVariants.Base
{
	public abstract class MagicVariant : ModType, ILocalizedModType
	{
		public virtual string LocalizationCategory => "MagicVariants";

		public virtual LocalizedText DisplayName => this.GetLocalization("DisplayName", PrettyPrintName);

		private static int count = 0;

		internal static List<MagicVariant> all = [];

		private int _type = -1;

		public static MagicVariant GetFromID(int id) => id >= 0 ? all[id] : null;
		public static int GetIDFromName(string name) => all.FindIndex(e => e.FullName == name);
		public static int GetID<T>() where T : MagicVariant => ModContent.GetInstance<T>().Type;

		public int Type
		{
			get 
			{ 
				if (_type == -1)
				{
					var id = GetIDFromName(FullName);
					_type = id;
					return id;
				}
				return _type;
			}

			private set
			{
				_type = value;
			}
		}

		public sealed override void SetupContent()
		{
			_ = DisplayName;
			SetStaticDefaults();
		}
		protected sealed override void Register()
		{
			Type = count++;
			all.Add(this);
			ModTypeLookup<MagicVariant>.Register(this);
		}
		public abstract int Imbue { get; }
		public abstract Color Colour { get; }
		public virtual Color? Colour2 => null;
	}
}
