using ArcaneOdyssey.Imbues.Base;
using ReLogic.Reflection;
using System.Collections.Generic;

namespace ArcaneOdyssey.Imbues
{
	public class ImbueID
	{
		public const byte BasicCombat = 0;
		public const byte Acid = 1;
		public const byte Ash = 2;
		public const byte Crystal = 3;
		public const byte Earth = 4;
		public const byte Explosion = 5;
		public const byte Fire = 6;
		public const byte Glass = 7;
		public const byte Ice = 8;
		public const byte Light = 9;
		public const byte Lightning = 10;
		public const byte Metal = 11;
		public const byte Magma = 12;
		public const byte Plasma = 13;
		public const byte Poison = 14;
		public const byte Sand = 15;
		public const byte Shadow = 16;
		public const byte Snow = 17;
		public const byte Water = 18;
		public const byte Wind = 19;
		public const byte Wood = 20;
		public const byte Spirit = 21;
		public static readonly byte Count = 22;
		public static readonly IdDictionary Search = IdDictionary.Create<ImbueID, byte>();

		[ReinitializeDuringResizeArrays]
		public class Sets : ModSystem
		{
			public static SetFactory Factory = new(Imbuable.count, nameof(ImbueID), Search);

			public static List<int>[] Mutations = Factory.CreateCustomSet<List<int>>(null);

			public static int?[] baseImbues = ItemID.Sets.Factory.CreateCustomSet<int?>(null);

			public static bool[] Solid = Factory.CreateBoolSet(Crystal, Earth, Glass, Ice, Metal, Wood);
			public static bool[] Bright = Factory.CreateBoolSet(Ash, Fire, Light, Lightning, Magma, Plasma, Spirit);

			public static int[] DefaultVariant = Factory.CreateIntSet();

			public static int[] BlastFrames = Factory.CreateIntSet(1);

			public static float[] AuraPower = Factory.CreateFloatSet(.7f,
					Acid, .8f,
					BasicCombat, 1f,
					Crystal, 1.2f,
					Earth, 1.3f,
					Glass, .2f,
					Ice, 1.1f,
					Magma, 1f,
					Metal, 1.4f,
					Sand, 1f,
					Shadow, 1f,
					Snow, .9f,
					Water, .8f,
					Wood, 1.2f,
					BasicCombat, 1f
				);

			public static float[] KBMulti = Factory.CreateFloatSet(1f,
					Wind, 2f
				);

			/// <summary>
			/// Whether this magic is a:
			/// <list>Lesser Lost Magic</list>
			/// <list>Lost Spirit Mutation</list>
			/// <list>Ancient Spirit Mutation</list>
			/// <list>Mythical Spirit Mutation</list>
			/// </summary>
			public static bool[] Special = Factory.CreateBoolSet(Acid, Ash, Crystal, Explosion, Glass, Ice, Magma, Metal, Plasma, Poison, Sand, Snow, Wood);

			public static Color?[] DefaultAnimatedColours = Factory.CreateCustomSet<Color?>(null);

			public override void ResizeArrays()
			{
				// manually change default value
				for (int i = 0; i < Mutations.Length; i++)
				{
					Mutations[i] = [];
				}
			}

			[ReinitializeDuringResizeArrays]
			public static class Assets
			{
				public static Asset<Texture2D>[] annihilationSprites = ItemID.Sets.Factory.CreateCustomSet(Asset<Texture2D>.Empty);

				public static Asset<Texture2D>[] raySprites = ItemID.Sets.Factory.CreateCustomSet(Asset<Texture2D>.Empty);

				public static Asset<Texture2D>[] rayEndSprites = ItemID.Sets.Factory.CreateCustomSet(Asset<Texture2D>.Empty);

				public static Asset<Texture2D>[] rayStartSprites = ItemID.Sets.Factory.CreateCustomSet(Asset<Texture2D>.Empty);

				public static Asset<Texture2D>[] blasts = ItemID.Sets.Factory.CreateCustomSet(Asset<Texture2D>.Empty);
			}
		}
	}
}
