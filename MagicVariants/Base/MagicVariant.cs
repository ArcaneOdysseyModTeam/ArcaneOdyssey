using ArcaneOdyssey.Imbues;
using ArcaneOdyssey.Imbues.Base;
using System;
using System.Collections.Generic;

namespace ArcaneOdyssey.MagicVariants.Base
{
	public abstract class MagicVariant : ModTexturedType, ILocalizedModType
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

			ImbueID.Sets.HasVariants[Imbue] = true;
			if (Icon is not null)
			{
				ImbueID.Sets.VariantsUseSprites[Imbue] = true;
			}

			SetStaticDefaults();
		}

		protected sealed override void Register()
		{
			Type = count++;
			all.Add(this);

			ModContent.RequestIfExists(Texture, out Icon);
			ModContent.RequestIfExists(AnnihilationTexture, out Annihilation);
			ModContent.RequestIfExists(RayTexture, out Ray);
			ModContent.RequestIfExists(RayEndTexture, out RayEnd);
			ModContent.RequestIfExists(RayStartTexture, out RayStart);
			ModContent.RequestIfExists(BlastTexture, out Blast); // load instantly for blasts specifically!

			ModTypeLookup<MagicVariant>.Register(this);
		}

		/// <summary>
		/// <seealso cref="AOUtils.ImbuableID{T}"/>
		/// </summary>
		public abstract int Imbue { get; }
		public abstract Color Colour { get; }
		public virtual Color? Colour2 => null;

		public Color AnimatedColour
		{
			get
			{
				if (Colour2.HasValue)
				{
					return Color.Lerp(Colour, Colour2.Value, MathF.Sin(AOUtils.UpdateCount) / 2f + .5f);
				}
				else if (ImbueID.Sets.DefaultAnimatedColours[Imbue].HasValue)
				{
					return Color.Lerp(Colour, ImbueID.Sets.DefaultAnimatedColours[Imbue].Value, MathF.Sin(AOUtils.UpdateCount) / 2f + .5f);
				}
				return Colour;
			}
		}

		public Asset<Texture2D> Icon = null;
		public Asset<Texture2D> Ray = null;
		public Asset<Texture2D> RayStart = null;
		public Asset<Texture2D> RayEnd = null;
		public Asset<Texture2D> Blast = null;
		public Asset<Texture2D> Annihilation = null;


		public override string Texture => base.Texture.Replace(Name, Name.Replace("Variant"));
		public string AnnihilationTexture => Texture + "Annhilation";
		public string RayTexture => Texture + "Ray";
		public string RayEndTexture => Texture + "RayEnd";
		public string RayStartTexture => Texture + "RayStart";
		public string BlastTexture => Texture + "Blast";
	}

	public abstract class MagicVariant<T> : MagicVariant where T : MagicType
	{
		public sealed override int Imbue => AOUtils.ImbuableID<T>();
	}
}
